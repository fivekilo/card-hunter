using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/SharedData")]
public class SharedData : ScriptableObject
{
    public PlayerInfo playerinfo;
    public Commission commission;
    public bool Complete=false;

}
