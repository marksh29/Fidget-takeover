using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Sound : MonoBehaviour
{
    public static Sound Instance;
    public AudioClip[] clip;
    public AudioClip[] music_clip;
    public float sd, ms;

    private void Awake()
    {
        //sd = (int)(transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume * 10);
        //ms = (int)(gameObject.GetComponent<AudioSource>().volume * 10);
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    //public void Set_ms(int count)
    //{
    //    if(count == 0)
    //    {
    //        transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume == 0 ? 1f : 0;
    //        transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume = transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume == 0 ? 1f : 0;
    //    }
    //    else
    //    {
    //        GetComponent<AudioSource>().volume = GetComponent<AudioSource>().volume == 0 ? 0.5f : 0;
    //    }          
    //}
    public void Set_voll(int id, float count)
    {
        if (id == 0)
        {
            //sd = count;
            transform.GetChild(0).gameObject.GetComponent<AudioSource>().volume = (float)count;// /10f;
            transform.GetChild(1).gameObject.GetComponent<AudioSource>().volume = (float)count;// / 10f;
        }
        else
        {
            //ms = count;
            GetComponent<AudioSource>().volume = (float)count;// / 10f;
        }
    }
    public void Play_Sound(int id)
    {
        transform.GetChild(0).GetComponent<AudioSource>().PlayOneShot(clip[id]);
    }
    public void Click()
    {
        transform.GetChild(1).GetComponent<AudioSource>().Play();
    }
}