using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour
{
    [Header("?????????????")]
    [SerializeField] bool enemy;     
    public int damage;
    [SerializeField] float start_fire_timer, speed, attack_dist;

    [Header("?? ???????")]
    [SerializeField] float fire_time;
    [SerializeField] bool end, attack, move;
    [SerializeField] Vector3 move_pos;
    GameObject target;

    void OnEnable()
    {
        //fire_time = 3;
        //damage = damage + PlayerPrefs.GetInt("Upgrade1");
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
        Set_move_pos();       
    }
    public void Set_move_pos()
    {
        float xx = 4.5f;
        float zz = enemy != true ? -43 : 40;
        move_pos = new Vector3(xx, 0, zz);       
        move = true;
    }
    private void Start()
    {
        

    }
    void Update()
    {
        if(!end)
        {
            if (move)
            {
                transform.position = Vector3.MoveTowards(transform.position, move_pos, speed * Time.deltaTime);
                transform.LookAt(move_pos, Vector3.up);

                if (transform.position == move_pos)
                {
                    transform.rotation = Quaternion.Euler(0, enemy == true ? 180 : 0, 0);
                    move = false;
                    transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
                }                   
            }
            else
            {
                fire_time -= Time.deltaTime;
                if (fire_time <= 0)
                {
                    StartCoroutine(Fire(0.2f));
                    fire_time = start_fire_timer;
                }
            }            
        }
    }
    IEnumerator Fire(float time)
    {           
        yield return new WaitForSeconds(time);        
        GameObject[] list = GameObject.FindGameObjectsWithTag(enemy == false ? "Enemy" : "Player");
        if(list.Length != 0)
        {            
            List<GameObject> add_list = new List<GameObject>();
            for (int i = 0; i < list.Length; i++)
            {
                if (!enemy && list[i].transform.position.z - transform.position.z < attack_dist)
                {
                    add_list.Add(list[i]);
                }
                else if (enemy && transform.position.z - list[i].transform.position.z < attack_dist)
                {
                    add_list.Add(list[i]);
                }
            }
            if (add_list.Count > 0)
            {
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
                GameObject arr = PoolControll.Instance.Spawn("arrow", (enemy == false ? 0 : 1));
                arr.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                arr.transform.rotation = transform.rotation;
                arr.GetComponent<Arrow>().Start_fly(add_list[Random.Range(0, add_list.Count)].transform);
            }
        }        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!enemy)
        {
            if (!attack && other.gameObject.tag == "Enemy")
            {
                attack = true;
                target = other.gameObject;
                target.GetComponent<Enemy>().Set_battle(gameObject.transform);
            }
            else if (attack && other.gameObject == target)
            {
                other.gameObject.GetComponent<Enemy>().Attack(damage);
                Blood();
                StartCoroutine(Disable(0));
            }
        }
        else
        {
            if (!attack && other.gameObject.tag == "Player")
            {
                attack = true;
                target = other.gameObject;
                other.gameObject.GetComponent<Players>().Set_target(gameObject.transform);
            }
        }
    }  
    public void Damage()
    {
        Blood();
        StartCoroutine(Disable(0));
    }
    void Blood()
    {
        GameObject bl = PoolControll.Instance.Spawn("blood", enemy == false ? 0 : 1);
        bl.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
    }
    IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);
        if(target != null)
        {
            if (target.GetComponent<Players>() != null)
                target.GetComponent<Players>().target = null;
            else
                target.GetComponent<Enemy>().target = null;
        }
        PoolControll.Instance.Visual();
        gameObject.SetActive(false);
    }

    public void Win()
    {
        end = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("win");
    }
}
