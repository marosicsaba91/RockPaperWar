using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RpsManager : MonoBehaviour
{
	// Játékosok létrehozása 
	[SerializeField] GameObject rockPrefab;
	[SerializeField] GameObject paperPrefab;
	[SerializeField] GameObject scissorsPrefab;
	[SerializeField] Stage stage;
	 
	// Sprite-ok
	[Space]
	[SerializeField] Sprite rock;
	[SerializeField] Sprite paper;
	[SerializeField] Sprite scissors;

	// StartSetup
	[Space]
	public int startSpawnCount = 10;
	public float spawnTime = 10f;

	// Események
	public event Action OnGameStart;
	public event Action<RpsHand> OnGameOver;

	// Játékosokat tároljuk itt:
	public List<RpsActor> players = new();

	// Ezt a property-t csak olvasni lehet:
	public IReadOnlyList<RpsActor> Players => players;

	// public float TimeScale { get; internal set; } = 1f;
	public bool IsGameOn { get; set; } = false;

	void Start()
	{
		StartGame();
	}

	float _time = 0;
	void Update()
	{
		if (!IsGameOn) return;
		_time += Time.deltaTime; // * TimeScale;

		if(_time>= spawnTime)
		{
			_time -= spawnTime;

			int randomIndex = Random.Range(0, 3);
			GameObject prefab =
				randomIndex == 0 ? rockPrefab :
				randomIndex == 1 ? paperPrefab :
				scissorsPrefab;

			Spawn(prefab, stage.StageRect);
		}
	}

	public void StartGame() 
	{
		IsGameOn = true;
		_time = 0;
		ClearPlayers();
		SpawnPlayers();
		OnGameStart?.Invoke();
	}

	// Játékosok törlése:
	void ClearPlayers()
	{
		for (int index = players.Count - 1; index >= 0; index--)
		{
			RpsActor player = players[index];
			Destroy(player.gameObject);
		}
		players.Clear();
	}

	// Játékosok létrehozása:
	void SpawnPlayers()
	{
		Rect stage = this.stage.StageRect;
		for (int i = 0; i < startSpawnCount; i++)
		{
			Spawn(rockPrefab, stage);
			Spawn(paperPrefab, stage);
			Spawn(scissorsPrefab, stage);
		}
	}

	void Spawn(GameObject prefab, Rect stage)
	{ 
		Vector3 position = new(
			Random.Range(stage.xMin, stage.xMax),
			Random.Range(stage.yMin, stage.yMax)
		);
		GameObject player = Instantiate(prefab, position, Quaternion.identity);
		player.transform.SetParent(transform);
		players.Add(player.GetComponent<RpsActor>());
	}

	public int CountElement(RpsHand hand)       // Megszámolja, egy fajtából mennyi van.
	{
		int count = 0;
		foreach (RpsActor player in players)
		{
			if (player.Hand == hand)
				count++;
		}
		return count;
	}

	// Mi a másik player viszonya hozzánk képest? Ragadozó, préda vagy semleges?
	public static RpsRelation GetRelation(RpsHand myHandType, RpsHand otherHandType)
	{
		// Döntetlen
		if (myHandType == otherHandType)
			return RpsRelation.Neutral;

		// Nyertünk / Üldözzük
		if (myHandType == RpsHand.Rock && otherHandType == RpsHand.Scissors)
			return RpsRelation.Pray;
		if (myHandType == RpsHand.Paper && otherHandType == RpsHand.Rock)
			return RpsRelation.Pray;
		if (myHandType == RpsHand.Scissors && otherHandType == RpsHand.Paper)
			return RpsRelation.Pray;

		// Veszítettünk / Menekülünk
		return RpsRelation.Predator;
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

		if (isGameOver)
		{
			OnGameOver?.Invoke(hand);
			IsGameOn = false;
		}
	}
}
