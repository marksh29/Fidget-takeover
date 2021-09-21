using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons_controll : MonoBehaviour
{
    public static Buttons_controll Instance;
    [SerializeField] float timer, time;
    [SerializeField] GameObject[] buttons;
    public GameObject best;
    [SerializeField] List<GameObject> list;

    [SerializeField] Font nide_font;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        timer = 3;

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().font = nide_font;
            buttons[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 50);
        }
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if(timer <= 0)
        {
            timer = time;
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].GetComponent<Button>().Off();
            }
            best = null;
            Buttons_on();
        }
    }
    void Buttons_on()
    {
        list.Clear();
        list = new List<GameObject>(buttons);
        for (int i = 0; i < buttons.Length - Random.Range(2, 4); i++)
        {
            list.Remove(list[Random.Range(0, list.Count)]);
        }
        for (int i = 0; i < list.Count; i++)
        {
            list[i].GetComponent<Button>().On_true();
        }
        Enemy_controll.Instance.Start_select();
    }
    public void Remove_button(GameObject obj)
    {
        list.Remove(obj);
    }
    public GameObject Best()
    {
        best = null;        
        for (int i = 0; i < list.Count; i++)
        {
            if (best == null || best != null && best.GetComponent<Button>().count < list[i].GetComponent<Button>().count)
            {
                best = list[i];
            }
        }
        return best;
    }
}
