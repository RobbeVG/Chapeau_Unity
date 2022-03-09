using UnityEngine.Assertions;

namespace Seacore
{
    public class Roll
    {
        public const ushort c_amountDie = 5;

        public Die.DieValues[] Values { get; private set; }
        public float Score { get; private set; } = 0;

        public Roll()
        {
            Values = new Die.DieValues[c_amountDie] { Die.DieValues.None, Die.DieValues.None, Die.DieValues.None, Die.DieValues.None, Die.DieValues.None };
        }

        public Roll(Die.DieValues[] values)
        {
            Assert.IsNotNull(values);
            Assert.IsTrue(values.Length == c_amountDie, $"Values given in constructor have lenght {values.Length} while max-length is {c_amountDie}");
            
            Values = values;
            Sort();
        }

        public void ChangeValue(int index, Die.DieValues value)
        {
            Assert.IsTrue(index < c_amountDie, $"Changed roll value at index {index} while length is {c_amountDie}");
            Values[index] = value;
        }

        public void Sort()
        {
            System.Array.Sort(Values);            
        }

        public void CalculateScore()
        {


            //Calculate score
        }
    }
}
