namespace Seacore
{
    public enum RollType { Nothing, Pair, TwoPair, ThreeOfAKind, LowStraight, FullHouse, HighStraight, FourOfAKind, FiveOfAKind }

    public struct RollResult
    {
        public RollResult(RollType type, float score)
        {
            Type = type;
            Score = score;
        }

        public RollType Type { get; }
        public float Score { get; }

        public override string ToString()
        {
            return $"({Type} with a score of: {Score})";
        }
    }
}
