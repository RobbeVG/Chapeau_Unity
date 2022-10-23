using UnityEngine;
using UnityEngine.Assertions;

namespace Seacore
{
    [RequireComponent(typeof(Rigidbody))]
    public class Die : MonoBehaviour
    {
        public enum Faces { None, Nine, Ten, Jack, Queen, King, Ace }
        public static readonly Vector3[] s_directions = { Vector3.up, Vector3.right, Vector3.forward, Vector3.down, Vector3.left, Vector3.back }; //All the directions of the die's faces (Custom editor also uses this)

        public delegate void Roll(Die die);
        public static event Roll OnRoll;
        public static event Roll OnUnroll;

        [ReadOnly][SerializeField]
        private Faces _rolledValue = Faces.None;
        public Faces RolledValue { get { return _rolledValue; } private set { _rolledValue = value; } }

        [ReadOnly]
        [SerializeField]
        private Vector3 upvec;

        //public Faces RolledValue { 
        //    get 
        //    { 
        //        return _rolledValue; 
        //    } 
        //    private set 
        //    { 
        //        if (_rolledValue != value)
        //        {
        //            _rolledValue = value;
        //            if (_rolledValue != Faces.None)
        //                OnRoll?.Invoke(this);
        //            else
        //                OnUnroll?.Invoke(this);
        //        }
        //    }
        //}
        //public bool Rolled { get => _rolledValue != Faces.None; }

        [SerializeField]
        private Faces[] faces = new Faces[s_directions.Length]; //Coresponding faces to s_directions

        //private Transform sleepTransform =

        private Rigidbody _rigidbody = null;
        public bool Kinematic { get { return _rigidbody.isKinematic; } set { _rigidbody.isKinematic = value; } }

        #region Event functions
        //Initialization
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
                Log.Warning("No rigidbody found on dice");
        }

        //Updating
        private void Update()
        {
            ////Log.Debug($"{name} => sleep: {_rigidbody.IsSleeping()}");
            //if (_rigidbody.IsSleeping())
            //{
            //    if (RolledValue == Faces.None)
            //    {
            //        float highestDot = -1.0f;
            //        int face = 0;
            //        for (int i = 0; i < s_directions.Length; i++)
            //        {
            //            Vector3 direction = s_directions[i];
            //            Vector3 worldSpaceDirection = transform.localToWorldMatrix.MultiplyVector(direction);
            //            float dot = Vector3.Dot(worldSpaceDirection, Vector3.up);
            //            if (dot > highestDot)
            //            {
            //                highestDot = dot;
            //                face = i;
            //            }
            //        }
            //        RolledValue = faces[face]; //Invokes -> On Sleep
            //    }
            //}
            //else
            //    RolledValue = Faces.None; //Invokes -> On Awake
        }
        #endregion

        

        public void SetRolledValue(Faces face)
        {
            if (face != Faces.None)
            {
                RolledValue = face;


                for (int i = 0; i < faces.Length; i++)
                {
                    if (faces[i] != face)
                        continue;
                    transform.rotation = Quaternion.FromToRotation(s_directions[i], Vector3.up);
                    
                    break;
                }
            }
        }

        public void Throw(Vector3 force, Vector3 torque)
        {
            _rigidbody.AddForce(force);
            _rigidbody.AddTorque(torque);

            RolledValue = Faces.None;
        }

        public void TriggerSleep()
        {
            _rigidbody.Sleep();
        }
    }
}