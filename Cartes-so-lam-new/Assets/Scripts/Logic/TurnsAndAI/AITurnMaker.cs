using UnityEngine;
using System.Collections;

//this class will take all decisions for AI. 

public class AITurnMaker : TurnMaker
{
	public override void OnTurnStart()
	{
		base.OnTurnStart();
		// dispay a message that it is enemy`s turn
		new ShowMessageCommand("Enemy turn!", 2.0f).AddToQueue();
		p.DrawACard();
		StartCoroutine(MakeAITurn());
	}

	// THE LOGIC FOR AI
	private IEnumerator MakeAITurn()
	{
		var strategyAttackFirst = false;
		if (Random.Range(0, 2) == 0)
		{
			strategyAttackFirst = true;
		}

		while (MakeOneAIMove(strategyAttackFirst))
		{
			yield return null;
		}

		InsertDelay(1f);

		TurnManager.Instance.EndTurn();
	}

	private bool MakeOneAIMove(bool attackFirst)
	{
		if (Command.CardDrawPending())
		{
			return true;
		}
		else if (attackFirst)
		{
			return AttackWithACreature() || PlayACardFromHand() || UseHeroPower();
		}
		//return AttackWithACreature() || PlayACardFromHand();
		else
		{
			return PlayACardFromHand() || AttackWithACreature() || UseHeroPower();
		}
		//return PlayACardFromHand() || AttackWithACreature();
	}

	private bool PlayACardFromHand()
	{
		foreach (var c in p.hand.CardsInHand)
		{
			if (c.CanBePlayed)
			{
				if (c.ca.MaxHealth == 0)
				{
					// code to play a spell from hand
					// TODO: depending on the targeting options, select a random target.
					if (c.ca.Targets == TargetingOptions.NoTarget)
					{
						p.PlayASpellFromHand(c, null);
						InsertDelay(1.5f);
						//Debug.Log("Card: " + c.ca.name + " can be played");
						return true;
					}
				}
				else
				{
					// it is a creature card
					p.PlayACreatureFromHand(c, 0);
					InsertDelay(1.5f);
					return true;
				}

			}

			Debug.Log("Card: " + c.ca.name + " can NOT be played");
		}
		return false;
	}

	private bool UseHeroPower()
	{
		if (p.ManaLeft >= 2 && !p.usedHeroPowerThisTurn)
		{
			// use HP
			p.UseHeroPower(false);
			InsertDelay(1.5f);

			Debug.Log("AI used hero power");

			return true;
		}

		return false;
	}

	private bool AttackWithACreature()
	{
		foreach (var cl in p.table.CreaturesOnTable)
		{
			if (cl.AttacksLeftThisTurn > 0)
			{
				// attack a random target with a creature
				if (p.otherPlayer.table.CreaturesOnTable.Count > 0)
				{
					var index = Random.Range(0, p.otherPlayer.table.CreaturesOnTable.Count);
					var targetCreature = p.otherPlayer.table.CreaturesOnTable[index];
					cl.AttackCreature(targetCreature);
				}
				else
				{
					cl.GoFace();
				}

				InsertDelay(1f);

				Debug.Log("AI attacked with creature");

				return true;
			}
		}
		return false;
	}

	private void InsertDelay(float delay)
	{
		new DelayCommand(delay).AddToQueue();
	}
}
