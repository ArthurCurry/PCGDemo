using System.Collections;
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
    [Tooltip("每次被攻击扣的血量")]
    public int lifeCostPerHit;
    [Tooltip("防御点数，若大于0被攻击时优先消耗dp")]
    public int defensePoint;

    public bool dead = false;

    private void Awake()
    {
        EventDispatcher.playerAttributeUpdate = UpdateAttribute;
    }

    // Use this for initialization
    void Start () {
        rb = this.GetComponent<Rigidbody2D>();
        resistance = 1f;
        animator = this.GetComponent<Animator>();
        attackTimer = attackFrequency;
        preSpeed = Vector2.up;
        Init();
	}
	
	// Update is called once per frame
	void Update () {
        Move();
        attackTimer += Time.deltaTime;
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
                    rb.velocity = directions[key]*speed/resistance;
                    preSpeed = rb.velocity;
                    break;
                }
            }
            if(Input.GetKey(KeyCode.Mouse0)&&attackTimer>=attackFrequency)
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
        temp.GetComponent<Rigidbody2D>().velocity = preSpeed.normalized*projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Contains("Tool"))
        {
            if (EventDispatcher.GameobjectActions.ContainsKey(collision.gameObject))
            {
                EventDispatcher.DispatchGameobjectAction(collision.gameObject);
            }
            Debug.Log(collision.gameObject.name);
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
}
 