using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChestTool : Tool {

    public List<Tool> gadgets;
    public List<AnimatorOverrideController> controllers;
    public System.Random seed;
    public string seedCode;
    private Animator animator;
    private Tool gadget;
    

    public int hp;

    private void Awake()
    {
        
    }

    // Use this for initialization
    void Start () {
        InitData();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitData()
    {
        seed = new System.Random(transform.position.GetHashCode());
        animator = this.GetComponent<Animator>();
        animator.runtimeAnimatorController = controllers[seed.Next(0, controllers.Count)];
        //Debug.Log(animator.runtimeAnimatorController.name);

        gadget = gadgets[seed.Next(0, gadgets.Count)];
    }

    public void Hit()
    {
        hp -= 1;
        if (hp <= 0)
            animator.SetTrigger("destroyed");
        else
            animator.SetTrigger("hit");
    }

    public void DestroySelf()
    {
        
        GameObject.Instantiate(gadget,transform.position,gadget.transform.rotation);
        Destroy(this.gameObject);
    }
}
