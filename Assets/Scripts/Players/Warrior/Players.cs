using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    [Header("Настраиваемое")]
    [SerializeField] int damage;
    [SerializeField] int start_life;
    [SerializeField] float speed;

    [Header("Не трогать")]
    [SerializeField] int life;
    [SerializeField] Transform target;
    public bool spawn, move, battle;    
   
    void OnEnable()
    {
        life = start_life;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
        Enable_param();
    }
    private void Update()
    {
        if (!battle)
        {
            if (move)
            {
                gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
            }
            else
            {
                if (target != null)
                {
                    transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
                    transform.LookAt(target.position, Vector3.up);
                }                           
            }
        }       
    }

    void Enable_param()
    {
        gameObject.tag = "Player";
        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(0).gameObject.GetComponent<Battle_collision>().on = false;
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
        target = null;
        move = true;
        battle = false;       
    }
    public void Set_target(Transform obj)
    {
        target = obj;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().isTrigger = true;   
        if(target.GetComponent<Enemy>() != null)
            target.GetComponent<Enemy>().Set_battle(gameObject.transform);        
        move = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gate" && other.gameObject.GetComponent<Gate_controll>().count > 0 && spawn)
        {
            other.gameObject.GetComponent<Gate_controll>().Set_spawn();
            spawn = false;
        }
        if(other.gameObject.tag == "Finish")
        {
            GetComponent<BoxCollider>().enabled = false;
            Enemy_controll.Instance.Damage(damage);
            gameObject.SetActive(false);
        }
        if (target != null && other.gameObject == target.gameObject)
        {
            battle = true;
            other.gameObject.GetComponent<Enemy>().Attack(damage);
            if(target.GetComponent<Enemy>() != null)
                Attack(target.GetComponent<Enemy>().damage);
            if (target.GetComponent<Archer>() != null)
                Attack(target.GetComponent<Archer>().damage);
        }
    }   
    public void Attack(int id)
    {        
        life -= id;
        if(life <= 0)
        {
            if (target != null)
                target.gameObject.GetComponent<Enemy>().Continue();
            Blood();
            StartCoroutine(Disable(0));
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
            StartCoroutine(Move_on(0.8f));
        }
    }
    IEnumerator Move_on(float timer)
    {
        yield return new WaitForSeconds(timer);
        if (target != null)
        {
            if (target.gameObject.activeSelf)
            {
                target.GetComponent<Enemy>().Attack(damage);
                Attack(target.GetComponent<Enemy>().damage);
            }
            else
            {
                Enable_param();
                transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
            }                      
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");            
            Enable_param();
        }       
    }
    void Blood()
    {
        GameObject bl = PoolControll.Instance.Spawn("blood", 0);
        bl.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
    }
    IEnumerator Disable(float time)
    {
        yield return new WaitForSeconds(time);
        gameObject.SetActive(false);
    }
    public void Damage(int id)
    {
        life -= id;
        if (life <= 0)
        {
            Blood();
            StartCoroutine(Disable(0));
        }
    }
}
