using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus_Gate_controll : MonoBehaviour
{
    public static Bonus_Gate_controll Instance;
    [SerializeField] bool enemy;
    [SerializeField] Text count_text;
    [SerializeField] GameObject player_prefab, gate_wall;
    public int xx, count; 
    [SerializeField] Material[] mat;
    [SerializeField] float gate_timer;
    float timer;
    bool time_on;
    GameObject sp;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;        
    }
    void Start()
    {
        int level = PlayerPrefs.GetInt("level");
        int lvl = level - (5 * (int)(level / 5));
        gameObject.SetActive(lvl != 4 ? false : true);
    }   
    private void Update()
    {
        if(time_on)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                Set_text(Random.Range(1, 10));
            }
        }
    }

    public void Set_text(int id)
    {
        count = id;
        count_text.text = "+" + count;
        gate_wall.SetActive(true);

        timer = gate_timer;
        time_on = true;
    }

    public void Set_spawn(int id)
    {
        count = id;
        StartCoroutine(Spawn(count)); 
    } 
    IEnumerator Spawn(int id)
    {
        while(id > 0)
        {
            Spawn_obj();
            id--;
            yield return new WaitForSeconds(0.1f);
        }
        Drop();
        yield return null;               
    }
    void Spawn_obj()
    {
        sp = PoolControll.Instance.Spawn(enemy ? "en_warrior" : "pl_warrior", 0);
        sp.transform.position = new Vector3(Random.Range(-xx, xx), 0, transform.position.z + Random.Range(1.5f, 3));
        sp.transform.rotation = transform.rotation;
    }
    public void Drop()
    {
        time_on = false;
        count = 0;
        count_text.text = "";
        gate_wall.SetActive(false);
    }
    IEnumerator Start_timer(float time)
    {
        yield return new WaitForSeconds(time);
        timer = Random.Range(5, 10);
        time_on = true;
    }
}
