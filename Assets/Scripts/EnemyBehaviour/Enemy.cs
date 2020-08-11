using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    public int hp;
    public int pointPerHit;
    protected GameObject player;
    public System.Random seed;
    public List<ToolPotion> tools = new List<ToolPotion>();
    public int attackPoint;
    public ActionMode action;
    public AttackMode attack;
    
    protected void Awake()
    {
        //GameManager.RegisterInitialization(this.GetType(),InitEnemy);
        EventDispatcher.OnHitActions.Add(this.gameObject, Hit);
        if(!this.gameObject.name.Contains("Boss"))
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

    protected virtual void Hit()
    {
        //Debug.Log(this.gameObject.name);
        hp -= pointPerHit;
        if (hp <= 0)
            OnDead();
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
        ToolPotion.PotionType temp = DiffultyAdjuster.EvaluatePotionType();
        foreach(ToolPotion potion in tools)
        {
            if(potion.type.Equals(temp))
            {
                GameObject.Instantiate(potion.gameObject, transform.position, potion.gameObject.transform.rotation);
                break;
            }
        }
        Destroy(this.gameObject);
    }


}

public class EnemyBehaviourManager
{

}
