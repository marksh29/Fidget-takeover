using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_collision : MonoBehaviour
{
    bool on;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Enemy" && !on)
        {
            transform.parent.gameObject.GetComponent<Players>().Set_target(coll.gameObject.transform);
            on = true;
        }
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
