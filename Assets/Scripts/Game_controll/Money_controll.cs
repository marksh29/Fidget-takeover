using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money_controll : MonoBehaviour
{
    public static Money_controll Instance;
    public float money, end_money, add_money;
    [SerializeField] Text end_money_text, end_logo;
    [SerializeField] Text[] money_text;
    [SerializeField] List<Money> list;
    [SerializeField] GameObject[] all_money;
    bool win;
    int add_count;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        money = PlayerPrefs.GetFloat("money");
    }
    void Start()
    {
        Mon();
    }   
    public void End_money(bool wn)
    {
        StopAllCoroutines();
        win = wn;
        for(int i =0; i < all_money.Length; i++)
        {
            all_money[i].SetActive(true);
        }

        if (win)
        {
            end_logo.text = "VICTORY";
            end_money = Enemy_controll.Instance.Get_win_money() + (50 * PlayerPrefs.GetInt("Upgrade2"));                  
            //PlayerPrefs.SetInt("money", money + end_money);  
        }
        else
        {
            end_logo.text = "YOU LOSE";
            end_money = 500;
            //PlayerPrefs.SetInt("money", money + end_money);
        }
        end_money_text.text = "+" + end_money;
        add_money = end_money / all_money.Length;

        //StartCoroutine(Money_count());
        add_count = list.Count;
        StartCoroutine(Money_coins());
    }
    IEnumerator Money_count()
    {        
        yield return new WaitForSeconds(1);
        Game_Controll.Instance.Next_level();
    }
    public void Change_money(float count)
    {
        money += count + 0.0001f;
        if (money < 0)
            money = 0;
        Mon();
        PlayerPrefs.SetFloat("money", money);
    }
    IEnumerator Money_coins()
    {
        List<Money> list2 = new List<Money>(list);
        while (list.Count > 0)
        {            
            int d = Random.Range(1, 3);
            if (d > list2.Count)
                d = list2.Count;
            for(int i = 0; i < d; i++)
            {
                int r = Random.Range(0, list2.Count);
                list2[r].Start_move();
                list2.Remove(list2[r]);
            }            
            yield return new WaitForSeconds(0.1f);            
        }        
    }
    public void Add()
    {
        add_count--;
        if(add_count <= 0)
            StartCoroutine(Money_count());
        Change_money(add_money);
        end_money -= add_money;
    }
    void Mon()
    {
        for(int i =0; i < money_text.Length; i++)
        {
            money_text[i].text = money.ToString(money > 0.1f ? "0,0" : "f0");
        }
    }
}
