using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Load : MonoSingleton<Load>
{
    public bool isReturn;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
}
