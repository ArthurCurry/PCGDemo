using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
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
        if(timer>=aliveTime)
        {
            this.gameObject.SetActive(false);
            GameManager.playerProjectiles.Enqueue(this.gameObject);
            timer = 0f;
        }
        timer += Time.deltaTime;

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
            }
            timer = 0f;
            this.gameObject.SetActive(false);
            GameManager.playerProjectiles.Enqueue(this.gameObject);
        }
        if (collision.gameObject.tag.Equals("Tilemap"))
        {
            ContactPoint2D[] contacts=new ContactPoint2D[4];
            Vector3 targetPos;
            collision.GetContacts(contacts);
            foreach (ContactPoint2D hit in contacts)
            {
                targetPos = new Vector3(hit.point.x - hit.normal.x * 0.1f, hit.point.y - hit.normal.y * 0.1f,0);
                TileBase tile = TileManager.Instance.GetTile(TileManager.Instance.WorldToCell(targetPos));
                Debug.Log(TileManager.Instance.WorldToCell(targetPos));

                if (tile is ObstacleTile)
                {
                    ObstacleTile obstacle = (ObstacleTile)tile;
                    obstacle.hps[Vector3Int.CeilToInt(targetPos)] -= 20;
                    TileManager.Instance.RefreshTile(TileManager.Instance.WorldToCell(targetPos));
                }
            }

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}