using System.ComponentModel;
using Microsoft.Maui.Controls;

namespace Laconic.Demo;

class AnimateTextSize : Behavior<Microsoft.Maui.Controls.Label>
{
    protected override void OnAttachedTo(xf.Label bindable) => bindable.PropertyChanged += OnPropertyChanged;

    protected override void OnDetachingFrom(xf.Label bindable) => bindable.PropertyChanged -= OnPropertyChanged;

    void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == xf.Label.TextProperty.PropertyName) {
            var lbl = (xf.Label)sender;
            lbl.Animate("text-size", s => lbl.Scale = s, 5.0, 1.0);
        }
    }
}
    
static class BehaviorPage
{
    public static View Content() => Element.WithContext("behavior", ctx => {
        var (count, setCount) = ctx.UseLocalState(0);
        return new Grid {
            ["lbl"] = new Label {
                Text = count.ToString(),
                FontSize = 50,
                Behaviors = {["anim"] = new AnimateTextSize()},
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
            }, ["btn", row: 1] = new Button {
                Text = "Update",
                Clicked = () => setCount(count + 1),
                TextColor = Color.White,
                FontSize = 20,
                BackgroundColor = Color.Coral,
                BorderColor = Color.Chocolate,
                BorderWidth = 2,
                CornerRadius = 20,
                HeightRequest = 40,
                Margin =  (0, 30),
                Padding = (30, 0),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.End,
            }
        };
    });
}