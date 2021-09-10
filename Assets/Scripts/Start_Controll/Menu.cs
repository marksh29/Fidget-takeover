using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Game
{
    public class Menu : MonoBehaviour
    {
        public Slider load_slider;
        public Text load_text;
        public GameObject load_panel;
        private void Awake()
        {
            Screen.orientation = ScreenOrientation.Portrait;
        }       
        private void Start()
        {

        }
        private void Update()
        {

        }
        public void Load_level(string name)
        {
            load_panel.SetActive(true);
            StartCoroutine(Load(name));    
        } 
        
        IEnumerator Load(string name)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(name);
            asyncLoad.allowSceneActivation = false;

            while (!asyncLoad.isDone)
            {
                load_slider.value = asyncLoad.progress;
                load_text.text = (load_slider.value * 100).ToString("f0") + "%";  
                if(asyncLoad.progress >= 0.9f && !asyncLoad.allowSceneActivation)
                {
                    asyncLoad.allowSceneActivation = true;
                }
                yield return null;
            }
        }
        public void Click()
        {
            Sound.Instance.Click();
        }       
        public void Quit()
        {
            Application.Quit();
        }        
    }
}
