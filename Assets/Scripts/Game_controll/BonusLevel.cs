using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusLevel : MonoBehaviour
{
    [SerializeField] List<Unit> units;
    [SerializeField] int[] bonus_money;
    [SerializeField] Transform[] buttons;
    [SerializeField] GameObject close_button;
    [SerializeField] Text try_text, best_text;
    int try_int, best_unit;

    void Start()
    {
    }
    private void OnEnable()
    {
        PlayerPrefs.DeleteAll();
        close_button.SetActive(false);
        try_int = 3;
        try_text.text = "TRY: " + try_int + "/3";
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].GetChild(2).gameObject.SetActive(true);
            bonus_money[i] = 100 * (Random.Range(1, 11));
            buttons[i].GetChild(0).gameObject.GetComponent<Text>().text = "+" + bonus_money[i];
            buttons[i].GetChild(1).gameObject.SetActive(false);
        }
        List<int> list = new List<int>();
        for (int i = 0; i < units.Count; i++)
        {
            if (PlayerPrefs.GetInt("buy_" + units[i].name) == 0)
            {
                list.Add(i);
            }
        }

        if (list.Count != 0)
        {
            int rand_butt = Random.Range(0, buttons.Length);
            int rand_unit = list[Random.Range(0, list.Count)];
            bonus_money[rand_butt] = rand_unit;
            buttons[rand_butt].GetChild(0).gameObject.GetComponent<Text>().text = units[rand_unit].name;            
            Set_best(rand_unit);
        }
        else
        {
            Set_best(Best_loot());
        }
    }        
    int Best_loot()
    {
        best_unit = 0;
        for (int i = 0; i < bonus_money.Length; i++)
        {
            if (best_unit < bonus_money[i])
                best_unit = bonus_money[i];
        }
        return best_unit;
    }    
    void Set_best(int id)
    {
        if(id < 100)
        {
            best_text.text = units[id].name;

        }
        else
        {
            best_text.text = "+" + best_unit;
        }
    }
    public void Get_bonus(int id)
    {
        if(try_int > 0)
        {
            buttons[id].GetChild(2).gameObject.SetActive(false);
            Add(bonus_money[id]);           
            try_int--;
            try_text.text = "TRY: " + try_int + "/3";
            if (try_int <= 0)
                close_button.SetActive(true);
        }        
    }
    void Add(int id)
    {
        if(id >= 100)
        {
            Money_controll.Instance.Change_money(id);
        }
        else
        {
            switch (id)
            {
                case (0):
                    PlayerPrefs.SetInt("buy_Gaint", 1);
                    break;
                case (1):
                    PlayerPrefs.SetInt("buy_Archer", 1);
                    break;
                case (2):
                    PlayerPrefs.SetInt("buy_Rocket", 1);
                    break;
            }
        }
    }   
}
