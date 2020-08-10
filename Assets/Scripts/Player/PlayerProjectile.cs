using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System;
public class PlayerProjectile : MonoBehaviour {

    private Rigidbody2D rb;
    public float speed;
    public float aliveTime;
    private float timer;
    private Vector2 preSpeed;
    public  Action<int> UpdateAttackTimes;

    private void Awake()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(timer>=aliveTime)
        {
            this.gameObject.SetActive(false);
            GameManager.playerProjectiles.Enqueue(this.gameObject);
            timer = 0f;
        }
        timer += Time.deltaTime;
        preSpeed = rb.velocity;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.tag.Contains("Player"))
        {
            if (collision.gameObject.tag.Contains("Enemy") || collision.gameObject.tag.Contains("Destroyable"))
            {
                if (EventDispatcher.OnHitActions.ContainsKey(collision.gameObject))
                {
                    EventDispatcher.DispatchGameobjectAction(collision.gameObject);
                    
                }
                if(collision.gameObject.tag.Contains("Enemy"))
                {
                    DiffultyAdjuster.playerHitTimes += 1;
                }
                timer = 0f;
                this.gameObject.SetActive(false);
                GameManager.playerProjectiles.Enqueue(this.gameObject);
            }
            
        }
        if (collision.gameObject.tag.Equals("Tilemap"))
        {
            //Debug.Log(collision.gameObject.name);

            //ContactPoint2D[] contacts=new ContactPoint2D[5];
            Vector3 targetPos;
            //collision.GetContacts(contacts);
            //foreach (ContactPoint2D hit in contacts)
            //{
            //    if (hit.point != Vector2.zero)
            //    {
                    targetPos = transform.position+new Vector3( preSpeed.normalized.x,preSpeed.normalized.y,0);
                    TileBase tile = TileManager.Instance.GetTile(TileManager.Instance.WorldToCell(targetPos));
                    //Debug.Log(targetPos +" "+ TileManager.Instance.WorldToCell(targetPos) + "  " + tile.name);

                    if (tile is ObstacleTile)
                    {
                        ObstacleTile obstacle = (ObstacleTile)tile;
                        obstacle.hps[TileManager.Instance.WorldToCell(targetPos)] -= 20;
                        TileManager.Instance.RefreshTile(TileManager.Instance.WorldToCell(targetPos));
                    }
            //    }
            //}

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}