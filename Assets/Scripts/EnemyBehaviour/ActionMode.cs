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
    private float timer=0f;
    private float moveTime;


    public SingleAxisAction(Rigidbody2D rb)
    {
        startVelocity = new Vector2(Random.Range(-1, 1), 0).normalized * speed;
        this.rb = rb;
        //this.seed = seed;
    }

    public SingleAxisAction(Rigidbody2D rb, System.Random seed,float minChangingTime,float maxChangingTime)
    {
        this.rb = rb;
        this.seed = seed;
        this.minChangingTime = minChangingTime;
        this.maxChangingTime = maxChangingTime;
        startVelocity = new Vector2(Random.Range(-1, 1), 0).normalized * speed;

    }

    public void Start()
    {
        rb.velocity = startVelocity;
    }

    public void Move()
    {
        if (timer >= moveTime)
        {
            SwitchDirection();
        }
        timer += Time.deltaTime;
    }

    private void SwitchDirection()
    {

            timer = 0;
            rb.velocity = rb.velocity * -1f;
        moveTime = Random.Range(minChangingTime,maxChangingTime);
    }
}
