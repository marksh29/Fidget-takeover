using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_controll : MonoBehaviour
{    
    public static Player_controll Instance;
    [Header("Настраиваемое")]
    [SerializeField] int life;
    [SerializeField] float spawn_time, freez_timer;

    [Header("Не трогать")]
    [SerializeField] Gate_controll gate;
    [SerializeField] GameObject obj;
    [SerializeField] Image freez_image;
    bool freez_on;
    float timer;

    void Start()
    {
        if (Instance == null)
            Instance = this;
    }

    void Update()
    {
        if (Game_Controll.Instance.game)
        {
            if (Input.GetMouseButtonDown(0) && !freez_on)
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Button" && hit.collider.gameObject.GetComponent<Button>().On())
                    {
                        gate.Set_text(hit.collider.gameObject.GetComponent<Button>().count);
                        hit.collider.gameObject.GetComponent<Button>().Off();
                        StartCoroutine(Freez_timer());
                    }
                }
            }

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = spawn_time;
                Spawn();
            }
        }
    }
    IEnumerator Freez_timer()
    {
        freez_on = true;
        freez_image.color = new Color32(255, 255, 255, 100);
        yield return new WaitForSeconds(freez_timer);
        freez_on = false;
        freez_image.color = new Color32(255, 255, 255, 255);
    }
    public void Damage()
    {
        life--;
        if (life <= 0)
            Game_Controll.Instance.Lose();
    }
    void Spawn()
    {
        Instantiate(obj, transform.position, transform.rotation);   
    }   
}
