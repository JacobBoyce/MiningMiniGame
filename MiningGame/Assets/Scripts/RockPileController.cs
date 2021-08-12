using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockPileController : MonoBehaviour
{
    public List<GameObject> rocks;
    public List<GameObject> rocksToBlowup;
    public ParticleSystem expl;
    
    public void TurnOnRocks(int score)
    {
        StartCoroutine("StartExpl");
        if(score == 2)
        {
            rocksToBlowup = rocks;
        }
        else if(score == 1)
        {
            for(int i = 0; i < rocks.Count; i++)
            {
                rocksToBlowup.Add(rocks[i]);
                i++;
            }
        }

        for(int i = 0; i < rocksToBlowup.Count; i++)
        {
            rocksToBlowup[i].GetComponent<Rigidbody>().isKinematic = false;
            rocksToBlowup[i].GetComponent<Rigidbody>().AddForce(new Vector3(0,10,10));
            //get script on each rock that destroys or shrinks themselves
        }
        DestroyRocks();
    }

    public void DestroyRocks()
    {
        for(int i = 0; i < rocksToBlowup.Count; i++)
        {
            rocksToBlowup[i].transform.localScale = Vector3.Lerp(rocksToBlowup[i].transform.localScale, new Vector3(0, 0, 0), Time.deltaTime*2f);
            //get script on each rock that destroys or shrinks themselves
        }
    }

    public IEnumerator StartExpl()
    {
        expl.Play();
        yield return new WaitForSeconds(1.2f);
        expl.Stop();
        for(int i = 0; i < rocksToBlowup.Count; i++)
        {
            Destroy(rocksToBlowup[i].gameObject, 2f);
            //get script on each rock that destroys or shrinks themselves
        }
    }
}
