## Introduction

Laconic is an MVU library for writing apps with Xamarin.Forms in plain C#, using React + Redux approach.

Code written with Laconic is:

- **Declarative**: Your code simply declares how the app views should look, given the current state of the app, and
  Laconic figures out the most efficient way of updating the actual views.

- **Functional**: Laconic gently pushes you towards functional programming, with a strong emphasis on immutable state
  and pure functions. Code that is written in this style is easier to test, debug, and understand.

- **Familiar**: Code that uses Laconic consists mostly of collection and property initializers, LINQ and switch
  expressions. No DSL, no new semantics. The API is very similar to the API of Xamarin.Forms.

## Key Concepts

<div style="max-width:369px;max-height:669px;">
    <img src="assets/flow-with-middleware.png">
</div>

_(Blue elements is the app code, usually in the form of pure functions; middleware is optional)_

### State

Everything your app displays or manipulates should be kept in a single POCO. There are no special requirements for the
state, but please try to make it immutable. [Laconic.CodeGeneration]() can help.

### Blueprints

Blueprints are _virtual_ representations of app views, calculated from the current state by your code. They are
super-lightweight and can be effortlessly created thousands of times per second. This is what allows Laconic to be declarative.

### Signals

In Laconic based apps signals are a mechanism for reacting to changes in the app. Signals are tiny objects that usually 
carry a payload.

### Reducer

Reducer is a pure function that receives the current state, a signal, and calculates the new state. In most cases the
reducer is a big `switch` expression.

### Binder

Binder is what ties State and Reducer together, and provides methods for creating actual Xamarin.Forms views using
Blueprint maker functions supplied by you. 

### Example

Here's a complete working app:

``` csharp
using System;

namespace Laconic.Demo
{
    public class App : Xamarin.Forms.Application
    {
        static ContentPage Counter(int state) => new ContentPage { Content = new StackLayout
        {
            Padding = 50,
            ["lbl"] = new Label
            {
                Text = $"You clicked {state} times",
                FontSize = 30,
                FontAttributes = FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                HorizontalOptions = LayoutOptions.Center
            },
            ["btn"] = new Button
            {
                Text = "Click Me",
                Clicked = () => new Signal("inc"),
                TextColor = Color.White,
                FontSize = 20,
                BackgroundColor = Color.Coral,
                BorderColor = Color.Chocolate,
                BorderWidth = 3,
                CornerRadius = 10,
                HorizontalOptions = LayoutOptions.Center,
                Padding = (30, 0)
            }
        }};

        static int Reducer(int state, Signal signal) => signal.Payload switch
        {
            "inc" => state + 1,
            "dec" => throw new NotImplementedException(), // Left as an exercise for the reader
            _ => state
        };

        readonly Binder<int> _binder;

        public App()
        {

            _binder = Binder.Create(0, Reducer);
            MainPage = _binder.CreatePage(Counter);
        }
    }
}
```

For more samples check the Demo app in this repository, and code in these repositories:

[KickassUI.Banking](https://github.com/shirshov/KickassUI.Banking) -- a complete port to Laconic of a sample app 
( UI only) by Steven Thewissen.

[My Contacts](https://github.com/shirshov/app-contacts) -- a step by step porting to Laconic of a sample app by Xamarin.
A log of the journey is [here](https://omnitalented.com/converting-my-contacts-step-1/).

## Keys

Laconic's API mostly mimics the API of Xamarin.Forms elements with one distinction: when adding a view to `Children`
collection it's required to provide a key:

```csharp
new StackLayout  {
    ["first-name"] = new Label { Text = "First Name" },
    ...
}
```
The keys are used by Laconic to perform efficient tracking of added/removed/changed subviews during diffing and
patching. The keys can be of type `string`, `int`, `long` or `Guid`. The key must not change if it refers to the same view,
even if the view properties changed.

*Note:* the above snippet is technically equivalent to:
```csharp
new StackLayout  {
    Children = {
        ["first-name"] = new Label { Text = "First Name" },
        ...
    }
}
```

Sample code in the Demo app uses the former notation to reduce nesting.

### Grid

Dictionary initializer for `Grid.Children` has optional parameters for specifying a child's position:
```csharp
new Grid {
    ...
    ["footer", row: 3, column: 0, rowSpan: 2] = new Label {...}
}
```

Helper methods `ToViewList` and `ToGridViewList` can be used for creating `Chidren` dictionary from an `IEnumerable<T>` source.

## Reuse keys

When adding views to a `CollectionView` in addition to providing the key you must provide a *reuse key*. The reuse key
tells Laconic that this view can be reused after it was scrolled out of screen. Behind the scenes Laconic creates a
`DataTempate` for each reuse key. For an example of usage of reuse keys in heterogenous `CollectionView` 
[check this code in the Demo app](https://github.com/shirshov/laconic/blob/master/demo/app/GroupedCollectionView.cs).

## LocalContext

For controlling scope Laconic has a concept of *local context*:

```csharp
Element.WithContext(ctx => {
    var (state, setter) = ctx.UseLocalState(initial);
    return new ContentPage {
        ["button"] = new Button { Text = $"You clicked {state} times", Clicked = () => setter(state + 1) },
    ]};
});
```
More details on `LocalContext` can be found [here](https://omnitalented.com/converting-my-contacts-step-3/).
For an example of more or less real world usage check out a port of [My Contacts](https://github.com/shirshov/app-contacts) app.

## Niceties

Laconic comes with built in implicit conversions where it makes sense. 

For example, grid's row/column definitions can be created from strings:
```csharp
var g = new Grid { 
    RowDefinition = "Auto, 2*, *, 50",
    ColumnDefinition = "*, 2*, Auto, 30"
};
```

`Thickness` and `CornerRadius` structs can be created from a single `double`, two element tuple or four element tuple:

```csharp
var l1 = new Label { Margin = 15 }; // Thickness: uniform margin
var l2 = new Label { Margin = (10, 20) }; // Thickness: 10 for left and right, 20 for top and bottom
var l3 = new BoxView { CornerRadius = (10, 20, 30, 40) }; // CornerRadius: top left, top right, bottom left, bottom right
```

`Color` can be created from a tuple with three or four (with alpha) `byte` elements, or from a hex string:

```csharp
var l1 = new Label { TextColor = (255, 123, 123) };
var l2 = new Label { TextColor = "#222222" };
```

## Code Generation

When writing an app with Laconic you should make your state immutable. A sister project, 
[Laconic.CodeGeneration](https://github.com/shirshov/laconic/tree/master/codegen), can help with that.
___

[![Join the chat at https://gitter.im/laconiclib/community](https://badges.gitter.im/laconiclib/community.svg)](https://gitter.im/laconiclib/community?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=badge)

