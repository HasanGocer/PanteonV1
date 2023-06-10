using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BarracksData", menuName = "Barracks Data")]

public class BarracksData : ScriptableObject
{
    public List<int> HPs = new List<int>();
    public List<int> Costs = new List<int>();
    public List<float> countDowns = new List<float>();
    public List<float> hitSpeeds = new List<float>();
    public List<int> damages = new List<int>();
}
