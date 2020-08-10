using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy {
    public float speed;
    public float minChangingTime;
    public float maxChangingTime;
    public float attackFrequency;
    public float skillFrequency;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        this.seed = new System.Random(this.transform.position.GetHashCode());
        this.action = new AllDirctionsAction(speed, minChangingTime, maxChangingTime, seed, this.gameObject);
        this.attack = new ProjectileLauncher(ref attackFrequency,seed,this.gameObject);
    }
	
	// Update is called once per frame
	void Update () {
		
	}


}
