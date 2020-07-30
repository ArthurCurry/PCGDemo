using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChestTool : Tool {

    public List<Tool> gadgets;
    public List<RuntimeAnimatorController> controllers;
    public System.Random seed;
    private Animator animator;
    private Tool gadget;
    

    public int hp;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitData()
    {
        animator = this.GetComponent<Animator>();
        animator.runtimeAnimatorController = controllers[seed.Next(0, controllers.Count)];
        gadget = gadgets[seed.Next(0, gadgets.Count)];
    }

    public void Hit()
    {
        hp -= 1;
        animator.SetTrigger("hit");
    }

    public void DestroySelf()
    {
        GameObject.Instantiate(gadget,transform.position,gadget.transform.rotation);
        Destroy(this.gameObject);
    }
}
