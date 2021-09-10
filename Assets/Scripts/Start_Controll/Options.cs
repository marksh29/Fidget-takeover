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
        //sd = Sound.Instance.sd;
        //ms = Sound.Instance.ms;

        if (slide)
        {
            slider_sound.value = Sound.Instance.gameObject.GetComponent<AudioSource>().volume;
            slider_sound.value = Sound.Instance.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume;
        }

        //for (int i = 0; i < sd_objs.Length; i++)
        //{
        //    sd_objs[i].SetActive(i >= sd ? false : true);
        //    ms_objs[i].SetActive(i >= ms ? false : true);
        //}

        //if (Sound.Instance != null && sprt_s[0] != null)
        //{
        //    ms_obj.GetComponent<Image>().sprite = sprt_m[Sound.Instance.gameObject.GetComponent<AudioSource>().volume == 0 ? 0 : 1];
        //    sd_obj.GetComponent<Image>().sprite = sprt_s[Sound.Instance.gameObject.transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume == 0 ? 0 : 1];
        //}
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

    public void Set_sound()
    {        
        //Sound.Instance.Set_ms(0);
        Visual();
    }
    public void Set_music()
    {
        //Sound.Instance.Set_ms(1);
        Visual();
    }
    public void Add(int id)
    {
        if(Sound.Instance != null)
        {
            if (id == 0 && sd < sd_objs.Length)
                sd++;
            else if (id == 1 && ms < ms_objs.Length)
                ms++;
            Sound.Instance.Set_voll(0, sd);
            Sound.Instance.Set_voll(1, ms);
            Visual();
        }        
    }
    public void Remove(int id)
    {
        if (Sound.Instance != null)
        {
            if (id == 0 && sd > 0)
                sd--;
            else if (id == 1 && ms > 0)
                ms--;
            Sound.Instance.Set_voll(0, sd);
            Sound.Instance.Set_voll(1, ms);
            Visual();
        }
    }
}