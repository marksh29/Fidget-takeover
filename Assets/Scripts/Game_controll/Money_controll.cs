using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money_controll : MonoBehaviour
{
    public static Money_controll Instance;
    public int money, end_money;
    [SerializeField] Text end_money_text;
    [SerializeField] Text[] money_text;
    [SerializeField] int[] end_money_list;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        money = PlayerPrefs.GetInt("money");
        money_text[0].text = money.ToString();
        money_text[1].text = money.ToString();
    }
    void Update()
    {
        
    }

    public void End_money()
    {
        end_money = end_money_list[PlayerPrefs.GetInt("level")];
        end_money_text.text = "+" + end_money;
        PlayerPrefs.SetInt("money", money + end_money);
        StartCoroutine(Money_count(money + end_money));
    }

    IEnumerator Money_count(int mn)
    {
        while(money < mn)
        {
            money += 1;
            end_money -= 1;
            money_text[0].text = money.ToString();
            money_text[1].text = money.ToString();
            end_money_text.text = "+" + end_money;
            yield return null;
        }
    }

    public void Change_money(int count)
    {
        money += count;
        if (money < 0)
            money = 0;
        money_text[0].text = money.ToString();
        money_text[1].text = money.ToString();
        PlayerPrefs.SetInt("money", money);
    }
}
