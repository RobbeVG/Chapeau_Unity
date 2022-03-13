using UnityEngine;

namespace Seacore
{
    [RequireComponent(typeof(Rigidbody))]
    public class Die : MonoBehaviour
    {
        public enum Faces { None, Nine, Ten, Jack, Queen, King, Ace }
        public static readonly Vector3[] s_directions = { Vector3.up, Vector3.right, Vector3.forward, Vector3.down, Vector3.left, Vector3.back }; //All the directions of the die's faces (Custom editor also uses this)

        public delegate void Sleep(Die die);
        public static event Sleep OnSleep;

        public delegate void WakeUp(Die die);
        public static event WakeUp OnAwake;

        private bool _sleep;
        public bool Sleeping { 
            get 
            { 
                return _sleep; 
            } 
            private set 
            { 
                _sleep = value; 
                if (_sleep) 
                    OnSleep?.Invoke(this);
                else 
                    OnAwake?.Invoke(this); 
            } 
        }

        [SerializeField]
        private Faces[] faces = new Faces[s_directions.Length]; //Coresponding faces to s_directions
        [SerializeField]
        private float sleepThreshold = 0.005f; // Default value of sleep threshold

        //private Transform sleepTransform =

        public Faces RolledValue { get; private set; } = Faces.None;
        private Rigidbody _rigidbody = null;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.sleepThreshold = sleepThreshold;
        }

        private void Update()
        {
            if (_rigidbody.IsSleeping())
            {
                if (RolledValue == Faces.None)
                {
                    float highestDot = -1.0f;
                    int face = 0;
                    for (int i = 0; i < s_directions.Length; i++)
                    {
                        Vector3 direction = s_directions[i];
                        Vector3 worldSpaceDirection = transform.localToWorldMatrix.MultiplyVector(direction);
                        float dot = Vector3.Dot(worldSpaceDirection, Vector3.up);
                        if (dot > highestDot)
                        {
                            highestDot = dot;
                            face = i;
                        }
                    }
                    RolledValue = faces[face];
                    Sleeping = true; //Invokes -> On Sleep
                }
            }
            else
            {
                if (RolledValue != Faces.None)
                {
                    RolledValue = Faces.None;
                    Sleeping = false; //Invokes -> On Awake
                }
            }
        }

        public void Throw(Vector3 force, Vector3 torque)
        {
            _rigidbody.AddForce(force);
            _rigidbody.AddTorque(torque);

            RolledValue = Faces.None;
        }
    }
}