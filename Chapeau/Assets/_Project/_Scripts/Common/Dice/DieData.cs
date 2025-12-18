using System;
using UnityEngine;

namespace Seacore.Common
{
    public interface IDieValueGetter<T>
    {
        public T Value { get; }

        public delegate void OnEditDeclareRollHandler(IDieValueGetter<T> obj);
        public event OnEditDeclareRollHandler OnEditValue;
    }

    interface IDieData<T>
    {
        public T Value { get; set; }
        public int Sides { get; }
    }

    [Serializable]
    public struct DieData : IDieData<int>
    {
        [SerializeField]
        private int value;
        public readonly int Sides { get; }
        public readonly bool NoneExclusive { get; }

        public int Value { 
            get => value; 
            set {
                int newval = value % (Sides + 1);
                this.value = newval < (NoneExclusive ? 1 : 0) ? Sides: newval;
            }
        }

        public DieData(int sides = 6, int initValue = 1, bool noneExclusive = false)
        {
            value = initValue % sides;
            Sides = sides;
            NoneExclusive = noneExclusive;
        }
    }

    [Serializable]
    public class TypedDieData<T> : IDieData<T> where T : Enum
    {
        private DieData _diceValue;
        public TypedDieData(T typedValue, bool noneExclusive = false)
        {
            int sides = Enum.GetValues(typeof(T)).Length - (noneExclusive? 0 : 1);
            _diceValue = new DieData(sides, (int)(object)typedValue, noneExclusive);
        }

        [field: SerializeField]
        public T Value { get => (T)(object)_diceValue.Value; set => _diceValue.Value = (int)(object)value; }
        public int Sides => _diceValue.Sides;
        
        public T Next { get { return (T)(object)++_diceValue.Value; } }
        public T Previous { get { return (T)(object)--_diceValue.Value; } }
    }
}
