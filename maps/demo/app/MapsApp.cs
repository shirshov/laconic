using System;
using Laconic.CodeGeneration;
using Xamarin.Forms.Maps;
using xf = Xamarin.Forms;

namespace Laconic.Maps.Demo
{
    [Records]
    interface AppStateRecords
    {
        record State(MapType mapType, bool isShowingUser, bool trafficEnabled, bool hasScrollEnabled, bool hasZoomEnabled);
    }

    [Flags]
    enum MapSwitch
    {
        IsShowingUser = 0,
        TrafficEnabled = 1,
        HasScrollEnabled = 2,
        HasZoomEnabled = 4
    }
    
    public class MapsApp : Xamarin.Forms.Application
    {
        static State Reducer(State state, Signal signal) => signal switch {
            (MapType t, _) => state.With(mapType: t),
            (MapSwitch.IsShowingUser, bool val) => state.With(isShowingUser: val),
            (MapSwitch.TrafficEnabled, bool val) => state.With(trafficEnabled: val),
            (MapSwitch.HasScrollEnabled, bool val) => state.With(hasScrollEnabled: val),
            (MapSwitch.HasZoomEnabled, bool val) => state.With(hasZoomEnabled: val),
            _ => state
        };
        
        static View Controls(State state)
        {
            static Button MapTypeButton (MapType type, bool isSelected) => new Button {
                Text = type.ToString(),
                TextColor = isSelected ? Color.White : Color.DarkGray,
                BackgroundColor = isSelected ? Color.DarkGray : Color.Default,
                HorizontalOptions = LayoutOptions.Fill,
                CornerRadius = 15,
                Clicked = () => new Signal(type)
            };
            
            var mapTypeControls = new Grid {
                [0] = MapTypeButton(MapType.Street, state.MapType == MapType.Street),
                [1, column: 1] = MapTypeButton(MapType.Satellite, state.MapType == MapType.Satellite),
                [2, column: 2] = MapTypeButton(MapType.Hybrid, state.MapType == MapType.Hybrid),
            };

            static View SwitchRow(string text, bool isToggled, MapSwitch toggle) => new Grid {
                ["l"] = new Label { Text = text, VerticalOptions = LayoutOptions.Center, TextColor = (0, 0, 0, 160)},
                ["s"] = new Switch { 
                    HorizontalOptions = LayoutOptions.End, 
                    IsToggled = isToggled,
                    Toggled = e => new Signal(toggle, e.Value)
                }
            };

            var line = new BoxView {HeightRequest = 1, Color = (0, 0, 0, 50)};
            
            return new StackLayout {
                Padding = (20, 40),
                ["type"] = mapTypeControls,
                ["l1"] = line,
                ["user"] = SwitchRow("Show your location", state.IsShowingUser, MapSwitch.IsShowingUser),
                ["l2"] = line,
                ["traffic"] = SwitchRow("Show traffic", state.TrafficEnabled, MapSwitch.TrafficEnabled),
                ["l3"] = new BoxView { HeightRequest = 1, Color = Color.LightGray}, //line,
                ["scroll"] = SwitchRow("Allow scrolling", state.HasScrollEnabled, MapSwitch.HasScrollEnabled),
                ["l4"] = line,
                ["zoom"] = SwitchRow("Allow zooming", state.HasZoomEnabled, MapSwitch.HasZoomEnabled),
            };
        }

        static Grid MainContent(State state) => new Grid {
            RowDefinitions = "*, Auto",
            ["map"] = new Map {
                MapType = state.MapType,
                IsShowingUser = state.IsShowingUser,
                TrafficEnabled = state.TrafficEnabled,
                HasScrollEnabled = state.HasScrollEnabled,
                HasZoomEnabled = state.HasZoomEnabled
            },
            ["controls", row: 1] = Controls(state)
        };
        
        readonly Binder<State> _binder;

        public MapsApp()
        {
            _binder = Binder.Create(new State(MapType.Hybrid, false, false, true, true), Reducer);

            MainPage = _binder.CreateElement(s => new ContentPage {
                Content = MainContent(s)
            });
        }
    }
}