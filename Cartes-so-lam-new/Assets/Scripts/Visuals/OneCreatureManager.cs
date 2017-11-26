using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OneCreatureManager : MonoBehaviour
{
	public CardAsset cardAsset;
	public OneCardManager PreviewManager;
	[Header("Text Component References")]
	public Text HealthText;
	public Text AttackText;
	public Text ArmorText;
	[Header("Image References")]
	public GameObject ArmorIcon;
	public Image CreatureGraphicImage;
	public Image CreatureGlowImage;

	void Awake()
	{
		if (cardAsset != null)
		{
			ReadCreatureFromAsset();
		}
	}

	private bool canAttackNow = false;
	public bool CanAttackNow
	{
		get
		{
			return canAttackNow;
		}

		set
		{
			canAttackNow = value;

			CreatureGlowImage.enabled = value;
		}
	}

	public void ReadCreatureFromAsset()
	{
		// Change the card graphic sprite
		CreatureGraphicImage.sprite = cardAsset.CardImage;

		AttackText.text = cardAsset.Attack.ToString();
		HealthText.text = cardAsset.MaxHealth.ToString();
		ArmorText.text = cardAsset.Armor.ToString();
		if (cardAsset.Armor == 0)
		{
			ArmorIcon.SetActive(false);
		}

		if (PreviewManager != null)
		{
			PreviewManager.cardAsset = cardAsset;
			PreviewManager.ReadCardFromAsset();
		}
	}

	public void TakeDamage(int amount, int healthAfter, int armorAfter)
	{
		Debug.Log("Il a " + healthAfter + " HP et " + armorAfter + " armure. Dans OneCreatureManager");
		/*if (amount > 0)
        {*/
		if (armorAfter == 0)
		{
			ArmorIcon.SetActive(false);
		}

		DamageEffect.CreateDamageEffect(transform.position, amount);
		//}

		HealthText.text = healthAfter.ToString();
		ArmorText.text = armorAfter.ToString();
	}
}
