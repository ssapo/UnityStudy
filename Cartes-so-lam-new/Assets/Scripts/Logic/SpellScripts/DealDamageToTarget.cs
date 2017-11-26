using UnityEngine;
using System.Collections;


public class DealDamageToTarget : SpellEffect
{
	public override void ActivateEffect(int specialAmount = 0, ICharacter target = null)
	{
		int HPAfter;
		int damageAfter;
		int armorAfter;

		if (target.Armor == 0)
		{
			damageAfter = specialAmount;
			armorAfter = target.Armor;
		}
		else if (ArmorPiercing)
		{
			damageAfter = specialAmount;
			armorAfter = target.Armor;
		}
		else
		{
			armorAfter = target.Armor - specialAmount;
			damageAfter = specialAmount - target.Armor;
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

		HPAfter = target.Health - damageAfter;
		new DealDamageCommand(target.ID, specialAmount, HPAfter, armorAfter).AddToQueue();
		target.Health = HPAfter;
		target.Armor = armorAfter;
	}
}
