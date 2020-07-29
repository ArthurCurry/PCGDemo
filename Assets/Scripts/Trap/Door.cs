using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;
    private bool opened = false;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(anim.isPlaying);

	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        //Debug.Log("stay");
        if(collision.gameObject.tag.Equals("Player"))
        {
            anim.SetTrigger("play");
        }
    }

    public void OnDoorClosed()
    {
        this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
