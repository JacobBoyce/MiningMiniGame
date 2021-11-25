using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OreDetected : MonoBehaviour
{
    public enum OreTypes
    {
        NONE,
        COPPER,
        IRON, 
        COAL
    }

    public GameObject particleSys;

    public OreTypes oreT;

    // Start is called before the first frame update
    void Start()
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
