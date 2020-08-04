using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMode {
    public int difficultyLevel;
    protected float attackFrequency;
    protected float timer=0;
    protected System.Random seed;

    public AttackMode(float attackFrequency)
    {
        this.attackFrequency = attackFrequency;
    }

    public virtual void Attack()
    {

    }

    public virtual void Attack(Vector2 normalized)
    {
        throw new NotImplementedException();
    }
}

public class MeleeAttack:AttackMode
{

    public MeleeAttack(float attackFrequency):base(attackFrequency)
    {

    }

}

public class ProjectileLauncher:AttackMode
{
    private  string projectilePath = "Prefabs/Projectile/";
    private float projectileSpeed;
    public static UnityEngine.Object[] projectilePrefabs;
    GameObject projectile;
    public static Dictionary<string, Queue<GameObject>> projectiles = new Dictionary<string, Queue<GameObject>>();
    GameObject self;


    public ProjectileLauncher(float attackFrequency,System.Random seed,GameObject go):base(attackFrequency)
    {
        if (projectilePrefabs == null)
        {
            projectilePrefabs = Resources.LoadAll(projectilePath);
        }
        this.seed = seed;
        this.self = go;
        //this.projectileSpeed = projectileSpeed;
        projectile = (GameObject)projectilePrefabs[seed.Next(0,projectilePrefabs.Length)];
        Debug.Log(projectile.name+" "+ projectiles.ContainsKey(projectile.name));

        if (!projectiles.ContainsKey(projectile.name))
        {
            projectiles.Add(projectile.name, new Queue<GameObject>());
        }
    }

    public override void Attack()
    {
        base.Attack();
        
    }

    public override void Attack(Vector2 direction)
    {
        if (timer >= attackFrequency)
        {
            Shoot(direction);
            timer = 0;
        }
        timer += Time.deltaTime;
        Debug.Log(projectiles[projectile.name].Count);
    }

    private void Shoot(Vector2 direction)
    {
        GameObject temp;
        if(projectiles[projectile.name].Count>0)
        {
            temp = projectiles[projectile.name].Dequeue();
            temp.SetActive(true);
        }
        else
        {
            temp = GameObject.Instantiate(projectile,self.transform.position,projectile.transform.rotation);
            temp.name = projectile.name;
;       }
        temp.GetComponent<Rigidbody2D>().velocity = direction * temp.GetComponent<EnemyProjectile>().speed;
        //projectiles[temp.name].Enqueue(temp);
    }
}