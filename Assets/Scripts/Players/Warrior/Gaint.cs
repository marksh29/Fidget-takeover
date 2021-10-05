using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaint : MonoBehaviour
{
    [SerializeField] bool enemy;
    [SerializeField] float dist_attack;
    [SerializeField] int count_attack;
    [SerializeField] List<GameObject> list, list2;

    void Start()
    {
        
    }
    public void Mass_attack(int damage)
    {
        list.Clear();
        list2.Clear();
        if(!enemy)
        {
            list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Enemy"));
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    print((list[i].transform.position - transform.position).sqrMagnitude);
                    if ((list[i].transform.position - transform.position).sqrMagnitude < dist_attack)
                    {
                        list2.Add(list[i]);
                    }
                }
            }
        }            
        else
        {
            list = new List<GameObject>(GameObject.FindGameObjectsWithTag("Player"));
            if (list.Count > 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    if ((transform.position - list[i].transform.position).sqrMagnitude < dist_attack)
                    {
                        list2.Add(list[i]);
                    }
                }
            }
        }

        if(list2.Count != 0)
        {
            for (int i = 0; i < (count_attack > list2.Count ? count_attack : list2.Count); i++)
            {
                int r = Random.Range(0, list2.Count);
                if (!enemy)
                {
                    if (list2[r].GetComponent<Enemy>() != null)
                        list2[r].GetComponent<Enemy>().Damage(damage);
                    else
                        list2[r].GetComponent<Archer>().Damage();
                }
                else
                {
                    if (list2[r].GetComponent<Players>() != null)
                        list2[r].GetComponent<Players>().Damage(damage);
                    else
                        list2[r].GetComponent<Archer>().Damage();
                }
            }
        }       
    }    
}
