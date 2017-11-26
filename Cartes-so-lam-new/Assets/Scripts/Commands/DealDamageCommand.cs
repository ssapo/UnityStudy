using UnityEngine;
using System.Collections;

public class DealDamageCommand : Command
{
	private int targetID;
	private int amount;
	private int healthAfter;
	private int armorAfter;

	public DealDamageCommand(int targetID, int amount, int healthAfter, int armorAfter)
	{
		this.targetID = targetID;
		this.amount = amount;
		this.healthAfter = healthAfter;
		this.armorAfter = armorAfter;
	}

	public override void StartCommandExecution()
	{
		Debug.Log("Il a "+healthAfter+" HP et "+armorAfter+" armure. Dans DealDamageCOmmand");

		var target = IDHolder.GetGameObjectWithID(targetID);
		if (targetID == GlobalSettings.Instance.LowPlayer.PlayerID || targetID == GlobalSettings.Instance.TopPlayer.PlayerID)
		{
			// target is a hero
			target.GetComponent<PlayerPortraitVisual>().TakeDamage(amount, healthAfter, armorAfter);
		}
		else
		{
			// target is a creature
			target.GetComponent<OneCreatureManager>().TakeDamage(amount, healthAfter, armorAfter);
		}

		CommandExecutionComplete();
	}
}
