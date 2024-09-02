using Seacore;
using UnityEngine;



public class RoundContext
{
    public Roll CurrentRoll { get; }
    public Roll DeclaredRoll { get; }
    public Roll PhysicalRoll { get; }
    public int AmountRolled { get; private set; }

    public RoundContext(Roll current, Roll declare, Roll physical)
    {
        CurrentRoll = current;
        DeclaredRoll = declare;
        PhysicalRoll = physical;
    }

    private void Reset()
    {
        Clear();
    }

    public void IncrementAmountRolled()
    {
        AmountRolled++;
    }

    public void Clear()
    {
        CurrentRoll.Clear();
        DeclaredRoll.Clear();
        PhysicalRoll.Clear();
        AmountRolled = 0;
    }
}
