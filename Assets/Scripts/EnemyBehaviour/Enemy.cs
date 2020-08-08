using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hp;
    public int pointPerHit;
    private GameObject player;
    public System.Random seed;
    public List<ToolPotion> tools = new List<ToolPotion>();
    public int attackPoint;
    public ActionMode action;
    public AttackMode attack;
    
    private void Awake()
    {
        GameManager.RegisterInitialization(this.GetType(),InitEnemy);
        EventDispatcher.OnHitActions.Add(this.gameObject, Hit);
        EventDispatcher.DevourActoins.Add(this.gameObject, Devour);
    }

    // Use this for initialization
    void Start () { 
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}




    private void InitEnemy()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Hit()
    {
        hp -= pointPerHit;
        if (hp <= 0)
            Destroy(this.gameObject);
    }

    private void Devour(ref int targetHp)
    {
        targetHp += hp;
        hp = 0;
        Debug.Log(targetHp);
        Destroy(this.gameObject);
    }

    public void OnDead()
    {
        Destroy(this.gameObject);
    }


}

public class EnemyBehaviourManager
{

}
