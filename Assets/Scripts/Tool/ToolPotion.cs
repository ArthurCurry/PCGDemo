using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ToolPotion : Tool {
    public enum PotionType
    {
        Blood,
        Speed,
        Defense
    }

    private System.Random seed;
    public PotionType type;
    public string seedCode;

    public int buffHp;
    public int buffDefense;
    public int buffSpeed;
    private int hp=0;
    private int dp=0;
    private int speed=0;

    private void Awake()
    {
        EventDispatcher.OnHitActions.Add(this.gameObject, OnHitWithPlayer);
    }

    // Use this for initialization
    void Start () {
        SetAttribute();
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.Log(this.gameObject.name);
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player"))
        {

        }
    }

    private void SetAttribute()
    {
        switch(type)
        {
            case PotionType.Blood:
                hp = buffHp;
                break;
            case PotionType.Defense:
                dp = buffDefense;
                break;
            case PotionType.Speed:
                speed = buffSpeed;
                break;
            default:
                break;
        }
    }

    private void OnHitWithPlayer()
    {
        EventDispatcher.playerAttributeUpdate(hp, dp, speed);
        Destroy(this.gameObject);
    }

}
