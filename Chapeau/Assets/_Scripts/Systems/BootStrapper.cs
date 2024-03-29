using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public static class BootStrapper 
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
    }
}
