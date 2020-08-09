using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : EnemyProjectile {


    public float countDown;
    private float timeCounter;
    private Animator animator;
    private CircleCollider2D collider2d;
    


	// Use this for initialization
	void Start (){
        animator = this.GetComponent<Animator>();
        collider2d = this.GetComponent<CircleCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
        timeCounter += Time.deltaTime;
        if(timeCounter>=countDown)
        {
            animator.SetTrigger("explode");

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log(collision.contacts.Length);
        if (collision.gameObject.tag.Equals("Player"))
        {
            EventDispatcher.hitPlayer(attackPoint);
        }
        if(collision.gameObject.tag.Equals("Tilemap"))
        {
            foreach(ContactPoint2D hit in collision.contacts)
            {
                if(TileManager.Instance.GetTile(TileManager.Instance.WorldToCell(hit.point)) is ObstacleTile)
                {
                    ObstacleTile temp =(ObstacleTile) TileManager.Instance.GetTile(TileManager.Instance.WorldToCell(hit.point));
                    temp.hps[TileManager.Instance.WorldToCell(hit.point)] -= this.attackPoint;
                }
            }
        }
    }

    public void Ignite()
    {
        collider2d.enabled = true;
    }

    public void DestroySelf()
    {
        //Destroy(this.gameObject);
        ResetStatus();

    }

    private void ResetStatus()
    {
        timeCounter = 0f;
        collider2d.enabled = false;
        ProjectileLauncher.projectiles[this.name].Enqueue(this.gameObject);

        this.gameObject.SetActive(false);

    }
}
