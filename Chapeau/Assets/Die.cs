using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Die : MonoBehaviour
{
    public enum DieValue { None, Nine, Ten, Jack, Queen, King, Ace }
    public static readonly Vector3[] s_directions = { Vector3.up, Vector3.right, Vector3.forward, Vector3.down, Vector3.left, Vector3.back }; 

    [SerializeField]
    private DieValue[] faces = new DieValue[s_directions.Length];

    [SerializeField]
    private float sleepThreshold = 0.005f; // Default value of sleep threshold
    private Rigidbody _rigidbody = null;

    public DieValue Value { get; private set; } = DieValue.None;


    // Start is called before the first frame update
    void Start()
    {
        transform.rotation = Random.rotation;   

        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.sleepThreshold = sleepThreshold;
        _rigidbody.AddTorque(Random.insideUnitSphere * 10);
    }
    

    private void Update()
    {
        if (_rigidbody.IsSleeping() && Value == DieValue.None)
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

            Value = faces[face];
            Debug.Log(Value);
        }
        // If rb awake? -> Reset die value
    }

    private void OnDrawGizmos()
    {
        
    }
}
