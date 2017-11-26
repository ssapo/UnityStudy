using UnityEngine;
using System.Collections;

public class DamageAllOpponentCreatures : SpellEffect
{
	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
	{
		var CreaturesToDamage = TurnManager.Instance.whoseTurn.otherPlayer.table.CreaturesOnTable.ToArray();
		foreach (var cl in CreaturesToDamage)
		{
			int armorAfter;
			int damageAfter;

			if (ArmorPiercing)
			{
				damageAfter = specialAmount;
				armorAfter = cl.Armor;
			}
			else
			{
				armorAfter = cl.Armor - specialAmount;
				damageAfter = specialAmount - armorAfter;
				if (armorAfter < 0)
				{
					armorAfter = 0;
					if (GlobalSettings.Instance.ArmorBehavior == ArmorDamageStyle.ArmorPreventsOtherDamage)
					{
						damageAfter = 0;
					}
				}
				if (damageAfter < 0)
				{
					damageAfter = 0;
				}
			}

			var HPAfter = cl.Health - damageAfter;
			new DealDamageCommand(cl.ID, specialAmount, HPAfter, armorAfter).AddToQueue();
			cl.Health = HPAfter;
			cl.Armor = armorAfter;
		}
	}
}
