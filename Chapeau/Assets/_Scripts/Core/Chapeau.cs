using System.Collections;

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    public class Chapeau : MonoBehaviour
    {
        [SerializeField]
        Animator liftingAnimationController = null;

        [SerializeField]
        Material material = null;

        [SerializeField]
        [Range(0.0f, 1.0f)]
        float seethroughOpacity = 0.5f;

        public void OnValidate()
        {
            if (material != null)
            {
                Color current = material.color;
                current.a = seethroughOpacity;
                material.color = current;
            }
        }

        /// <summary>
        /// Setting the transparacy of the material being used
        /// </summary>
        /// <param name="value">0 is transparent, 1 is opaque</param>
        public void SetTransparancy(float value) 
        {
            Assert.IsTrue(value >= 0.0f && value <= 1.0f);
            if (material != null)
            {
                Color current = material.color;
                current.a = value;
                material.color = current;
            }
        }

        private void Start()
        {
            Assert.IsNotNull(liftingAnimationController, $"Chapeau script does not have animation Controller");
        }

        private bool IsClosed()
        {
            return liftingAnimationController.GetCurrentAnimatorStateInfo(0).IsName("Closed");
        }
        private bool IsOpened()
        {
            return !IsClosed();
        }

        public IEnumerator Open()
        {
            if (!IsOpened())
            {
                liftingAnimationController.SetTrigger("Open");
                do
                {
                    yield return null;
                }
                while (!IsOpened());
            }
        }
        public IEnumerator Close()
        {
            if (!IsClosed())
            {
                liftingAnimationController.SetTrigger("Close");
                do
                {
                    yield return null;
                }
                while (!IsClosed());
            }
        }
        public IEnumerator Lift()
        {
            if (!IsClosed())
            {
                liftingAnimationController.SetTrigger("Rise");
                do
                {
                    yield return null;
                }
                while (!IsClosed());
            }
        }
    }
}
