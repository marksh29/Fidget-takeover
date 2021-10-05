using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Big_hand : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] bool enemy;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (enemy)
        {
            if (coll.gameObject.tag == "Player")
            {
                if (coll.gameObject.GetComponent<Players>() != null)
                {
                    coll.gameObject.GetComponent<Players>().Damage(damage);
                }
                else
                {
                    coll.gameObject.GetComponent<Archer>().Damage();
                }
            }
        }
        else
        {
            if (coll.gameObject.tag == "Enemy")
            {
                if (coll.gameObject.GetComponent<Enemy>() != null)
                {
                    coll.gameObject.GetComponent<Enemy>().Damage(damage);
                }
                else
                {
                    coll.gameObject.GetComponent<Archer>().Damage();
                }
            }
        }
    }   
}
