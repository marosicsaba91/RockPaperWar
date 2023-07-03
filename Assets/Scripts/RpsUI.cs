using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class RpsUI : MonoBehaviour
{
	[SerializeField] TMP_Text rockCount;
	[SerializeField] TMP_Text paperCount;
	[SerializeField] TMP_Text scissorsCount;

	[SerializeField] TMP_Text speedText;
	[SerializeField] Slider speedSlider;
	[SerializeField] Button restartButton;

	[SerializeField] RpsManager gameManager;
	[SerializeField] GameObject gameOverCanvas;
	[SerializeField] TMP_Text ameOverText;

	void OnValidate()
	{
		gameManager = FindObjectOfType<RpsManager>();
	}

	void Awake()
	{
		restartButton.onClick.AddListener(Restart);
		gameManager.OnGameOver += OnGameOver;
	}

	void Restart()
	{
		gameManager.StartGame();
		gameOverCanvas.SetActive(false);
	}

	void Update()
	{
		rockCount.text = gameManager.CountElement(RpsHand.Rock).ToString();
		paperCount.text = gameManager.CountElement(RpsHand.Paper).ToString();
		scissorsCount.text = gameManager.CountElement(RpsHand.Scissors).ToString();

		float speed = speedSlider.value;
		gameManager.speed = speed;
		speedText.text = "Speed: " + speed.ToString("0.0");

	}

	void OnGameOver(RpsHand hand)
	{
		gameOverCanvas.SetActive(true);
		ameOverText.text = hand + " Wins!";
	}
}
