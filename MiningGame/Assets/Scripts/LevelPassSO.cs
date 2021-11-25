using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SaveBetweenScenes")]
public class LevelPassSO : ScriptableObject
{
    [SerializeField]
    public List<SaveStuffObj> saveList;  
}

[System.Serializable]
public class SaveStuffObj
{
    public string upName;
    public int level;
}