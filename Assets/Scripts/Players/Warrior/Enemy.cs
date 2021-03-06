using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("?????????????")]
    public int damage;
    public int start_life;   
    [SerializeField] float speed, force_speed;

    [Header("?? ???????")]
    public Transform target;
    public int life;
    public bool move, battle,  end, gaint;
    //public bool spawn;
    [SerializeField] RigidbodyConstraints open, close;
    public GameObject body;

    void OnEnable()
    {
        end = false;
        speed = Enemy_controll.Instance.move_speed + (0.1f * PlayerPrefs.GetInt("level"));
        life = start_life;        
        Enable_param();
        //if (body != null)
        //    body.gameObject.GetComponent<Animator>().SetTrigger("move");
    }
    private void Update()
    {
        if(!battle && !end)
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
                else
                    Enable_param();
            }
        }
        else if(battle && !end)
        {
            if (target != null)
            {
                Enable_param();
            }
        }
    }    
    public void Enable_param()
    {
        GetComponent<Rigidbody>().constraints = close;
        if(body != null)
        {
            body.gameObject.GetComponent<Animator>().enabled = true;
            body.gameObject.GetComponent<Animator>().SetTrigger("move");
        }       
        gameObject.tag = "Enemy";
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        transform.rotation = Quaternion.Euler(0, 180, 0);
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
        target = null;
        move = true;
        battle = false;
        //spawn = false;
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
        //if (other.gameObject.tag == "Gate" && !gaint && spawn)
        //{
        //    other.gameObject.GetComponent<Bonus_Gate_controll>().Set_spawn(1);
        //}
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
            if (target != null)
            {
                if (gaint)
                    StartCoroutine(Gaint_attack(1));
                body.gameObject.GetComponent<Animator>().SetTrigger("attack");                    
            }            
        }
    }
    IEnumerator Gaint_attack(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Gaint>().Mass_attack(damage);
    }

    IEnumerator Disable(float timer)
    {
        gameObject.tag = "Untagged";
        if (target != null && target.GetComponent<Players>() != null)
            target.GetComponent<Players>().target = null;
        yield return new WaitForSeconds(timer);
        Off();
    }
    void Off()
    {
        if (target != null && target.GetComponent<Players>() != null)
            target.GetComponent<Players>().target = null;
        PoolControll.Instance.Visual();
        gameObject.SetActive(false);
    }

    public void Continue()
    {
        StartCoroutine(Move_on(1f));        
    }
    IEnumerator Move_on(float attack_timer)
    {
        yield return new WaitForSeconds(attack_timer);
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
    void Blood()
    {
        if (Game_Controll.Instance.game)
        {
            GameObject bl = PoolControll.Instance.Spawn("blood", 1);
            bl.transform.position = new Vector3(transform.position.x, 0.01f, transform.position.z);
        }
    }
    public void Win()
    {
        end = true;
        body.gameObject.GetComponent<Animator>().SetTrigger("win");
    }
    //void Add_force()
    //{
    //    if (target != null && target.GetComponent<Players>() != null)
    //        target.GetComponent<Players>().target = null;

    //    GetComponent<Rigidbody>().constraints = open;
    //    GetComponent<Rigidbody>().useGravity = true;
    //    GetComponent<CapsuleCollider>().isTrigger = true;
    //    body.gameObject.GetComponent<Animator>().enabled = false;
    //    GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 2, Random.Range(1, 2)) * force_speed, ForceMode.Impulse);
    //}
}
