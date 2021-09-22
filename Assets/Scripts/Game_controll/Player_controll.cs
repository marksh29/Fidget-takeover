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
    [SerializeField] GameObject hand;
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
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color32(231, 155, 155, 150);
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color32(14, 114, 224, 150);
        yield return new WaitForSeconds(freez_timer);
        freez_on = false;
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().material.color = new Color32(231, 155, 155, 255);
        hand.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().materials[1].color = new Color32(14, 114, 224, 255);
    }
    public void Damage()
    {
        life--;
        if (life <= 0)
            Game_Controll.Instance.Lose();
    }
    void Spawn()
    {
        GameObject sp = PoolControll.Instance.Spawn_player();
        sp.transform.position = transform.position;
        sp.transform.rotation = transform.rotation;        
        sp.GetComponent<Players>().spawn = true;
        //GameObject sp = Instantiate(obj, transform.position, transform.rotation) as GameObject;
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
