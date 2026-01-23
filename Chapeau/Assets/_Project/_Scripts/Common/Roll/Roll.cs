using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;

using Seacore.Common;

namespace Seacore.Game
{   
    [CreateAssetMenu(fileName = "Roll", menuName = "ScriptableObjects/Roll")]
    [Serializable]
    public class Roll : ScriptableObject
    {
        [SerializeField] 
        private Die.Faces[] values;
        //[ReadOnly]
        [SerializeField]
        private RollResult result;

        public Die.Faces[] Values { get => values; }
        public RollResult Result { get => result; }

        public Roll()
        {
            Clear();
        }
        public Roll(Die.Faces[] values)
        {
            Assert.IsNotNull(values);
            Assert.IsTrue(values.Length == Globals.c_amountDie, $"Values given in constructor have lenght {values.Length} while max-length is {Globals.c_amountDie}");
            this.values = values;
        }

        #region Equality Operators/Methods override
        public static bool operator <= (Roll r1, Roll r2)
        {
            return r1.result.Score <= r2.result.Score;
        }
        public static bool operator >= (Roll r1, Roll r2)
        {
            return r1.result.Score >= r2.result.Score;
        }
        public static bool operator == (Roll r1, Roll r2)
        {
            if (r1 is Roll && r2 is Roll)
                return r1.result.Score == r2.result.Score;
            return ReferenceEquals(r1, r2);
        }
        public static bool operator != (Roll r1, Roll r2)
        {
            return !(r1 == r2);
        }
        public static bool operator < (Roll r1, Roll r2)
        {
            return !(r1 >= r2);
        }
        public static bool operator > (Roll r1, Roll r2)
        {
            return !(r1 <= r2);
        }
        //Generated
        public override bool Equals(object obj)
        {
            return obj is Roll roll &&
                   base.Equals(obj) &&
                   name == roll.name &&
                   hideFlags == roll.hideFlags &&
                   EqualityComparer<Die.Faces[]>.Default.Equals(values, roll.values) &&
                   EqualityComparer<RollResult>.Default.Equals(result, roll.result);
        }
        public override int GetHashCode()
        {
            int hashCode = -1435252889;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(name);
            hashCode = hashCode * -1521134295 + hideFlags.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<Die.Faces[]>.Default.GetHashCode(values);
            hashCode = hashCode * -1521134295 + result.GetHashCode();
            return hashCode;
        }
        #endregion Equality Operators/Methods override

        public void Clear()
        {
            values = Enumerable.Repeat(Die.Faces.None, Globals.c_amountDie).ToArray();
            result.Clear();
        }

        public void ChangeValue(int index, Die.Faces value)
        {
            Assert.IsTrue(index < Globals.c_amountDie, $"Changed roll value at index {index} while length is {Globals.c_amountDie}");
            values[index] = value;
        }

        public void ChangeValue(int index, int value)
        {
            ChangeValue(index, (Die.Faces)value);
        }

        public void ChangeValueTo(Roll other)
        {
            other.values.CopyTo(values, 0);
            result = other.result;
        }

        public void Sort()
        {
            System.Array.Sort(Values);
        }

        public void CalculateResult()
        {
            result = GetRollResult();
        }

        public bool IsEmpty()
        {
            return Values.All(value => value == Die.Faces.None);
        }

        //Caclulating the throw
        private RollResult GetRollResult()
        {
            Assert.IsFalse(Values.Length > Globals.c_amountDie, $"Roll calculater get combination given value array was of lenght {Values.Length} while it should be smaller than {Globals.c_amountDie}");

            Dictionary<ushort, List<Die.Faces>> countValueFaces = new Dictionary<ushort, List<Die.Faces>>()
                {
                    { 1, new List<Die.Faces>() },
                    { 2, new List<Die.Faces>() },
                    { 3, new List<Die.Faces>() },
                    { 4, new List<Die.Faces>() },
                    { 5, new List<Die.Faces>() }
                };

            foreach (Die.Faces face in Values.Distinct())
            {
                if (face == Die.Faces.None)
                    continue;

                countValueFaces[(ushort)Values.Count(item => item == face)].Add(face);
            }

            if (countValueFaces[5].Any())
                return new RollResult(RollType.FiveOfAKind, GetBaseScoreType(RollType.FiveOfAKind) + (int)countValueFaces[5].Single());
            if (countValueFaces[4].Any())
                return new RollResult(RollType.FourOfAKind, GetBaseScoreType(RollType.FourOfAKind) + (int)countValueFaces[4].Single() + GetScoreHighFace(countValueFaces[1].SingleOrDefault()));
            if (countValueFaces[3].Any())
            {
                if (countValueFaces[2].Any())
                    return new RollResult(RollType.FullHouse, GetBaseScoreType(RollType.FullHouse) + (int)countValueFaces[3].Single() + GetScoreHighFace(countValueFaces[2].SingleOrDefault()));
                else
                {
                    return new RollResult(RollType.ThreeOfAKind, GetBaseScoreType(RollType.ThreeOfAKind) + (int)countValueFaces[3].Single() + GetScoreHighFaces(countValueFaces[1]));
                }
            }
            if (countValueFaces[2].Any())
            {
                if (countValueFaces[2].Count() == 2)
                    return new RollResult(RollType.TwoPair, GetBaseScoreType(RollType.TwoPair) + (int)countValueFaces[2][0] + GetScoreHighFace(countValueFaces[2][1]) + GetScoreHighFace(countValueFaces[1].SingleOrDefault()) / 100.0f);
                else
                {
                    return new RollResult(RollType.Pair, GetBaseScoreType(RollType.Pair) + (int)countValueFaces[2].Single() + GetScoreHighFaces(countValueFaces[1]));
                }
            }

            //For sure only 1'
            if (countValueFaces[1].Contains(Die.Faces.Ten) && countValueFaces[1].Contains(Die.Faces.Jack) && countValueFaces[1].Contains(Die.Faces.Queen) && countValueFaces[1].Contains(Die.Faces.King))
            {
                if (countValueFaces[1].Contains(Die.Faces.Nine))
                    return new RollResult(RollType.LowStraight, GetBaseScoreType(RollType.LowStraight));
                else if (countValueFaces[1].Contains(Die.Faces.Ace))
                    return new RollResult(RollType.HighStraight, GetBaseScoreType(RollType.HighStraight));
            }

            return new RollResult(RollType.Nothing, GetScoreHighFaces(countValueFaces[1]));
        }

        private static float GetBaseScoreType(RollType type)
        {
            return 7.0f * (int)type;
        }

        private static float GetScoreHighFace(Die.Faces face)
        {
            return (int)face / 100.0f;
        }

        private static float GetScoreHighFaces(List<Die.Faces> faces)
        {
            float highCardsScore = 0.0f;
            foreach (Die.Faces face in faces)
            {
                highCardsScore += GetScoreHighFace(face);
            }
            return highCardsScore;
        }

        public override string ToString()
        {
            string temp = base.ToString() + '\n';
            for (int i = 0; i < Values.Length; i++)
            {
                Die.Faces face = Values[i];
                temp += i + ": " + face.ToString() + '\t';
            }
            temp += '\n' + "Result = " + result.ToString();
            return temp;

        }
    }
}
