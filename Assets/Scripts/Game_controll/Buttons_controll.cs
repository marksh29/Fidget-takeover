using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons_controll : MonoBehaviour
{
    [SerializeField] float timer, time;
    [SerializeField] GameObject[] buttons;
    void Start()
    {
        timer = 3;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = 6;
            Buttons_on();
        }
    }
    void Buttons_on()
    {
        int ct = Random.Range(3, 5);
        List<GameObject> list = new List<GameObject>(buttons);
        for (int i = 0; i < list.Count - ct; i++)
        {
            list.Remove(list[Random.Range(0, list.Count)]);
        }
        for (int i = 0; i < list.Count; i++)
        {
            list[i].GetComponent<Button>().On_true();
        }
    }
}
