namespace Laconic.Tests;

public class PickerTests
{
    [Fact]
    public void do_not_update_Items_until_changed()
    {
        var binder = Binder.Create(0, (s, g) => 1);
        var picker = binder.CreateElement(s => new Picker
        {
            Items = new [] {"0", "1", "2"}, 
            SelectedIndex = s,
            SelectedIndexChanged = e => new Signal(e)
        });
            
        picker.SelectedIndex = 1;
             
        picker.SelectedIndex.ShouldBe(1);
    }
}