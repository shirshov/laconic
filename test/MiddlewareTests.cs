using Shouldly;
using Xunit;

namespace Laconic.Tests
{
    public class MiddlewareTests
    {
        [Fact]
        public void middleware_is_called()
        {
            var binder = Binder.Create("", (s, g) => s);
            var isCalled = false;
            binder.UseMiddleware((context,  next) =>
            {
                isCalled = true;
                return next(context);
            });
            
            binder.Dispatch(new Signal("_"));

            isCalled.ShouldBeTrue();
        }

        [Fact]
        public void middleware_modifies_state_before_reducer()
        {
            var binder = Binder.Create("initial", (s, g) => s +  " - reducer");
            binder.UseMiddleware((context, next) =>
            {
                var modified = context.WithState(context.State + " - modified");
                return next(modified);
            });
            
            binder.Dispatch(new Signal("_"));

            binder.State.ShouldBe("initial - modified - reducer");
        }
        
        [Fact]
        public void middleware_modifies_state_after_reducer()
        {
            var binder = Binder.Create("initial", (s, g) => "reducer");
            binder.UseMiddleware((context, next) =>
            {
                var ctx = next(context);
                return ctx.WithState(ctx.State + " - modified");
            });
            
            binder.Dispatch(new Signal("_"));

            binder.State.ShouldBe("reducer - modified");
        }

        [Fact]
        public void middleware_can_be_chained()
        {
            var binder = Binder.Create("initial", (s, g) => "reducer");
            binder.UseMiddleware((context, next) =>
            {
                var ctx = next(context);
                return ctx.WithState(ctx.State + " - outer");
            });
            binder.UseMiddleware((context, next) =>
            {
                var ctx = next(context);
                return ctx.WithState(ctx.State + " - inner");
            });
            
            binder.Dispatch(new Signal("_"));

            binder.State.ShouldBe("reducer - inner - outer");
        }
    }
}