using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RandomObject : MonoBehaviour
{
    public static RandomObject Instance;
    public int id;
    GameObject cur_obj;
    [SerializeField] bool objects, material, skined;
    [SerializeField] GameObject[] obj;    
    [SerializeField] Material[] mat;
    [SerializeField] bool boss, stickman;
    [SerializeField] Enemy enem;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        
    }   
    private void OnEnable()
    {
        Random_on();
    }
    public void Random_on()
    {
        if (objects)
        {
            int r = new int();
            if (!stickman)
                r = Random.Range(0, obj.Length);
            else
            {
                r = Enemy_controll.Instance.stickman_id;
                enem.body = obj[r];
            }

            for (int i = 0; i < obj.Length; i++)
            {
                obj[i].SetActive(i == r ? true : false);
            }
 
            if (boss)
            {
                obj[r].GetComponent<Animator>().SetTrigger(Random.Range(1, 3).ToString());
            }                           
        }        
        if(material)
        {
            GetComponent<MeshRenderer>().sharedMaterial = mat[Random.Range(0, mat.Length)];
        }
        if (skined)
        {
            GetComponent<SkinnedMeshRenderer>().sharedMaterial = Mat();
        }
    }
    Material Mat()
    {
        Material mater = mat[(stickman == false ? Random.Range(0, mat.Length) : Enemy_controll.Instance.skin_id)];
        return mater;
    }
}
