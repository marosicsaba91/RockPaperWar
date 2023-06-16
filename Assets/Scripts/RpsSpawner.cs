using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RpsSpawner : MonoBehaviour
{
	[SerializeField] Rect spawnArea;
	[SerializeField] GameObject rockPrefab;
	[SerializeField] GameObject paperPrefab;
	[SerializeField] GameObject scissorsPrefab;
	[SerializeField] public int spawnCount = 10;
	[SerializeField] RpsManager rpsManager;

	void OnValidate()
	{
		rpsManager = FindObjectOfType<RpsManager>();
	}

	void Start()
	{
		ClearAndSpawn();

		string myString = "Hello World";
		char[] myArray = myString.ToCharArray();
	}

	public void ClearAndSpawn()
	{
		ClearPlayers();
		SpawnPlayers();
	}

	void ClearPlayers()
	{
		List<RpsPlayer> players = rpsManager.Players;
		for (int index = players.Count - 1; index >= 0; index--)
		{
			RpsPlayer player = players[index];
			Destroy(player.gameObject);
		}
	}

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
		Vector3 position = new Vector3(
			Random.Range(spawnArea.xMin, spawnArea.xMax),
			Random.Range(spawnArea.yMin, spawnArea.yMax),
			0f
		);
		GameObject player = Instantiate(prefab, position, Quaternion.identity);
		player.transform.SetParent(transform);
	}
}
