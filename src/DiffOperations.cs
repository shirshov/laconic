using System;
using Laconic.CodeGeneration;
using Laconic.Shapes;

namespace Laconic
{
    [Union]
    interface __DiffOperation
    {
        record AddGestureRecognizer(IGestureRecognizer blueprint, params DiffOperation[] operations);
        record RemoveGestureRecognizer(int index);
        record UpdateGestureRecognizer(int index, params DiffOperation[] operations);
        record AddToolbarItem(ToolbarItem blueprint, params DiffOperation[] operations);
        record RemoveToolbarItem(int index);
        record UpdateToolbarItem(int index, DiffOperation[] operations);
        record SetProperty(Xamarin.Forms.BindableProperty property, object value);
        record ResetProperty(Xamarin.Forms.BindableProperty property);
        record SetContent(View contentView, DiffOperation[] operations);
        record UpdateContent(DiffOperation[] operations);
        record RemoveContent();
        record WireEvent(string eventName, Func<EventArgs, Signal> signalMaker, Action<Xamarin.Forms.BindableObject, EventHandler> subscribe);
        record UnwireEvent(string eventName, Action<Xamarin.Forms.BindableObject, EventHandler> unsubscribe);
        record SetClip(Geometry geometry);
        record UpdateChildren(params ListOperation[] operations);
        record UpdateItems(ListOperation[] operations);
        record GridPositionChange(GridPositionChangeType type, int value);
        record RowDefinitionsChange(params Xamarin.Forms.RowDefinition[] definitions);
        record ColumnDefinitionsChange(params Xamarin.Forms.ColumnDefinition[] definitions);
    }
    
    [Union]
    interface __ListOperation
    {
        record AddChild(Key key, string reuseKey, int index, View blueprint, DiffOperation[] operations);
        record RemoveChild(int index);
        record UpdateChild(Key key, int index, View blueprint,DiffOperation[] operations);
        record ReplaceChild(int index, View newView, DiffOperation[] operations);
    }
}