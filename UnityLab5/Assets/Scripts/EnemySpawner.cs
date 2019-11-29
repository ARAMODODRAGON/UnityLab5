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

	// state
	enum SpawnerState {
		waitingToFinish,
		spawning,
		paused,
	}

	private int enemiesLeft = 0;
	private int waveLevel = 0;
	private SpawnerState state = SpawnerState.paused;

	// timers
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
			eb.gameObject.hideFlags = HideFlags.HideInHierarchy;
			// add to inactive queue and allenemies list
			inactiveEnemies.Enqueue(eb);

		}
	}

	private void Start() {
		// TODO: remove this later
		StartWave(1);
	}

	public bool SpawnEnemy(int type) {
		// first check if you cant spawn one
		if (inactiveEnemies.Count == 0) return false;

		// now remove it from the inactive queue
		EnemyBehaviour eb = inactiveEnemies.Dequeue();

		// spawn it
		eb.EnableAndSpawnThis(type, pathToFollow);
		
		// show it
		eb.gameObject.hideFlags = HideFlags.None;

		return true;
	}

	public void DespawnEnemy(EnemyBehaviour eb) {
		// add to inactive list
		inactiveEnemies.Enqueue(eb);

		// deactivate it
		eb.gameObject.SetActive(false);

		// hide it
		eb.gameObject.hideFlags = HideFlags.HideInHierarchy;
	}

	public void StartWave(int level) {
		// reset timer
		timer = 0f;

		// set level
		waveLevel = level;

		// determin the number of enemies
		enemiesLeft = 15 + ((level - 1) * 3);
		if (enemiesLeft > ENEMIES_TO_POOL) enemiesLeft = ENEMIES_TO_POOL;

		// make sure we know to be spawning
		state = SpawnerState.spawning;
	}

	private void Update() {
		if (state == SpawnerState.spawning) {
			// increase timer
			timer += Time.deltaTime;
			// spawn bullet
			if (timer >= SPAWN_FREQUENCY) {
				timer -= SPAWN_FREQUENCY;
				// call this function to spawn the enemy
				// it figures out the level of the enemy
				SpawnLogic();
			}

			// end the wave if there are no more enemies to spawn
			if (enemiesLeft == 0) {
				Debug.Log("Wave has ended");
				state = SpawnerState.waitingToFinish;
			}
		} else if (state == SpawnerState.waitingToFinish && inactiveEnemies.Count == ENEMIES_TO_POOL) {
			// change state again
			state = SpawnerState.paused;

			// call end wave
			EndWave();

			// TODO: remove this later
			StartWave(waveLevel + 1);
			Debug.Log("New wave started at level " + waveLevel);
			// end remove
		}
	}

	private void SpawnLogic() {
		// set the enemies level to this var
		int enemylevel = 0;

		#region Figuring out the level of the enemy

		// roll
		float roll = Random.Range(0f, 100f);

		if (waveLevel < 6) { // levels 1 to 5
			if (roll < 20f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 11) { // levels 6 to 10
			if (roll < 15f)
				enemylevel = 2;
			else if (roll < 54f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 16) { // levels 11 to 15
			if (roll < 15f)
				enemylevel = 2;
			else if (roll < 45f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 21) { // levels 16 to 20
			if (roll < 10f)
				enemylevel = 3;
			else if (roll < 30f)
				enemylevel = 2;
			else if (roll < 60f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 26) { // levels 21 to 25
			if (roll < 7f)
				enemylevel = 4;
			else if (roll < 20f)
				enemylevel = 3;
			else if (roll < 35f)
				enemylevel = 2;
			else if (roll < 65f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 31) { // levels 26 to 30
			if (roll < 15f)
				enemylevel = 4;
			else if (roll < 30f)
				enemylevel = 3;
			else if (roll < 55f)
				enemylevel = 2;
			else if (roll < 80f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 36) { // levels 31 to 35
			if (roll < 25f)
				enemylevel = 4;
			else if (roll < 45f)
				enemylevel = 3;
			else if (roll < 70f)
				enemylevel = 2;
			else if (roll < 85f)
				enemylevel = 1;
			else
				enemylevel = 0;
		} else if (waveLevel < 41) { // levels 36 to 40
			if (roll < 30f)
				enemylevel = 4;
			else if (roll < 55f)
				enemylevel = 3;
			else if (roll < 80f)
				enemylevel = 2;
			else
				enemylevel = 1;
			// level 0 doesnt spawn anymore
		} else {
			if (roll < 37f)
				enemylevel = 4;
			else if (roll < 65f)
				enemylevel = 3;
			else if (roll < 80f)
				enemylevel = 2;
			else
				enemylevel = 1;
		}

		#endregion

		// spawn enemy
		SpawnEnemy(enemylevel);

		enemiesLeft--;
	}

	public void EndWave() {
		// TODO: do stuff here
	}
}
