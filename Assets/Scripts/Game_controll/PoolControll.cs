using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolControll : MonoBehaviour
{
    public static PoolControll Instance;
    [SerializeField] private GameObject enemy_prefab, player_prefab, blood_prefab, pl_gaint, en_gaint, arrow_prefab;
    [SerializeField] private List<GameObject> enemy_stack, player_stack, blood_stack, pl_gaints_stack, en_gaints_stack, arrow_stack;
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
                new_obj = Spawn(player_stack, player_prefab);
                break;
            case ("en_warrior"):
                new_obj = Spawn(enemy_stack, enemy_prefab);
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
}
