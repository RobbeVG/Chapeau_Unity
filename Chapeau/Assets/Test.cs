using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seacore
{
    public class Test : MonoBehaviour
    {
        float after = 2.0f;
        float elapsed = 0.0f;

        Rigidbody rb;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        void Update()
        {
            elapsed += Time.deltaTime;
            if (elapsed > after)
            {
                rb.isKinematic = true;
                transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 0.01f);
            }
            Debug.Log($"Sleep: { rb.IsSleeping() }" );
        }
    }
}
