using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPileController : MonoBehaviour
{
    public List<GameObject> rocks;
    public ParticleSystem expl;
    
    public void TurnOnRocks()
    {
        StartCoroutine("StartExpl");
        for(int i = 0; i < rocks.Count; i++)
        {
            rocks[i].GetComponent<Rigidbody>().isKinematic = false;
            rocks[i].GetComponent<Rigidbody>().AddForce(new Vector3(0,10,10));
            //get script on each rock that destroys or shrinks themselves
        }
        DestroyRocks();
    }

    public void DestroyRocks()
    {
        for(int i = 0; i < rocks.Count; i++)
        {
            rocks[i].transform.localScale = Vector3.Lerp(rocks[i].transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*2f);
            //get script on each rock that destroys or shrinks themselves
        }
    }

    public IEnumerator StartExpl()
    {
        expl.Play();
        yield return new WaitForSeconds(1.2f);
        expl.Stop();
        for(int i = 0; i < rocks.Count; i++)
        {
            Destroy(rocks[i].gameObject, 2f);
            //get script on each rock that destroys or shrinks themselves
        }
    }
}
