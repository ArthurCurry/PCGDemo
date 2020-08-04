using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Slime : Enemy {

    private Animator animator;
    private Rigidbody2D rb;
    public List<RuntimeAnimatorController> controllers;
    public float speed;
    public float minActionChangingTime;
    public float maxActionChangingTime;
    public string seedCode;
    //public SingleAxisAction action;

    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
        animator.runtimeAnimatorController = controllers[Random.Range(0, controllers.Count)];
        rb = this.GetComponent<Rigidbody2D>();
        if(seed==null)
        {
            seed = new System.Random(seedCode.GetHashCode());
        }
        this.action = new SingleAxisAction(this.speed,this.seed,minActionChangingTime,maxActionChangingTime,this.gameObject);
        //action.Start();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = action.Move();
        animator.SetFloat("speedX", rb.velocity.x);
        //Debug.Log(rb.velocity + " " + this.gameObject.name);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            EventDispatcher.hitPlayer(attackPoint);
        }

        action.OnCollision(collision);
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
}
