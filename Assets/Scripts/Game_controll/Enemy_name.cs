using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_name : MonoBehaviour
{
    [SerializeField] Image flag_img;
    [SerializeField] Sprite[] all_sprt;
    [SerializeField] Text en_name;
    [SerializeField] string[] all_name;

    void Start()
    {
        flag_img.sprite = all_sprt[Random.Range(0, all_sprt.Length)];
        en_name.text = all_name[Random.Range(0, all_name.Length)];
    }
    void Update()
    {
        
    }
}
