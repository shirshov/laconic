using System.ComponentModel;
using System.Linq;
using Xunit;
using Shouldly;

namespace Laconic.Tests
{
    public class ContextExpanderTests
    {
        [Fact]
        public void LocalContextInfo_is_created()
        {
            var (el, contexts) = ContextExpander.Expand(Element.WithContext(ctx => new Button {
                Text = "foo"
            }), Enumerable.Empty<ExpansionInfo>(), _ => { });

            contexts.Count().ShouldBe(1);
            var info = contexts.First();
            info.Blueprint.ContextKey.ShouldBe(info.Context.Key);
            info.Blueprint.ShouldBe(info.BlueprintMaker(info.Context));
        }
            
        [Fact]
        public void Content_is_ContextElement()
        {
            var (el, contexts) = ContextExpander.Expand(new ContentPage {
                Content = Element.WithContext(_ => new Label())
            }, Enumerable.Empty<ExpansionInfo>(), _ => { });

            contexts.Count().ShouldBe(1);
            contexts.First().Context.Key.ShouldBe("./Content");
            var p = (ContentPage)el;
            p.Content.ShouldBeOfType<Label>();
        }
        
        [Fact]
        public void Content_is_ContextElement_with_named_context()
        {
            var (el, contexts) = ContextExpander.Expand(new ContentPage {
                Content = Element.WithContext("lbl", _ => new Label())
            }, Enumerable.Empty<ExpansionInfo>(), _ => { });

            contexts.Count().ShouldBe(1);
            contexts.First().Context.Key.ShouldBe("./Content[lbl]");
            var p = (ContentPage)el;
            p.Content.ShouldBeOfType<Label>();
        }

        [Fact]
        public void Context_is_reused()
        {
            ContextElement<Xamarin.Forms.Label> MakeView() => Element.WithContext(ctx => {
                var (state, _) = ctx.UseLocalState("a");
                return new Label {Text = state};
            });
        
            var (el, contexts) = ContextExpander.Expand(MakeView(), new ExpansionInfo[0], _ => { });
            
            el.ShouldBeOfType<Label>().Text.ShouldBe("a");
            contexts.Count().ShouldBe(1);
            
            contexts.First(x => x.Context.Key == ".").Context.SetValue("laconic.localstate", "b");
        
            var (el2, contexts2) = ContextExpander.Expand(MakeView(), contexts, _ => { });
            
            contexts2.Count().ShouldBe(1);
            el2.ShouldBeOfType<Label>().Text.ShouldBe("b");
        }
        
        [Fact]
        public void Context_is_keyed_to_child_key()
        {
            var (el, contexts) = ContextExpander.Expand(new StackLayout {
                ["lbl"] = Element.WithContext(_ => new Label()),
                ["btn"] = Element.WithContext(_ => new Button())
            }, Enumerable.Empty<ExpansionInfo>(), _ => { });

            contexts.Count().ShouldBe(2);
            contexts.ShouldContain(x => x.Context.Key == "./lbl");
            contexts.ShouldContain(x => x.Context.Key == "./btn");
        
            var sl = el.ShouldBeOfType<StackLayout>();
            sl.Children["lbl"].ShouldBeOfType<Label>();
            sl.Children["btn"].ShouldBeOfType<Button>();
        }
        
        [Fact]
        public void named_Contexts()
        {
            var (el, contexts) = ContextExpander.Expand(new StackLayout {
                ["lbl"] = Element.WithContext("a", _ => new Label()),
                ["btn"] = Element.WithContext("b", _ => new Button())
            }, Enumerable.Empty<ExpansionInfo>(), _ => { });
        
            contexts.ShouldContain(x => x.Context.Key ==  "./a");
            contexts.ShouldContain(x => x.Context.Key ==  "./b");
        
            var sl = el.ShouldBeOfType<StackLayout>();
            sl.Children["lbl"].ShouldBeOfType<Label>();
            sl.Children["btn"].ShouldBeOfType<Button>();
        }

        [Fact]
        public void changed_contexts_same_hierarchy()
        {
            var stringStateView = Element.WithContext("string", ctx => {
                var (state, _) = ctx.UseLocalState("string");
                return new Label {Text = state};
            });

            var intStateView = Element.WithContext("int", ctx => {
                var (state, _) = ctx.UseLocalState(1);
                return new Label {Text = state.ToString()};
            });

            var (_, contexts) = ContextExpander.Expand(stringStateView, Enumerable.Empty<ExpansionInfo>(), _ => { });
            contexts.Count().ShouldBe(1);

            var (_, contexts2) = ContextExpander.Expand(intStateView, contexts, _ => { });
            contexts2.Count().ShouldBe(1);
        }
    }
}