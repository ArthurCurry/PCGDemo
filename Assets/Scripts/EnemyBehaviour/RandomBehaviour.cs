using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBehaviour : Enemy {

    private Animator animator;
    private Rigidbody2D rb;
    public float minActionChangingTime;
    public float maxActionChangingTime;
    public string seedCode;
    public float speed;

    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        this.seed = new System.Random(seedCode.GetHashCode());
        this.action = new AllDirctionsAction(this.speed,minActionChangingTime,maxActionChangingTime,seed,this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = this.action.Move();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        action.OnCollision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        action.OnTrigger(collision);
    }

    private void OnDrawGizmos()
    {
        action.Test();
    }
}
