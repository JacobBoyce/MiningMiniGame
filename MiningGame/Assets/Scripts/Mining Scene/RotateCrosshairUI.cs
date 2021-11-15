using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCrosshairUI : MonoBehaviour
{
    public Vector3 spinVector;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.GetComponent<RectTransform>().Rotate(spinVector*(Time.deltaTime / Time.timeScale));
    }
}
