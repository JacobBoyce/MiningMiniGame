using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Mineral", menuName = "New Mineral")]
public class MineralSO : ScriptableObject
{
    //img
    //points for target to appear on
    public int level, maxVel, durability;
    //max velocity matches the level of the rock
    // durability is affected by players level, and str?

    //if players level > rocks level then  
        //velocity and durability come down a certain percentage per level
        //gear and stats then brings it down more

    //if players level < rocks level
        //vel, and durability go up by a certain percentage per level
        //gear and stats still bring it down, but maybe not as much?
}
