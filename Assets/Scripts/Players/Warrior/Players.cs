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
    public Transform target;
    public bool spawn, move, battle, end;    
   
    void OnEnable()
    {
        life = start_life;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
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
            Enemy_controll.Instance.Damage(damage);
            gameObject.SetActive(false);
        }
        if (target != null && other.gameObject == target.gameObject)
        {
            battle = true;
            if(target.GetComponent<Enemy>() != null)
                Attack(target.GetComponent<Enemy>().damage, "warrior");
            if (target.GetComponent<Archer>() != null)
                Attack(target.GetComponent<Archer>().damage, "archer");
        }
    }
    public void Attack(int id, string target_type)
    {
        life -= id;
        switch(target_type)
        {
            case ("warrior"):
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
            StartCoroutine(Move_on(0.8f));
        }
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
        if (life <= 0 && gameObject.activeSelf)
        {
            Blood();
            StartCoroutine(Disable(0));
        }
    }

    public void Win()
    {
        print("win");
        end = true;
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("win");
    }    
    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
