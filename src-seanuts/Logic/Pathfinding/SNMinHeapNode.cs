namespace Seanuts.Logic.PathFinding
{
    /// <summary>
    /// Node in a heap
    /// </summary>
    internal sealed class SNMinHeapNode
    {        
        public SNMinHeapNode(SNPosition position, float expectedCost)
        {
            this.Position     = position;
            this.ExpectedCost = expectedCost;            
        }

        public SNPosition Position { get; }
        public float ExpectedCost { get; set; }                
        public SNMinHeapNode Next { get; set; }
    }
}
