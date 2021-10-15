using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    [Header("Настраиваемое")]
    public int damage;
    [SerializeField] int start_life, dop_attack;
    [SerializeField] float speed, force_speed;

    [Header("Не трогать")]
    [SerializeField] int life;
    public Transform target;
    public bool gaint, spawn, move, battle, end;
    [SerializeField] RigidbodyConstraints open, close;

    void OnEnable()
    {
        end = false;
        life = start_life;
        damage = damage + PlayerPrefs.GetInt("Upgrade1");
        Enable_param();
    }
    private void Update()
    {
        if (!battle && !end)
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
                else
                    Enable_param();
            }
        }
        else if (battle && !end)
        {
            if (target != null)
            {
                Enable_param();
            }
        }
    }

    void Enable_param()
    {      
        gameObject.tag = "Player";

        GetComponent<Rigidbody>().constraints = close;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);        
        transform.rotation = Quaternion.Euler(0, 0, 0);
        target = null;
        move = true;
        battle = false;

        transform.GetChild(1).gameObject.GetComponent<Battle_collision>().on = false;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().isTrigger = false;
        
        transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
    }
    public void Set_target(Transform obj)
    {
        if(target == null)
            target = obj;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<CapsuleCollider>().isTrigger = true;   
        
        if(target.GetComponent<Enemy>() != null)
        {
            target.GetComponent<Enemy>().Set_battle(gameObject.transform);            
        }               
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
            Enemy_controll.Instance.Damage(damage);
            gameObject.SetActive(false);
        }

        if (target != null && other.gameObject == target.gameObject)
        {
            battle = true;
            if (target != null && target.GetComponent<Enemy>() != null)
                Attack(target.GetComponent<Enemy>().damage, "warrior");
            if (target != null && target.GetComponent<Archer>() != null)
                Attack(target.GetComponent<Archer>().damage, "archer");
        }     
    }
    public void Attack(int id, string target_type)
    {
        life -= id;
        switch(target_type)
        {
            case ("warrior"):               
                if (gaint)
                    StartCoroutine(Gaint_attack(0.5f));
                else
                    target.GetComponent<Enemy>().Attack(damage);
                break;
            case ("archer"):
                target.GetComponent<Archer>().Damage();
                break;
        }

        if(life <= 0)
        {
            if (target != null && target.gameObject.GetComponent<Enemy>() != null)
            {
                target.gameObject.GetComponent<Enemy>().Continue();
            }
            Blood();
            StartCoroutine(Disable(0));
        }
        else
        {
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("attack");
            StartCoroutine(Move_on(1.5f));
        }
    }
    IEnumerator Gaint_attack(float time)
    {
        if (target != null && target.GetComponent<Enemy>() != null)
            target.GetComponent<Enemy>().Attack(damage);
        yield return new WaitForSeconds(time);        
        GetComponent<Gaint>().Mass_attack(damage);
    }

    IEnumerator Move_on(float attack_timer)
    {
        yield return new WaitForSeconds(attack_timer);
        if (target != null)
        {
            Attack(target.GetComponent<Enemy>().damage, "warrior");                             
        }
        else
        {
            transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);
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
        gameObject.tag = "Untagged";
        if (target != null && target.GetComponent<Enemy>() != null)
            target.GetComponent<Enemy>().target = null;    
        yield return new WaitForSeconds(time);
        Off();       
    }
    void Off()
    {
        if (Sound.Instance != null)
            Sound.Instance.Play_Sound(1);

        if (target != null && target.GetComponent<Enemy>() != null)
            target.GetComponent<Enemy>().target = null;
        gameObject.SetActive(false);
    }

    public void Damage(int id)
    {
        life -= id;
        if (life <= 0 && gameObject.activeSelf)
        {
            Blood();
            StartCoroutine(Disable(0));
        }
    }

    public void Win()
    {
        end = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("win");
        transform.rotation = Quaternion.Euler(0, 180, 0);
    }    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
    //void Add_force()
    //{
    //    if (target != null && target.GetComponent<Enemy>() != null)
    //        target.GetComponent<Enemy>().target = null;

    //    GetComponent<Rigidbody>().constraints = open;
    //    GetComponent<Rigidbody>().useGravity = true;
    //    GetComponent<CapsuleCollider>().isTrigger = true;
    //    transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = false;
    //    GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-2, 2), 2, Random.Range(-2, -1)) * force_speed, ForceMode.Impulse);
    //}    
}
