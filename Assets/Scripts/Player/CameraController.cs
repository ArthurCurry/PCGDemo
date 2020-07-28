using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {

    private GameObject player;
    private Vector3 relativePos;
    private Vector2 bottomLeft;
    private Vector2 topRight;
    private Vector3 targetPosition;
    public float trackingSpeed;

    private void Awake()
    {
        GameManager.RegisterInitialization(this.GetType(),new System.Action(InitData));
    }

    // Use this for initialization
    void Start () {
        //Debug.Log(player == null);
	}
	
	// Update is called once per frame
	void Update () {
        FollowPlayer();
        //Debug.Log(player == null);
    }

    private void FixedUpdate()
    {
        
    }

    public void InitData()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        relativePos = new Vector3(0,0,(this.transform.position-player.transform.position).z);
        targetPosition = player.transform.position + relativePos;
    }

    private void FollowPlayer()
    {
        targetPosition = player.transform.position + relativePos;
        transform.position =Input.GetKey(KeyCode.Space)?targetPosition:(transform.position+(targetPosition-transform.position)*trackingSpeed*Time.deltaTime);
    }

    private void RestraintPosition()
    {

    }
}
