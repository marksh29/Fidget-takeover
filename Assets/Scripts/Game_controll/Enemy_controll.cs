using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_controll : MonoBehaviour
{
    public static Enemy_controll Instance;
    [SerializeField] int life;
    [SerializeField] float timer, time;
    [SerializeField] GameObject obj;
    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Game_Controll.Instance.game)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = time;
                Spawn();
            }

        }        
    }
    void Spawn()
    {
        Instantiate(obj, new Vector3(0, 0, transform.position.z - 5), transform.rotation);        
    }
    public void Damage()
    {
        life--;
    }
}
