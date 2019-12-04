using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameMaster : MonoBehaviour
{
	public enum GameMasterStates : byte
	{
		Begin = 1,
		Battle = 2,
		Rest = 3,
		GameOver = 4,
	}

	public static GameMaster instance;
	// keep track of our current state
    [HideInInspector]
	public GameMasterStates state;
	// keep track of our money amount
	private int money;
	// keep track of how many waves we've been through
	private int waves;
	// how many kills do you have so far
	private int totalKills;
	// how much health does the player have before they lose the game?
	private int health;
	// enemy spawner reference
	public EnemySpawner es;
	public Text killText;
	public Text waveText;
	public Text moneyText;
	public Text healthText;
	public Button reset;
	public GameObject gameOver;

	private int towerCount;

	private void Update()
	{
		UpdateUI();
	}

	private void Awake()
	{
		// singleton notation
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}
	void Start()
	{
		// set money to it's initial value
		waves = 1;
		totalKills = 0;
		state = GameMasterStates.Battle;
		money = 0;
		health = 5;
        // arbitrary number we set for now

        gameOver.gameObject.SetActive(false);
	}

	public void AddMoney(int m)
	{
		if (state == GameMasterStates.Battle)
		{
			// call this whenever you want to add money to our total
			money += m;
		}
	}

	public void TakeMoney(int m)
	{
		// takes m amount of money away whenever 
		int temp = money - m;
		if (temp >= 0)
		{
			money -= m;
			Debug.Log("You took money, total is now" + "" + money);
		}
		else if (temp < 0)
		{
			money = 0;
			Debug.Log("You took money, total is now" + "" + money);
		}
	}

	public int ReturnMoney()
	{
		// call this when ever you need access to the current money amount
		return money;
	}

	// if you want to access the waves then here you are
	public int GetWaves()
	{
		return waves;
	}

	// set it
	public void AddWave()
	{
		waves += 1;

	}
	// returns the total kills
	public int GetKills()
	{
		return totalKills;
	}

	// set the total kills
	public void AddKills(int k)
	{
		if (state == GameMasterStates.Battle)
		{
			totalKills += k;
			
		}
		else
		{
			Debug.Log("Not in battle state");
		}
	}

	// get health
	public int GetHealth()
	{
		return health;
	}

	// set health
	public void TakeHealth(int h)
	{
		if (state == GameMasterStates.Battle)
		{
			if (health > 0)
			{
				health -= h;
				Debug.Log("You took health, total is now" + "" + health);
			}
			else
			{
				Debug.Log("Game Over");
				GameOver();
			}
			
		}
		else
		{
			Debug.Log("Called Take Health when not in battle");
		}

	}

	// call this to start a new round
	public void StartRound()
	{
		if (state == GameMasterStates.Rest || state == GameMasterStates.GameOver)
		{
			Debug.Log("Started a new round");
			state = GameMasterStates.Battle;
			if (waves != 1)
			{
				waves++;
			}
			es.StartWave(waves);
		}
	}

	//end the round and enter the rest phase
	public void EndRound()
	{
		if (state == GameMasterStates.Battle)
		{
			state = GameMasterStates.Rest;
			Debug.Log("End Battle");
		}
	}

	public void GameOver()
	{
		if (state == GameMasterStates.Battle)
		{
			state = GameMasterStates.GameOver;
			Debug.Log("Gameover");
			gameOver.gameObject.SetActive(true);
			reset.gameObject.SetActive(true);
		}
	}

	public void Restart()
	{
		if (state == GameMasterStates.GameOver)
		{/*
			gameOver.gameObject.SetActive(false);
			reset.gameObject.SetActive(false);
			waves = 1;
			totalKills = 0;
			money = 0;
			health = 5;
			state = GameMasterStates.Rest;
			StartRound();
            */
            UnityEngine.SceneManagement.SceneManager.LoadScene("Level");
		}
	}

	//public void AddTurret(Vector2 position)
	//{
	//	// call this function when spawing a turret
	//	if (state == GameMasterStates.Rest && money > 500)
	//	{
	//		ObjectPooler.instance.SpawnFromPool("Towers", new Vector3(position.x, position.y, 0), Quaternion.identity);
	//		TakeMoney(500);
	//		towerCount++;
	//	}
	//	if (state == GameMasterStates.Begin && money > 500)
	//	{
	//		ObjectPooler.instance.SpawnFromPool("Towers", new Vector3(position.x, position.y, 0), Quaternion.identity);
	//		TakeMoney(500);
	//		state = GameMasterStates.Battle;
	//		StartRound();
	//		towerCount++;
	//	}
	//}

	//Update the ui relevant to the correct values 
	public void UpdateUI()
	{
		moneyText.text = ReturnMoney().ToString();
		killText.text = GetKills().ToString();
		waveText.text = GetWaves().ToString();
		healthText.text = GetHealth().ToString();
	}
}