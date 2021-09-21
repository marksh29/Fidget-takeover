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
    public void On_true(int ct)
    {
        transform.localPosition = new Vector3(transform.localPosition.x, 1.7f, transform.localPosition.z);
        count_txt.gameObject.SetActive(true);
        count = ct;
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
        transform.localPosition = new Vector3(transform.localPosition.x, 0.7f, transform.localPosition.z);
        if(count != 0)
            Buttons_controll.Instance.Remove_button(gameObject);
        count = 0;
    }
}
