using System;
using Laconic.CodeGeneration;
using m = Xamarin.Forms.Maps;

namespace Laconic.Maps.Demo
{
    [Records]
    interface AppStateRecords
    {
        record State(m.MapType mapType, bool isShowingUser, bool trafficEnabled, bool hasScrollEnabled, bool hasZoomEnabled);
    }

    enum MapSwitch
    {
        IsShowingUser,
        TrafficEnabled,
        HasScrollEnabled,
        HasZoomEnabled
    }
    
    public class MapsApp : Xamarin.Forms.Application
    {
        static State Reducer(State state, Signal signal) => signal switch {
            (m.MapType t, _) => state.With(mapType: t),
            (MapSwitch.IsShowingUser, bool val) => state.With(isShowingUser: val),
            (MapSwitch.TrafficEnabled, bool val) => state.With(trafficEnabled: val),
            (MapSwitch.HasScrollEnabled, bool val) => state.With(hasScrollEnabled: val),
            (MapSwitch.HasZoomEnabled, bool val) => state.With(hasZoomEnabled: val),
            _ => state
        };
        
        static View Controls(State state)
        {
            static Button MapTypeButton (m.MapType type, bool isSelected) => new Button {
                Text = type.ToString(),
                TextColor = Color.White,
                BackgroundColor = isSelected ? (0, 0, 0, 130) : Color.Default,
                HorizontalOptions = LayoutOptions.Fill,
                CornerRadius = 15,
                Clicked = () => new Signal(type)
            };
            
            var mapTypeControls = new Grid {
                ["bg", columnSpan: 3] = new BoxView {
                    HeightRequest = 35,
                    CornerRadius = 15, 
                    BackgroundColor = (0, 0, 0, 40), 
                    VerticalOptions = LayoutOptions.Center,
                },
                [0] = MapTypeButton(m.MapType.Street, state.MapType == m.MapType.Street),
                [1, column: 1] = MapTypeButton(m.MapType.Satellite, state.MapType == m.MapType.Satellite),
                [2, column: 2] = MapTypeButton(m.MapType.Hybrid, state.MapType == m.MapType.Hybrid),
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
                Padding = (20, 20, 20, 50),
                ["type"] = mapTypeControls,
                // ["l1"] = line,
                ["user"] = SwitchRow("Show your location", state.IsShowingUser, MapSwitch.IsShowingUser),
                ["l2"] = line,
                ["traffic"] = SwitchRow("Show traffic", state.TrafficEnabled, MapSwitch.TrafficEnabled),
                ["l3"] = line,
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
                HasZoomEnabled = state.HasZoomEnabled,
                Pins = {
                    ["sydney"] = new Pin {
                        Type = m.PinType.SavedPin,
                        Label = "Sydney",
                        Position = new m.Position(-33.86785, 151.20732)
                    }
                },
            },
            ["controls", row: 1] = Controls(state)
        };
        
        readonly Binder<State> _binder;

        public MapsApp()
        {
            _binder = Binder.Create(new State(m.MapType.Hybrid, false, false, true, true), Reducer);

            MainPage = _binder.CreateElement(s => new ContentPage {
                Content = MainContent(s)
            });
        }
    }
}