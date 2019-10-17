using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Viewer")]

namespace Seanuts.Logic.PathFinding
{
    
    internal static partial class SNPathFinder
    {
        internal static List<Step> StepList { get; } = new List<Step>(0);

        [Conditional("DEBUG")]
        private static void MessageCurrent(SNPosition position, IReadOnlyList<SNPosition> path)
        {                        
            StepList.Add(new Step(StepType.Current, position, path));
        }            

        [Conditional("DEBUG")]
        private static void MessageOpen(SNPosition position)
            => StepList.Add(new Step(StepType.Open, position, new List<SNPosition>(0)));

        [Conditional("DEBUG")]
        private static void MessageClose(SNPosition position)
            => StepList.Add(new Step(StepType.Close, position, new List<SNPosition>(0)));

        [Conditional("DEBUG")]
        private static void ClearStepList()
            => StepList.Clear();
        
        private static List<SNPosition> PartiallyReconstructPath(SNGrid grid, SNPosition start, SNPosition end, SNPosition[] cameFrom)
        {
            var path = new List<SNPosition> { end };

#if DEBUG          
            var current = end;
            do
            {
                var previous = cameFrom[grid.GetIndexUnchecked(current.X, current.Y)];

                // If the path is invalid, probably becase we've not closed
                // a node yet, return an empty list
                if (current == previous)
                    return new List<SNPosition>();

                current = previous;
                path.Add(current);
            } while (current != start);

#endif
            return path;
        }
    }

    internal class Step
    {
        public Step(StepType type, SNPosition position, IReadOnlyList<SNPosition> path)
        {
            this.Type = type;
            this.Position = position;
            this.Path = path;
        }

        public StepType Type { get; }
        public SNPosition Position { get; }
        public IReadOnlyList<SNPosition> Path { get; }
    }

    internal enum StepType
    {
        Current,
        Open,
        Close
    }
}
