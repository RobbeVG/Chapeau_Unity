using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    /// <summary>
    /// The BootStrapper class will initialize the necesary systems before a scene is loaded and mark them as Do Not Destroy
    /// </summary>
    public static class BootStrapper 
    {
        //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        //public static void Execute() => Object.DontDestroyOnLoad(Object.Instantiate(Resources.Load("Systems")));
    }
}
