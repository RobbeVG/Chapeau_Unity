using System;
using System.Collections;
using UnityEngine;
using UnityEngine.WSA;

namespace Seacore.Common
{
    public static class Helpers
    {
        /// <summary>
        /// Executes the specified action on the next frame.
        /// </summary>
        /// <param name="func">The action to execute. This parameter cannot be null.</param>
        /// <returns>An enumerator that can be used to wait until the next frame before executing the action.</returns>
        public static IEnumerator NextFrame(Action func)
        {
            yield return null;
            func();
        }

        public static IEnumerator WaitForSeconds(Action func, float seconds)
        {
            yield return new WaitForSeconds(seconds);
            func();
        }

        /// <summary>
        /// Returns the <see cref="GameObject"/> located at the specified screen position, if any, using a raycast
        /// against the provided layer mask.
        /// </summary>
        /// <remarks>Uses the current camera to cast a ray from the specified screen position. If multiple
        /// objects overlap at the position, the closest one is returned.</remarks>
        /// <param name="screenPos">The screen coordinates, in pixels, from which to cast the ray. Typically corresponds to the mouse or pointer
        /// position.</param>
        /// <param name="mask">A <see cref="LayerMask"/> that specifies which layers to include in the raycast. Only objects on these
        /// layers will be considered.</param>
        /// <returns>The <see cref="GameObject"/> hit by the raycast at the given screen position and layer mask; or <c>null</c>
        /// if no object is found.</returns>
        public static GameObject GetObjectFromScreen(Vector2 screenPos, LayerMask mask)
        {
            Ray ray = Camera.main.ScreenPointToRay(screenPos);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mask))
            {
                return hit.transform.gameObject;
            }
            return null;
        }
    }
}
