using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour {

    public int attackPoint;
    private Rigidbody2D rb;
    public float speed;
    public float rotationSpeed;
    public float attackFrequency;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter2D(Collision2D collision)
    {

        //Destroy(this.gameObject);
        //this.gameObject.SetActive(false);
    }
}
