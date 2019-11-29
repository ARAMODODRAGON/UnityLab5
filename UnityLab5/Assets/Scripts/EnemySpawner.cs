using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

	[SerializeField]
	private GameObject enemyPrefab;

	[Header("Path")]
	[SerializeField]
	private Path pathToFollow;

	[Header("Constants")]
	[SerializeField]
	private int ENEMIES_TO_POOL;
	[SerializeField]
	private float END_RANGE_CATCH;
	[SerializeField]
	private float SPAWN_FREQUENCY;

	// enemy lists
	private List<EnemyBehaviour> allEnemies;
	private Queue<EnemyBehaviour> inactiveEnemies;

	// timer
	[SerializeField]
	private float timer = 0f;

	private void Awake() {
		// initialize lists
		allEnemies = new List<EnemyBehaviour>();
		inactiveEnemies = new Queue<EnemyBehaviour>();

		// center this gameobject to the center of the world
		transform.position = Vector3.zero;

		// start pooling enemies
		for (int i = 0; i < ENEMIES_TO_POOL; i++) {
			// create instance
			EnemyBehaviour eb = Instantiate(enemyPrefab, transform).GetComponent<EnemyBehaviour>();
			// initialize it
			eb.Init(this);
			// hide in the higherarchy
			// eb.gameObject.hideFlags = HideFlags.HideInHierarchy;
			// add to inactive queue and allenemies list
			inactiveEnemies.Enqueue(eb);

		}
	}

	public bool SpawnEnemy(int type) {
		// first check if you cant spawn one
		if (inactiveEnemies.Count == 0) return false;

		// now remove it from the inactive queue
		EnemyBehaviour eb = inactiveEnemies.Dequeue();

		// spawn it
		eb.EnableAndSpawnThis(type, pathToFollow);

		return true;
	}

	public void DespawnEnemy(EnemyBehaviour eb) {
		// add to inactive list
		inactiveEnemies.Enqueue(eb);

		// deactivate it
		eb.gameObject.SetActive(false);
	}

	private void Update() {
		// TODO: remove this later
		timer += Time.deltaTime;
		if (timer >= SPAWN_FREQUENCY) {
			timer -= SPAWN_FREQUENCY;

			SpawnEnemy(1);
		}


	}
}
