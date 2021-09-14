using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public int count;
    [SerializeField] Text count_txt;
    void Start()
    {
        Off();
    }
    void Update()
    {
        
    }
    public void On_true()
    {
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        count_txt.gameObject.SetActive(true);
        count = Random.Range(1, 15);
        count_txt.text = "+" + count.ToString();
        //StartCoroutine(Timer_off());
    }
    IEnumerator Timer_off()
    {
        yield return new WaitForSeconds(3);
        Off();
    }

    public bool On()
    {
        return count_txt.gameObject.activeSelf;
    }
    public void Off()
    {        
        count_txt.gameObject.SetActive(false);
        transform.position = new Vector3(transform.position.x, -5, transform.position.z);
        if(count != 0)
            Buttons_controll.Instance.Remove_button(gameObject);
        count = 0;
    }
}
