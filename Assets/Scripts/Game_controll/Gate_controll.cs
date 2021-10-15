using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gate_controll : MonoBehaviour
{
    public static Gate_controll Instance;
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
        
    }
    private void OnEnable()
    {
        
    }
    private void Update()
    {
        if(time_on)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
            {
                time_on = false;
                count = 0;
                count_text.text = "";
                gate_wall.SetActive(false);
            }
        }
    }
    public void Set_text(  int id)
    {
        count = id;
        count_text.text = "+" + count;
        gate_wall.SetActive(true);

        timer = gate_timer;
        time_on = true;
        //GetComponent<MeshRenderer>().material = mat[1];
    }

    public void Set_spawn()
    {
        StartCoroutine(Spawn(count));
        //count = 0;
        //count_text.text = "";
        //gate_wall.SetActive(false);
        //GetComponent<MeshRenderer>().material = mat[0];        
    }  
    IEnumerator Spawn(int id)
    {
        for (int i = 0; i < id; i++)
        {
            sp = PoolControll.Instance.Spawn(enemy ? "en_warrior" : "pl_warrior", 0);
            sp.transform.position = new Vector3(Random.Range(-xx, xx), 0, transform.position.z + Random.Range(1.5f, 3));
            sp.transform.rotation = transform.rotation;       
            yield return new WaitForSeconds(0.1f);
        }
    } 
    public void Drop()
    {
        time_on = false;
        count = 0;
        count_text.text = "";
        gate_wall.SetActive(false);
    }
}
