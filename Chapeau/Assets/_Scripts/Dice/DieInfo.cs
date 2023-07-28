using System;
using UnityEngine;

namespace Seacore
{
    public class DieInfo
    {
        //public Material DieMateriel { get; set; }
        public bool IsOutside { get; set; } = false;
        public Outline Outline { get; set; }
        public Vector3 RolledPosition{ get; set; }
        public int Index { get; set; }
    }
}
