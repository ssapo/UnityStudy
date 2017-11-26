using UnityEngine;
using System.Collections;

public class CreatureAttackCommand : Command
{
	// position of creature on enemy`s table that will be attacked
	// if enemyindex == -1 , attack an enemy character 
	private int TargetUniqueID;
	private int AttackerUniqueID;
	private int AttackerHealthAfter;
	private int TargetHealthAfter;
	private int AttackerArmorAfter;
	private int TargetArmorAfter;
	private int DamageTakenByAttacker;
	private int DamageTakenByTarget;

	public CreatureAttackCommand
	(int targetID, int attackerID, int damageTakenByAttacker, int damageTakenByTarget, int attackerHealthAfter, int targetHealthAfter, int attackerArmorAfter, int targetArmorAfter)
	{
		TargetUniqueID = targetID;
		AttackerUniqueID = attackerID;
		AttackerHealthAfter = attackerHealthAfter;
		TargetHealthAfter = targetHealthAfter;
		DamageTakenByTarget = damageTakenByTarget;
		DamageTakenByAttacker = damageTakenByAttacker;
		AttackerArmorAfter = attackerArmorAfter;
		TargetArmorAfter = targetArmorAfter;
	}

	public override void StartCommandExecution()
	{
		var Attacker = IDHolder.GetGameObjectWithID(AttackerUniqueID);

		//Debug.Log(TargetUniqueID);
		Attacker.GetComponent<CreatureAttackVisual>().AttackTarget
			(TargetUniqueID, DamageTakenByTarget, DamageTakenByAttacker, AttackerHealthAfter, TargetHealthAfter, AttackerArmorAfter, TargetArmorAfter);
	}
}
