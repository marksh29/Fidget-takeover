using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float time_speed;

    void Start()
    {
        float r = Random.Range(0.8f, 1f);
        //transform.localScale = new Vector3(r, r, r);
        StartCoroutine(DoMove(5));
    }
    public void Start_move()
    {
       StartCoroutine(DoMove(0.3f));
    }
    void Update()
    {
        
    }
    private IEnumerator DoMove(float time)
    {
        if (Sound.Instance != null)
            Sound.Instance.Play_Sound(4);
        Vector3 startPosition = transform.localPosition;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.localPosition = Vector3.Lerp(startPosition, target.localPosition, fraction);
            time -= 0.03f;
            yield return null;
        }
        gameObject.SetActive(false);
        if (Sound.Instance != null)
            Sound.Instance.Play_Sound(4);
    }
}
