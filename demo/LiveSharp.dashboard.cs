using LiveSharp;

[assembly: LiveSharpInject("*")]

namespace LiveSharp 
{
    class LiveSharpDashboard : ILiveSharpDashboard
    {
        public void Configure(ILiveSharpRuntime app) => app.OnCodeUpdateReceived(_ => Laconic.Demo.App._binder.Send(new(null)));

        public void Run(ILiveSharpRuntime app)
        {
        }
    } 
}