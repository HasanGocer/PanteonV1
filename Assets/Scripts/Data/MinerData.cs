using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinerData", menuName = "Miner Data")]

public class MinerData : ScriptableObject
{
    public List<int> HP = new List<int>();
    public List<int> Cost = new List<int>();
    public List<int> PerGem = new List<int>();
}
