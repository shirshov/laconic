namespace Laconic.Tests;

public class BehaviorTests
{
    class TestBehavior : Behavior<xf.Label>
    {
        public bool IsAttached;
        readonly string _text;
        xf.Label? _label;

        public TestBehavior(string text) : base(text) => _text = text;

        protected internal override void OnValuesUpdated(object value) => _label!.Text = (string)value;

        protected internal override void OnAttachedTo(xf.Label bindable)
        {
            _label = bindable;
            IsAttached = true;
            bindable.Text = _text;
        }

        protected internal override void OnDetachingFrom(xf.Label bindable) => IsAttached = false;
    }

    [Fact]
    public void XF_Behavior_is_added_and_removed()
    {
        var real = new xf.Label();
        var testBehavior = new TestBehavior("");
        var originalBlueprint = new Label {Behaviors = {[0] = testBehavior}};
            
        Patch.Apply(real, Diff.Calculate(null, originalBlueprint), _ => { });
            
        real.Behaviors.Count.ShouldBe(1);
        testBehavior.IsAttached.ShouldBeTrue();

        Patch.Apply(real, Diff.Calculate(originalBlueprint, new Label()), _ => { });
            
        real.Behaviors.Count.ShouldBe(0);
        testBehavior.IsAttached.ShouldBeFalse();
    }
        
    [Fact]
    public void Behavior_is_reused()
    {
        var real = new xf.Label();
        var originalBlueprint = new Label {Behaviors = {[0] = new TestBehavior("") }};
            
        Patch.Apply(real, Diff.Calculate(null, originalBlueprint), _ => { });
            
        real.Behaviors.Count.ShouldBe(1);
        var realBehavior = real.Behaviors[0];

        Patch.Apply(real, Diff.Calculate(
                originalBlueprint, 
                new Label{Behaviors = {[0] = new TestBehavior("")}}),
            _ => { });
            
        real.Behaviors[0].ShouldBe(realBehavior);
    }

    [Fact]
    public void Behavior_updates_view_values()
    {
        var real = new xf.Label();
            
        var originalBlueprint = new Label {Behaviors = {[0] = new TestBehavior("")}};
        Patch.Apply(real, Diff.Calculate(null, originalBlueprint), _ => { });
            
        real.Text.ShouldBe("");

        Patch.Apply(real, Diff.Calculate(
                originalBlueprint, 
                new Label{Behaviors = {[0] = new TestBehavior("updated")}}),
            _ => { });
            
        real.Text.ShouldBe("updated");
    }
}