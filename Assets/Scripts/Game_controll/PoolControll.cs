using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControll : MonoBehaviour
{
    public static PoolControll Instance;
    [SerializeField] private GameObject enemy_prefab, player_prefab, blood_prefab;
    [SerializeField] private List<GameObject> enemy_stack, player_stack, blood_stack;
    GameObject new_en, new_pl, new_blood;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;        
    }    
    void Start()
    {

    }
    public GameObject Spawn_enemy()
    {
        bool not_empty = false;       
        for (int i = 0; i < enemy_stack.Count; i++)
        {
            if (!enemy_stack[i].activeSelf)
            {
                enemy_stack[i].SetActive(true);
                not_empty = true;
                new_en = enemy_stack[i];
                break;
            }
        }
        if (not_empty == false)
        {
            GameObject new_enemy = Instantiate(enemy_prefab) as GameObject;
            new_enemy.SetActive(true);
            enemy_stack.Add(new_enemy);
            new_en = new_enemy;
        }
        return new_en;
    }
    public GameObject Spawn_player()
    {
        bool not_empty2 = false;
        for (int i = 0; i < player_stack.Count; i++)
        {
            if (!player_stack[i].activeSelf)
            {
                player_stack[i].SetActive(true);
                not_empty2 = true;
                new_pl = player_stack[i];
                break;
            }
        }
        if (not_empty2 == false)
        {
            GameObject new_player = Instantiate(player_prefab) as GameObject;
            new_player.SetActive(true);
            player_stack.Add(new_player);
            new_pl = new_player;
        }
        return new_pl;
    }

    public GameObject Spawn_blood(int id)
    {
        bool not_empty3 = false;
        for (int i = 0; i < blood_stack.Count; i++)
        {
            if (!blood_stack[i].activeSelf)
            {
                blood_stack[i].SetActive(true);
                not_empty3 = true;
                new_blood = blood_stack[i];
                break;
            }
        }
        if (not_empty3 == false)
        {
            GameObject new_bl = Instantiate(blood_prefab) as GameObject;
            new_bl.SetActive(true);
            blood_stack.Add(new_bl);
            new_blood = new_bl;
        }
        new_blood.GetComponent<Blood>().Set_color(id);
        return new_blood;
    }
}
