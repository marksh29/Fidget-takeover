using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_controll : MonoBehaviour
{    
    public static Player_controll Instance;
    [Header("Настраиваемое")]
    [SerializeField] int life;
    [SerializeField] float spawn_time, freez_timer, gaint_spawn_timer;

    [Header("Не трогать")]
    [SerializeField] GameObject hand;
    [SerializeField] bool warrior, gaint, archer;
    [SerializeField] Gate_controll gate;    
    bool freez_on;
    float timer, gaint_timer;
    Vector3 hand_pos;
    GameObject sp;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {                   
    }

    public void Set_level()
    {
        hand_pos = hand.transform.position;
        gaint_timer = gaint_spawn_timer;

        if (PlayerPrefs.GetInt("level") != 100)
        {
            warrior = true;
        }
        gaint = PlayerPrefs.GetInt("buy_Gaint") == 1 ? true : false;

        if (PlayerPrefs.GetInt("buy_Archer") == 1 || archer)
        {
            Spawn(2);
        }
    }
    void Update()
    {
        if (Game_Controll.Instance.game)
        {
            if (Input.GetMouseButtonDown(0) && !freez_on)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Button" && hit.collider.gameObject.GetComponent<Button>().On())
                    {
                        StartCoroutine(DoMove(0.5f, hit.collider.gameObject));                       
                    }
                }
            }
            if (warrior)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = spawn_time;
                    Spawn(0);
                }
            }            
            if (gaint)
            {
                gaint_timer -= Time.deltaTime;
                if (gaint_timer <= 0)
                {
                    gaint_timer = gaint_spawn_timer;
                    Spawn(1);
                }
            }            
        }
    }   
    IEnumerator Freez_timer()
    {
        freez_on = true;
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color32(231, 155, 155, 150);
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color32(14, 114, 224, 150);
        yield return new WaitForSeconds(freez_timer);
        freez_on = false;
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color32(231, 155, 155, 255);
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color32(14, 114, 224, 255);
    }
    public void Damage(int id)
    {
        life -= id;
        if (life <= 0)
            Game_Controll.Instance.Lose();
    }
    void Spawn(int id)
    {
        switch(id)
        {
            case (0):
                sp = PoolControll.Instance.Spawn("pl_warrior", 0);
                sp.GetComponent<Players>().spawn = true;
                break;
            case (1):
                sp = PoolControll.Instance.Spawn("pl_gaint", 0);
                break;
            case (2):
                sp = PoolControll.Instance.Spawn("pl_archer", 0);
                break;
        }
        sp.transform.position = new Vector3(transform.position.x + Random.Range(-5,5), 0, transform.position.z);
        sp.transform.rotation = transform.rotation;
    }
    private IEnumerator DoMove(float time, GameObject target)
    {
        Vector3 startPosition = hand.transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            hand.transform.position = Vector3.Lerp(startPosition, target.transform.position, fraction);
            yield return null;
        }
        hand.transform.position = hand_pos;
        if (target.GetComponent<Button>().count != 0)
        {
            gate.Set_text(target.GetComponent<Button>().count);
            target.GetComponent<Button>().Off();
            StartCoroutine(Freez_timer());
        }
    }
    public void Archer_off()
    {

    }
}
