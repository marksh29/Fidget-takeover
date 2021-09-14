using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{ 

    [SerializeField] float speed;
    [SerializeField] bool move,battle;
    [SerializeField] Transform target;
    void Start()
    {
        move = true;
    }
    private void Update()
    {
        if (!battle)
        {
            if (move)
            {
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
               
                if(target == null)
                {
                    List<GameObject> list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
                    for (int i = 0; i < list.Count; i++)
                    {
                        if (list[i].GetComponent<Enemy>().target == null && (target == null || target != null && Dist(list[i])))
                        {
                            target = list[i].transform;                              
                        }
                    }
                    if(target != null)
                        target.gameObject.GetComponent<Enemy>().target = gameObject.transform;
                }
                if (target != null && (transform.position - target.transform.position).sqrMagnitude <= 200)
                {
                    target.GetComponent<Enemy>().Set_battle();
                    move = false;
                }
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
            }
        }       
    } 
    bool Dist(GameObject obj)
    {
        return (transform.position - obj.transform.position).sqrMagnitude < (transform.position - target.transform.position).sqrMagnitude ? true : false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gate" && other.gameObject.GetComponent<Gate_controll>().count > 0)
        {
            other.gameObject.GetComponent<Gate_controll>().Set_spawn();
            //Player_controll.Instance.Gate_on();
        }
        if(other.gameObject.tag == "Finish")
        {
            Enemy_controll.Instance.Damage();
            Destroy(gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy" && target != null && collision.gameObject == target.gameObject)
        {
            collision.gameObject.GetComponent<Enemy>().Attack();
            Attack();            

            Destroy(collision.gameObject, 1);
            Destroy(gameObject, 1);
        }
    }
    void Attack()
    {
        battle = true;
    }
}
