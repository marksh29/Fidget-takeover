using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Button : MonoBehaviour
{
    public int count;
    [SerializeField] SkinnedMeshRenderer mesh;
    [SerializeField] Text count_txt;
    void Start()
    {
        mesh = transform.GetChild(0).gameObject.GetComponent<SkinnedMeshRenderer>();
        count_txt.gameObject.SetActive(false);
        count = 0;
    }
    public void On_true(int ct)
    {
        count_txt.gameObject.SetActive(true);
        count = ct;
        count_txt.text = "+" + count.ToString();
    }
    IEnumerator Timer_off()
    {
        while(mesh.GetBlendShapeWeight(0) > 0)
        {
            mesh.SetBlendShapeWeight(0, (mesh.GetBlendShapeWeight(0) - 1f));
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
        count_txt.gameObject.SetActive(false);
        if (count != 0)
            Buttons_controll.Instance.Remove_button(gameObject);
        count = 0;
        StartCoroutine(Timer_off());
    }
}
