using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RpsManager : MonoBehaviour
{
	// Játékosok létrehozása
	[SerializeField] Rect spawnArea;
	[SerializeField] GameObject rockPrefab;
	[SerializeField] GameObject paperPrefab;
	[SerializeField] GameObject scissorsPrefab;
	[SerializeField] int spawnCount = 10;
	 
	// Sprite-ok
	[Space]
	[SerializeField] Sprite rock;
	[SerializeField] Sprite paper;
	[SerializeField] Sprite scissors;

	// Játékosok sebessége
	[Space]
	public float speed = 1f;

	// Események
	public event Action OnGameStart;
	public event Action<RpsHand> OnGameOver;

	// Játékosokat tároljuk itt:
	public List<RpsPlayer> players = new();

	// Ezt a property-t csak olvasni lehet:
	public IReadOnlyList<RpsPlayer> Players => players;

	void Start()
	{
		StartGame();
	}

	public void StartGame() 
	{
		ClearPlayers();
		SpawnPlayers();
		OnGameStart?.Invoke();
	}

	// Játékosok törlése:
	void ClearPlayers()
	{
		for (int index = players.Count - 1; index >= 0; index--)
		{
			RpsPlayer player = players[index];
			Destroy(player.gameObject);
		}
		players.Clear();
	}

	// Játékosok létrehozása:
	void SpawnPlayers()
	{
		for (int i = 0; i < spawnCount; i++)
		{
			Spawn(rockPrefab);
			Spawn(paperPrefab);
			Spawn(scissorsPrefab);
		}
	}

	void Spawn(GameObject prefab)
	{
		Vector3 position = new(
			Random.Range(spawnArea.xMin, spawnArea.xMax),
			Random.Range(spawnArea.yMin, spawnArea.yMax),
			0f
		);
		GameObject player = Instantiate(prefab, position, Quaternion.identity);
		player.transform.SetParent(transform);
		players.Add(player.GetComponent<RpsPlayer>());
	}

	public int CountElement(RpsHand hand)       // Megszámolja, egy fajtából mennyi van.
	{
		int count = 0;
		foreach (RpsPlayer player in players)
		{
			if (player.Hand == hand)
				count++;
		}
		return count;
	}

	// Lejátszása a játékot két játékos között és egy int-et ad vissza,
	// ami azt jelzi, hogy ki nyert:
	public int PlayRps(RpsHand myHandType, RpsHand otherHandType)
	{
		// Döntetlen
		if (myHandType == otherHandType)
			return 0;

		// Nyertünk / Üldözzük
		if (myHandType == RpsHand.Rock && otherHandType == RpsHand.Scissors)
			return 1;
		if (myHandType == RpsHand.Paper && otherHandType == RpsHand.Rock)
			return 1;
		if (myHandType == RpsHand.Scissors && otherHandType == RpsHand.Paper)
			return 1;

		// Veszítettünk / Menekülünk
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

	public void TestGameEnd() 
	{
		bool isGameOver = true;
		RpsHand hand = players[0].Hand;
		for (int index = 1; index < players.Count; index++)
		{
			if (hand != players[index].Hand)
			{
				isGameOver = false;
				break;
			}
		}

		if(isGameOver)
			OnGameOver?.Invoke(hand);
	}
}
