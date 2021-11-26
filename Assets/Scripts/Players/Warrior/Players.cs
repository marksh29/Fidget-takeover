using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Players : MonoBehaviour
{
    [Header("Настраиваемое")]
    public int damage;
    [SerializeField] int start_life;
    [SerializeField] float speed, force, max_speed;

    [Header("Не трогать")]
    [SerializeField] int life;
    public Transform target;
    public bool gaint, spawn, move, battle, end, jump;
   // [SerializeField] RigidbodyConstraints open, close;
    [SerializeField] SkinnedMeshRenderer skin;
    [SerializeField] Material[] mater;
    [SerializeField] GameObject tutor, vect;

    void Start()
    {
        Vect_spawn();
    }
    void OnEnable()
    {
        end = false;        
        life = start_life;
        //damage = damage + PlayerPrefs.GetInt("Upgrade1");
        speed = 5 + (0.2f * PlayerPrefs.GetInt("Upgrade1"));
        Enable_param();
        skin.sharedMaterial = mater[0];
    }
    public void Jump()
    {
        if (!end)
        {
            GetComponent<Rigidbody>().mass = 30;
            transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("jump");
            jump = true;
            move = false;
            spawn = true;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
            GetComponent<Rigidbody>().AddForce(new Vector3(Random.Range(-3, 3), 8, 120) * force, ForceMode.Impulse);
        }       
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

        target = null;
        battle = false;

        transform.GetChild(1).gameObject.GetComponent<Battle_collision>().on = false;

        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<CapsuleCollider>().isTrigger = false;

        transform.GetChild(0).gameObject.GetComponent<Animator>().enabled = true;
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

        skin.sharedMaterial = mater[1];
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");

        move = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Path" && !end)
        {
            if (PlayerPrefs.GetInt("tutorial") == 0)
            {
                tutor.SetActive(true);
                PlayerPrefs.SetInt("tutorial", 1);
            }
            //transform.rotation = Quaternion.Euler(0, 0, 0);
            transform.GetChild(0).rotation = transform.rotation;
            GetComponent<Rigidbody>().mass = 3;
            jump = false;
        }        
    }
    public void ArrowOn()
    {
        vect.SetActive(true);
        tutor.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gate" && !gaint)
        {
            other.gameObject.GetComponent<Bonus_Gate_controll>().Set_spawn(0, speed);
        }
        if (other.gameObject.tag == "Finish")
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
            //transform.GetChild(0).localRotation = Quaternion.Euler(0, 0, 0);
            transform.GetChild(0).rotation = transform.parent.rotation;
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
        PoolControll.Instance.Visual();

        GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);

        //GetComponent<Rigidbody>().constraints = open;
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
        skin.sharedMaterial = mater[1];
    }    
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void Start_move(float spd)
    {
        speed = (spd < max_speed ? spd : max_speed);
        //GetComponent<Rigidbody>().constraints = close;
        tutor.SetActive(false);
        vect.SetActive(false);
        //transform.rotation = Quaternion.Euler(0, 0, 0);
        transform.GetChild(0).rotation = Quaternion.Euler(0, 0, 0);

        skin.sharedMaterial = mater[1];
        transform.position = new Vector3(transform.position.x, 0, transform.position.z); 
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("move");
        move = true;
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
    
    void Vect_spawn()
    {
        for(int i = 0; i < 20; i++)
        {
            int ct = 255 - (i * 10);
            GameObject obj = Instantiate(vect.transform.GetChild(0).gameObject, vect.transform) as GameObject;
            obj.transform.localPosition = new Vector3(0, 0, vect.transform.GetChild(0).localPosition.z * (i + 1));
            obj.GetComponent<SpriteRenderer>().color = new Color32(255,255,255, (byte)ct);
        }
    }
}
