using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RpsUI : MonoBehaviour
{
	[SerializeField] TMP_Text rockCount;
	[SerializeField] TMP_Text paperCount;
	[SerializeField] TMP_Text scissorsCount;

	[SerializeField] TMP_Text speedText;
	[SerializeField] Slider speedSlider;

	[SerializeField] RpsManager gameManager;
	[SerializeField] RpsSpawner spawner;
	[SerializeField] GameObject gameOverCanvas;
	[SerializeField] TMP_Text ameOverText;

	void OnValidate()
	{
		gameManager = FindObjectOfType<RpsManager>();
		spawner = FindObjectOfType<RpsSpawner>();
	}

	void Update()
	{
		rockCount.text = gameManager.CountElement(RpsHand.Rock).ToString();
		paperCount.text = gameManager.CountElement(RpsHand.Paper).ToString();
		scissorsCount.text = gameManager.CountElement(RpsHand.Scissors).ToString();

		float speed = speedSlider.value;
		gameManager.speed = speed;
		speedText.text = "Speed: " + speed.ToString("0.0");

		bool isGameOver = true;
		RpsHand hand = gameManager.Players[0].Hand;
		for (int index = 1; index < gameManager.Players.Count; index++)
		{
			if (hand != gameManager.Players[index].Hand)
			{
				isGameOver = false;
				break;
			}
		}

		gameOverCanvas.SetActive(isGameOver);

		if (isGameOver)
		{
			ameOverText.text = hand + " Wins!";
		}
	}
}
