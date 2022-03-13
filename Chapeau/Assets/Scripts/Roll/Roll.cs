using UnityEngine.Assertions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Seacore
{
    [CreateAssetMenu(fileName = "Roll", menuName = "ScriptableObjects/Roll")]
    public class Roll : ScriptableObject
    {
        public const ushort c_amountDie = 5;
        [SerializeField] 
        private Die.Faces[] values;
        //[ReadOnly]
        [SerializeField]
        private RollResult result;

        public Die.Faces[] Values { get => values; private set => values = value; }
        public RollResult Result { get => result; private set => result = value; }

        public Roll()
        {
            Clear();
        }

        public Roll(Die.Faces[] values, bool calculateResult = false)
        {
            Assert.IsNotNull(values);
            Assert.IsTrue(values.Length == c_amountDie, $"Values given in constructor have lenght {values.Length} while max-length is {c_amountDie}");

            Values = values;
            if (calculateResult)
                CalculateResult();
        }

        public void Clear()
        {
            Values = new Die.Faces[c_amountDie] { Die.Faces.None, Die.Faces.None, Die.Faces.None, Die.Faces.None, Die.Faces.None };
        }

        public void ChangeValue(int index, Die.Faces value)
        {
            Assert.IsTrue(index < c_amountDie, $"Changed roll value at index {index} while length is {c_amountDie}");
            Values[index] = value;
        }

        public void Sort()
        {
            System.Array.Sort(Values);
        }

        public void CalculateResult()
        {
            Result = GetRollResult();
        }

        //Caclulating the throw
        private RollResult GetRollResult()
        {
            Assert.IsFalse(Values.Length > c_amountDie, $"Roll calculater get combination given value array was of lenght {Values.Length} while it should be smaller than {c_amountDie}");

            Sort();

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
    }
}
