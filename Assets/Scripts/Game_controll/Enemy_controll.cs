using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_controll : MonoBehaviour
{
    public static Enemy_controll Instance;
    [Header("Настраиваемое")]
    [SerializeField] int life;
    [SerializeField] float spawn_time, freez_timer;

    [Header("Не трогать")]
    [SerializeField] Gate_controll gate;
    [SerializeField] GameObject obj, hand;
    bool select, frize_on;
    Vector3 hand_pos;
    float timer, select_timer;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        hand_pos = hand.transform.position;
    }
    void Update()
    {
        if (Game_Controll.Instance.game)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = spawn_time;
                Spawn();
            }
            if(select && !frize_on)
            {
                select_timer -= Time.deltaTime;
                if(select_timer <= 0)
                {
                    Hand_move();
                }
            }          
        }        
    }
    public void Start_select()
    {
        select_timer = Random.Range(1, 2);
        select = true;
    }
    void Hand_move()
    {
        select = false;
        StartCoroutine(DoMove(0.5f, Buttons_controll.Instance.Best()));
    }
    void Spawn()
    {
        Instantiate(obj, new Vector3(0, 0, transform.position.z - 5), transform.rotation);        
    }
    public void Damage()
    {
        life--;
        if (life <= 0)
            Game_Controll.Instance.Win();
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
        else
        {
            Hand_move();
        }
    }
    IEnumerator Freez_timer()
    {
        frize_on = true;
        yield return new WaitForSeconds(freez_timer);
        frize_on = false;
    }     
}

