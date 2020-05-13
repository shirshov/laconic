using System;
using Xunit;
using Shouldly;

namespace Laconic.Tests
{
    public class BasicTests
    {
        [Fact]
        public void visual_element_equality()
        {
            new Label {Text = "a"}.Equals(new Label {Text = "a"}).ShouldBeTrue();
            new Label {Text = "a"}.Equals(new Label {Text = "b"}).ShouldBeFalse();

            (new Label {Text = "a"} == new Label {Text = "a"}).ShouldBeTrue();
            (new Label {Text = "a"} == new Label {Text = "b"}).ShouldBeFalse();
        }

        [Fact]
        public void key_casting_equality()
        {
            (new Key(1) == 1).ShouldBeTrue();
            (new Key(1L) == 1L).ShouldBeTrue();
            (new Key("a") == "a").ShouldBeTrue();

            (new Key(2) == 1).ShouldBeFalse();
            (new Key(2L) == 1L).ShouldBeFalse();
            (new Key("a") == "b").ShouldBeFalse();
        }

        [Fact]
        public void throw_on_setting_child_key_twice() =>
            Should.Throw<ArgumentException>(() =>
            {
                var _ = new StackLayout {["1"] = new Label(), ["1"] = new Label()};
            }).Message.ShouldBe("An item with the same key has already been added. Key: Key: 1");
    }
}