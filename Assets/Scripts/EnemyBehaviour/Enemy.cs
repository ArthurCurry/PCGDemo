using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    private GameObject player;

    private void Awake()
    {
        GameManager.RegisterInitialization(this.GetType(),InitEnemy);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void InitEnemy()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }
}
