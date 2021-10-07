using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public bool slide;
    public Slider slider_sound, slider_music;
    public GameObject sd_obj, ms_obj;
    public Sprite[] sprt_s, sprt_m;
    int sd, ms;
    public GameObject[] sd_objs, ms_objs;

    private void OnEnable()
    {
        Visual();
    }
    public void Click()
    {
        if (Sound.Instance != null) Sound.Instance.Click();
    }
    public void Visual()
    {
        if (slide)
        {
            slider_sound.value = Sound.Instance.gameObject.GetComponent<AudioSource>().volume;
            slider_sound.value = Sound.Instance.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume;
        }
    }
    private void Update()
    {
        if(slide)
        {
            if(Input.GetMouseButton(0))
            {
                Sound.Instance.Set_voll(0, slider_sound.value);
                Sound.Instance.Set_voll(1, slider_music.value);
            }
        }
    }
}