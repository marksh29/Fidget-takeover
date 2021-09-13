using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn_player : MonoBehaviour
{

    [SerializeField] float timer, time;
    [SerializeField] GameObject obj;
    void Start()
    {
        
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
        Instantiate(obj, transform.position, transform.rotation);
    }
}
