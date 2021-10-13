using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEffect : MonoBehaviour
{
    public static EndEffect Instance;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] GameObject stay_prefabs, spawn_pos, explos, boss;
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
        if (Sound.Instance != null)
            Sound.Instance.Play_Sound(3);

        end_count = 5;
        end = true;
        Game_Controll.Instance.game = false;
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
    public void Lose()
    {
        if (Sound.Instance != null)
            Sound.Instance.Play_Sound(2);

        end = true;
        Game_Controll.Instance.game = false;
        Game_Controll.Instance.Lose();
        PoolControll.Instance.Lose();
        for (int i = 0; i < end_object.Length; i++)
        {
            end_object[i].SetActive(false);
        }
    }
    IEnumerator On()
    {
        yield return new WaitForSeconds(1.2f);
        Effect_on();
    }
    void Effect_on()
    {
        //boss.SetActive(false);
        //explos.SetActive(true);
        stay_prefabs.SetActive(true);
        Game_Controll.Instance.Win();
        StartCoroutine(Effect());        
    }
    IEnumerator Effect()
    {
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;
        obj.transform.position = new Vector3(spawn_pos.transform.position.x + Random.Range(-25, 25), spawn_pos.transform.position.y + Random.Range(-10, 10), spawn_pos.transform.position.z);
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(Effect());       
    }
    public void Play_game()
    {
        for (int i = 0; i < end_object.Length; i++)
        {
            end_object[i].SetActive(true);
        }
    }
}
