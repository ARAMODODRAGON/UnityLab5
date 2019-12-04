using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    protected TowerManager tmInstance;
    protected ObjectPooler objPooler;

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
    protected int numberOfAttack = 1; //Becomes 2 after reaching level 6

    //Leveling up
    protected int level = 1;
    protected int currentExp = 0;
    protected int neededExpForNextLevel = 100;
    protected SpriteRenderer spriteRenderer;
    //Level One // Used in case the tower is sold or destroyed and re-bought
    protected int levelOneExpForNextLevel = 100; 
    protected float levelOneStartingTimeToNextAttack = 1.5f;
    protected float levelOneProjectileStartingSpeed = 5.0f;
    protected float levelOneDetectionRadiusModifier = 3.0f;
    protected int levelOneProjectileDamage = 1;

    protected void Start() 
    {
        tmInstance = TowerManager.instance;
        objPooler = ObjectPooler.instance;
        directionToEnemy = Vector2.zero;
        detectionRadius = gameObject.GetComponent<CircleCollider2D>().radius + detectionRadiusModifier;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        timeToNextAttack = 0.0f;
        projectileSpeed = projectileStartingSpeed;

        if (tmInstance != null)
        {
            tmInstance.towers.Add(this); //Tower is removed from the list when destroyed by the enemies. Done directly by the TM
            timeToNextAttack = 0.0f;
            neededExpForNextLevel = levelOneExpForNextLevel;
            projectileStartingSpeed = levelOneProjectileStartingSpeed;
            startingTimeToNextAttack = levelOneStartingTimeToNextAttack;
            detectionRadiusModifier = levelOneDetectionRadiusModifier;
            projectileDamage = levelOneProjectileDamage;
            level = 1;
            currentExp = 0;
        }
    }

    protected void Update()
    {
        if (tmInstance.target != null)
        {
            AttackEnemy();
        }
    }

    #region selling and buying towers
    protected void SellTower() //Remove from towers list and add money
    {
        //TODO
    }
    #endregion
    #region Attacking and leveling up
    protected void AttackEnemy()
    {
        //Get distance and direction
        directionToEnemy = (Vector2)tmInstance.target.transform.position - (Vector2)gameObject.transform.position;
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
                projectileObject.towerReference = this;
                projectileObject.numberOfAttacks = numberOfAttack;
                projectileObject.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                projectileObject.AttackProperties(directionToEnemy, gameObject.transform.position, detectionRadius, projectileSpeed,projectileDamage);
                timeToNextAttack = startingTimeToNextAttack;
            }

        }
    }

    public void LevelUp() //Called by the projectile when an enemy dies
    {
        currentExp += 10; //Exp increases by twenty for every kill
        if(currentExp>=neededExpForNextLevel)
        {
            level++;

            switch(level)
            {
                case 2:
                    Debug.Log("Level 2");
                    startingTimeToNextAttack -= 0.2f;
                    spriteRenderer.color = Color.red;
                    break;
                case 3:
                    Debug.Log("Level 3");
                    projectileSpeed += 2.0f;
                    spriteRenderer.color = Color.green;
                    break;
                case 4:
                    Debug.Log("Level 4");
                    detectionRadiusModifier += 1.0f;
                    spriteRenderer.color = Color.blue;
                    break;
                case 5:
                    Debug.Log("Level 5");
                    projectileDamage += 1;
                    spriteRenderer.color = Color.yellow;
                    break;
                case 6:
                    Debug.Log("Level 6");
                    numberOfAttack = 2;
                    spriteRenderer.color = Color.black;
                    break;
            }

            neededExpForNextLevel += 100;
        }
    }
    #endregion

    #region collisions
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
            }
        }
    }
    #endregion
}
