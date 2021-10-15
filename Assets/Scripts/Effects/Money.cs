using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float time_speed;
    Vector3 start_pos;
    [SerializeField] bool move;

    private void Awake()
    {
        start_pos = transform.localPosition;       
    }
    void Start()
    {
        
    }
    public void Start_move()
    {
        move = true;
    }
    void Update()
    {
        if(move)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, time_speed * Time.deltaTime);
            time_speed += 60;
            if(transform.localPosition == target.localPosition)
            {
                move = false;
                transform.localPosition = start_pos;
                if (Sound.Instance != null)
                    Sound.Instance.Play_Sound(4);
                Money_controll.Instance.Add();
                gameObject.SetActive(false);                
            }
        }
    }   
}
