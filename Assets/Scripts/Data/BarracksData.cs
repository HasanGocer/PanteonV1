using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BarracksData", menuName = "Barracks Data")]

public class BarracksData : ScriptableObject
{
    public List<float> countDowns = new List<float>();
    public List<float> hitSpeeds = new List<float>();
    public List<int> damages = new List<int>();
}
