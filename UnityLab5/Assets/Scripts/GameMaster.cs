using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameMasterStates : byte
{
	Begin = 1,
	Battle = 2,
	Rest = 3,
}

public class GameMaster : MonoBehaviour
{
	private GameMasterStates state;
	private int money;
    // Start is called before the first frame update
    void Start()
    {
        // set money to it's initial value
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void BeginState()
	{
		// the begin state will only be active once, once you start you get a set amount of money and the round doesn't start until you place the 
		// first tower
	}

	public void Battle()
	{
		// the battle happens after the begin phase and after every rest phase
	}

	public void Rest()
	{
		// rest happens at the end of every battle phase. Transitions back to battle after a given time limit
	}

	public void AddMoney(int m)
	{
		// call this whenever you want to add money to our total
	}

	public void TakeMoney(int m)
	{
		// takes m amount of money away whenever 
		int temp = money - m;
		if (temp >= 0)
		{
			money -= m;
		}
		else if (temp < 0)
		{
			money = 0;
		}
	}
	
	public int ReturnMoney()
	{
		// call this when ever you need access to the current money amount
		return money;
	}

}
