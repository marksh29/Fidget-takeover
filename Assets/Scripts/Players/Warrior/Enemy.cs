using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Настраиваемое")]
    public int damage;
    public int start_life;   
    [SerializeField] float speed;

    [Header("Не трогать")]
    public Transform target;
    public int life;
    public bool move, battle, spawn, end;
   

    void OnEnable()
    {
        speed = Enemy_controll.Instance.move_speed;
        life = start_life;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
        Enable_param(); 
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
                if(target != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    transform.LookAt(target.position, Vector3.up);
                }               
            }
        }        
    }

    void Enable_param()
    {        
        gameObject.tag = "Enemy";
        transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
        target = null;
        move = true;
        battle = false;
    }
    public void Set_battle(Transform obj)
    {
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().isTrigger = true;
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
        }
        if (other.gameObject.tag == "Pfinish")
        {
            Player_controll.Instance.Damage(damage);
            gameObject.SetActive(false);
        }
    }
    public void Attack(int id)
    {
        battle = true;
        life -= id;
        if (life <= 0)
        {
            Blood();
            StartCoroutine(Disable(0));
        }
        else
        {
            if(target != null)
            {
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
            }            
        }
    }
    
    void Blood()
    {
        GameObject bl = PoolControll.Instance.Spawn("blood", 1);
        bl.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
    }
    IEnumerator Disable(float timer)
    {
        yield return new WaitForSeconds(timer);
        if(target != null)
            target.GetComponent<Players>().target = null;
        gameObject.SetActive(false);
    }

    public void Continue()
    {
        StartCoroutine(Move_on(1));        
    }
    IEnumerator Move_on(float attack_timer)
    {
        yield return new WaitForSeconds(attack_timer);
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
        Enable_param();
    }
    public void Damage(int id)
    {
        life -= id;
        if (life <= 0)// && gameObject.activeSelf)
        {
            Blood();
            StartCoroutine(Disable(0));
        }
    }

    public void Win()
    {
        end = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("win");
    }   
}
