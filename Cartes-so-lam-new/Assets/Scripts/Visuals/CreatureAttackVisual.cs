using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CreatureAttackVisual : MonoBehaviour
{
	private OneCreatureManager manager;
	private WhereIsTheCardOrCreature w;

	void Awake()
	{
		manager = GetComponent<OneCreatureManager>();
		w = GetComponent<WhereIsTheCardOrCreature>();
	}

	public void AttackTarget
		(int targetUniqueID, int damageTakenByTarget, int damageTakenByAttacker, int attackerHealthAfter, int targetHealthAfter, int attackerArmorAfter, int targetArmorAfter)
	{
		Debug.Log(targetUniqueID);
		manager.CanAttackNow = false;
		var target = IDHolder.GetGameObjectWithID(targetUniqueID);

		// bring this creature to front sorting-wise.
		w.BringToFront();
		var tempState = w.VisualState;
		w.VisualState = VisualStates.Transition;

		transform.DOMove(target.transform.position, 0.5f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InCubic).OnComplete(() =>
			{
				if (damageTakenByTarget > 0)
				{
					DamageEffect.CreateDamageEffect(target.transform.position, damageTakenByTarget);
				}

				if (damageTakenByAttacker > 0)
				{
					DamageEffect.CreateDamageEffect(transform.position, damageTakenByAttacker);
				}

				if (attackerArmorAfter == 0)
				{
					GetComponent<OneCreatureManager>().ArmorIcon.SetActive(false);
				}

				if (targetUniqueID == GlobalSettings.Instance.LowPlayer.PlayerID || targetUniqueID == GlobalSettings.Instance.TopPlayer.PlayerID)
				{
					// target is a player
					target.GetComponent<PlayerPortraitVisual>().HealthText.text = targetHealthAfter.ToString();
					target.GetComponent<PlayerPortraitVisual>().ArmorText.text = targetArmorAfter.ToString();
					if (targetArmorAfter == 0)
					{
						target.GetComponent<PlayerPortraitVisual>().ArmorIcon.SetActive(false);
					}
				}
				else
				{
					target.GetComponent<OneCreatureManager>().HealthText.text = targetHealthAfter.ToString();
					target.GetComponent<OneCreatureManager>().ArmorText.text = targetArmorAfter.ToString();
					if (targetArmorAfter == 0)
					{
						target.GetComponent<OneCreatureManager>().ArmorIcon.SetActive(false);
					}
				}

				w.SetTableSortingOrder();
				w.VisualState = tempState;

				manager.HealthText.text = attackerHealthAfter.ToString();
				manager.ArmorText.text = attackerArmorAfter.ToString();
				var s = DOTween.Sequence();
				s.AppendInterval(1f);
				s.OnComplete(Command.CommandExecutionComplete);
				//Command.CommandExecutionComplete();
			});
	}

}
