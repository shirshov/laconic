using System;
using System.Linq;
using Xamarin.Forms;

[assembly: ExportFont("Font Awesome 5 Free-Solid-900.otf", Alias = "IconFont")]

namespace Laconic.Maps.Demo
{
    public class MapsApp : Xamarin.Forms.Application
    {
        static StackLayout Features(State state)
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

        static View CityRow(City city) => new StackLayout {
            Orientation = StackOrientation.Horizontal,
            Padding = (20, 0),
            ["name"] = new Label { Text = city.Name,VerticalOptions = LayoutOptions.Center},
            ["population"] = new Label { 
                Text = city.Population.ToString(), 
                FontSize = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.Center,
            },
            ["show"] = new ImageButton {
                Source = new FontImageSource {
                    FontFamily = "IconFont", 
                    // Glyph = city.IsShownOnMap ? "\uf070" : "\uf06e", 
                    Glyph = "\uf06e", 
                    Color = city.IsShownOnMap ? Color.DarkGray : Color.Silver
                },
                BorderColor = Color.Silver,
                BorderWidth = 1,
                Clicked = () => new Signal("toggle-city", city.Name),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            },
            
            ["locate"] = new ImageButton {
                Source = new FontImageSource {
                    FontFamily = "IconFont", 
                    Glyph = "\uf05b",
                    Color = Color.Silver
                },
                Clicked = () => new Signal("toggle-city", city.Name),
                HorizontalOptions = LayoutOptions.End,
                VerticalOptions = LayoutOptions.Center
            }
        };
        
        static View Cities(City[] cities) => new StackLayout {
            ["lbl"] = new Label { Text = "CITIES"},
            ["lst"] = new CollectionView {
                Items = cities.ToItemsList(x => "city", x => x.Name, CityRow)
            }
        };

        static Grid MainContent(State state) => new Grid {
            RowDefinitions = "*, 300",
            ["map"] = new Map {
                MapType = state.MapType,
                IsShowingUser = state.Features.IsShowingUser,
                TrafficEnabled = state.Features.TrafficEnabled,
                HasScrollEnabled = state.Features.HasScrollEnabled,
                HasZoomEnabled = state.Features.HasZoomEnabled,
                MapElements = (
                    from c in state.Cities
                    from polyWithIndex in c.Boundaries.Select((p, i) => (Polygon: p, Index: i))
                    where c.IsShownOnMap
                    select (Key: $"{c.Name}({polyWithIndex.Index})", Polygon:  polyWithIndex.Polygon)
                ).ToDictionary(x => x.Key, x => (Element) x.Polygon) // TODO: why the cast is required?
            },
            ["controls", row: 1] = new CarouselView {
                Items = {
                    ["features", "f"] = Features(state), 
                    ["cities", "c"] = Cities(state.Cities)
                },
            }
        };

        readonly Binder<State> _binder;

        public MapsApp()
        {
            var cities = CityLoader.Load();
            
            _binder = Binder.Create(new State(MapType.Street, new Features(false, false, true, true),  cities), Reducers.Main);

            MainPage = _binder.CreateElement(s => new ContentPage {
                Content = MainContent(s)
            });
        }
    }
}