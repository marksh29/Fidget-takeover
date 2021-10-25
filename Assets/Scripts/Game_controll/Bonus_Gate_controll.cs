using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bonus_Gate_controll : MonoBehaviour
{
    public static Bonus_Gate_controll Instance;
    [SerializeField] Text count_text;
    [SerializeField] GameObject gate_wall, sp, effect;
    public int xx, count, rot;
    [SerializeField] float timer;
    bool time_on;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;        
    }
    private void OnEnable()
    {
        timer = Random.Range(3, 5);
        int level = PlayerPrefs.GetInt("level");
        int lvl = level - (5 * (int)(level / 5));
        gameObject.SetActive(lvl != 4 ? false : true);
    }
    void Start()
    {
        
    }   
    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {            
            if(time_on)
            {              
                timer = Random.Range(3, 5);                
                time_on = false;
            }
            else
            {
                timer = Random.Range(5, 10);
                Set_text();
                time_on = true;
            }
        }
    }

    public void Set_text()
    {
        count = Random.Range(1, 20);
        count_text.text = "+" + count;
        gate_wall.SetActive(true);

        timer = Random.Range(5, 10);
        time_on = true;
    }

    public void Set_spawn(int id)
    {
        if (time_on)
        {
            rot = id;
            StartCoroutine(Effect());
            StartCoroutine(Spawn(count));
            Drop();
        }       
    }
    IEnumerator Effect()
    {
        effect.SetActive(true);
            yield return new WaitForSeconds(1f);
        effect.SetActive(false);
    }
    IEnumerator Spawn(int id)
    {
        while(id > 0)
        {
            Spawn_obj();
            id--;
            yield return new WaitForSeconds(0.1f);
        }
        yield return null;               
    }
    void Spawn_obj()
    {
        sp = PoolControll.Instance.Spawn(rot == 1 ? "en_warrior" : "pl_warrior", 0);
        sp.transform.position = new Vector3(Random.Range(-xx, xx), 0, transform.position.z);
        sp.transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + (rot == 0 ? 0 : 180), transform.rotation.z);
    }
    public void Drop()
    {
        count = 0;
        count_text.text = "";
        gate_wall.SetActive(false);
        timer = Random.Range(3, 5);
        time_on = false;
    }  
}
