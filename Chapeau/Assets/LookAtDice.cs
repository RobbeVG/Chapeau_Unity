using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtDice : MonoBehaviour
{
    [SerializeField]
    private GameObject dice = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.LookAt(dice.transform.position);
    }
}
