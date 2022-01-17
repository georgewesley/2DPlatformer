using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    public float fest = 5.0123f;
    public string lest = "hi";
    void Start()
    {
        fest = 5.12321f;
        lest = lest + fest.ToString();
    }
}
