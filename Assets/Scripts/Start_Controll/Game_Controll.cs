﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game_Controll : MonoBehaviour
{
    public static Game_Controll Instance;
    public bool game, pause;
    [SerializeField] GameObject win_panel, shop_panel, abill_panel;

    public Slider load_slider;
    public Text load_text;
    public GameObject load_panel, start_panel, game_panel, lootbox_panel, pause_panel;
    [SerializeField] int level, lvl;
    [SerializeField] Transform[] level_icon;
    [SerializeField] Sprite[] level_sprt;

    [SerializeField] float game_timer, lvl_timer;

    public delegate void SomeAction();
    public event SomeAction reloadSkin;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (Instance == null)
            Instance = this;
    }
    private void Start()
    {
        GameAnalityc.Instance.Start_game();
        Next_level();     
    }   
    private void Update()
    {    
        if(Input.GetKey(KeyCode.Space))
        {
            EndEffect.Instance.Win();
        }
        if (!game && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.7f && Input.mousePosition.y > Screen.height * 0.3f && !shop_panel.activeSelf && !abill_panel.activeSelf && !pause_panel.activeSelf && Input.mousePosition.x < Screen.width * 0.85f && !win_panel.activeSelf)
        {
            EndEffect.Instance.Play_game();

            game = true;
            start_panel.SetActive(false);
            game_panel.SetActive(true);

            Player_controll.Instance.Set_level();
            Enemy_controll.Instance.Set_level();

            GameAnalityc.Instance.Start_level(level + 1);
        }
        if(game)
        {
            game_timer += Time.deltaTime;
            lvl_timer += Time.deltaTime;
        }
    }      
    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            pause = false;
            Time.timeScale = 1;
        }
        else
        {
            pause = true;
            Time.timeScale = 0f;
        }
    }
    public void Click()
    {
        if (Sound.Instance != null)
            Sound.Instance.Click();
    }
    public void Win()
    {
        PlayerPrefs.SetInt("level", level + 1);
        StartCoroutine(Open_panel("Win"));        
    }
    public void Lose()
    {
        StartCoroutine(Open_panel("Lose"));
    }   
    
    IEnumerator Open_panel(string name)
    {
        yield return new WaitForSeconds(3);
        switch (name)
        {
            case ("Win"):
                GameAnalityc.Instance.Win_level(level + 1, (int)lvl_timer);
                game_panel.SetActive(false);
                win_panel.SetActive(true);
                Money_controll.Instance.End_money(true);
                break;
            case ("Lose"):
                GameAnalityc.Instance.Lose_level(level + 1, (int)lvl_timer);
                game_panel.SetActive(false);
                win_panel.SetActive(true);
                Money_controll.Instance.End_money(false);
                break;
        }       
    }
    public void Next_level()
    {
        PoolControll.Instance.DisableAll();        
        start_panel.SetActive(true);
        game_panel.SetActive(false);
        win_panel.SetActive(false);
        level = PlayerPrefs.GetInt("level");
        lvl = level - (5 * (int)(level / 5));
        level_icon[lvl].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        for (int i = 0; i < level_icon.Length; i++)
        {
            if (i < lvl)
            {
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[2];
                level_icon[i].localScale = new Vector3(0.8f, 0.8f, 1);
            }
            else if (i == lvl)
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[lvl != 4 ? 0 : 1];
        }
        EndEffect.Instance.Off_all();
        Reload_skin();
        Camera.main.gameObject.GetComponent<Animator>().SetTrigger("stay");
    }
    void Reload_skin()
    {
        reloadSkin?.Invoke();
    }

    public void Play_game()
    {
        game = true;
        start_panel.SetActive(false);
        game_panel.SetActive(true);

        Player_controll.Instance.Set_level();
        Enemy_controll.Instance.Set_level();
    }
    public void Lootbox()
    {
        //lootbox_panel.SetActive(true);
    }
    private void OnApplicationPause(bool pause)
    {
        GameAnalityc.Instance.Game_timer(game_timer);

        PlayerPrefs.SetFloat("game_timer", 0);
        GameAnalityc.Instance.Close_game(level + 1);
        PlayerPrefs.SetInt("First", 1);
    }
}
