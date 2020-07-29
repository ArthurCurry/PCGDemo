using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {

    private Animator anim;
    private bool opened = false;
    private GameObject player;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(anim.isPlaying);
        if (player!=null&&Input.GetKeyDown(KeyCode.E)&&(transform.position-player.transform.position).magnitude<=1.5f)
        {
            anim.SetTrigger("play");

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            if(player == null)
                player = collision.gameObject;
            Debug.Log("stay");
        }
    }

    public void OnDoorClosed()
    {
        this.gameObject.SetActive(false);
        //this.GetComponent<BoxCollider2D>().enabled = false;
    }
}
