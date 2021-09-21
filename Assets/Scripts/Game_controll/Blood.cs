using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public byte speed;
    void Start()
    {
        
    }
    private void OnEnable()
    {
        //GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, speed);
        if (GetComponent<SpriteRenderer>().color.a == 0)
            gameObject.SetActive(false);
    }
}
