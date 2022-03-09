using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    [RequireComponent(typeof(Rigidbody))]
    public class Die : MonoBehaviour
    {
        public enum DieValues { None, Nine, Ten, Jack, Queen, King, Ace }
        public static readonly Vector3[] s_directions = { Vector3.up, Vector3.right, Vector3.forward, Vector3.down, Vector3.left, Vector3.back }; //All the directions of the die's faces (Custom editor also uses this)

        public delegate void Roll(Die die);
        public static event Roll OnRoll;

        [SerializeField]
        private DieValues[] faces = new DieValues[s_directions.Length]; //Coresponding faces to s_directions
        [SerializeField]
        private float sleepThreshold = 0.005f; // Default value of sleep threshold

        public DieValues RolledValue { get; private set; } = DieValues.None;
        private Rigidbody _rigidbody = null;


        private void Start()
        {
            transform.rotation = Random.rotation;

            _rigidbody = GetComponent<Rigidbody>();
            _rigidbody.sleepThreshold = sleepThreshold;
            _rigidbody.AddTorque(Random.insideUnitSphere * 10);
        }

        private void Update()
        {
            if (_rigidbody.IsSleeping() && RolledValue == DieValues.None)
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

                if (OnRoll != null) //Check no subscribers
                    OnRoll(this);
            }
        }

        //public void Roll(Vector3 force, Vector3 torque)
        //{
        //    _rigidbody.AddForce(force);
        //    _rigidbody.AddTorque(torque);

        //    RolledValue = DieValues.None;
        //}
    }
}