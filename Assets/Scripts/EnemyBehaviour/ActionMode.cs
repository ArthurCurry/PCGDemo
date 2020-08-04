using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModeManager
{

}

public class ActionMode {
    public System.Random seed;

}

[System.Serializable]
public class SingleAxisAction:ActionMode
{
    public Rigidbody2D rb;
    public float maxChangingTime;
    public float minChangingTime;
    public Vector2 startVelocity;
    public float speed;
    public Vector2 velocity;
    private float timer=0f;
    private float moveTime;


    public SingleAxisAction(float speed)
    {

        this.speed = speed;
        startVelocity = new Vector2(Random.Range(-1, 1), 0).normalized * this.speed;
        if(startVelocity==Vector2.zero)
        {
            startVelocity = new Vector2(1, 0)*this.speed;
        }
        //velocity = startVelocity;
        //this.seed = seed;
    }

    public SingleAxisAction(float speed, System.Random seed,float minChangingTime,float maxChangingTime)
    {
        this.speed = speed;
        this.seed = seed;
        this.minChangingTime = minChangingTime;
        this.maxChangingTime = maxChangingTime;
        startVelocity = new Vector2(Random.Range(-1, 1), 0).normalized * speed;

    }

    public void Start()
    {
        velocity = startVelocity;
    }

    public Vector2 Move()
    {
        if (timer >= moveTime)
        {
            SwitchDirection();
        }
        timer += Time.deltaTime;
        return velocity;
    }

    private void SwitchDirection()
    {

        timer = 0;
        velocity = velocity * -1f;
        moveTime = Random.Range(minChangingTime,maxChangingTime);
    }

    public void OnCollision(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Tilemap"))
        {
            SwitchDirection();
        }
    }
}
