using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    //Instance
    public static TowerManager instance;

    //Enemies
    [HideInInspector]
    public GameObject target = null; //Which enemy should the towers target?
    private List<GameObject> detectedEnemies; //When detected by towers, enemies are added to the TM
    public Transform finalPos; //If the enemies reach this, you lose
    private float distanceBetweenFinalPosAndEnemy; //Used to sort the enemies
    private int targetIndex = 0; //Used to track the enemy with the closest enemy

    //Towers
    [HideInInspector]
    public List<Tower> towers;

    #region singleton
    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if(instance == this)
        {
            instance = null;
        }
    }

    #endregion

    void Start()
    {
        detectedEnemies = new List<GameObject>();
        towers = new List<Tower>();
        distanceBetweenFinalPosAndEnemy = 0.0f;
    }

    #region Handle Enemies
    public void DetectNewEnemy(GameObject newEnemy)
    {
        //The following is a second check before adding enemies to the list. The first check happens when an enemy enters a tower's radius
        bool enemyHasAlreadyBeenAdded = false; //Make sure the enemy is not part of the list already
        for(int i = 0; i<detectedEnemies.Count; i++)
        {
            if(newEnemy == detectedEnemies[i])
            {
                enemyHasAlreadyBeenAdded = true;
                break; //Enemy has already been detected so don't add it again
            }
        }

        if(!enemyHasAlreadyBeenAdded)
        {
            detectedEnemies.Add(newEnemy);
            if (detectedEnemies.Count > 1) //If we have more than one enemy in the list, find the closest enemy to the final pos
                FindTheClosestEnemyToFinalPos();
            else //Otherwise, this enemy is the closest as it's the only one
                distanceBetweenFinalPosAndEnemy = ((Vector2)finalPos.position - (Vector2)detectedEnemies[0].transform.position).magnitude;
        }
    }

    //Which enemy is the closest to the final target?
    private void FindTheClosestEnemyToFinalPos()
    {
        float distance = 0.0f;
        for(int i =0;i<detectedEnemies.Count;i++)
        {
            distance = ((Vector2)finalPos.position - (Vector2)detectedEnemies[0].transform.position).magnitude;
            if (distance < distanceBetweenFinalPosAndEnemy) //We're looking for the minimum
            {
                distanceBetweenFinalPosAndEnemy = distance;
                targetIndex = i;
            }
        }

        target = detectedEnemies[targetIndex];
    }

    public void EnemyIsDead(GameObject deadEnemy)
    {
        for(int i =0;i<detectedEnemies.Count;i++)
        {
            if(deadEnemy == detectedEnemies[i])
            {
                detectedEnemies.RemoveAt(i); //Remove the dead enemy from the list of enemies
                break;
            }
        }

        FindTheClosestEnemyToFinalPos(); //Find the next closest enemy
    }

    #endregion
}
