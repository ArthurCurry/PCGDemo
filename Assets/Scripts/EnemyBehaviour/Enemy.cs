using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hp;
    public int pointPerHit;
    private GameObject player;

    private void Awake()
    {
        GameManager.RegisterInitialization(this.GetType(),InitEnemy);
        EventDispatcher.HitActions.Add(this.gameObject, Hit);
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

    private void Hit()
    {
        hp -= pointPerHit;
        if (hp <= 0)
            Destroy(this.gameObject);
    }
}
