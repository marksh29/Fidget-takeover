using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bust_controll : MonoBehaviour
{
    public static Bust_controll Instance;
    [SerializeField] GameObject button;

    public int bust_count;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(int id)
    {

    }
    public void X(int id)
    {

    }
    public void Drop(int id)
    {

    }
}
