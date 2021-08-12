using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreCollisionChecker : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ore")
        {
            other.gameObject.SetActive(true);
            Debug.Log("Go to the rock!");
        }
    }
}
