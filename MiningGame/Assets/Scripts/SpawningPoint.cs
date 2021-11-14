using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoint : MonoBehaviour
{
    public GameObject rockSpotPrefab, parentObj;
    private GameObject _instancedRockSpot;
    public bool isOff;
    private Vector3 range;
    void Start()
    {
        _instancedRockSpot = Instantiate(rockSpotPrefab, parentObj.transform);
        _instancedRockSpot.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        TurnOff();
    }

    public void MoveRockSpot()
    {
        range = this.transform.localScale / 2.0f;
        float x = Random.Range(-range.x, range.x);
        float y = Random.Range(-range.y, range.y);
        _instancedRockSpot.transform.localPosition = new Vector3(x, y, 0); 
    }

    public void TurnOff()
    {
        _instancedRockSpot.SetActive(false);
        isOff = true;
    }

    public void TurnOn()
    {
        _instancedRockSpot.SetActive(true);
        isOff = false;
    }

    public Color GizmosColor = new Color(0.5f, 0.5f, 0.5f, 0.2f);

    void OnDrawGizmos()
    {
        Gizmos.color = GizmosColor;
        Gizmos.matrix = this.transform.localToWorldMatrix;
        Gizmos.DrawCube(Vector3.zero, Vector3.one);
    }
}
