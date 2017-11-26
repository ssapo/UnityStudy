using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerPortraitVisual : MonoBehaviour
{
	// TODO : get ID from players when game starts

	public CharacterAsset charAsset;
	[Header("Text Component References")]
	//public Text NameText;
	public Text HealthText;
	public Text ArmorText;
	public GameObject ArmorIcon;

	[Header("Image References")]
	public Image HeroPowerIconImage;
	public Image HeroPowerBackgroundImage;
	public Image PortraitImage;
	public Image PortraitBackgroundImage;

	private void Awake()
	{
		if (charAsset != null)
		{
			ApplyLookFromAsset();
		}
	}

	public void ApplyLookFromAsset()
	{
		HealthText.text = charAsset.MaxHealth.ToString();
		ArmorText.text = charAsset.Armor.ToString();
		HeroPowerIconImage.sprite = charAsset.HeroPowerIconImage;
		HeroPowerBackgroundImage.sprite = charAsset.HeroPowerBGImage;
		PortraitImage.sprite = charAsset.AvatarImage;
		PortraitBackgroundImage.sprite = charAsset.AvatarBGImage;
		HeroPowerBackgroundImage.color = charAsset.HeroPowerBGTint;
		PortraitBackgroundImage.color = charAsset.AvatarBGTint;
	}

	public void TakeDamage(int amount, int healthAfter, int armorAfter)
	{
		if (armorAfter == 0)
		{
			ArmorIcon.SetActive(false);
		}

		if (amount > 0)
		{
			DamageEffect.CreateDamageEffect(transform.position, amount);
			HealthText.text = healthAfter.ToString();
			ArmorText.text = armorAfter.ToString();
		}
	}

	public void Explode()
	{
		Instantiate(GlobalSettings.Instance.ExplosionPrefab, transform.position, Quaternion.identity);
		var s = DOTween.Sequence();
		s.PrependInterval(2f);
		s.OnComplete(() => GlobalSettings.Instance.GameOverPanel.SetActive(true));
	}
}
