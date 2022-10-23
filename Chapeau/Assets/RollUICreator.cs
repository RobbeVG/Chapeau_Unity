using System;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;


namespace Seacore
{

    public class RollUICreator : MonoBehaviour
    {
        [SerializeField]
        private Roll declaredRoll;

        private Dropdown[] _dropDowns;

        private void Awake()
        {
            _dropDowns = GetComponentsInChildren<Dropdown>();

            Assert.IsTrue(_dropDowns.Length == Globals.c_amountDie, $"Amount of dropdowns = {_dropDowns.Length}, amount of rolled dice = {Globals.c_amountDie}");
            Assert.IsNotNull(declaredRoll);
        }

        void OnEnable()
        {
            for (int i = 0; i < _dropDowns.Length; i++)
            {
                Dropdown dropdown = _dropDowns[i];
                dropdown.value = (int)declaredRoll.Values[i];
                dropdown.onValueChanged.AddListener(delegate { UpdateDeclaredRoll(dropdown); });
            }
        }

        private void OnDisable()
        {
            foreach (var dropdown in _dropDowns)
                dropdown.onValueChanged.RemoveListener(delegate { UpdateDeclaredRoll(dropdown); });
        }

        void Start()
        {
            if (declaredRoll == null)
                Debug.LogError("No declared roll assigned");
        }

        private void UpdateDeclaredRoll(Dropdown dropdown)
        {
            declaredRoll.ChangeValue(Array.IndexOf(_dropDowns, dropdown), (Die.Faces)dropdown.value);     
            declaredRoll.CalculateResult(); //Calculate new result
        }
    }
}
