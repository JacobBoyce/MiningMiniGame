using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDetected : MonoBehaviour
{
    public GameObject particleSys;
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
        if(other.tag == "PlayerSearchRadius")
        {
            particleSys.GetComponent<ParticleSystem>().Play();
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if(other.tag == "PlayerSearchRadius")
        {
            particleSys.GetComponent<ParticleSystem>().Stop();
        }
    }
}
