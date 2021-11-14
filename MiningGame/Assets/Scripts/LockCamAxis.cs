using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockCamAxis : MonoBehaviour
{
    private float _initialYPosition;
    
    private void Start()
    {
        _initialYPosition = transform.position.x;
    }

    private void Update()
    {

        //var position = transform.position;
        //position.x = _initialYPosition;
        transform.position = Vector3.zero;
    }
}
