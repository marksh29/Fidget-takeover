using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon_color : MonoBehaviour
{
    [SerializeField] Material[] sprt;
    void Start()
    {
        GetComponent<MeshRenderer>().sharedMaterial = sprt[Random.Range(0, sprt.Length)];
    }  
}
