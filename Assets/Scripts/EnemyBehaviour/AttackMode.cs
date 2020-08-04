using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMode {
    public int difficultyLevel;
    protected float attackFrequency;
    protected float timer;

    public AttackMode(float attackFrequency)
    {
        this.attackFrequency = attackFrequency;
    }

    public virtual void Attack()
    {

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
    private  string projectilePath = "Prefabs/Enemy/Projectile/";
    public Object[] projectilePrefabs;
    GameObject projectile;
    Queue<GameObject> projectiles=new Queue<GameObject>();



    public ProjectileLauncher(float attackFrequency):base(attackFrequency)
    {
        projectilePrefabs = Resources.LoadAll(projectilePath);

    }

    public override void Attack()
    {
        base.Attack();
        
    }

    private void Shoot(Vector2 direction,GameObject projectile)
    {

    }
}