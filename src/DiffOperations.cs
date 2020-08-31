using System;
using System.Collections;
using System.Collections.Generic;
using Laconic.CodeGeneration;
using xf = Xamarin.Forms;

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
        record SetProperty(xf.BindableProperty property, object value);
        record ResetProperty(xf.BindableProperty property);
        record SetContent(View contentView, DiffOperation[] operations);
        record UpdateContent(DiffOperation[] operations);
        record RemoveContent();
        record WireEvent(string eventName, Func<EventArgs, Signal> signalMaker, Action<xf.BindableObject, EventHandler> subscribe);
        record UnwireEvent(string eventName, Action<xf.BindableObject, EventHandler> unsubscribe);
        record UpdateChildViews(params ListOperation[] operations);
        record SetChildElement(xf.BindableProperty childElementProperty, 
            Func<xf.BindableObject> createElement, params DiffOperation[] operations);
        record SetChildElementToNull(xf.BindableProperty childElementProperty);
        record UpdateChildElement(Xamarin.Forms.BindableProperty childElementProperty, DiffOperation[] operations);
        record UpdateChildElementList(Func<xf.Element, IList> getList, params ListOperation[] operations);
        record UpdateItems(ListOperation[] operations);
        record GridPositionChange(GridPositionChangeType type, int value);
        record RowDefinitionsChange(params xf.RowDefinition[] definitions);
        record ColumnDefinitionsChange(params xf.ColumnDefinition[] definitions);
        record SetAbsoluteLayoutPositioning(Bounds bounds, AbsoluteLayoutFlags flags);
    }
    
    [Union]
    interface __ListOperation
    {
        record AddChild(Key key, string reuseKey, int index, IElement blueprint, DiffOperation[] operations);
        record AddChildWithContext(Key key, string reuseKey, int index, View blueprint, Guid contextId, DiffOperation[] operations);
        record RemoveChild(int index);
        record UpdateChild(Key key, int index, View blueprint,DiffOperation[] operations);
        record ReplaceChild(int index, View newView, DiffOperation[] operations);
    }
}