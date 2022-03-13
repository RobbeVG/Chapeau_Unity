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
            Debug.Log("Started Task Open");
            if (!IsOpened())
            {
                liftingAnimationController.SetTrigger("Open");
                do
                {
                    yield return null;
                }
                while (!IsOpened());
            }
            Debug.Log("Finished Task Open");
        }
        public IEnumerator Close()
        {
            Debug.Log("Started Task Close");
            if (!IsClosed())
            {
                liftingAnimationController.SetTrigger("Close");
                do
                {
                    yield return null;
                }
                while (!IsClosed());
            }
            Debug.Log("Finished Task Close");
        }
    }
}
