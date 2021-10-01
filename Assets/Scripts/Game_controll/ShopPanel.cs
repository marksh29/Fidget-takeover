using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Unit
{
    public int cena;
    public string name;
    public Sprite icon;
}
public class ShopPanel : MonoBehaviour
{
    [SerializeField] List<Unit> units;    
    [SerializeField] Transform[] all_buttons;

    private void OnEnable()
    {        
        for(int i = 0; i < all_buttons.Length; i++)
        {
            if(PlayerPrefs.GetInt("buy_" + units[i].name) == 0)
            {
                all_buttons[i].GetChild(0).gameObject.GetComponent<Text>().text = units[i].cena.ToString();
                all_buttons[i].GetChild(1).gameObject.GetComponent<Text>().text = units[i].name;
            }
            else
            {
                all_buttons[i].GetChild(0).gameObject.SetActive(false);
                all_buttons[i].GetChild(2).gameObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Buy(int id)
    {
        if (units[id].cena <= Money_controll.Instance.money)
        {
            Money_controll.Instance.Change_money(units[id].cena);
            PlayerPrefs.SetInt("buy_" + units[id].name, 1);
            all_buttons[id].GetChild(0).gameObject.SetActive(false);
            all_buttons[id].GetChild(2).gameObject.SetActive(false);
        }
    }
}
