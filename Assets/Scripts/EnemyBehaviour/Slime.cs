using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class Slime : Enemy {

    private Animator animator;
    public List<RuntimeAnimatorController> controllers;

	// Use this for initialization
	void Start () {
        animator = this.GetComponent<Animator>();
        animator.runtimeAnimatorController = controllers[Random.Range(0, controllers.Count)];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
