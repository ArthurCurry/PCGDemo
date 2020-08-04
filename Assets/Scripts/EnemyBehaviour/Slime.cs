﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Slime : Enemy {

    private Animator animator;
    private Rigidbody2D rb;
    public List<RuntimeAnimatorController> controllers;
    [Range(5,10)]
    public float patrolTime;
    public float speed;
    public SingleAxisAction action;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
        animator.runtimeAnimatorController = controllers[Random.Range(0, controllers.Count)];
        rb = this.GetComponent<Rigidbody2D>();
        action .rb=this.rb;
        action.Start();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = action.Move();
        animator.SetFloat("speedX", rb.velocity.x);
        Debug.Log(rb.velocity + " " + this.gameObject.name);
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
