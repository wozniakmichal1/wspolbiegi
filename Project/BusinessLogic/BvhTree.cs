using Data;

namespace BusinessLogic
{
    internal record AABB(double MinX, double MinY, double MaxX, double MaxY)
    {
        internal bool Intersects(AABB other) =>
            MinX < other.MaxX && MaxX > other.MinX &&
            MinY < other.MaxY && MaxY > other.MinY;
    }

    internal class BVHNode
    {
        internal AABB Bounds { get; }
        internal IBall? Ball { get; }
        internal BVHNode? Left { get; }
        internal BVHNode? Right { get; }
        internal bool IsLeaf => Ball != null;

        internal BVHNode(IBall ball)
        {
            Ball = ball;
            Bounds = ToAABB(ball);
        }

        internal BVHNode(BVHNode left, BVHNode right)
        {
            Left = left;
            Right = right;
            Bounds = Merge(left.Bounds, right.Bounds);
        }

        private static AABB ToAABB(IBall b) =>
            new(b.Position.X, b.Position.Y,
                b.Position.X + b.Diameter, b.Position.Y + b.Diameter);

        private static AABB Merge(AABB a, AABB b) =>
            new(Math.Min(a.MinX, b.MinX), Math.Min(a.MinY, b.MinY),
                Math.Max(a.MaxX, b.MaxX), Math.Max(a.MaxY, b.MaxY));

        internal void CollectCandidatePairs(List<(IBall, IBall)> pairs)
        {
            if (IsLeaf) return;
            Left!.CollectPairsWith(Right!, pairs);
            Left.CollectCandidatePairs(pairs);
            Right!.CollectCandidatePairs(pairs);
        }

        private void CollectPairsWith(BVHNode other, List<(IBall, IBall)> pairs)
        {
            if (!Bounds.Intersects(other.Bounds)) return;

            if (IsLeaf && other.IsLeaf) { pairs.Add((Ball!, other.Ball!)); return; }

            if (IsLeaf)
            {
                CollectPairsWith(other.Left!, pairs);
                CollectPairsWith(other.Right!, pairs);
            }
            else
            {
                Left!.CollectPairsWith(other, pairs);
                Right!.CollectPairsWith(other, pairs);
            }
        }
    }

    internal static class BVHTree
    {
        internal static BVHNode? Build(List<IBall> balls)
        {
            if (balls.Count == 0) return null;
            if (balls.Count == 1) return new BVHNode(balls[0]);
            var sorted = balls.OrderBy(b => b.Position.X + b.Diameter / 2).ToList();
            int mid = sorted.Count / 2;
            return new BVHNode(Build(sorted.Take(mid).ToList())!,
                               Build(sorted.Skip(mid).ToList())!);
        }
    }
}