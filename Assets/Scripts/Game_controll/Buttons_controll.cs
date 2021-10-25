using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Buttons_controll : MonoBehaviour
{
    public static Buttons_controll Instance;
    [SerializeField] float time;
    [SerializeField] GameObject[] buttons, all_buttons;
    public GameObject best;
    [SerializeField] List<GameObject> list;
    [SerializeField] int[] count;    
    [SerializeField] Font nide_font;
    float timer;
    public bool game;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        Start_game();
    }
    public void Start_game()
    {
        game = true;
        timer = 3;
        count = Enemy_controll.Instance.Get_count();

        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().font = nide_font;
            buttons[i].transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 50);
        }
    }
    void Update()
    {
        if (Game_Controll.Instance.game && game)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = time;
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].GetComponent<Button>().Full_off();
                }
                best = null;
                Buttons_on();
            }
        }       
    }
    void Buttons_on()
    {
        list.Clear();
        list = new List<GameObject>(buttons);
        List<int> count_list = new List<int>(count);
        for (int i = 0; i < buttons.Length - Random.Range(2, 4); i++)
        {
            list.Remove(list[Random.Range(0, list.Count)]);
        }
        for (int i = 0; i < list.Count; i++)
        {
            int r = Random.Range(0, count_list.Count);
            list[i].GetComponent<Button>().On_true(count_list[r]);
            count_list.Remove(count_list[r]);
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
    public void Drop()
    {
        game = false;
        for (int i = 0; i < all_buttons.Length; i++)
        {
            all_buttons[i].GetComponent<Button>().Start_Off();
        }
    }
}
