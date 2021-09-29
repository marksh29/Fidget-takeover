using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money_controll : MonoBehaviour
{
    public static Money_controll Instance;
    [SerializeField] int money;
    [SerializeField] Text money_text;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        money = PlayerPrefs.GetInt("money");
        money_text.text = money.ToString();
    }
    void Update()
    {
        
    }
    public void Change_money(int count)
    {
        money += count;
        if (money < 0)
            money = 0;
        money_text.text = money.ToString();
        PlayerPrefs.SetInt("money", money);
    }
}
