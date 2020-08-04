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
        this.action = new FourAxisAction(this.speed,minActionChangingTime,maxActionChangingTime,seed);
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = this.action.Move();
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        action.OnCollision(collision);
    }
}
