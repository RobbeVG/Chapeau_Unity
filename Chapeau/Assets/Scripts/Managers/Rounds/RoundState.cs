using System;
using UnityEngine;

namespace Seacore
{
    public abstract class RoundState
    {
        public abstract void Enter(RoundManager rm);

        public abstract void Update(RoundManager rm);

        public abstract void Exit(RoundManager rm);
    }
}
