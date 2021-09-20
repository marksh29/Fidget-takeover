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
    [SerializeField] GameObject obj, hand;
    [SerializeField] Image freez_image;
    bool freez_on;
    float timer;
    Vector3 hand_pos;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        hand_pos = hand.transform.position;
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
                        StartCoroutine(DoMove(0.5f, hit.collider.gameObject));
                        //gate.Set_text(hit.collider.gameObject.GetComponent<Button>().count);
                        //hit.collider.gameObject.GetComponent<Button>().Off();
                        //StartCoroutine(Freez_timer());
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
        GameObject sp = Instantiate(obj, transform.position, transform.rotation) as GameObject;
        sp.GetComponent<Players>().spawn = true;
    }

    void Hand_move()
    {
       
    }
    private IEnumerator DoMove(float time, GameObject target)
    {
        Vector3 startPosition = hand.transform.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / time);
            hand.transform.position = Vector3.Lerp(startPosition, target.transform.position, fraction);
            yield return null;
        }
        hand.transform.position = hand_pos;
        if (target.GetComponent<Button>().count != 0)
        {
            gate.Set_text(target.GetComponent<Button>().count);
            target.GetComponent<Button>().Off();
            StartCoroutine(Freez_timer());
        }
    }
}
