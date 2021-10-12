using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct Unit
{
    public int cena;
    public string name;
}
public class ShopPanel : MonoBehaviour
{
    int cur_id;
    [SerializeField] List<Unit> list;
    [SerializeField] GameObject[] all_buttons;
    [SerializeField] Text buy_text;
    [SerializeField] GameObject buy_button;
    [SerializeField] Sprite buy_sprt;
    private void Start()
    {
        PlayerPrefs.SetInt("buy_" + list[0].name, 1);
        Selected(0);
    }
    void Update()
    {
        
    }
    public void Selected(int id)
    {
        cur_id = id;
        for (int i = 0; i < all_buttons.Length; i++)
        {
            if (i == cur_id)
            {
                all_buttons[i].transform.GetChild(0).gameObject.SetActive(true);
                if (PlayerPrefs.GetInt("buy_" + list[i].name) == 1)
                {
                    buy_button.SetActive(false);
                }
                else
                {
                    buy_button.SetActive(true);
                    buy_text.text = list[cur_id].cena.ToString();
                }
            }
            else
            {
                all_buttons[i].transform.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
    public void Buy()
    {
        if (list[cur_id].cena <= Money_controll.Instance.money)
        {
            GameAnalityc.Instance.Buy_unit(list[cur_id].name);

            if (Sound.Instance != null)
                Sound.Instance.Play_Sound(4);

            Money_controll.Instance.Change_money(-list[cur_id].cena);
            PlayerPrefs.SetInt("buy_" + list[cur_id].name, 1);
            buy_button.SetActive(false);
        }
    }
}
