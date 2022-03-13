
using UnityEngine;
using UnityEngine.Assertions;
using TMPro;


namespace Seacore
{
    public class RollUICreator : MonoBehaviour
    {
        [SerializeField]
        private Roll declaredRoll;

        private TMP_Dropdown[] _dropDowns = null;


        void Awake() 
        {
            _dropDowns = GetComponentsInChildren<TMP_Dropdown>();
            Assert.IsTrue(_dropDowns.Length == Roll.c_amountDie, $"{_dropDowns.Length}");
        }

        void Start()
        {
            if (declaredRoll == null)
                Debug.LogError("No declared roll assigned");
        }

        public void UpdateDeclaredRoll()
        {
            for (int i = 0; i < _dropDowns.Length; i++)
            {
                TMP_Dropdown dropdown = _dropDowns[i];

                declaredRoll.ChangeValue(i, (Die.Faces)dropdown.value);
            }
        }
    }
}
