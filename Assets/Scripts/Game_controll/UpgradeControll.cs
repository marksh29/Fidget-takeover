using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeControll : MonoBehaviour
{
    [SerializeField] string name;
    [SerializeField] Transform[] obj;
    [SerializeField] int[] cena, count;

    private void OnEnable()
    {
        
    }
    void Start()
    {
        for (int i = 0; i < count.Length; i++)
        {
            count[i] = PlayerPrefs.GetInt(name + i);
        }
        Visual();
    }    
    public void Buy_upgrade(int id)
    {
        if(cena[id] + (cena[id] * count[id]) <= Money_controll.Instance.money)
        {
            Money_controll.Instance.Change_money(-(cena[id] + (cena[id] * count[id])));
            count[id]++;
            PlayerPrefs.SetInt(name + id, count[id]);
            Visual();
        }        
    }
    void Visual()
    {
        for (int i = 0; i < count.Length; i++)
        {
            obj[i].GetChild(0).gameObject.GetComponent<Text>().text = (cena[i] + (cena[i] * count[i])).ToString();
            obj[i].GetChild(1).gameObject.GetComponent<Text>().text = "LEVEL " + (count[i] + 1);
            obj[i].GetChild(2).gameObject.SetActive(Money_controll.Instance.money < (cena[i] + (cena[i] * count[i])) ? false : true);
        }
    }
}
