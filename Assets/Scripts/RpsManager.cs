using System.Collections.Generic;
using UnityEngine;

public class RpsManager : MonoBehaviour
{
	[SerializeField] public float speed = 1f;   // Játékosok sebessége

	// Sprite-ok
	[SerializeField] Sprite rock;
	[SerializeField] Sprite paper;
	[SerializeField] Sprite scissors;

	// Játékosokat tároljuk itt:
	public List<RpsPlayer> Players { get; } = new List<RpsPlayer>();

	public int CountElement(RpsHand hand)       // Megszámolja, egy fajtából mennyi van.
	{
		int count = 0;
		foreach (RpsPlayer player in Players)
		{
			if (player.Hand == hand)
				count++;
		}
		return count;
	}

	// Lejátszássa a játékot két játékos között és egy int-et ad vissza,
	// ami azt jelzi, hogy ki nyert:
	public int PlayRps(RpsHand myHandType, RpsHand otherHandType)
	{
		// Döntetlen
		if (myHandType == otherHandType)
			return 0;

		// Neyrtünk / Üldözzük
		if (myHandType == RpsHand.Rock && otherHandType == RpsHand.Scissors)
			return 1;
		if (myHandType == RpsHand.Paper && otherHandType == RpsHand.Rock)
			return 1;
		if (myHandType == RpsHand.Scissors && otherHandType == RpsHand.Paper)
			return 1;

		// Veszítettünk / Menekülünkk
		return -1;
	}

	// Sprite lekérése a típus alapján:
	public Sprite GetSprite(RpsHand value)
	{
		if (value == RpsHand.Rock)
			return rock;
		if (value == RpsHand.Paper)
			return paper;
		if (value == RpsHand.Scissors)
			return scissors;
		return null;
	}
}
