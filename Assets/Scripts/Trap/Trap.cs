using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour {

    public float triggerFrequency;
    private float timer = 0f;
    private Animator animator;
    public int attackPoint;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		
        if(timer>=triggerFrequency)
        {
            animator.SetTrigger("trigger");
            timer = 0f;
        }
        timer += Time.deltaTime;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            EventDispatcher.hitPlayer(this.attackPoint);
        }
    }
}
