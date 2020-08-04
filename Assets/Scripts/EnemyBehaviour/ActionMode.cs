using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionModeManager
{

}

public class ActionMode {
    public System.Random seed;
    public int difficultyLevel;
    public float maxChangingTime;
    public float minChangingTime;
    public Vector2 velocity;
    public float speed;
    protected float timer;
    protected GameObject self;

    public virtual Vector2 Move()
    {
        return Vector2.zero;
    }

    public ActionMode(GameObject go)
    {
        this.self = go;
    }

    public virtual void OnCollision(Collision2D collision)
    {

    }

    public virtual void OnTrigger(Collider2D collider)
    {

    }

    public void Test()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(self.transform.position,self.transform.position+(Vector3)velocity);
    }

}

[System.Serializable]
public class SingleAxisAction:ActionMode
{
    
    public Vector2 startVelocity;
    private float moveTime;


    public SingleAxisAction(float speed,GameObject go):base(go)
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

    public SingleAxisAction(float speed, System.Random seed,float minChangingTime,float maxChangingTime,GameObject go):base(go)
    {
        this.speed = speed;
        this.seed = seed;
        this.minChangingTime = minChangingTime;
        this.maxChangingTime = maxChangingTime;
        startVelocity = (Random.Range(-1,1)>=0?Vector2.right:Vector2.left)* speed;
        this.velocity = startVelocity;
    }

    public void Start()
    {
        velocity = startVelocity;
    }

    public override Vector2 Move()
    {
        //Debug.Log("timer");
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

    public override void OnCollision(Collision2D collision)
    {
        if(collision.gameObject.tag.Equals("Tilemap"))
        {
            SwitchDirection();
        }
    }
}

public class FourAxisAction:ActionMode
{
    private List<Vector2> directions;
    private Vector2 preVelocity;
    private float patrolTime;

    public FourAxisAction(float speed,float minChangingTime,float maxChangingTime,System.Random seed,GameObject go):base(go)
    {
        InitDirections();
        this.speed = speed;
        this.minChangingTime = minChangingTime;
        this.maxChangingTime = maxChangingTime;
        this.seed = seed;
        velocity = directions[Random.Range(0,directions.Count)] * speed;
    }

    public override Vector2 Move()
    {
        if(timer>=patrolTime)
        {
            SwitchDirection();
        }
        timer += Time.deltaTime;
        preVelocity = velocity;
        return velocity;
    }

    private void SwitchDirection()
    {
        timer = 0;
        patrolTime = Random.Range(minChangingTime, maxChangingTime);
        velocity = directions[this.seed.Next(0,directions.Count)]*speed;
        if (velocity == preVelocity)
        {
            velocity = directions[(directions.IndexOf(velocity)+1)%directions.Count]*speed;
        }
        preVelocity = velocity;
    }

    public override void OnCollision(Collision2D collision)
    {
        //SwitchDirection();
        velocity = velocity * -1;
        //timer = 0;
    }

    private void InitDirections()
    {
        directions = new List<Vector2>();
        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);

    }
}

public class AllDirctionsAction:ActionMode
{
    private Vector2 preVelocity;
    private float patrolTime;
    private int minAngle=10;

    public AllDirctionsAction(float speed, float minChangingTime, float maxChangingTime,System.Random seed,GameObject go):base(go)
    {
        this.speed = speed;
        this.minChangingTime = minChangingTime;
        this.maxChangingTime = maxChangingTime;
        this.seed = seed;
        this.velocity = Quaternion.AngleAxis(seed.Next(0, 360 / minAngle) * minAngle, Vector3.forward) * Vector2.right*speed;
        //this.minAngle = minAngle;
    }

    public override Vector2 Move()
    {
        if(timer>=patrolTime)
        {
            SwitchDirection();
        }
        timer += Time.deltaTime;
        preVelocity = velocity;
        
        return velocity;
    }

    private void SwitchDirection()
    {
        timer = 0;
        patrolTime = Random.Range(minChangingTime,maxChangingTime);
        this.velocity = Quaternion.AngleAxis(seed.Next(0,360/minAngle)*minAngle,Vector3.forward)*this.velocity;
        if(this.velocity==preVelocity)
        {
            this.velocity = this.velocity * -1;
        }
    }

    public override void OnCollision(Collision2D collision)
    {

        //SwitchDirection();
        //timer = 0;
        //patrolTime = Random.Range(minChangingTime, maxChangingTime);

        if (collision.gameObject.tag.Equals("Tilemap"))
        {
            ContactPoint2D[] hitPoints = collision.contacts;
            
            //foreach (ContactPoint2D hitPoint in hitPoints)
            //{
                if (hitPoints[0].point != Vector2.zero)
                {
                    this.velocity = Vector2.Reflect(this.velocity,hitPoints[0].normal) ;
                }
            //}
            //Debug.Log("collision  " + hitPoints.Length + self.transform.position);
        }
    }

    public override void OnTrigger(Collider2D collider)
    {
        //base.OnTrigger(collider);

    }

    
}
