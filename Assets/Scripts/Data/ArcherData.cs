using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ArcherData", menuName = "Archer Data")]

public class ArcherData : ScriptableObject
{
    public List<float> countDowns = new List<float>();
    public List<float> hitSpeeds = new List<float>();
    public List<int> damages = new List<int>();
}
