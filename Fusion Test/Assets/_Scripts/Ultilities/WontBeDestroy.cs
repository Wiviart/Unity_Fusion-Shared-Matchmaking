using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WontBeDestroy : MonoBehaviour
{
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
