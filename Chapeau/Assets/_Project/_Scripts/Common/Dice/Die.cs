using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections.Generic;
using Seacore.Common;

namespace Seacore
{
    [RequireComponent(typeof(Rigidbody))]
    public class Die : MonoBehaviour
    {
        /// <summary>
        /// A die class that manages rolling of the dice aswell as the RolledValue,
        /// Use the OnRolled Action to retrieve this value.
        /// </summary>

        public enum Faces { None, Nine, Ten, Jack, Queen, King, Ace }
        public static readonly Dictionary<Faces, Vector3> s_facedirections = new Dictionary<Faces, Vector3>
        {
            { Faces.Nine, Vector3.right},
            { Faces.Ten, Vector3.down},
            { Faces.Jack, Vector3.back},
            { Faces.Queen, Vector3.forward},
            { Faces.King, Vector3.up},
            { Faces.Ace, Vector3.left}
        }; //All the directions of the die's faces
        public static readonly int s_numberOfFaces = Enum.GetValues(typeof(Faces)).Length - 1; //Number of faces excluding None

        public event Action<Die> OnRolledValue;

        private bool _isRolling = false; // Value to check if the die is still rolling

        //Ful property to show dieValue in the inspector
        [field: ReadOnly][field: SerializeField]
        private Faces _dieValue;
        public Faces DieValue { get => _dieValue; private set => _dieValue = value; }

        //public TypedDieData<Faces> TypedDieData { get; private set; } = new TypedDieData<Faces>(Faces.Nine);


        private Rigidbody _rigidbody = null;
        public Rigidbody Rigidbody { get => _rigidbody; private set => _rigidbody = value; }

        public bool Kinematic { get { return _rigidbody.isKinematic; } set { _rigidbody.isKinematic = value; } }

        #region Event functions
        //Initialization
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            if (_rigidbody == null)
                Debug.LogError("No rigidbody found on dice");
        }

        private void Start()
        {
            CalculateDieValue();

        }

        //Updating
        private void FixedUpdate()
        {
            //TODO put this logic in the die manager and make use of threading

            if (_isRolling)
            {
                CalculateDieValue();
                if (_rigidbody.IsSleeping())
                {
                    _isRolling = false;
                    OnRolledValue?.Invoke(this);
                }
            }
        }
        #endregion

        private void CalculateDieValue()
        {
            float highestDot = -1.0f;
            Faces face = Faces.None;
            foreach (KeyValuePair<Faces, Vector3> facedirectionPair in s_facedirections)
            {
                Vector3 worldSpaceDirection = transform.localToWorldMatrix.MultiplyVector(facedirectionPair.Value);
                float dot = Vector3.Dot(worldSpaceDirection, Vector3.up);
                if (dot > highestDot)
                {
                    highestDot = dot;
                    face = facedirectionPair.Key;
                }
            }
            DieValue = face;
        }

        public void SetRolledValue(Faces face)
        {
            Assert.AreNotEqual(face, Faces.None, "You cannot change the rolled value of dice to None. Make use of the Roll function instead");
            Assert.AreNotEqual(_isRolling, true, "Make sure the die isn't rolling to set the value. Make proper use of the event OnRolled");

            DieValue = face;
            transform.rotation = Quaternion.FromToRotation(s_facedirections[face], Vector3.up);
        }

        /// <summary>
        /// Manual roll function that has nothing todo with physics like the above
        /// </summary>
        public void Roll()
        {
            SetRolledValue((Faces)UnityEngine.Random.Range((int)Faces.Nine, (int)Faces.Ace) + 1);
            OnRolledValue?.Invoke(this);
        }

        public void Throw(Vector3 force, Vector3 torque)
        {
            _rigidbody.AddForce(force);
            _rigidbody.AddTorque(torque);
            _isRolling = true;
        }
    }
}