using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
    public float speed;
    public float minChangingTime;
    public float maxChangingTime;
    public float attackFrequency;
    public float skillFrequency;
    private Rigidbody2D rb;
    private Animator animator;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.seed = new System.Random(this.transform.position.GetHashCode());
        this.action = new FourAxisAction(speed, minChangingTime, maxChangingTime, seed, this.gameObject);
        this.attack = new ProjectileLauncher(ref attackFrequency,seed,this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        rb.velocity=action.Move();
        UpdateAnimator(animator);
	}

    private void UpdateAnimator(Animator animator)
    {
        animator.SetFloat("speed_x", rb.velocity.x);
        animator.SetFloat("speed_y", rb.velocity.y);
        animator.SetInteger("speed", (rb.velocity.magnitude != 0) ? 1 : 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //action.OnCollision(collision);
        action.OnCollision(collision);
    }
}
