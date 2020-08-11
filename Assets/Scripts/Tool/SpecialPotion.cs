using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpecialPotion : MonoBehaviour {
    public enum SpecialPotionType
    {
        ZeroShiled,
        DoubleHit,
        HalfHP,
        
    }

    public SpecialPotionType type;
    public string tipText = "";

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void ZeroShield(Boss boss)
    {
        boss.defensePoint = 0;
    }

    private void DoubleHit(Boss boss)
    {
        boss.pointPerHit *= 2;
    }

    private void HalfHP(Boss boss)
    {
        boss.hp/=2;
    }

    private void AddBossDebuffTips()
    {
        EventDispatcher.bossDebuffTips += this.tipText + '\n';
        Debug.Log(EventDispatcher.bossDebuffTips+"  "+ this.tipText + '\n');
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {
            AddDebuffToBoss();
            AddBossDebuffTips();
            Destroy(this.gameObject);
        }
    }

    private void AddDebuffToBoss()
    {
        switch(type)
        {
            case SpecialPotionType.DoubleHit:
                EventDispatcher.bossDebuffActions.Add(DoubleHit);
                this.tipText = "易伤";
                break;
            case SpecialPotionType.HalfHP:
                EventDispatcher.bossDebuffActions.Add(HalfHP);
                this.tipText = "血量减半";
                break;
            case SpecialPotionType.ZeroShiled:
                EventDispatcher.bossDebuffActions.Add(ZeroShield);
                this.tipText = "削去护盾";
                break;
            default:
                break;
        }
    }

}
