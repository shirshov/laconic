using System;
using System.Collections;
using xf = Xamarin.Forms;

namespace Laconic
{
    interface DiffOperation {}

    record AddGestureRecognizer(IGestureRecognizer Blueprint, params DiffOperation[] Operations) : DiffOperation;
    record RemoveGestureRecognizer(int Index) : DiffOperation;
    record UpdateGestureRecognizer(int Index, params DiffOperation[] Operations) : DiffOperation;
    record AddToolbarItem(ToolbarItem Blueprint, params DiffOperation[] Operations) : DiffOperation;
    record RemoveToolbarItem(int Index) : DiffOperation;
    record UpdateToolbarItem(int Index, DiffOperation[] Operations) : DiffOperation;
    record SetProperty(xf.BindableProperty Property, object Value) : DiffOperation;
    record ResetProperty(xf.BindableProperty Property) : DiffOperation;
    record SetContent(View ContentView, DiffOperation[] Operations) : DiffOperation;
    record UpdateContent(DiffOperation[] Operations) : DiffOperation;
    class RemoveContent : DiffOperation{} 
    record WireEvent(string EventName, Func<EventArgs, Signal> SignalMaker, Action<xf.BindableObject, EventHandler> Subscribe) : DiffOperation;
    record UnwireEvent(string EventName, Action<xf.BindableObject, EventHandler> Unsubscribe) : DiffOperation;
    record UpdateChildViews(params ListOperation[] Operations) : DiffOperation;
    record SetChildElement(xf.BindableProperty ChildElementProperty, Func<xf.BindableObject> CreateElement, params DiffOperation[] Operations) : DiffOperation;
    record SetChildElementToNull(xf.BindableProperty ChildElementProperty) : DiffOperation;
    record UpdateChildElement(Xamarin.Forms.BindableProperty ChildElementProperty, DiffOperation[] Operations) : DiffOperation;
    record UpdateChildElementList(Func<xf.BindableObject, IList> GetList, params ListOperation[] Operations) : DiffOperation;
    record UpdateItems(ListOperation[] Operations) : DiffOperation;
    record GridPositionChange(GridPositionChangeType Type, int Value) : DiffOperation;
    record RowDefinitionsChange(params xf.RowDefinition[] Definitions) : DiffOperation;
    record ColumnDefinitionsChange(params xf.ColumnDefinition[] Definitions) : DiffOperation;
    record SetAbsoluteLayoutPositioning(Bounds Bounds, AbsoluteLayoutFlags Flags) : DiffOperation;
    
    interface ListOperation {}

    record AddChild(Key Key, string ReuseKey, int Index, Element Blueprint, DiffOperation[] Operations) : ListOperation;
    record AddChildWithContext(Key Key, string ReuseKey, int Index, View Blueprint, Guid ContextId, DiffOperation[] Operations) : ListOperation;
    record RemoveChild(int Index) : ListOperation;
    record UpdateChild(Key Key, int Index, Element Blueprint, DiffOperation[] Operations) : ListOperation;
    record ReplaceChild(int Index, Element NewView, DiffOperation[] Operations) : ListOperation;
}

// Records won't work without this 
namespace System.Runtime.CompilerServices
{
    sealed class IsExternalInit
    {
    }
}