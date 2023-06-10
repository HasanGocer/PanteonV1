using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoldierData", menuName = "Soldier Data")]

public class SoldierData : ScriptableObject
{
    public List<int> maxHPs = new List<int>();
    public List<int> damages = new List<int>();
    public List<float> speedFactors = new List<float>();
    public List<float> hitCountDowns = new List<float>();
}
