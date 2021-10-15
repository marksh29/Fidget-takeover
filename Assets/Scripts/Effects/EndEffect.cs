using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEffect : MonoBehaviour
{
    public static EndEffect Instance;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] GameObject spawn_pos, boss, boss_effeect;
    [SerializeField] GameObject[] end_object, warriors, gaints, archers;
    int end_count;
    public bool end;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        
    }  
    public void Win()
    {
        if (!end)
        {
            Game_Controll.Instance.end = true;
            Game_Controll.Instance.game = false;
            List<GameObject> enemy_list = new List<GameObject>();

            if (Sound.Instance != null)
                Sound.Instance.Play_Sound(3);

            end_count = 5;
            end = true;
            
            PoolControll.Instance.Win();
            for (int i = 0; i < end_object.Length; i++)
            {
                end_object[i].SetActive(false);
            }
            if (PlayerPrefs.GetInt("buy_Gaint") == 1)
            {
                end_count--;
                int r = Random.Range(0, gaints.Length);
                gaints[r].SetActive(true);
                gaints[r].GetComponent<Players>().Win();
            }
            if (PlayerPrefs.GetInt("buy_Archer") == 1)
            {
                end_count--;
                int r = Random.Range(0, archers.Length);
                archers[r].SetActive(true);
                archers[r].GetComponent<Archer>().Win();
            }
            List<GameObject> list = new List<GameObject>(warriors);
            for (int i = 0; i < end_count; i++)
            {
                int r = Random.Range(0, list.Count);
                list[r].SetActive(true);
                list[r].GetComponent<Players>().Win();
                list.Remove(list[r]);
            }

            Camera.main.gameObject.GetComponent<Animator>().SetTrigger("win");
            StartCoroutine(On());
        }
    }    

    public void Lose()
    {
        if (!end)
        {
            Game_Controll.Instance.end = true;
            Game_Controll.Instance.game = false;

            if (Sound.Instance != null)
                Sound.Instance.Play_Sound(2);

            end = true;
            Game_Controll.Instance.Lose();
            PoolControll.Instance.Lose();
            for (int i = 0; i < end_object.Length; i++)
            {
                end_object[i].SetActive(false);
            }
        }
    }
    IEnumerator On()
    {
        yield return new WaitForSeconds(1.2f);
        Effect_on();
    }
    void Effect_on()
    {
        boss.SetActive(false);
        boss_effeect.SetActive(true);
        Game_Controll.Instance.Win();
        StartCoroutine(Effect());        
    }
    IEnumerator Effect()
    {
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;
        obj.transform.position = new Vector3(spawn_pos.transform.position.x + Random.Range(-20, 20), spawn_pos.transform.position.y + Random.Range(-8, 8), spawn_pos.transform.position.z);
        DestroyObject(obj, 3);
        yield return new WaitForSeconds(0.8f);
        StartCoroutine(Effect());       
    }
    public void Play_game()
    {       
        for (int i = 0; i < end_object.Length; i++)
        {
            end_object[i].SetActive(true);
        }
    }
    public void Off_all()
    {
        boss.SetActive(true);
        boss_effeect.SetActive(false);
        for (int i = 0; i < end_object.Length; i++)
        {
            if (i > 1)
                end_object[i].GetComponent<Gate_controll>().Drop();
            end_object[i].SetActive(false);            
        }
        for (int i = 0; i < warriors.Length; i++)
        {
            warriors[i].SetActive(false);
            if (i < 1)
                gaints[i].SetActive(false);
            if (i < 3)
                archers[i].SetActive(false);
        }
        StopAllCoroutines();
        end = false;
    }
}
