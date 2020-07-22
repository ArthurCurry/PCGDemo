using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private float xSpeed;
    private float ySpeed;
    [SerializeField]
    private float speed;
    [Range(0.1f,10)]
    public float resistance;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        resistance = 1f;
	}
	
	// Update is called once per frame
	void Update () {
        Move();
	}

    private void UpdateSpeed(float res)
    {
        resistance = res;
    }

    private void Move()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        //Debug.Log(x + " " + y);
        rb.velocity = (x==0&&y==0)?Vector2.zero:new Vector2(x,y).normalized*speed/resistance;
    }
}
 