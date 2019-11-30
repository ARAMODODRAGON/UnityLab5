using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerProjectile : MonoBehaviour
{
    protected TowerManager tmInstance;
    protected EnemyBehaviour damagedEnemy;
    protected Rigidbody2D rigidBody;
    protected Vector2 towerPosition;
    protected float towerRadius;
    protected Vector2 direction;
    protected float speed;
    protected int damage;
    protected float lifeTime = 3.0f;

    protected void Start()
    {
        tmInstance = TowerManager.instance;
        rigidBody = gameObject.GetComponent<Rigidbody2D>();
    }

    protected void FixedUpdate()
    {
        //Debug.Log("Move damn it, move");
        rigidBody.velocity = speed * direction;
    }

    protected void Update()
    {
        //Disabled yourself after 3.0f seconds
        if(lifeTime>0.0f)
        {
            lifeTime -= Time.deltaTime;
        }
        else
        {
            lifeTime = 3.0f;
            gameObject.SetActive(false);
        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag.Equals("Enemy"))
        {
            Debug.Log("BOMM");
            damagedEnemy = col.gameObject.GetComponent<EnemyBehaviour>();
            if(damagedEnemy.TakeDamage(damage, towerPosition, towerRadius)) //Returns a bool if the enemy is dead
            {
                tmInstance.EnemyIsDead(col.gameObject); //if the enemy is dead, tell the tmInstance to recalcualte which enemy is the closes
            }
            gameObject.SetActive(false);
        }
    }

    //Called from the Tower after summoning the projectile
    public void AttackProperties(Vector2 attackDirection, Vector2 towerPos,float radius, float projectileSpeed, int pDamage)
    {
        direction = attackDirection;
        towerPosition = towerPos;
        towerRadius = radius;
        speed = projectileSpeed;
        damage = pDamage;
    }
}
