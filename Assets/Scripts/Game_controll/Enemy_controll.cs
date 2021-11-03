using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Stage
{
    public int life, win_money;
    public float warrior_spawn_timer;
    public float gaint_spawn_timer;
    public float move_speed;   
    public float frize_timer;

    public bool warrior;
    public bool gaint;
    public bool archer;

    public float[] select_timer_min_max;  
    public int[] boost_variant;   
}

public class Enemy_controll : MonoBehaviour
{
    public static Enemy_controll Instance;
    [Header("Настраиваемое")]
    public List<Stage> stages;
    [SerializeField] float hand_move_speed;

    [Header("Не трогать")]    
    public float move_speed;
    public int skin_id, stickman_id;
    [SerializeField] Gate_controll gate;
    [SerializeField] GameObject hand, target, effect;
    float[] select_timer_min_max;    
    int level, life;
    bool select, frize_on, move, warrior, gaint, archer, end;
    [SerializeField] float timer, select_timer, gaint_timer, warrior_spawn_time, freez_timer, gaint_spawn_timer;
    GameObject sp;
    Vector3 hand_pos;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        hand_pos = hand.transform.position;
    }
    void Start()
    {
        
    }

    public void Set_level()
    {
        end = false;
        timer = 0.1f;
        level = PlayerPrefs.GetInt("level", 0);
        move = false;
        select = false;

        if (level < stages.Count)
        {
            life = stages[level].life;
            freez_timer = stages[level].frize_timer;
            move_speed = stages[level].move_speed;
            warrior_spawn_time = stages[level].warrior_spawn_timer;
            gaint_spawn_timer = stages[level].gaint_spawn_timer;

            warrior = stages[level].warrior;
            gaint = stages[level].gaint;
            archer = stages[level].archer;

            select_timer_min_max = stages[level].select_timer_min_max;
        }
        else
        {
            life = stages[stages.Count - 1].life;
            freez_timer = stages[stages.Count - 1].frize_timer;
            move_speed = stages[stages.Count - 1].move_speed;
            warrior_spawn_time = stages[stages.Count - 1].warrior_spawn_timer;
            gaint_spawn_timer = stages[stages.Count - 1].gaint_spawn_timer;

            warrior = stages[stages.Count - 1].warrior;
            gaint = stages[stages.Count - 1].gaint;
            archer = stages[stages.Count - 1].archer;

            select_timer_min_max = stages[stages.Count - 1].select_timer_min_max;
        }

        select_timer_min_max[0] = select_timer_min_max[0] - 0.065f * level + PlayerPrefs.GetFloat("add_time");
        if (select_timer_min_max[0] < 0.75)
            select_timer_min_max[0] = 0.75f;
        select_timer_min_max[1] = select_timer_min_max[1] - 0.2f * level + PlayerPrefs.GetFloat("add_time");
        if (select_timer_min_max[1] < 1)
            select_timer_min_max[1] = 1f;

        stickman_id = (Random.Range(0, 4) == 0 ? 0 : 1);
        skin_id = Random.Range(0, 5);
        hand.transform.position = hand_pos;
        gaint_timer = gaint_spawn_timer;
        if (archer)
            Spawn(2);
    }

    public int[] Get_count()
    {
        if (level < stages.Count)
        {
            int[] i = stages[level].boost_variant;
            return i;
        }
        else
        {
            int[] i = stages[stages.Count - 1].boost_variant;
            return i;
        }        
    }
    public int Get_win_money()
    {
        if (level < stages.Count)
        {
            int i = stages[level].win_money;
            return i;
        }
        else
        {
            int i = stages[stages.Count - 1].win_money;
            return i;
        }
    }

    void Update()
    {
        if (Game_Controll.Instance.game && !EndEffect.Instance.end)
        {
            if (warrior)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = warrior_spawn_time;
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
            
            if(move && target != null)
            {
                hand.transform.position = Vector3.MoveTowards(hand.transform.position, target.transform.position, hand_move_speed * Time.deltaTime);
                if (hand.transform.position == target.transform.position)
                {
                    if (Sound.Instance != null)
                        Sound.Instance.Play_Sound(0);
                    hand.transform.position = hand_pos;
                    //gate.Set_text(target.GetComponent<Button>().count);

                    if(target.GetComponent<Button>().count != 0)
                    {
                        gate.Set_spawn(target.GetComponent<Button>().count);
                        target.GetComponent<Button>().Get();
                        StartCoroutine(Effect());
                        StartCoroutine(Freez_timer());
                    }                   
                    //target.GetComponent<Button>().Off();
                    move = false;
                }

                //if (target.GetComponent<Button>().count != 0)
                //{
                    
                //}
                //else
                //{
                //    target = Buttons_controll.Instance.Best();
                //    if(target != null)
                //    {
                //        if (target.GetComponent<Button>().count < gate.count)
                //        {
                //            move = false;
                //            select = true;
                //            hand.transform.position = hand_pos;
                //        }
                //    }
                //    else
                //    {
                //        hand.transform.position = hand_pos;
                //    }
                //}
            }
        }        
    }
   
    public void Start_select()
    {
        select_timer = Random.Range(select_timer_min_max[0], select_timer_min_max[1]);
        select = true;
    }
    void Hand_move()
    {
        if(Buttons_controll.Instance.Best() != null && gate.count < Buttons_controll.Instance.Best().GetComponent<Button>().count)
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
                sp.GetComponent<Enemy>().spawn = true;
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
        if (life <= 0 && !end)
        {
            end = true;
            EndEffect.Instance.Win();
            //Game_Controll.Instance.Win();
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
    public void New_level()
    {

    }
    IEnumerator Effect()
    {
        effect.SetActive(true);
        yield return new WaitForSeconds(0.6f);
        effect.SetActive(false);
    }
}

