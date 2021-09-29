using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Stage
{
    public float spawn_timer;
    public float move_speed;
    public bool warrior;
    public bool gaint;
    public bool archer;
}

public class Enemy_controll : MonoBehaviour
{
    public static Enemy_controll Instance;
    [Header("Настраиваемое")]    
    [SerializeField] int life;
    [SerializeField] float spawn_time, freez_timer, hand_move_speed, gaint_spawn_timer;
    public List<Stage> stages;

    [Header("Не трогать")]
    [SerializeField] Gate_controll gate;
    [SerializeField] GameObject hand, target;
    [SerializeField] bool warrior, gaint, archer;
    int level;
    bool select, frize_on, move;
    Vector3 hand_pos;
    float timer, select_timer, gaint_timer;
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
        level = PlayerPrefs.GetInt("level", 0);

        warrior = stages[level].warrior;
        gaint = stages[level].gaint;
        archer = stages[level].archer;

       
        hand_pos = hand.transform.position;
        gaint_timer = gaint_spawn_timer;

        if (archer)
            Spawn(2);
    }
    void Update()
    {
        if (Game_Controll.Instance.game)
        {
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

            if (select && !frize_on)
            {
                select_timer -= Time.deltaTime;
                if(select_timer <= 0)
                {
                    Hand_move();
                }
            }  
            
            if(move)
            {
                if(target.GetComponent<Button>().count != 0)
                {
                    hand.transform.position = Vector3.MoveTowards(hand.transform.position, target.transform.position, hand_move_speed * Time.deltaTime);
                    if(hand.transform.position == target.transform.position)
                    {
                        hand.transform.position = hand_pos;
                        gate.Set_text(target.GetComponent<Button>().count);
                        target.GetComponent<Button>().Off();
                        StartCoroutine(Freez_timer());
                        move = false;
                    }
                }
                else
                {
                    target = Buttons_controll.Instance.Best();
                    if(target.GetComponent<Button>().count < gate.count)
                    {
                        move = false;
                        select = true;
                        hand.transform.position = hand_pos;
                    }
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
            target = Buttons_controll.Instance.Best();
            select = false;
            move = true;
        }        
    }

    void Spawn(int id)
    {
        switch (id)
        {
            case (0):
                sp = PoolControll.Instance.Spawn("en_warrior", 0);
                break;
            case (1):
                sp = PoolControll.Instance.Spawn("en_gaint", 0);
                break;
            case (2):
                sp = PoolControll.Instance.Spawn("en_archer", 0);
                break;
        }

        sp.transform.position = new Vector3(transform.position.x + Random.Range(-5, 5), 0, transform.position.z -5);
        sp.transform.rotation = transform.rotation;        
        if(sp.GetComponent<Enemy>() != null)
            sp.GetComponent<Enemy>().spawn = true;
    }  
    public void Damage(int id)
    {
        life -=id;
        if (life <= 0)
            Game_Controll.Instance.Win();
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

