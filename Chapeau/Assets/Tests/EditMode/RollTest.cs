using NUnit.Framework;
using Seacore;
using System;
using System.Linq;
using System.Collections.Generic;

public class RollTest
{
    static IEnumerable<IEnumerable<T>>
        GetKCombsWithRept<T>(IEnumerable<T> list, int length) where T : IComparable
    {
        if (length == 1) return list.Select(t => new T[] { t });
        return GetKCombsWithRept(list, length - 1)
            .SelectMany(t => list.Where(o => o.CompareTo(t.Last()) >= 0),
                (t1, t2) => t1.Concat(new T[] { t2 }));
    }

    [Test]
    public void CheckDefaultConstructor()
    {
        Roll roll = new Roll();
        Assert.IsTrue(roll.Values.SequenceEqual(new Die.Faces[] { Die.Faces.None, Die.Faces.None, Die.Faces.None, Die.Faces.None, Die.Faces.None }));
    }

    [Test]
    public void CheckConstructor()
    {
        Roll roll = new Roll(new Die.Faces[] { Die.Faces.Ten, Die.Faces.Nine, Die.Faces.Jack, Die.Faces.King, Die.Faces.Ten });
        //No calculation result
        Assert.IsTrue(roll.Values.SequenceEqual(new Die.Faces[] { Die.Faces.Ten, Die.Faces.Nine, Die.Faces.Jack, Die.Faces.King, Die.Faces.Ten }));

        roll = new Roll(new Die.Faces[] { Die.Faces.Ten, Die.Faces.Nine, Die.Faces.Jack, Die.Faces.King, Die.Faces.Ten });
        roll.CalculateResult();
        Assert.IsTrue(roll.Result.Type == RollType.Pair); 
    }

    [Test]
    public void CheckClear()
    {
        Roll roll = new Roll(new Die.Faces[] { Die.Faces.Ten, Die.Faces.Nine, Die.Faces.Jack, Die.Faces.King, Die.Faces.Ten });
        roll.Clear();

        Assert.IsTrue(roll.Values.SequenceEqual(new Roll().Values));
    }

    [Test]
    public void CheckSort()
    {
        Roll roll = new Roll();
        roll.ChangeValue(0, Die.Faces.King);
        roll.ChangeValue(1, Die.Faces.Nine);
        roll.ChangeValue(2, Die.Faces.Ten);
        roll.ChangeValue(3, Die.Faces.Jack);
        roll.ChangeValue(4, Die.Faces.Ten);
        roll.Sort();

        Assert.IsTrue(roll.Values.SequenceEqual(new Die.Faces[] { Die.Faces.Nine, Die.Faces.Ten, Die.Faces.Ten, Die.Faces.Jack, Die.Faces.King }));
    }

    [Test]
    public void CheckAllCombinations()
    {
        //https://stackoverflow.com/questions/1952153/what-is-the-best-way-to-find-all-combinations-of-items-in-an-array
        IEnumerable<IEnumerable<Die.Faces>> allCombinations = GetKCombsWithRept(Enum.GetValues(typeof(Die.Faces)) as Die.Faces[], 5);
        Dictionary<RollType, ushort> combinationsCount = new Dictionary<RollType, ushort>();

        Assert.IsTrue(allCombinations.Count() == 462, $"Amount of all possible combinations should be 462 - Received value: {allCombinations.Count()}");

        //Generate amount count of all combination type's
        foreach (var typeCombination in Enum.GetValues(typeof(RollType)) as RollType[])
        {
            combinationsCount[typeCombination] = 0;
        }
        foreach (IEnumerable<Die.Faces> combination in allCombinations)
        {
            Roll roll = new Roll(combination.ToArray());
            roll.CalculateResult();
            combinationsCount[roll.Result.Type]++;
        }

        Assert.IsTrue(combinationsCount[RollType.FiveOfAKind] == 6, $"Of all combinations there should be six five of a kinds - Received value: {combinationsCount[RollType.FiveOfAKind]}");
        Assert.IsTrue(combinationsCount[RollType.FourOfAKind] == 36, $"Of all combinations there should be 36 four of a kinds - Received value: {combinationsCount[RollType.FourOfAKind]}");
        Assert.IsTrue(combinationsCount[RollType.HighStraight] == 1, $"Of all combinations there should be one high straight - Received value: {combinationsCount[RollType.HighStraight]}");
        Assert.IsTrue(combinationsCount[RollType.FullHouse] == 30, $"Of all combinations there should be 30 full houses - Received value: {combinationsCount[RollType.FiveOfAKind]}");
        Assert.IsTrue(combinationsCount[RollType.LowStraight] == 1, $"Of all combinations there should be one low straight - Received value: {combinationsCount[RollType.LowStraight]}");
        Assert.IsTrue(combinationsCount[RollType.ThreeOfAKind] == 96, $"Of all combinations there should 96 three of a kinds - Received value: {combinationsCount[RollType.ThreeOfAKind]}");
        Assert.IsTrue(combinationsCount[RollType.TwoPair] == 75, $"Of all combinations there should be 75 two pairs - Received value: {combinationsCount[RollType.TwoPair]}");
        Assert.IsTrue(combinationsCount[RollType.Pair] == 156, $"Of all combinations there should be 156 pairs - Received value: {combinationsCount[RollType.Pair]}");
        Assert.IsTrue(combinationsCount[RollType.Nothing] == 61, $"Of all combinations there should be 61 nothings - Received value: {combinationsCount[RollType.Nothing]}");
    }
}
