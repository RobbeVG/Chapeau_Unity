using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class UIManager : MonoBehaviour
    {
        public GameObject background;
        public GameObject lookButton;
        public GameObject declareButton;
        public GameObject chapeauButton;
        public GameObject declaringRollDropdowns;
        public GameObject confirmingRollButton;

        private void Awake()
        {
            background?.SetActive(false);
            lookButton?.SetActive(false);
            declareButton?.SetActive(false);
            chapeauButton?.SetActive(false);
            declaringRollDropdowns?.SetActive(false);
            confirmingRollButton?.SetActive(false);
        }
    }
}
