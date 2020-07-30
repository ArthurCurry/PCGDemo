using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;
    public float aliveTime;
    private float timer;


    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timer += Time.deltaTime;
        if(timer>=aliveTime)
        {
            this.gameObject.SetActive(false);
            GameManager.playerProjectiles.Enqueue(this.gameObject);
            timer = 0f;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Contains("Enemy")||collision.gameObject.tag.Contains("Tool"))
        {
            EventDispatcher.DispatchAction(collision.gameObject);
            this.gameObject.SetActive(false);
            GameManager.playerProjectiles.Enqueue(this.gameObject);
        }
    }
}