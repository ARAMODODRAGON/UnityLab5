using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour {
	// ref to spawner
	private EnemySpawner enemySpawner;

    [HideInInspector]
    public bool addedToTM = false; //When the enemy enters a tower's radius for the first time, it's added to the Tower Manager

	// sprites
	[SerializeField]
	private Sprite spriteA;
	[SerializeField]
	private Sprite spriteB;
	[SerializeField]
	private Sprite spriteC;
	[SerializeField]
	private Sprite spriteD;
	[SerializeField]
	private Sprite spriteE;

	// components
	private SpriteRenderer spr;

	// path
	public Path pathToFollow;

	// movement
	public float PERC = 0.2f;
	[Min(0f)]
	public float SPEED_SLOW;
	[Min(0f)]
	public float SPEED_FAST;
	public float TYPEA_MULTIPLIER = 1f;
	public float TYPEB_MULTIPLIER = 1f;
	public float TYPEC_MULTIPLIER = 1f;
	public float TYPED_MULTIPLIER = 1f;
	public float TYPEE_MULTIPLIER = 1f;
	private float currentMultiplier;

	// state
	public int health = 1;
	private bool wasAttacked;
	private Vector2 towerPos = Vector2.zero;
	private float towerRangeSqr = 0f;
	private int pathIndex = 0;
	private Vector3 nextPoint;
	public float timeAwake { get; private set; }

	private void Awake() {
		spr = GetComponent<SpriteRenderer>();
	}

	public void Init(EnemySpawner spawner) {
		enemySpawner = spawner;
		gameObject.SetActive(false);
	}

	// initialize this enemy to follow a path
	public void EnableAndSpawnThis(int type, Path path) {
		// set the health and sprite
		switch (type) {
			case 0: health = 1; break;
			case 1: health = 2; break;
			case 2: health = 3; break;
			case 3: health = 5; break;
			case 4: health = 8; break;
			default: health = 1; Debug.LogError("Type " + type + " does not exist"); break;
		}
		ChangeSpriteAndMultiplier();

		// set the path
		pathToFollow = path;
		// set path state
		pathIndex = 0;
		nextPoint = path.GetPoint(pathIndex + 1);
		transform.position = path.GetPoint(pathIndex);

		// reset these values
		towerPos = Vector2.zero;
		towerRangeSqr = 0f;

		// reset this
		timeAwake = 0f;

		// finaly activate this
		gameObject.SetActive(true);
	}

	private void Update() {
		// increase on update
		timeAwake += Time.deltaTime;

		// get speed
		float speed = (wasAttacked ? SPEED_FAST : SPEED_SLOW);

		// move along path
		Vector3 direction = (nextPoint - transform.position).normalized;
		direction *= speed * currentMultiplier;
		direction.z = 0f;
		transform.position += direction * Time.deltaTime;

		// now check if you should change points
		direction = nextPoint - transform.position;
		direction.z = 0f;
		if (NearZero(direction.sqrMagnitude)) {
			// align with the current point
			//transform.position = nextPoint;

			// now determine what the next point is
			pathIndex++;
			if (pathIndex + 1 < pathToFollow.PointCount) {
				nextPoint = pathToFollow.GetPoint(pathIndex + 1);
			} else {
				ReachedEnd();
			}
		}
	}

	private void ReachedEnd() {
        TowerManager.instance.EnemyIsDead(this.gameObject);
        enemySpawner.DespawnEnemy(this);
		// TODO: add any extra functionality
	}

	private void ChangeSpriteAndMultiplier() {
		switch (health) {
			case 1:
				spr.sprite = spriteA;
				currentMultiplier = TYPEA_MULTIPLIER;
				break;
			case 2:
				spr.sprite = spriteB;
				currentMultiplier = TYPEB_MULTIPLIER;
				break;
			case 3:
				spr.sprite = spriteC;
				currentMultiplier = TYPEC_MULTIPLIER;
				break;
			case 4:
			case 5:
				spr.sprite = spriteD;
				currentMultiplier = TYPED_MULTIPLIER;
				break;
			case 6:
			case 7:
			case 8:
				spr.sprite = spriteE;
				currentMultiplier = TYPEE_MULTIPLIER;
				break;

			default: Debug.LogError("There is no sprite that matches the health index of " + health); break;
		}
	}

	// take damage
	public bool TakeDamage(int damage, Vector2 towerPosition, float towerRange) {
        Debug.Log("Got Damaged");
		wasAttacked = true;
		towerPos = towerPosition;
		towerRangeSqr = towerRange * towerRange;

		health -= damage;
		if (health <= 0) {
            enemySpawner.DespawnEnemy(this);
            //Destroy(gameObject);
			return true;
		} else {
			ChangeSpriteAndMultiplier();
			return false;
		}
	}

	// math
	private bool NearZero(float value) => Mathf.Abs(value) < PERC;
}
