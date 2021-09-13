using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] bool move;
    
    void Start()
    {
        move = true;
    }
    private void Update()
    {
        if (move)
        {
            gameObject.transform.Translate(-Vector3.forward * speed * Time.deltaTime);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy_gate")
        {
            Controll.Instance.Set_spawn();
        }
        //if (other.gameObject.tag == "Finish")
        //{
        //    Enemy_controll.Instance.Damage();
        //    Destroy(gameObject);
        //}
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
