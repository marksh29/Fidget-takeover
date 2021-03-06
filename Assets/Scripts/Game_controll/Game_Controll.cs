using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class Game_Controll : MonoBehaviour
{
    public static Game_Controll Instance;
    public bool game, pause, end;
    [SerializeField] GameObject win_panel, shop_panel, abill_panel, upgrade_panel, butt_shop, butt_ability, game_ability_panel;

    [SerializeField] Text level_tex;

    public GameObject start_panel, game_panel, lootbox_panel, pause_panel;
    [SerializeField] int level, lvl;
    [SerializeField] Transform[] level_icon;
    [SerializeField] Sprite[] level_sprt;

    [SerializeField] float game_timer, lvl_timer;

    public delegate void ReloadSkin();
    public event ReloadSkin reloadSkin;

    [SerializeField] List<RandomObject> list_random;
     
    private void Awake()
    {
        //PlayerPrefs.DeleteAll();       
        Screen.orientation = ScreenOrientation.Portrait;
        if (Instance == null)
            Instance = this;
    }
    
    private void Start()
    {
        OneSignal.SetLogLevel(OneSignal.LOG_LEVEL.VERBOSE, OneSignal.LOG_LEVEL.NONE);
        GameAnalityc.Instance.Start_game();
        Next_level();     
    }   
    private void Update()
    {    
        if(Input.GetKey(KeyCode.Space))
        {
            EndEffect.Instance.Win();
        }
        if (!end && !game && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.7f && Input.mousePosition.y > Screen.height * (upgrade_panel.activeSelf ? 0.2f : 0) && !shop_panel.activeSelf && !abill_panel.activeSelf && !pause_panel.activeSelf && Input.mousePosition.x < Screen.width * 0.75f && !win_panel.activeSelf)
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
        FBsdk.Instance.End_level_fb();

        PlayerPrefs.SetFloat("add_time", 0);
        //PlayerPrefs.SetInt("level", level + 1);
        StartCoroutine(Open_panel("Win"));
    }
    public void Lose()
    {
        PlayerPrefs.SetFloat("add_time", 0.5f);
        StartCoroutine(Open_panel("Lose"));
    }   
    
    IEnumerator Open_panel(string name)
    {
        end = true;
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
        level = PlayerPrefs.GetInt("level");
        lvl = level - (5 * (int)(level / 5));
        level_tex.text = "Level " + (level + 1);
        PoolControll.Instance.DisableAll();

        end = false;
        lvl_timer = 0;
        
        start_panel.SetActive(true);
        game_panel.SetActive(false);
        win_panel.SetActive(false);

        for (int i = 0; i < level_icon.Length; i++)
        {
            if (i < lvl)
            {
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[2];
                level_icon[i].localScale = new Vector3(0.8f, 0.8f, 1);
            }
            else if (i == lvl)
            {
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[lvl != 4 ? 0 : 1];
                level_icon[lvl].localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }                
            else if(i > lvl)
            {
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[i == 4 ? 4 : 3];
                level_icon[i].localScale = new Vector3(0.8f, 0.8f, 1);
            }                
        }
        EndEffect.Instance.Off_all();

        upgrade_panel.SetActive(level >= 2 ? true : false);
        game_ability_panel.SetActive(level >= 5 ? true : false);
        butt_ability.SetActive(level >= 5 ? true : false);
        butt_shop.SetActive(level >= 1 ? true : false);

        //Buttons_controll.Instance.Start_game();

        Camera.main.gameObject.GetComponent<Animator>().SetTrigger("stay");
        for (int i = 0; i < list_random.Count; i++)
        {
            list_random[i].Random_on();
        }
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
