using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] float speed;
    public bool move, battle, spawn;
    public Transform target;
    
    void Start()
    {
        move = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
    }
    private void Update()
    {
        if(!battle)
        {
            if (move)
            {
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                transform.LookAt(target.position, Vector3.up);
            }
        }        
    }
    public void Set_battle(Transform obj)
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<SphereCollider>().isTrigger = true;
        gameObject.tag = "Untagged";
        target = obj;
        move = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyGate" && other.gameObject.GetComponent<Gate_controll>().count > 0 && spawn)
        {
            other.gameObject.GetComponent<Gate_controll>().Set_spawn();
            spawn = false;
            //Enemy_controll.Instance.Gate_on();
        }
        if (other.gameObject.tag == "Pfinish")
        {
            Player_controll.Instance.Damage();
            Destroy(gameObject);
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.tag == "Player")
    //    {
    //        Destroy(collision.gameObject);
    //        Destroy(gameObject);
    //    }
    //}
    public void Attack()
    {
        battle = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");       
    }
}
