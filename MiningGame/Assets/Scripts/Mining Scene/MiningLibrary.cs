using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Mineral
{
    public string name;
    public int level, maxVel, durability;
}
public class MiningLibrary : MonoBehaviour
{
    [SerializeField]
    //public List<MineralSO> minerals;
    public List<Mineral> minerals = new List<Mineral>();
    private Mineral tempOre;

    public Mineral GetOre(string oreName)
    {
        foreach(Mineral or in minerals)
        {
            if(or.name.Equals(oreName))
            {
                tempOre = or;
            }
        }
        return tempOre;
    }

    

    /*
    coal, stone, iron, steel, gold, diamond, suprite
    */
}
