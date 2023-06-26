using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildData", menuName = "Build Data")]

public class BuildData : ScriptableObject
{
    [System.Serializable]
    public class BuildMainData
    {
        public List<int> HPs = new List<int>();
        public List<int> Costs = new List<int>();
    }

    public List<BuildMainData> buildMainDatas = new List<BuildMainData>();
}
