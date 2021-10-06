using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEffect : MonoBehaviour
{
    public static EndEffect Instance;
    [SerializeField] GameObject[] prefabs;
    [SerializeField] GameObject stay_prefabs, spawn_pos, explos, boss;
    [SerializeField] GameObject[] end_object, warriors, gaints, archers;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        boss.GetComponent<Animator>().SetTrigger(Random.Range(1, 6).ToString());
    }  
    public void Win()
    {
        Game_Controll.Instance.game = false;
        PoolControll.Instance.Win();
        for (int i = 0; i < end_object.Length; i++)
        {
            end_object[i].SetActive(false);
        }
        for(int i = 0; i < 3; i++)
        {
            int r = Random.Range(0, warriors.Length);
            warriors[r].SetActive(true);
            warriors[r].GetComponent<Players>().Win();
        }
        if (PlayerPrefs.GetInt("buy_Gaint") == 1)
        {
            int r = Random.Range(0, gaints.Length);
            gaints[r].SetActive(true);
            gaints[r].GetComponent<Players>().Win();
        }            
        if (PlayerPrefs.GetInt("buy_Arche") == 1)
        {
            int r = Random.Range(0, archers.Length);
            archers[r].SetActive(true);
            archers[r].GetComponent<Archer>().Win();
        }
        Camera.main.gameObject.GetComponent<Animator>().SetTrigger("win");
        StartCoroutine(On());
    }
    public void Lose()
    {
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
        boss.SetActive(false);
        explos.SetActive(true);
        stay_prefabs.SetActive(true);
        Game_Controll.Instance.Win();
        StartCoroutine(Effect());        
    }
    IEnumerator Effect()
    {
        GameObject obj = Instantiate(prefabs[Random.Range(0, prefabs.Length)]) as GameObject;
        obj.transform.position = new Vector3(spawn_pos.transform.position.x + Random.Range(-25, 25), spawn_pos.transform.position.y + Random.Range(-10, 10), spawn_pos.transform.position.z);
        yield return new WaitForSeconds(1);
        StartCoroutine(Effect());       
    }
}
