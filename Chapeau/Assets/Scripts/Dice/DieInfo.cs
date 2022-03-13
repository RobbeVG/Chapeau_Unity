using UnityEngine;

namespace Seacore
{
    public class DieInfo
    {
        public Transform RolledTransform { get; set; }
        public Material DieMateriel { get; set; }
        public bool IsInside { get; set; }
        public bool IsPrimedToRoll { get; set; }
    }
}
