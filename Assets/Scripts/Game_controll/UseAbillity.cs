using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UseAbillity : MonoBehaviour
{
    public static UseAbillity Instance;
    [SerializeField] int[] abill_count;
    [SerializeField] Transform[] buttons;
    [SerializeField] bool hand_drop;

    [SerializeField] Transform pricel, big_hand;
    [SerializeField] float big_hand_drop_time;
    bool hand_move;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Visual();
    }
    void Update()
    {
        if (hand_drop && pricel.gameObject.activeSelf && Input.GetMouseButtonDown(0))
        {
            hand_move = true;
        }
        if (hand_move)
        {
            if(Input.GetMouseButton(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider != null && hit.collider.gameObject.tag == "Path")
                    {
                        pricel.position = new Vector3(hit.point.x, 0.001f, hit.point.z);
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {                
                hand_move = false;
                pricel.gameObject.SetActive(false);                
                StartCoroutine(DoMove(pricel.position));
            }
        }        
    }
    private IEnumerator DoMove(Vector3 targetPosition)
    {
        Vector2 startPosition = big_hand.position;
        float startTime = Time.realtimeSinceStartup;
        float fraction = 0f;
        while (fraction < 1f)
        {
            fraction = Mathf.Clamp01((Time.realtimeSinceStartup - startTime) / big_hand_drop_time);
            big_hand.position = Vector3.Lerp(startPosition, targetPosition, fraction);
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        big_hand.position = new Vector3(pricel.position.x, 60, pricel.position.z);
    }
    public void Visual()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            abill_count[i] = PlayerPrefs.GetInt("Abillity" + i);
            buttons[i].GetChild(0).gameObject.GetComponent<Text>().text = abill_count[i].ToString();
            buttons[i].GetChild(1).gameObject.SetActive(abill_count[i] > 0 ? false : true);
        }
    }
    public void Use_abillity(int id)
    {
        if(abill_count[id] > 0 && !hand_drop)
        {
            abill_count[id]--;
            PlayerPrefs.SetInt("Abillity" + id, abill_count[id]);
            Visual();
            switch (id)
            {
                case (0):
                    hand_drop = true;
                    pricel.position = new Vector3(0, 0.001f, 0);
                    pricel.gameObject.SetActive(true);
                    break;
                case (1):

                    break;
                case (2):

                    break;
            }
        }        
    }
}
