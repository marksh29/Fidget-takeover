using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blood : MonoBehaviour
{
    public byte speed;
    [SerializeField] Color32[] color;
    void Start()
    {
      
    }
    private void OnEnable()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<SpriteRenderer>().color -= new Color32(0, 0, 0, speed);
        if (GetComponent<SpriteRenderer>().color.a <= 0)
            gameObject.SetActive(false);
    }
    public void Set_color(int id)
    {
        GetComponent<SpriteRenderer>().color = color[id];
        transform.GetChild(0).gameObject.GetComponent<ParticleSystem>().startColor = color[id];
    }
}
