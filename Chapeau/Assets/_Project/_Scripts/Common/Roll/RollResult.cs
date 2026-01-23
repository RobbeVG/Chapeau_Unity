namespace Seacore.Game
{
    public enum RollType { Nothing, Pair, TwoPair, ThreeOfAKind, LowStraight, FullHouse, HighStraight, FourOfAKind, FiveOfAKind }

    [System.Serializable]
    public struct RollResult
    {
        public RollResult(RollType type, float score)
        {
            this.type = type;
            this.score = score;
        }

        public RollType Type { get => type; }
        public float Score { get => score; }

        [ReadOnly][UnityEngine.SerializeField]
        private RollType type;
        [ReadOnly][UnityEngine.SerializeField]
        private float score;

        public void Clear()
        {
            type = RollType.Nothing;
            score = 0;
        }

        public override string ToString()
        {
            return $"({Type} with a score of: {Score})";
        }
    }
}
