using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Game_Controll : MonoBehaviour
{
    public static Game_Controll Instance;
    public bool game, pause;
    [SerializeField] GameObject lose_panel, win_panel, shop_panel, abill_panel;

    public Slider load_slider;
    public Text load_text;
    public GameObject load_panel, start_panel, game_panel, lootbox_panel, pause_panel;
    [SerializeField] int level;
    [SerializeField] Transform[] level_icon;
    [SerializeField] Sprite[] level_sprt;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (Instance == null)
            Instance = this;
       // PlayerPrefs.DeleteAll();
    }
    private void Start()
    {
        level = PlayerPrefs.GetInt("level");
        int lvl = level - (5 * (int)(level / 5));
        level_icon[lvl].localScale = new Vector3(1.2f, 1.2f, 1.2f);
        for(int i = 0; i < level_icon.Length; i++)
        {
            if(i < lvl)
            {
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[2];
                level_icon[i].localScale = new Vector3(0.8f, 0.8f, 1);
            }                
            else if(i == lvl)
                level_icon[i].gameObject.GetComponent<Image>().sprite = level_sprt[lvl != 4 ? 0 : 1];            
        }        
    }
    private void Update()
    {    
        if(Input.GetKey(KeyCode.Space))
        {
            EndEffect.Instance.Win();
        }

        if (!game && Input.GetMouseButtonDown(0) && Input.mousePosition.y < Screen.height * 0.7f && Input.mousePosition.y > Screen.height * 0.3f && !shop_panel.activeSelf && !abill_panel.activeSelf && !pause_panel.activeSelf && Input.mousePosition.x < Screen.width * 0.85f)
        {
            game = true;
            start_panel.SetActive(false);
            game_panel.SetActive(true);

            Player_controll.Instance.Set_level();
            Enemy_controll.Instance.Set_level();
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
    public void Continue()
    {
        game = true;
        Time.timeScale = 1;
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        SceneManager.LoadScene("Game");
    }

    public void Win()
    {
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
                game_panel.SetActive(false);
                win_panel.SetActive(true);

                Money_controll.Instance.End_money(true);
                break;
            case ("Lose"):                
                game_panel.SetActive(false);
                win_panel.SetActive(true);
                Money_controll.Instance.End_money(false);
                break;
        }       
    }
    public void Load_level(string name)
    {
        Time.timeScale = 1;
        load_panel.SetActive(true);
        StartCoroutine(Load(name));
    }
    IEnumerator Load(string name)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
        asyncLoad.allowSceneActivation = false;

        while (!asyncLoad.isDone)
        {
            load_slider.value = asyncLoad.progress;
            load_text.text = (load_slider.value * 100).ToString("f0") + "%";
            if (asyncLoad.progress >= 0.9f && !asyncLoad.allowSceneActivation)
            {
                asyncLoad.allowSceneActivation = true;
            }
            yield return null;
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
}
