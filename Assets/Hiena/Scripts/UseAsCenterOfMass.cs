using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAsCenterOfMass : MonoBehaviour
{

    private Rigidbody body;

    // Start is called before the first frame update
    void Start()
    {
        body = GetComponentInParent<Rigidbody>();

        
    }

    private void Update()
    {
        body.centerOfMass = transform.localPosition;
    }
}
