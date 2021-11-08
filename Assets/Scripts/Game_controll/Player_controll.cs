using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_controll : MonoBehaviour
{    
    public static Player_controll Instance;
    [Header("Настраиваемое")]
    [SerializeField] int life, move_distance, max_distance;
    [SerializeField] float spawn_time, freez_timer, gaint_spawn_timer, spawn_timer_upgrade, speed_booster;

    [Header("Не трогать")]
    [SerializeField] GameObject hand, effect;
    [SerializeField] bool warrior, gaint, archer;
    [SerializeField] Gate_controll gate;    
    bool freez_on, hand_move, end;
    float timer, gaint_timer, sp_upgrade;
    Vector3 hand_pos;
    Vector3 start_mouse_pos;
    [SerializeField] GameObject sp, cur_player;

    public Transform pricel;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;

    }
    void Start()
    {
        PlayerPrefs.SetInt("tutorial", 0);
    }

    public void Set_level()
    {
        sp_upgrade = spawn_timer_upgrade * PlayerPrefs.GetInt("Upgrade0");

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
            if (Input.GetMouseButtonDown(0))// && !freez_on && !hand_move)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {                   
                    start_mouse_pos = Input.mousePosition;
                    if (hit.collider != null && hit.collider.gameObject.tag == "Player" && !hit.collider.gameObject.GetComponent<Players>().jump)
                    {
                        cur_player = hit.collider.gameObject;
                        cur_player.GetComponent<Players>().ArrowOn();
                    }
                }
            }
            if (Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && cur_player != null)
                    {
                        cur_player.transform.LookAt(new Vector3(hit.point.x, 0, hit.point.z));
                    }
                }                
            }
            if (Input.GetMouseButtonUp(0))
            {
                //print((start_mouse_pos - Input.mousePosition).magnitude);
                if(cur_player != null)// && (start_mouse_pos - Input.mousePosition).magnitude >= 100)
                {
                    //cur_player.transform.position = new Vector3(cur_player.transform.position.x, 0, cur_player.transform.position.z);
                    float speed = speed_booster * (start_mouse_pos - Input.mousePosition).magnitude * Time.deltaTime;
                    cur_player.GetComponent<Players>().Start_move(speed);
                    cur_player = null;
                }                
            }


            if (warrior)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = spawn_time - sp_upgrade;
                    if (timer < 1)
                        timer = 1;
                    Spawn(0);
                }
            }            
            if (gaint)
            {
                gaint_timer -= Time.deltaTime;
                if (gaint_timer <= 0)
                {
                    gaint_timer = gaint_spawn_timer - sp_upgrade;
                    if (gaint_timer < 1)
                        gaint_timer = 1;
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
    IEnumerator Effect()
    {
        //effect.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        effect.SetActive(false);
    }
        public void Damage(int id)
    {
        life -= id;
        if (life <= 0 && !end)
        {
            EndEffect.Instance.Lose();
            //Game_Controll.Instance.Lose();
        }
    }
    void Spawn(int id)
    {
        switch(id)
        {
            case (0):
                sp = PoolControll.Instance.Spawn("pl_warrior", 0);
                sp.GetComponent<Players>().Jump();
                break;
            case (1):
                sp = PoolControll.Instance.Spawn("pl_gaint", 0);
                break;
            case (2):
                sp = PoolControll.Instance.Spawn("pl_archer", 0);
                break;
        }
        sp.transform.position = new Vector3(transform.position.x + Random.Range(-5,5), 5, transform.position.z);
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
        hand_move = false;
        hand.transform.position = hand_pos;
        if (Sound.Instance != null)
            Sound.Instance.Play_Sound(0);
        if (target.GetComponent<Button>().count != 0)
        {
            //gate.Set_text(target.GetComponent<Button>().count);
            gate.Set_spawn(target.GetComponent<Button>().count);

            //target.GetComponent<Button>().Off();
            target.GetComponent<Button>().Get();
            StartCoroutine(Effect());
            StartCoroutine(Freez_timer());
        }
    }
    public void Archer_off()
    {

    }
}
