using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum TargetingOptions
{
	NoTarget,
	AllCreatures,
	EnemyCreatures,
	YourCreatures,
	AllCharacters,
	EnemyCharacters,
	YourCharacters
}

public class CardAsset : ScriptableObject
{
	// this object will hold the info about the most general card
	[Header("General info")]
	public CharacterAsset characterAsset;  // if this is null, it`s a neutral card
	[TextArea(2, 3)]
	public string Description;  // Description for spell or character
	public Sprite CardImage;
	public int ManaCost;
	public bool ArmorPiercing;

	[Header("Creature Info")]
	public int MaxHealth;
	public int Attack;
	public int AttacksForOneTurn = 1;
	public bool Taunt;
	public bool Charge;
	public string CreatureScriptName;
	public int specialCreatureAmount;
	public int Armor;
	public int Dodge;
	public bool Venom;
	public bool Flying;
	public int Race;

	[Header("SpellInfo")]
	public string SpellScriptName;
	public int specialSpellAmount;
	public TargetingOptions Targets;

}
