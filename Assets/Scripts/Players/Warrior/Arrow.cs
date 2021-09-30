using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] int damage;
    public Transform target;
    public bool enemy;
    void Start()
    {
        
    }
    public void Start_fly(Transform obj)
    {
        target = obj; ;
        StartCoroutine(DoMove(1, target.position));
    }  
    private IEnumerator DoMove(float time, Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            transform.LookAt(target, Vector3.up);
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            transform.position = Vector3.Lerp(startPosition, new Vector3(target.position.x, target.position.y + 2, target.position.z), fraction);
            yield return null;
        }
        Damage();
    }
    void Damage()
    {
        if(target != null)
        {
            if (enemy)
                target.gameObject.GetComponent<Players>().Damage(damage);
            else
                target.gameObject.GetComponent<Enemy>().Damage(damage);
        }       
        gameObject.SetActive(false);
    }
}
