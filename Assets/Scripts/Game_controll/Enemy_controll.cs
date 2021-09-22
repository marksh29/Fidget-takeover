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

    [SerializeField] Image flag_img;
    [SerializeField] Sprite[] all_sprt;
    [SerializeField] Text en_name;
    [SerializeField] string[] all_name;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        hand_pos = hand.transform.position;

        flag_img.sprite = all_sprt[Random.Range(0, all_sprt.Length)];
        en_name.text = all_name[Random.Range(0, all_name.Length)];
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
        if(gate.count < Buttons_controll.Instance.Best().GetComponent<Button>().count)
        {
            select = false;
            StartCoroutine(DoMove(0.5f, Buttons_controll.Instance.Best()));
        }        
    }
    void Spawn()
    {
        GameObject sp = PoolControll.Instance.Spawn_enemy();
        sp.transform.position = new Vector3(0, 0, transform.position.z - 5);
        sp.transform.rotation = transform.rotation;
        sp.GetComponent<Enemy>().spawn = true;
        //GameObject sp = Instantiate(obj, new Vector3(0, 0, transform.position.z - 5), transform.rotation) as GameObject;
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
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color32(231, 155, 155, 150);
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color32(231, 0, 0, 150);
        frize_on = true;
        yield return new WaitForSeconds(freez_timer);
        frize_on = false;
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color32(231, 155, 155, 255);
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color32(231, 0, 0, 255);
    }     
}

