using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private float xSpeed;
    private float ySpeed;
    private Animator animator;

    [SerializeField]
    private float speed;
    [Range(0.1f,10)]
    public float resistance;

	// Use this for initialization
	void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        resistance = 1f;
        animator = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        UpdateAnimator(animator);
        Debug.Log(animator.GetCurrentAnimatorClipInfo(0)[0].clip.name);
    }

    private void LateUpdate()
    {
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
        rb.velocity = new Vector2(x,y).normalized*speed/resistance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("potion"))
            Debug.Log(collision.gameObject.name);
    }

    private void UpdateAnimator(Animator animator)
    {
        animator.SetFloat("speed_x",rb.velocity.x);
        animator.SetFloat("speed_y", rb.velocity.y);
        animator.SetInteger("speed", (rb.velocity.magnitude != 0) ? 1: 0);
    }
}
 