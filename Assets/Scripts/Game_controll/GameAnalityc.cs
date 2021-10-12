using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GameAnalityc : MonoBehaviour
{
    public static GameAnalityc Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        DontDestroyOnLoad(gameObject);
        GameAnalytics.Initialize();
    }
    void Start()
    {
        
    }  
    public void Game_timer(float time)
    {
        GameAnalytics.NewDesignEvent("Game timer: ", time);
        Debug.Log("GameTimer");
    }
    public void Level_timer(int lvl, float time)
    {
        GameAnalytics.NewDesignEvent("Timer for level " + (lvl +1).ToString(), time);
        Debug.Log("LevelTimer");
    }

    public void Buy_unit(string name)
    {
        GameAnalytics.NewDesignEvent("Buy: " + name);
        Debug.Log("BuyUnit");
    }
    public void Buy_upgrade(string name)
    {
        GameAnalytics.NewDesignEvent("Buy upgrade: " + name);
        Debug.Log("BuyUpgrade");
    }
    public void Buy_abillity(string name)
    {
        GameAnalytics.NewDesignEvent("Buy abillity: " + name);
        Debug.Log("BuyAbillity");
    }

    public void Start_game()
    {
        if (PlayerPrefs.GetInt("First") == 0)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Start game");
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "First start game");
        }
        Debug.Log("Start_game");
    }
    public void Start_level(int id)
    {
        if(PlayerPrefs.GetInt("First") == 0)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "Start level: ", id.ToString());
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, "First start level: ", id.ToString());
        }
    }
    public void Win_level(int id, int time)
    {
        if (PlayerPrefs.GetInt("First") == 0)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "Win level: ", id.ToString(), time);
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, "First win level: ", id.ToString(), time);
        }
        Debug.Log("Win_level");
    }
    public void Lose_level(int id, int time)
    {
        if (PlayerPrefs.GetInt("First") == 0)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "Lose level: ", id.ToString(), time);
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, "First lose level: ", id.ToString(), time);
        }
        Debug.Log("Lose_level");
    }

    public void Close_game(int id)
    {
        if (PlayerPrefs.GetInt("First") == 0)
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, "Close game in level: ", id.ToString());
        }
        else
        {
            GameAnalytics.NewProgressionEvent(GAProgressionStatus.Undefined, "First close game in level: ", id.ToString());
        }
        Debug.Log("Close");
    }
}
