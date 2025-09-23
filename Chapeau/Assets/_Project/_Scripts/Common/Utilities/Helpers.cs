using System;
using System.Collections;
using UnityEngine;

namespace Seacore.Common
{
    public static class Helpers
    {
        /// <summary>
        /// Executes the specified action on the next frame.
        /// </summary>
        /// <param name="func">The action to execute. This parameter cannot be null.</param>
        /// <returns>An enumerator that can be used to wait until the next frame before executing the action.</returns>
        static IEnumerator NextFrame(Action func)
        {
            yield return null;
            func();
        }

        static IEnumerator WaitForSeconds(Action func, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            func();
        }


    }
}
