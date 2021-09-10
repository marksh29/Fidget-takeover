using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controll : MonoBehaviour
{
    public static Controll Instance;

    [SerializeField] float speed;
    [SerializeField] GameObject player_prefab;
    [SerializeField] List<GameObject> players;
    [SerializeField] TrackedReference start_pos;
    [SerializeField] bool move;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        Start_spawn();
    }
    void Start_spawn()
    {
        Spawn_player(5);
        move = true;
    }

    // Update is called once per frame
    void Update()
    {
        if(move)
        {
            gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
    }   
    public void Spawn_player(int id)
    {
        for(int i = 0; i < id; i++)
        {
            GameObject obj = Instantiate(player_prefab, transform.position, transform.rotation, transform) as GameObject;
            players.Add(obj);
        }
    }
}
