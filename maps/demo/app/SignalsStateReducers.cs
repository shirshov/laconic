using System.Linq;

namespace Laconic.Maps.Demo
{
    enum FeatureSwitch
    {
        IsShowingUser,
        TrafficEnabled,
        HasScrollEnabled,
        HasZoomEnabled
    }
    
    record City(string Name, int Population,  Polygon[] Boundaries, bool IsShownOnMap);
    record Features(bool IsShowingUser, bool TrafficEnabled, bool HasScrollEnabled, bool HasZoomEnabled);
    record State(MapType MapType, Features Features, City[] Cities);

    static class Reducers
    {
        static Features Features(Features features, Signal signal) => signal switch {
            (FeatureSwitch.IsShowingUser, bool val) => features with { IsShowingUser = val },
            (FeatureSwitch.TrafficEnabled, bool val) => features with { TrafficEnabled = val },
            (FeatureSwitch.HasScrollEnabled, bool val) => features with { HasScrollEnabled = val },
            (FeatureSwitch.HasZoomEnabled, bool val) => features with { HasZoomEnabled = val }, 
            _ => features
        };

        static City[] Cities(City[] cities, Signal signal) => signal switch {
            ("toggle-city", string cityName) => cities
                .Select(c => c.Name == cityName ? c with { IsShownOnMap = ! c.IsShownOnMap } : c).ToArray(),
            _ => cities
        };
        
        public static State Main(State state, Signal signal) => signal switch {
            (MapType t, _) => state with { MapType = t },
            _ => state with { Features = Features(state.Features, signal), Cities = Cities(state.Cities, signal) }
        };
        
    }
}

// Records won't work without this 
namespace System.Runtime.CompilerServices
{
    sealed class IsExternalInit
    {
    }
}
