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
    [SerializeField] GameObject button, coin_prefab, target;
    bool win;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        money = PlayerPrefs.GetInt("money");
    }
    void Start()
    {        
        money_text[0].text = money.ToString("0,0");
        money_text[1].text = money.ToString("0,0");
    }
    void Update()
    {
        
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
            if(end_money >= 5)
            {
                money += 5;
                end_money -= 5;
            }
            else
            {
                money += end_money;
                end_money -= end_money;
            }            
            money_text[0].text = money.ToString("0,0");
            money_text[1].text = money.ToString("0,0");
            end_money_text.text = (end_money != 0 ? "+" : "") + end_money;           
            yield return null;
        }        
        button.transform.GetChild(0).gameObject.SetActive(true);
        if (win)
            button.transform.GetChild(1).gameObject.SetActive(true);
        yield return new WaitForSeconds(4);
        if(win)
            Game_Controll.Instance.Lootbox();
    }

    IEnumerator Money_coins()
    {
        while (end_money > 0)
        {            
            GameObject obj = Instantiate(coin_prefab, coin_prefab.transform) as GameObject;
            StartCoroutine(DoMove(obj, 0.3f));
            yield return new WaitForSeconds(0.1f);
            yield return null;
        }
    }
    private IEnumerator DoMove(GameObject obj,  float time)
    {
        Vector2 startPosition = obj.transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            obj.transform.position = Vector2.Lerp(startPosition, target.transform.position, fraction);
            yield return null;
        }
        obj.SetActive(false);
    }   
    public void Change_money(int count)
    {
        money += count;
        if (money < 0)
            money = 0;
        money_text[0].text = money.ToString("0,0");
        money_text[1].text = money.ToString("0,0");
        PlayerPrefs.SetInt("money", money);
    }
}
