using System.Collections.Generic;

namespace Laconic
{
    static class AbsoluteLayoutDiff
    {
        public static IEnumerable<SetAbsoluteLayoutPositioning> Calculate(
            Key key, AbsoluteLayoutViewList? existingList, AbsoluteLayoutViewList newList)
        {
            var existingPos = new AbsLayoutInfo(new Bounds(-1, -1, -1, -1), AbsoluteLayoutFlags.None);
            if (existingList != null && existingList.ContainsKey(key))
                existingPos = existingList.GetPositioning(key);
            
            var newPos = newList.GetPositioning(key);
            
            if (newPos != existingPos)
                yield return new SetAbsoluteLayoutPositioning(newPos.Bounds, newPos.Flags);
        }
    }
}