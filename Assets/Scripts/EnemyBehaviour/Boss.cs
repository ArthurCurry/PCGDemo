using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;

public class Boss : Enemy
{
    public float speed;
    public float minChangingTime;
    public float maxChangingTime;
    public float attackFrequency;
    public float skillFrequency;
    public int defensePoint;
    public int minHp;
    public int minDP;
    public int activateDistance;
    public List<GameObject> enemiesToSummon;
    private bool dead=false;
    private float skillTimer=0f;

    public List<GameObject> enemy;
    private bool activated = false;
    private Rigidbody2D rb;
    private Animator animator;
    private BoxCollider2D boxCollider;


    public GameObject nextLevelButton;
    public GameObject exitButton;
    private void Awake()
    {
        base.Awake();
        if (!UIManager.uiUpdateActions.ContainsKey(this.gameObject.name))
            UIManager.uiUpdateActions.Add(this.gameObject.name, UpdateBossUI);
        else
            UIManager.uiUpdateActions[this.gameObject.name] = UpdateBossUI;
    }

    // Use this for initialization
    void Start()
    {
        enemiesToSummon = Resources.LoadAll<GameObject>("Prefabs/Enemy").ToList();
        player = GameObject.FindGameObjectWithTag("Player");
        UpdateBossAttribute(player.GetComponent<PlayerController>());
        this.rb = this.GetComponent<Rigidbody2D>();
        this.animator = this.GetComponent<Animator>();
        this.boxCollider = this.GetComponent<BoxCollider2D>();
        this.seed = new System.Random(this.transform.position.GetHashCode());
        this.action = new FourAxisAction(speed, minChangingTime, maxChangingTime, seed, this.gameObject);
        this.attack = new ProjectileLauncher(ref attackFrequency, seed, this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(!activated&&Activated())
        {
            activated = true;
            boxCollider.enabled = true;
            animator.SetTrigger("activate");
            EvaluateDebuffs();
        }
        if (activated&&!dead)
        {
            rb.velocity = action.Move();
            attack.Attack((player.transform.position - this.transform.position).normalized);
            SummonEnemis();
            UpdateAnimator(animator);
        }
        if(this.hp<=0&&!dead)
        {
            BossOnDying();
        }
    }

    private void UpdateAnimator(Animator animator)
    {
        animator.SetFloat("speed_x", rb.velocity.x);
        animator.SetFloat("speed_y", rb.velocity.y);
        animator.SetInteger("speed", (rb.velocity.magnitude != 0) ? 1 : 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //action.OnCollision(collision);
        action.OnCollision(collision);
    }

    protected override void Hit()
    {
        Debug.Log(this.gameObject.name);
        if (activated)
        {
            if (defensePoint > 0)
            {
                defensePoint = (defensePoint > pointPerHit) ? defensePoint - pointPerHit : 0;
                hp = (defensePoint > pointPerHit) ? hp : hp - Mathf.Abs(defensePoint - pointPerHit);
            }
            else
            {
                hp -= pointPerHit;
            }
            hp = (hp < 0) ? 0 : hp;
        }
    }

    private void UpdateBossAttribute(PlayerController player)
    {
        this.hp = (player.lifePoint<=minHp)?minHp:player.lifePoint;
        this.defensePoint = hp;
    }
    private void UpdateBossUI(params Text[] texts)
    {
        texts[0].text = "Boss血量：" + this.hp.ToString();
        texts[1].text = "Boss护盾:" + this.defensePoint.ToString();
    }

    private bool Activated()
    {
        if((player.transform.position-transform.position).magnitude<=activateDistance&&Input.GetKeyDown(KeyCode.E))
        {
            return true;
        }
        return false;
    }

    private void BossOnDying()
    {
        dead = true;
        boxCollider.enabled = false;
        rb.velocity = Vector3.zero;
        animator.SetTrigger("die");
        
    }

    public void BossDead()
    {
        EventDispatcher.OnBossDead();
        Destroy(this.gameObject);
    }

    private void EvaluateDebuffs()
    {
        foreach(Action<Boss> action in EventDispatcher.bossDebuffActions)
        {
            action(this);
        }
        EventDispatcher.bossDebuffActions.Clear();
    }

    private void SummonEnemis()
    {
        if(skillTimer>=skillFrequency&&GameObject.FindGameObjectsWithTag("Enemy").Length<=6)
        {
            skillTimer = 0f;
            GameObject temp = enemiesToSummon[UnityEngine.Random.Range(0, enemiesToSummon.Count)];
            GameObject.Instantiate(temp,this.transform.position,temp.transform.rotation,this.transform);
        }
        skillTimer += Time.deltaTime;

    }
}
