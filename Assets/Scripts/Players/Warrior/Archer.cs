using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour
{
    [Header("Настраиваемое")]
    [SerializeField] bool enemy;     
    public int damage;
    [SerializeField] float start_fire_timer, speed, attack_dist;

    [Header("Не трогать")]
    float fire_time;
    [SerializeField] bool end, attack;
    GameObject target;

    void OnEnable()
    {
        fire_time = 3;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
    }
    void Update()
    {
        if(!end)
        {
            fire_time -= Time.deltaTime;
            if(fire_time <= 0)
            {
                StartCoroutine(Fire(0.2f));
                fire_time = start_fire_timer;
            }
        }
    }   
    IEnumerator Fire(float time)
    {
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
        yield return new WaitForSeconds(time);

        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
        List<GameObject> add_list = new List<GameObject>();
        for (int i = 0; i < list.Length; i++)
        {
            if(list[i].transform.position.z - transform.position.z > attack_dist)
            {
                add_list.Add(list[i]);
            }            
        }
        if (add_list.Count > 0)
        {
            GameObject arr = PoolControll.Instance.Spawn("arrow", (enemy == false ? 0 : 1));
            arr.transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
            arr.transform.rotation = transform.rotation;
            arr.GetComponent<Arrow>().Start_fly(add_list[Random.Range(0, add_list.Count)].transform);
        }
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
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
                other.gameObject.GetComponent<Enemy>().Continue();
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
        gameObject.SetActive(false);
    }
}
