using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baloon : MonoBehaviour
{
    [SerializeField] float xx, speed;
    void Start()
    {
        
    }
    void Update()
    {
        transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        if (transform.position.y > xx)
        {
            transform.position = new Vector3(Random.Range(4, 14), -100, transform.position.z);
        }
    }
}
