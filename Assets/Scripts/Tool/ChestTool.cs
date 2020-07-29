using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class ChestTool : Tool {

    public List<Tool> gadgets;
    public List<RuntimeAnimatorController> controllers;
    public System.Random seed;
    private Animator animator;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void InitData()
    {
        animator.runtimeAnimatorController = controllers[seed.Next(0, controllers.Count)];
    }
}
