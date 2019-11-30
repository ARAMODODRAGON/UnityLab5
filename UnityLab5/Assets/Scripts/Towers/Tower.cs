using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected TowerManager tmInstance;
    protected ObjectPooler objPooler;
    protected GameObject towerTarget;

    //Attack
    protected float startingTimeToNextAttack = 0.8f; //Can be upgraded
    protected float timeToNextAttack; //Fire rate
    protected float projectileStartingSpeed = 5.0f; //Can be upgraded
    protected float projectileSpeed;
    public string projectile;
    protected TowerProjectile projectileObject;
    protected Vector2 directionToEnemy;
    protected float distanceToEnemy = 0.0f;
    protected float detectionRadius;
    protected float detectionRadiusModifier = 3.0f; //Can be upgraded
    public int projectileDamage = 1; //Can be upgraded


    protected void Start()
    {
        tmInstance = TowerManager.instance;
        objPooler = ObjectPooler.instance;
        directionToEnemy = Vector2.zero;
        detectionRadius = gameObject.GetComponent<CircleCollider2D>().radius + detectionRadiusModifier;
        timeToNextAttack = 0.0f;
        projectileSpeed = projectileStartingSpeed;
    }

    protected void OnEnable() //When the tower is enabled, add it to the TM list
    {
        if (tmInstance != null)
        {
            towerTarget = tmInstance.target;
            tmInstance.towers.Add(this); //Tower is removed from the list when destroyed by the enemies. Done directly by the TM
        }
    }

    protected void Update()
    {
        if (towerTarget != null)
        {
            //Debug.Log("Miss");
            AttackEnemy();
        }
    }

    protected void SellTower() //Remove from towers list and add money
    {
        //TODO
    }

    protected void AttackEnemy()
    {
        //Get distance and direction
        directionToEnemy = (Vector2)towerTarget.transform.position - (Vector2)gameObject.transform.position;
        distanceToEnemy = directionToEnemy.magnitude;
        directionToEnemy.Normalize();

        if (timeToNextAttack > 0.0f)
        {
            timeToNextAttack -= Time.deltaTime;
        }
        else
        {
            //Only if the enemy is close enough, summon the bullet
            if (distanceToEnemy <= detectionRadius)
            {
                projectileObject = objPooler.SpawnFromPool(projectile, gameObject.transform.position, Quaternion.identity).GetComponent<TowerProjectile>();
                projectileObject.AttackProperties(directionToEnemy, gameObject.transform.position, detectionRadius, projectileSpeed,projectileDamage);
                timeToNextAttack = startingTimeToNextAttack;
            }

        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag.Equals("Enemy"))
        {
            EnemyBehaviour newEnemy = col.gameObject.GetComponent<EnemyBehaviour>();
            if (!newEnemy.addedToTM)
            {
               // Debug.Log("Hit");
                tmInstance.DetectNewEnemy(col.gameObject);
                newEnemy.addedToTM = true;
                towerTarget = tmInstance.target; //Update the tower's target
            }
        }
    }
}
