using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[ExecuteInEditMode]
public class ManaPoolVisual : MonoBehaviour
{

	public int TestFullCrystals;
	public int TestTotalCrystalsThisTurn;

	public int m_maxCrystals;
	public Text ProgressText;

	private int m_totalCrystals;
	private int m_availableCrystals;

	public int TotalCrystals
	{
		get { return m_totalCrystals; }

		set
		{
			Debug.Log("Changed total mana to: " + value);

			if (value > m_maxCrystals)
			{ 
				m_totalCrystals = m_maxCrystals;
			}
			else if (value < 0)
			{
				m_totalCrystals = 0;
			}
			else
			{
				m_totalCrystals = value;
			}

			// update the text
			ProgressText.text = string.Format("{0}/{1}", m_availableCrystals.ToString(), m_totalCrystals.ToString());
		}
	}

	public int AvailableCrystals
	{
		get { return m_availableCrystals; }

		set
		{
			Debug.Log("Changed mana this turn to: " + value);

			if (value > m_totalCrystals)
			{
				m_availableCrystals = m_totalCrystals;
			}
			else if (value < 0)
			{
				m_availableCrystals = 0;
			}
			else
			{
				m_availableCrystals = value;
			}

			// update the text
			ProgressText.text = string.Format("{0}/{1}", m_availableCrystals.ToString(), m_totalCrystals.ToString());
		}
	}

	void Update()
	{
		if (Application.isEditor && !Application.isPlaying)
		{
			TotalCrystals = TestTotalCrystalsThisTurn;
			AvailableCrystals = TestFullCrystals;
		}
	}

}
