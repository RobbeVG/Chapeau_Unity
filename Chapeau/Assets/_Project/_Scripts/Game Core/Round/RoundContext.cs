using Seacore;
using UnityEngine;

namespace Seacore.Game
{
    /// <summary>
    /// Defines a contract for accessing roll information within a round, including the current, declared, and physical
    /// rolls.
    /// </summary>
    /// <remarks>Implementations of this interface provide access to different representations of a roll in a
    /// round. The meaning of each roll may vary depending on the game or domain context. Typically, the current roll
    /// reflects the roll in play, the declared roll represents a roll that has been announced or chosen, and the
    /// physical roll corresponds to the actual roll result.</remarks>
    public interface IRoundRolls
    {
        Roll CurrentRoll { get; }
        Roll DeclaredRoll { get; }
        Roll PhysicalRoll { get; }
    }


    public class RoundContext : IRoundRolls
    {
        public Roll CurrentRoll { get; }
        public Roll DeclaredRoll { get; }
        public Roll PhysicalRoll { get; }
        public int PlayerTotal {  get; set; }
        public int AmountRolled { get; private set; }

        public RoundContext(Roll current, Roll declare, Roll physical)
        {
            CurrentRoll = current;
            DeclaredRoll = declare;
            PhysicalRoll = physical;
        }

        public void ListenToDiceRollEvents(DiceController diceController)
        {
            diceController.OnAllDiceRolled += IncrementAmountRolled;
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
}
