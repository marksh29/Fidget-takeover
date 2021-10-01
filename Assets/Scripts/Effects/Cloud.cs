using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] float xx, speed;
    [SerializeField] float[] yy;
    void Start()
    {
        
    }
    void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        if (transform.position.x > xx)
        {
            transform.position = new Vector3(-transform.position.x, Random.Range(yy[0], yy[1]), transform.position.z);
            float scl = Random.Range(0.15f, 0.4f);
            transform.localScale = new Vector3(scl, scl, scl);
        }
    }
}
