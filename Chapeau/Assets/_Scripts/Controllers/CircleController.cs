using UnityEngine;

namespace Seacore
{
    public class CircleController : MonoBehaviour
    {
        [SerializeField] private Material material;
        [SerializeField][ReadOnly] private Vector3 _position;
        [SerializeField][ReadOnly] private float _radius;

        public Vector3 Position { get => _position; set {
                if (value != _position) 
                {
                    _position = value;
                    material.SetVector("_C_Position", _position); //Data to GPU
                }; 
            }
        }
        public float Radius { get => _radius; set { 
                if (value != _radius) 
                {
                    _radius = value;
                    material.SetFloat("_C_Radius", _radius); //Data to GPU
                }; 
            } 
        }

        private void Start()
        {
            if (material == null)
            {
                enabled = false;
                Debug.LogError("Material is not assigned.");
                return;
            }

            GetPropertiesMaterial();
        }

        private void GetPropertiesMaterial()
        {
            // Fetch initial values from the material
            _position = material.GetVector("_C_Position");
            _radius = material.GetFloat("_C_Radius");
        }

        public bool IsPositionInCircle(Vector3 position)
        {
            return (Position - position).sqrMagnitude <= Radius * Radius;
        }
    }
}
//10.44
//8.38