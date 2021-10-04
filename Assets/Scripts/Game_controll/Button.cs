using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public bool butt;
    public int count;
    [SerializeField] SkinnedMeshRenderer mesh;
    [SerializeField] Text count_txt;
    bool stay, up;
    float speed, max;
    void Start()
    {
        if (butt)
        {
            transform.GetChild(2).localPosition = new Vector3(0, 0, 0);
            transform.GetChild(2).gameObject.SetActive(false);
        }        
        mesh = transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        count_txt.gameObject.SetActive(false);
        count = 0;
        stay = true;
    }
    private void Update()
    {
        if(stay)
        {

        }
    }
    public void On_true(int ct)
    {
        transform.GetChild(2).gameObject.SetActive(true);
        count_txt.gameObject.SetActive(true);
        count = ct;
        count_txt.text = "+" + count.ToString();
    }
    IEnumerator Timer_off()
    {
        while(mesh.GetBlendShapeWeight(0) > 0)
        {
            mesh.SetBlendShapeWeight(0, (mesh.GetBlendShapeWeight(0) - 3.5f));
            yield return null;
        }        
    }
    public bool On()
    {
        return count_txt.gameObject.activeSelf;
    }
    public void Off()
    {
        mesh.SetBlendShapeWeight(0, 70);
        Drop();
    }
    public void Drop()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        count_txt.gameObject.SetActive(false);
        if (count != 0)
            Buttons_controll.Instance.Remove_button(gameObject);
        count = 0;
        StartCoroutine(Timer_off());
    }
}
