using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControll : MonoBehaviour
{
    public static PoolControll Instance;
    [SerializeField] private GameObject pl_warrior, pl_gaint, pl_archer, blood_prefab,  arrow_prefab;
    [SerializeField] private GameObject en_warrior, en_gaint, en_archer;
    [SerializeField] private List<GameObject> player_stack, pl_gaints_stack, pl_archer_stack, arrow_stack, blood_stack;
    [SerializeField] private List<GameObject> enemy_stack, en_gaints_stack, en_archer_stack;
    GameObject new_obj, new_blood;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;        
    }    
    void Start()
    {

    }
    public GameObject Spawn(string name, int color_id)
    {
        switch (name)
        {
            case ("blood"):
                new_obj = Spawn(blood_stack, blood_prefab);
                new_obj.GetComponent<Blood>().Set_color(color_id);
                break;
            case ("pl_warrior"):
                new_obj = Spawn(player_stack, pl_warrior);
                break;
            case ("en_warrior"):
                new_obj = Spawn(enemy_stack, en_warrior);
                break;
            case ("pl_gaint"):
                new_obj = Spawn(pl_gaints_stack, pl_gaint);
                break;
            case ("en_gaint"):
                new_obj = Spawn(en_gaints_stack, en_gaint);
                break;
            case ("arrow"):
                new_obj = Spawn(arrow_stack, arrow_prefab);
                new_obj.GetComponent<Arrow>().enemy = (color_id == 0 ? false : true);
                break;
            case ("pl_archer"):
                new_obj = Spawn(pl_archer_stack, pl_archer);
                break;
            case ("en_archer"):
                new_obj = Spawn(en_archer_stack, en_archer);
                break;
        }
        return new_obj;
    }
    GameObject Spawn(List<GameObject> list, GameObject prefab)
    {
        bool not_empty = false;
        for (int i = 0; i < list.Count; i++)
        {
            if (!list[i].activeSelf)
            {
                list[i].SetActive(true);
                not_empty = true;
                new_blood = list[i];
                break;
            }
        }
        if (not_empty == false)
        {
            GameObject new_obj = Instantiate(prefab) as GameObject;
            new_obj.SetActive(true);
            list.Add(new_obj);
            new_blood = new_obj;
        }
        return new_blood;
    } 

    public void Win()
    {
        for (int i = 0; i < player_stack.Count; i++)
        {
            player_stack[i].GetComponent<Players>().Win();
        }
        for (int i = 0; i < pl_gaints_stack.Count; i++)
        {
            pl_gaints_stack[i].GetComponent<Players>().Win();
        }
        for (int i = 0; i < pl_archer_stack.Count; i++)
        {
            pl_archer_stack[i].GetComponent<Archer>().Win();
        }
        for (int i = 0; i < enemy_stack.Count; i++)
        {
            enemy_stack[i].GetComponent<Enemy>().Damage(100);
        }
        for (int i = 0; i < en_gaints_stack.Count; i++)
        {
            enemy_stack[i].GetComponent<Enemy>().Damage(100);
        }
        for (int i = 0; i < en_archer_stack.Count; i++)
        {
            en_archer_stack[i].GetComponent<Archer>().Damage();
        }
    }
    public void Lose()
    {
        for (int i = 0; i < player_stack.Count; i++)
        {
            player_stack[i].GetComponent<Players>().Damage(100);
        }
        for (int i = 0; i < pl_gaints_stack.Count; i++)
        {
            pl_gaints_stack[i].GetComponent<Players>().Damage(100); 
        }
        for (int i = 0; i < pl_archer_stack.Count; i++)
        {
            pl_archer_stack[i].GetComponent<Archer>().Damage();
        }
        for (int i = 0; i < enemy_stack.Count; i++)
        {
            enemy_stack[i].GetComponent<Enemy>().Win();
        }
        for (int i = 0; i < en_gaints_stack.Count; i++)
        {
            enemy_stack[i].GetComponent<Enemy>().Win();
        }
        for (int i = 0; i < en_archer_stack.Count; i++)
        {
            en_archer_stack[i].GetComponent<Archer>().Win();
        }
    }
}
