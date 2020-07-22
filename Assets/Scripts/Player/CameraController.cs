using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    private GameObject player;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
        //Debug.Log(player==null);
	}

    private void FixedUpdate()
    {
        
    }
}
