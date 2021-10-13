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
        //StartCoroutine(DoMove(0.3f));
        move = true;
    }
    void Update()
    {
        if(move)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target.localPosition, time_speed * Time.deltaTime);
            if(transform.localPosition == target.localPosition)
            {
                move = false;
                transform.localPosition = start_pos;
            }
        }
    }
    //private IEnumerator DoMove(float time)
    //{
    //    if (Sound.Instance != null)
    //        Sound.Instance.Play_Sound(4);
    //    Vector3 startPosition = transform.localPosition;
    //    float startTime = Time.realtimeSinceStartup;
    //    float fraction = 0f;
    //    while (fraction < 1f)
    //    {
    //        fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
    //        transform.localPosition = Vector3.Lerp(startPosition, target.localPosition, fraction);
    //        time -= 0.03f;
    //        yield return null;
    //    }
    //    gameObject.SetActive(false);
    //    if (Sound.Instance != null)
    //        Sound.Instance.Play_Sound(4);
    //}
}
