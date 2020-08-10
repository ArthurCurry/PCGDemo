using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightening : EnemyProjectile {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag.Equals("Player")||collision.gameObject.tag.Contains("Tilemap"))
        {

            if(collision.gameObject.tag.Equals("Player"))
            {
                EventDispatcher.hitPlayer(this.attackPoint);
            }
            ProjectileLauncher.projectiles[this.name].Enqueue(this.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
