using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RpsUI : MonoBehaviour
{
	[SerializeField] RpsManager gameManager;
	[SerializeField] GameObject pausePanel;

	[Header("Main HUD")]
	[SerializeField] TMP_Text rockCount;
	[SerializeField] TMP_Text paperCount;
	[SerializeField] TMP_Text scissorsCount;
	[SerializeField] TMP_Text speedText;
	[SerializeField] Slider timeScaleSlider;
	[SerializeField] Button pauseButton;
	[SerializeField] Button fullScreenButton;

	[Header("Main HUD")]
	[SerializeField] TMP_Text pauseText;
	[SerializeField] Button restartButton;
	[SerializeField] Button continueButton;
	[SerializeField] Slider startingAgentsSlider;
	[SerializeField] TMP_Text startingAgentsText;
	[SerializeField] Slider spawningTimeSlider;
	[SerializeField] TMP_Text spawningTimeText;

	FullScreenMode _defaultMode;

	void OnValidate()
	{
		gameManager = FindObjectOfType<RpsManager>();
		startingAgentsSlider.value = gameManager.startSpawnCount;
		spawningTimeSlider.value = gameManager.spawnTime;
	}

	void Awake()
	{
		restartButton.onClick.AddListener(Restart);
		pauseButton.onClick.AddListener(Pause);
		continueButton.onClick.AddListener(Pause);
		fullScreenButton.onClick.AddListener(FullScreen);
		gameManager.OnGameOver += OnGameOver;
		pausePanel.SetActive(false);
	}

	void Pause() 
	{
		pauseText.text = "Pause";
		pausePanel.SetActive(gameManager.IsGameOn);
		gameManager.IsGameOn = !gameManager.IsGameOn; 
	}

	void FullScreen() 
	{

		if (_defaultMode == FullScreenMode.ExclusiveFullScreen)
		{
			Screen.fullScreenMode = _defaultMode;
		}
		else
		{
			_defaultMode = Screen.fullScreenMode;
			Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
		}
	}

	void Restart()
	{
		gameManager.StartGame();
		pausePanel.SetActive(false);
	}

	void Update()
	{
		rockCount.text = gameManager.CountElement(RpsHand.Rock).ToString();
		paperCount.text = gameManager.CountElement(RpsHand.Paper).ToString();
		scissorsCount.text = gameManager.CountElement(RpsHand.Scissors).ToString();
		if (!gameManager.IsGameOn)
		{
			startingAgentsText.text = (startingAgentsSlider.value * 3).ToString();

			// Max 1 digit after the decimal point
			spawningTimeText.text = spawningTimeSlider.value.ToString("0.0");

			gameManager.startSpawnCount = (int)startingAgentsSlider.value;
			gameManager.spawnTime = spawningTimeSlider.value;
		}

		float timescale = timeScaleSlider.value;
		Time.timeScale = timescale;
		speedText.text = "Time Scale: " + timescale.ToString("0.0");

	}

	void OnGameOver(RpsHand hand)
	{
		Pause();
		pauseText.text = hand + " Wins!";
	}
}
