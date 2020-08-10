using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RandomBehaviour : Enemy {

    private Animator animator;
    private Rigidbody2D rb;
    private GameObject player;
    public float minActionChangingTime;
    public float maxActionChangingTime;
    public float attackFrequency;
    public float projectileSpeed;
    public string seedCode;
    public float speed;

    // Use this for initialization
    void Start () {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player");
        this.seed = new System.Random(transform.position.GetHashCode());
        //this.action = new AllDirctionsAction(this.speed,minActionChangingTime,maxActionChangingTime,seed,this.gameObject);
        //this.attack = new ProjectileLauncher(attackFrequency,seed,this.gameObject);
        InitBehaviour();
	}
	
	// Update is called once per frame
	void Update () {
        rb.velocity = this.action.Move();
        UpdateAnimator();
        attack.Attack((player.transform.position-this.transform.position).normalized);
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {
        action.OnCollision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //action.OnTrigger(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //action.OnCollision(collision);
        rb.velocity = -rb.velocity;
    }


    private void OnDrawGizmos()
    {
        //action.Test();
    }

    private void UpdateAnimator()
    {
        animator.SetFloat("speed_x", rb.velocity.x);
    }

    private void InitBehaviour()
    {
        ActionType[] actionModes = Enum.GetValues(typeof(ActionType)) as ActionType[];
        AttackModeType[] attackModes = Enum.GetValues(typeof(AttackModeType)) as AttackModeType[];
        ActionType actionType = (ActionType)actionModes[seed.Next(0,actionModes.Length)];
        AttackModeType attackType = (AttackModeType)attackModes[seed.Next(0, attackModes.Length)];
        //Debug.Log(seed.Next(0, actionModes.Length) + " " + seed.Next(0, attackModes.Length));
        //Debug.Log(actionType + " " + attackType);
        //Debug.Log(actionModes.Length + " " + attackModes.Length);
        switch(actionType)
        {
            case ActionType.AllDirection:
                this.action = new AllDirctionsAction(this.speed, minActionChangingTime, maxActionChangingTime, seed, this.gameObject);
                break;
            case ActionType.FourAxis:
                this.action = new FourAxisAction(this.speed, minActionChangingTime, maxActionChangingTime, seed, this.gameObject);
                break;
            case ActionType.Horizontal:
                this.action = new SingleAxisAction(this.speed, minActionChangingTime, maxActionChangingTime, seed, this.gameObject);
                break;
            default:
                break;
        }
        switch(attackType)
        {
            case AttackModeType.Melee:
                this.attack = new MeleeAttack(attackFrequency);
                break;
            case AttackModeType.Shoot:
                this.attack = new ProjectileLauncher(ref attackFrequency, seed, this.gameObject);
                break;
            default:
                break;
        }
    }
}
