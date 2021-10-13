using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money_controll : MonoBehaviour
{
    public static Money_controll Instance;
    public int money, end_money;
    [SerializeField] Text end_money_text, end_logo;
    [SerializeField] Text[] money_text;
    [SerializeField] int[] end_money_list;
    [SerializeField] GameObject button;
    [SerializeField] List<Money> list;
    bool win;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        money = PlayerPrefs.GetInt("money");
    }
    void Start()
    {
        Mon();
    }   
    public void End_money(bool wn)
    {
        win = wn;
        if (win)
        {
            end_logo.text = "VICTORY";
            end_money = end_money_list[PlayerPrefs.GetInt("level")] + (50 * PlayerPrefs.GetInt("Upgrade2"));
            end_money_text.text = "+" + end_money;
            PlayerPrefs.SetInt("money", money + end_money);

            StartCoroutine(Money_count(money + end_money));
            StartCoroutine(Money_coins());
        }
        else
        {
            end_logo.text = "YOU LOSE";
            end_money = 100 + (50 * PlayerPrefs.GetInt("Upgrade2"));
            end_money_text.text = "+" + end_money;
            PlayerPrefs.SetInt("money", money + end_money);

            StartCoroutine(Money_count(money + end_money));
            StartCoroutine(Money_coins());
        }
    }

    IEnumerator Money_count(int mn)
    {
        while(money < mn)
        {
            if(end_money >= 50)
            {
                money += 50;
                end_money -= 50;
            }
            else
            {
                money += end_money;
                end_money -= end_money;
            }
            Mon();
            //end_money_text.text = (end_money != 0 ? "+" : "") + end_money;           
            yield return null;
        }        
        //button.transform.GetChild(0).gameObject.SetActive(true);
        //if (win)
        //    button.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        //if(win)
        //    Game_Controll.Instance.Lootbox();
        Game_Controll.Instance.Load_level("Game");
    }
    public void Change_money(int count)
    {
        money += count;
        if (money < 0)
            money = 0;
        Mon();
        PlayerPrefs.SetInt("money", money);
    }
    IEnumerator Money_coins()
    {
        while(list.Count > 0)
        {            
            int d = Random.Range(1, 4);
            for(int i = 0; i < d; i++)
            {
                int r = Random.Range(0, list.Count);
                list[r].Start_move();
                list.Remove(list[r]);
            }            
            yield return new WaitForSeconds(0.1f);            
        }        
    }
    void Mon()
    {
        for(int i =0; i < money_text.Length; i++)
        {
            money_text[i].text = money.ToString(money > 0 ? "0,0" : "");
        }
    }
}
