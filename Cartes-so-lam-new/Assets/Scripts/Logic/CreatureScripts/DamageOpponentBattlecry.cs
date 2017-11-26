using UnityEngine;
using System.Collections;

public class DamageOpponentBattlecry : CreatureEffect
{
	public DamageOpponentBattlecry(Player owner, CreatureLogic creature, int specialAmount) : base(owner, creature, specialAmount)
	{ }

	// BATTLECRY
	public override void WhenACreatureIsPlayed()
	{
		new DealDamageCommand(owner.otherPlayer.PlayerID, specialAmount, owner.otherPlayer.Health - specialAmount, 0).AddToQueue();
		owner.otherPlayer.Health -= specialAmount;
	}
}
