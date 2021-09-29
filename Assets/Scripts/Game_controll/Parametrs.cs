using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public struct Stage
//{
//    public float spawn_timer;
//    public float move_speed;
//    public bool warrior;
//    public bool gaint;
//    public bool archer;
//}
public class Parametrs : MonoBehaviour
{
    public static Parametrs Instance;
    //public List<Stage> stages;

    [SerializeField] int level;

    public float spawn_timer;
    public float move_speed;
    public bool enemy_warrior;
    public bool enemy_gaint;
    public bool enemy_archer;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        //level = PlayerPrefs.GetInt("level", 0);
        //spawn_timer = stages[level].spawn_timer;
        //move_speed = stages[level].move_speed;
        //enemy_warrior = stages[level].warrior;
        //enemy_gaint = stages[level].gaint;
        //enemy_archer = stages[level].archer;
    }   
}
