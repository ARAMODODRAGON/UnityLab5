using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMasterStates : byte
{
	Begin = 1,
	StartNewRound = 2,
	Battle = 3,
	UpdateRoundInfo = 4,
	Rest = 5,
	GameOver = 6,
	Reset = 7,
}

public class GameMaster : MonoBehaviour
{
	public GameMaster instance;
	// keep track of our current state
	private GameMasterStates state;
	// keep track of our money amount
	[SerializeField] private int money;
	// keep track of how many waves we've been through
	private int waves;
	// how many kills do you have so far
	private int totalKills;
	// how much health does the player have before they lose the game?
	[SerializeField] private int health;
    void Start()
    {
		// set money to it's initial value
		waves = 1;
		totalKills = 0;
		// arbitrary number we set for now

		// singleton notation
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(instance.gameObject);
		}
    }

    // Update is called once per frame
    void Update()
    {
		UpdateLogic();
	}

	public void UpdateLogic()
	{
		// this function will be put in update and be responsable for updating any logic
		// this is the only thing that will be put into update

		switch (state)
		{
			case GameMasterStates.Begin:
				BeginState();
				break;

			case GameMasterStates.Battle:
				BattleState();
				break;
			case GameMasterStates.UpdateRoundInfo:
				UpdateRoundInfoState();
				break;
			case GameMasterStates.Rest:
				RestState();
				break;
			case GameMasterStates.GameOver:
				GameOverState();
				break;
			case GameMasterStates.Reset:
				RestartState();
				break;
		}
	}

	public void BeginState()
	{
		// the begin state will only be active once, once you start you get a set amount of money and the round doesn't start until you place the 
		// first tower
	}

	public void BattleState()
	{
		// the battle happens after the begin phase and after every rest phase
	}

	public void UpdateRoundInfoState()
	{
		// this is only ever going to fire once, amp up the wave count and do anything necessary before swapping to the 
		// rest state
		waves++;
	}

	public void RestState()
	{
		// rest happens at the end of every battle phase. Transitions back to battle after a given time limit
	}

	public void GameOverState()
	{
		// call this whenever the health of the objective reaches 0
	}

	public void RestartState()
	{
		// call this to reset everything back to it's defaults
		waves = 0;
		money = 500;
		totalKills = 0;
	}

	public void AddMoney(int m)
	{
		// call this whenever you want to add money to our total
		money += m;
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
	public void AddWave(int w)
	{
		waves += w;
		Debug.Log("You started a new wave, total is now" + "" + waves);
	}
	// returns the total kills
	public int GetKills()
	{
		return totalKills;
	}

	// set the total kills
	public void AddKills(int k)
	{
		totalKills += k;
		Debug.Log("You added a kill, total is now" + "" + totalKills);
	}

	// get health
	public int GetHealth()
	{
		return health;
	}

	// set health
	public void TakeHealth(int h)
	{
		health -= h;
		Debug.Log("You took health, total is now" + "" + health);
	}

}
