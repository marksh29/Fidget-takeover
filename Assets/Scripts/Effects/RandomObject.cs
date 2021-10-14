using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomObject : MonoBehaviour
{
    [SerializeField] bool objects, sprite, material, skined;
    [SerializeField] GameObject[] obj;

    [SerializeField] MeshRenderer mesh;
    [SerializeField] Sprite[] sprt;    
    [SerializeField] Material[] mat;

    void Start()
    {
        
    }
    public void Random_on()
    {
        if (objects)
        {
            int r = Random.Range(0, obj.Length);
            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(i == r ? true : false);
            }
        }
        if(sprite)
        {

        }
        if(material)
        {
            mesh.sharedMaterial = mat[Random.Range(0, mat.Length)];
        }
        if (skined)
        {
            
        }
    }
}
