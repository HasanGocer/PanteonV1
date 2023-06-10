using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CentralData", menuName = "Central Data")]

public class CentralData : ScriptableObject
{
    public List<int> HP = new List<int>();
    public List<int> Cost = new List<int>();
    public List<int> PerEnergy = new List<int>();
}
