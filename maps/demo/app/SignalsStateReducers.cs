using System.Linq;
using Laconic.CodeGeneration;

namespace Laconic.Maps.Demo
{
    enum FeatureSwitch
    {
        IsShowingUser,
        TrafficEnabled,
        HasScrollEnabled,
        HasZoomEnabled
    }
    
    [Records]
    interface AppStateRecords
    {
        record City(string name, int population,  Polygon[] boundaries, bool isShownOnMap);
        record Features(bool isShowingUser, bool trafficEnabled, bool hasScrollEnabled, bool hasZoomEnabled);
        record State(MapType mapType, Features features, City[] cities);
    }

    static class Reducers
    {
        static Features Features(Features features, Signal signal) => signal switch {
            (FeatureSwitch.IsShowingUser, bool val) => features.With(isShowingUser: val),
            (FeatureSwitch.TrafficEnabled, bool val) => features.With(trafficEnabled: val),
            (FeatureSwitch.HasScrollEnabled, bool val) => features.With(hasScrollEnabled: val),
            (FeatureSwitch.HasZoomEnabled, bool val) => features.With(hasZoomEnabled: val),
            _ => features
        };

        static City[] Cities(City[] cities, Signal signal) => signal switch {
            ("toggle-city", string cityName) => cities
                .Select(c => c.Name == cityName ? c.With(isShownOnMap: !c.IsShownOnMap) : c).ToArray(),
            _ => cities
        };
        
        public static State Main(State state, Signal signal) => signal switch {
            (MapType t, _) => state.With(mapType: t),
            _ => state.With(features: Features(state.Features, signal), cities: Cities(state.Cities, signal))
        };
        
    }
}