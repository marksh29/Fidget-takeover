using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_collision : MonoBehaviour
{
    public bool on;
    private void OnEnable()
    {
        on = false;
    }
    void Update()
    {
        
    }   
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && !on)
        {
            transform.parent.gameObject.GetComponent<Players>().Set_target(other.gameObject.transform);
            on = true;
        }
    }
}
