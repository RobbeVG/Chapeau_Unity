using Seacore.Common;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

namespace Seacore.Game.UI
{
    public class DeclareMenu : MonoBehaviour
    {
        [SerializeField]
        IDieValueGetter<Die.Faces>[] selectors;

        [SerializeField]
        Roll _declaredRoll = null;

        public event Action OnEditDeclareRoll;

        private void Awake()
        {
            if (_declaredRoll == null)
                _declaredRoll = Resources.Load<Roll>("Rolls/DeclaredRoll");
            Assert.IsNotNull(_declaredRoll, "Declared Roll in the Declare Menu cannot be null");

            selectors = gameObject.GetComponentsInChildren<IDieValueGetter<Die.Faces>>(true);
            if (selectors.Length != Globals.c_amountDie)
            {
                Debug.LogError($"Declared Roll selectors length {selectors.Length} does not match Globals.c_amountDie {Globals.c_amountDie}");
                return;
            }
        }

        private void OnEnable()
        {
            foreach (IDieValueGetter<Die.Faces> selector in selectors)
                selector.OnEditValue += AdjustDeclaredRoll;
        }

        private void OnDisable()
        {
            foreach (IDieValueGetter<Die.Faces> selector in selectors)
                selector.OnEditValue -= AdjustDeclaredRoll;
        }

        public void AdjustDeclaredRoll(IDieValueGetter<Die.Faces> obj)
        {
            int index = Array.IndexOf(selectors, obj);
            _declaredRoll.ChangeValue(index, obj.Value);
            _declaredRoll.CalculateResult();
            OnEditDeclareRoll?.Invoke();
        } 
    }
}
