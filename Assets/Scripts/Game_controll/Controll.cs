using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controll : MonoBehaviour
{
    public static Controll Instance;
    [SerializeField] Text count_text;
    [SerializeField] GameObject player_prefab;
    [SerializeField] int xx, count; 

    [SerializeField] Material[] mat;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Set_text(8);
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider != null && hit.collider.gameObject.tag == "Button" && hit.collider.gameObject.GetComponent<Button>().On())
                {
                    Set_text(hit.collider.gameObject.GetComponent<Button>().count);
                    hit.collider.gameObject.GetComponent<Button>().Off();
                }
            }
        }
    }


    public void Set_text(int id)
    {
        count = id;
        count_text.text = "+" + count;
        GetComponent<MeshRenderer>().material = mat[1];
    }

    public void Set_spawn()
    {
        StartCoroutine(Spawn(count));
        count = 0;
        count_text.text = "";
        GetComponent<MeshRenderer>().material = mat[0];        
    }
   
    
    IEnumerator Spawn(int id)
    {
        for (int i = 0; i < id; i++)
        {
            Instantiate(player_prefab, new Vector3(Random.Range(-xx, xx), 0, transform.position.z), transform.rotation);
            yield return new WaitForSeconds(0.1f);
        }
    }   
}
