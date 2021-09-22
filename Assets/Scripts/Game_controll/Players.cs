using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{ 

    [SerializeField] float speed;
    public bool move, battle, spawn;
    [SerializeField] Transform target;
    void OnEnable()
    {
        move = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
        target = null;
    }
    private void Update()
    {
        if (!battle)
        {
            if (move)
            {
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
               
                //if(target == null)
                //{
                //    List<GameObject> list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
                //    for (int i = 0; i < list.Count; i++)
                //    {
                //        if (list[i].GetComponent<Enemy>().target == null && (target == null || target != null && Dist(list[i])))
                //        {
                //            target = list[i].transform;                              
                //        }
                //    }
                //    if(target != null)
                //        target.gameObject.GetComponent<Enemy>().target = gameObject.transform;
                //}
                //if (target != null && (transform.position - target.transform.position).sqrMagnitude <= 200)
                //{
                //    target.GetComponent<Enemy>().Set_battle();
                //    move = false;
                //}
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                transform.LookAt(target.position, Vector3.up);
            }
        }       
    }
    public void Set_target(Transform obj)
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().isTrigger = true;
        target = obj;
        target.GetComponent<Enemy>().Set_battle(gameObject.transform);
        move = false;
    }
    //bool Dist(GameObject obj)
    //{
    //    return (transform.position - obj.transform.position).sqrMagnitude < (transform.position - target.transform.position).sqrMagnitude ? true : false;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gate" && other.gameObject.GetComponent<Gate_controll>().count > 0 && spawn)
        {
            other.gameObject.GetComponent<Gate_controll>().Set_spawn();
            spawn = false;
            //Player_controll.Instance.Gate_on();
        }
        if(other.gameObject.tag == "Finish")
        {
            Enemy_controll.Instance.Damage();
            Destroy(gameObject);
        }
        if (target != null && other.gameObject == target.gameObject)
        {
            Attack();
            other.gameObject.GetComponent<Enemy>().Attack();
        }
    }   
    void Attack()
    {
        //battle = true;
        //transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
        Blood();
        StartCoroutine(Disable(0));
    }
    void Blood()
    {
        GameObject bl = PoolControll.Instance.Spawn_blood(0);
        bl.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
    }
    IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
}
