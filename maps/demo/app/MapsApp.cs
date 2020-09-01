using System.Linq;
using Laconic.CodeGeneration;

namespace Laconic.Maps.Demo
{
    public class MapsApp : Xamarin.Forms.Application
    {
        static View Controls(State state)
        {
            static Button MapTypeButton (MapType type, bool isSelected) => new Button {
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
                [0] = MapTypeButton(MapType.Street, state.MapType == MapType.Street),
                [1, column: 1] = MapTypeButton(MapType.Satellite, state.MapType == MapType.Satellite),
                [2, column: 2] = MapTypeButton(MapType.Hybrid, state.MapType == MapType.Hybrid),
            };

            static View SwitchRow(string text, bool isToggled, FeatureSwitch toggle) => new Grid {
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
                ["user"] = SwitchRow("Show your location", state.Features.IsShowingUser, FeatureSwitch.IsShowingUser),
                ["l2"] = line,
                ["traffic"] = SwitchRow("Show traffic", state.Features.TrafficEnabled, FeatureSwitch.TrafficEnabled),
                ["l3"] = line,
                ["scroll"] = SwitchRow("Allow scrolling", state.Features.HasScrollEnabled, FeatureSwitch.HasScrollEnabled),
                ["l4"] = line,
                ["zoom"] = SwitchRow("Allow zooming", state.Features.HasZoomEnabled, FeatureSwitch.HasZoomEnabled),
            };
        }

        static Grid MainContent(State state, Polygon p) => new Grid {
            RowDefinitions = "*, Auto",
            ["map"] = new Map {
                MapType = state.MapType,
                IsShowingUser = state.Features.IsShowingUser,
                TrafficEnabled = state.Features.TrafficEnabled,
                HasScrollEnabled = state.Features.HasScrollEnabled,
                HasZoomEnabled = state.Features.HasZoomEnabled,
                Pins = {
                    ["Rome"] = new Pin {
                        Type = PinType.SavedPin,
                        Label = "Rome",
                        Position = (41.890202, 12.492049)
                    }
                },
                MapElements = (
                        from c in state.Cities
                        from poly in c.Boundaries
                        select poly
                    )
                    .Select((p, i) => (Polygon: p, Index: i))
                    .ToDictionary(x => x.Index, x => (Element)x.Polygon) // TODO: why the casting is required?
            },
            ["controls", row: 1] = Controls(state)
        };
        
        readonly Binder<State> _binder;

        public MapsApp()
        {
            var cities = CityLoader.Load();
            
            _binder = Binder.Create(new State(MapType.Street, new Features(false, false, true, true),  cities), Reducers.Main);

            MainPage = _binder.CreateElement(s => new ContentPage {
                Content = MainContent(s, cities[0].Boundaries[0])
            });
        }
    }
}