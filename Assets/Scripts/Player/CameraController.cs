using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour {

    private GameObject player;
    private Vector3 relativePos;
    private Vector2 bottomLeft;
    private Vector2 topRight;
    private Vector3 targetPosition;
    public float trackingSpeed;
    private Tilemap tilemap;
    private float viewPortWidth;
    private float viewPortHeight;

    private void Awake()
    {
        GameManager.RegisterInitialization(this.GetType(),new System.Action(InitData));
    }

    // Use this for initialization
    void Start () {
        //Debug.Log(player == null);
        //InitData();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(player == null);
    }

    private void FixedUpdate()
    {
        if(player!=null)
            FollowPlayer();

    }

    public void InitData()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        relativePos = new Vector3(0,0,(this.transform.position-player.transform.position).z);
        targetPosition = player.transform.position + relativePos;
        tilemap = GameObject.FindGameObjectWithTag("Tilemap").GetComponent<Tilemap>();
        Debug.Log(tilemap.size);
        this.bottomLeft = Vector2.zero;
        this.topRight = new Vector2(tilemap.size.x - 1, tilemap.size.y - 1);
        this.gameObject.transform.position = targetPosition;
        //Debug.Log(bottomLeft + " " + topRight);
    }

    private void FollowPlayer()
    {
        targetPosition = player.transform.position + relativePos;
        transform.position =Input.GetKey(KeyCode.Space)?targetPosition:(transform.position+(targetPosition-transform.position)*trackingSpeed*Time.deltaTime);
        RestrainPosition();

    }

    private void RestrainPosition()
    {
        Vector3 viewPortCorner = Camera.main.ViewportToWorldPoint(new Vector3(1,1,Mathf.Abs(Camera.main.transform.position.z)));
        viewPortWidth = 2 * (viewPortCorner.x - transform.position.x);
        viewPortHeight = 2 * (viewPortCorner.y - transform.position.y);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x,bottomLeft.x+viewPortWidth/2,topRight.x-viewPortWidth/2+1),Mathf.Clamp(transform.position.y,bottomLeft.y+viewPortHeight/2,topRight.y-viewPortHeight/2+1),transform.position.z);
    }

    private void UpdateRestraintArea(Vector2 bottomLeft,Vector2 topRight)
    {
        this.bottomLeft = bottomLeft;
        this.topRight = topRight;
    }
}
