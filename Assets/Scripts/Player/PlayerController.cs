﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;
    private float xSpeed;
    private float ySpeed;
    private Animator animator;
    //private Queue<GameObject> projectiles = new Queue<GameObject>();

    [SerializeField]
    private float speed;
    [SerializeField]
    private float deadSpeed;
    [SerializeField]
    private float projectileSpeed;

    public float attackFrequency;
    private float attackTimer;

    [Range(0.1f,10)]
    public float resistance;
    private Dictionary<KeyCode, Vector2> directions;
    public GameObject projectile;
    private Vector2 preSpeed;

    public int lifePoint;
    [Tooltip("每次攻击消耗的血量")]
    public int lifeCostPerAttack;
    //[Tooltip("每次被攻击扣的血量")]
    //public int lifeCostPerHit;
    [Tooltip("防御点数，若大于0被攻击时优先消耗dp")]
    public int defensePoint;
    [Tooltip("复活时间")]
    public float resurrectTime;
    [Tooltip("生命数")]
    public int lifeNum;
    private float resurrectTimeCounter;
    private bool lifeStatusLastFrame;

    public bool dead = false;

    private void Awake()
    {
        EventDispatcher.playerAttributeUpdate = UpdateAttribute;
        EventDispatcher.hitPlayer = HPLost;
    }

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        resistance = 1f;
        animator = this.GetComponent<Animator>();
        attackTimer = attackFrequency;
        resurrectTimeCounter = resurrectTime;
        lifeStatusLastFrame = dead;
        preSpeed = Vector2.up;
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        attackTimer += Time.deltaTime;
        CheckLifeStatus(lifePoint);
        if(dead&&!lifeStatusLastFrame&&lifeNum>0)
        {
            resurrectTimeCounter = resurrectTime;
            StartCoroutine(Resurrecting());
        }
        lifeStatusLastFrame = dead;
    }

    private void LateUpdate()
    {
        UpdateAnimator(animator);

    }

    private void UpdateSpeed(float res)
    {
        resistance = res;
    }

    private void Move()
    {
        rb.velocity = Vector2.zero;
        if(Input.anyKey)
        {
            foreach(KeyCode key in directions.Keys)
            {
                if (Input.GetKey(key))
                {
                    rb.velocity = directions[key]*((dead==true)?deadSpeed:speed)/resistance;
                    preSpeed = rb.velocity;
                    break;
                }
            }
            if(Input.GetKey(KeyCode.Mouse0)&&attackTimer>=attackFrequency&&lifePoint>0&&!dead)
            {
                LaunchProjectile();
                attackTimer = 0f;
            }
        }
    }

    private void LaunchProjectile()
    {
        GameObject temp;
        if(GameManager.playerProjectiles.Count==0)
        {
            temp = GameObject.Instantiate(projectile, transform.position, projectile.transform.rotation);
            //GameManager.playerProjectiles.Enqueue(temp);
        }
        else
        {
            temp = GameManager.playerProjectiles.Dequeue();
            temp.SetActive(true);
            temp.transform.position = this.transform.position;
        }
        lifePoint -= lifeCostPerAttack;
        temp.GetComponent<Rigidbody2D>().velocity = preSpeed.normalized*projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Tool"))
        {
            if (EventDispatcher.OnHitActions.ContainsKey(collision.gameObject))
            {
                EventDispatcher.DispatchGameobjectAction(collision.gameObject);
            }
            Debug.Log(collision.gameObject.name);
        }
        if(dead&& collision.gameObject.tag.Equals("Enemy"))
        {
            Devour(collision.gameObject);
        }
    }

    private void UpdateAnimator(Animator animator)
    {
        animator.SetFloat("speed_x",rb.velocity.x);
        animator.SetFloat("speed_y", rb.velocity.y);
        animator.SetInteger("speed", (rb.velocity.magnitude != 0) ? 1: 0);
    }

    private void Init()
    {
        directions = new Dictionary<KeyCode, Vector2>();
        directions.Add(KeyCode.A, Vector2.left);
        directions.Add(KeyCode.S, Vector2.down);
        directions.Add(KeyCode.D, Vector2.right);
        directions.Add(KeyCode.W, Vector2.up);
    }

    private void UpdateAttribute(int hp,int dp,int speed)
    {
        Debug.Log("updated");
        this.lifePoint += hp;
        this.defensePoint += dp;
        this.speed*=((speed+100)/100);
        this.projectileSpeed *= ((speed + 100) / 100);
    }

    private void CheckLifeStatus(int hp)
    {
        if(!dead&&hp==0)
        {
            animator.SetTrigger("die");
            dead = true;
        }
    }

    private void HPLost(int hit)
    {
        if(defensePoint>0)
        {
            defensePoint = (defensePoint > hit) ? defensePoint - hit : 0;
            lifePoint = (defensePoint > hit) ? lifePoint : lifePoint - Mathf.Abs(defensePoint - hit);
        }
        else
        {
            lifePoint -= hit;
        }
        lifePoint = (lifePoint < 0) ? 0 : lifePoint;
    }

    private void Devour(GameObject enemy)
    {
        if(EventDispatcher.DevourActoins.ContainsKey(enemy))
        {
            EventDispatcher.DevourActoins[enemy](ref lifePoint);
        }
    }

    IEnumerator Resurrecting()
    {
        lifeNum -= 1;
        while(resurrectTimeCounter>=0)
        {
            resurrectTimeCounter -= Time.deltaTime;
            Debug.Log(resurrectTimeCounter);
            yield return null;
        }
        yield return new WaitUntil(()=>resurrectTimeCounter < 0);
        dead = false;
        if(this.lifePoint<=0)
        {
            this.gameObject.SetActive(false);
        }
        else
        {
            animator.SetTrigger("resurrect");
        }
    }
}
 