using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CreatureLogic : ICharacter
{
	// PUBLIC FIELDS
	public Player owner;
	public CardAsset ca;
	public CreatureEffect effect;
	public int UniqueCreatureID;
	public bool Frozen = false;

	// PROPERTIES
	// property from ICharacter interface
	public int ID
	{
		get { return UniqueCreatureID; }
	}

	// the basic health that we have in CardAsset
	private int baseHealth;
	// health with all the current buffs taken into account
	public int MaxHealth
	{
		get { return baseHealth; }
	}

	// current health of this creature
	private int health;
	public int Health
	{
		get { return health; }

		set
		{
			if (value > MaxHealth)
			{
				health = MaxHealth;
			}
			else if (value <= 0)
			{
				Die();
			}
			else
			{
				health = value;
			}
		}
	}

	// current Armor of this creature
	private int armor;
	public int Armor
	{
		get { return armor; }

		set
		{
			armor = value;
		}
	}

	// returns true if we can attack with this creature now
	public bool CanAttack
	{
		get
		{
			var ownersTurn = (TurnManager.Instance.whoseTurn == owner);
			return (ownersTurn && (AttacksLeftThisTurn > 0) && !Frozen);
		}
	}

	// property for Attack
	private int baseAttack;
	public int Attack
	{
		get { return baseAttack; }
	}

	//Amor piercing
	public bool ArmorPiercing
	{
		get;
		set;
	}

	// number of attacks for one turn if (attacksForOneTurn==2) => Windfury
	private int attacksForOneTurn = 1;
	public int AttacksLeftThisTurn
	{
		get;
		set;
	}

	// CONSTRUCTOR
	public CreatureLogic(Player owner, CardAsset ca)
	{
		this.ca = ca;
		baseHealth = ca.MaxHealth;
		Health = ca.MaxHealth;
		baseAttack = ca.Attack;
		attacksForOneTurn = ca.AttacksForOneTurn;
		Armor = ca.Armor;
		ArmorPiercing = ca.ArmorPiercing;
		// AttacksLeftThisTurn is now equal to 0
		if (ca.Charge)
		{
			AttacksLeftThisTurn = attacksForOneTurn;
		}

		this.owner = owner;
		UniqueCreatureID = IDFactory.GetUniqueID();
		if (ca.CreatureScriptName != null && ca.CreatureScriptName != "")
		{
			effect = System.Activator.CreateInstance(System.Type.GetType(ca.CreatureScriptName), new object[] { owner, this, ca.specialCreatureAmount }) as CreatureEffect;
			effect.RegisterEventEffect();
		}
		CreaturesCreatedThisGame.Add(UniqueCreatureID, this);
	}

	// METHODS
	public void OnTurnStart()
	{
		AttacksLeftThisTurn = attacksForOneTurn;
	}

	public void Die()
	{
		owner.table.CreaturesOnTable.Remove(this);

		// cause Deathrattle Effect
		if (effect != null)
		{
			effect.WhenACreatureDies();
			effect.UnRegisterEventEffect();
			effect = null;
		}

		new CreatureDieCommand(UniqueCreatureID, owner).AddToQueue();
	}

	public void GoFace()
	{
		int targetArmorAfter;
		int attackerAttackAfter;
		int targetHealthAfter;

		AttacksLeftThisTurn--;
		Debug.Log("Il a" + owner.otherPlayer.Armor);

		if (owner.otherPlayer.Armor == 0)
		{
			targetArmorAfter = 0;
			attackerAttackAfter = Attack;
		}

		else
		{
			if (ArmorPiercing)
			{
				targetArmorAfter = owner.otherPlayer.Armor;
				attackerAttackAfter = Attack;
			}
			else
			{
				targetArmorAfter = owner.otherPlayer.Armor - Attack;
				attackerAttackAfter = Attack - owner.otherPlayer.Armor;
				if (targetArmorAfter < 0)
				{
					targetArmorAfter = 0;
					if (GlobalSettings.Instance.ArmorBehavior == ArmorDamageStyle.ArmorPreventsOtherDamage)
					{
						attackerAttackAfter = 0;
					}
				}
				if (attackerAttackAfter < 0)
				{
					attackerAttackAfter = 0;
				}
			}
		}

		targetHealthAfter = owner.otherPlayer.Health - attackerAttackAfter;
		new CreatureAttackCommand(owner.otherPlayer.PlayerID, UniqueCreatureID, 0, attackerAttackAfter, Health, targetHealthAfter, Armor, targetArmorAfter).AddToQueue();
		owner.otherPlayer.Health = targetHealthAfter;
		owner.otherPlayer.Armor = targetArmorAfter;
		Debug.Log("Il reste " + targetArmorAfter + " armure et " + targetHealthAfter + " HP");
	}

	public void AttackCreature(CreatureLogic target)
	{
		int attackerArmorAfter;
		int targetArmorAfter;
		int attackerAttackAfter;
		int targetAttackAfter;
		int attackerHealthAfter;
		int targetHealthAfter;

		AttacksLeftThisTurn--;

		// calculate the values so that the creature does not fire the DIE command before the Attack command is sent

		if (target.Armor == 0)
		{
			targetArmorAfter = 0;
			attackerAttackAfter = Attack;
		}
		else
		{
			if (ArmorPiercing)
			{
				targetArmorAfter = target.Armor;
				attackerAttackAfter = Attack;
			}
			else
			{
				targetArmorAfter = target.Armor - Attack;
				attackerAttackAfter = Attack - target.Armor;
				if (targetArmorAfter < 0)
				{
					targetArmorAfter = 0;
					if (GlobalSettings.Instance.ArmorBehavior == ArmorDamageStyle.ArmorPreventsOtherDamage)
					{
						attackerAttackAfter = 0;
					}
				}

				// if the Armor was greather than attack
				if (attackerAttackAfter < 0)
				{
					attackerAttackAfter = 0;
				}
			}
		}

		if (Armor == 0)
		{
			attackerArmorAfter = 0;
			targetAttackAfter = target.Attack;
		}
		else
		{
			if (target.ArmorPiercing)
			{
				attackerArmorAfter = Armor;
				targetAttackAfter = target.Attack;
			}
			else
			{
				attackerArmorAfter = Armor - target.Attack;
				targetAttackAfter = target.Attack - Armor;
				if (attackerArmorAfter < 0)
				{
					attackerArmorAfter = 0;
					if (GlobalSettings.Instance.ArmorBehavior == ArmorDamageStyle.ArmorPreventsOtherDamage)
					{
						targetAttackAfter = 0;
					}
				}

				if (targetAttackAfter < 0)
				{
					targetAttackAfter = 0;
				}
			}
		}

		targetHealthAfter = target.Health - attackerAttackAfter;
		attackerHealthAfter = Health - targetAttackAfter;

		new CreatureAttackCommand(target.UniqueCreatureID, UniqueCreatureID, target.Attack, Attack, attackerHealthAfter, targetHealthAfter, attackerArmorAfter, targetArmorAfter).AddToQueue();

		target.Armor = targetArmorAfter;
		Armor = attackerArmorAfter;
		target.Health -= attackerAttackAfter;
		Health -= targetAttackAfter;
		Debug.Log("Il reste " + targetArmorAfter + " armure et " + targetHealthAfter + " HP");
	}

	public void AttackCreatureWithID(int uniqueCreatureID)
	{
		var target = CreaturesCreatedThisGame[uniqueCreatureID];
		AttackCreature(target);
	}

	// STATIC For managing IDs
	public static Dictionary<int, CreatureLogic> CreaturesCreatedThisGame = new Dictionary<int, CreatureLogic>();

}
