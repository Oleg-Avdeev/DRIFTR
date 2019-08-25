using UnityEngine.UI;
using UnityEngine;
using System;
using Game;

namespace UI
{
    public class GameScreen : Controller
    {
        public event Action OnGameStarted;

        [SerializeField] private TitleTextEffect titleText;

        [SerializeField] private Text scoreText;
        [SerializeField] private Text multiplierText;
    
        [SerializeField] private HighscoresPanel highscoresPanel;
    
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private GameObject startGamePanel;
        
        private bool gameStarted;

        public override void Initialize()
        {
            highscoresPanel.Initialize();
        }
        
        public override void Act()
        {
            scoreText.text = string.Format("Score: {0}", GameController.Score.ToString().PadLeft(11, '0'));
            multiplierText.text = string.Format("MULTIPLIER: <color=#FFB900>{0:0.00}</color>", GameController.Multiplier);
            
            if (!gameStarted)
            {
                titleText.Act();
            }
        }

        public void StartGame()
        {
            scoreText.gameObject.SetActive(true);
            multiplierText.gameObject.SetActive(true);
            startGamePanel.SetActive(false);
            endGamePanel.SetActive(false);
            OnGameStarted?.Invoke();
            gameStarted = true;
        }

        public void RestartGame()
        {
            highscoresPanel.SetHighscores();
            StartGame();
        }

        public void ShowEndGame()
        {
            scoreText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
            endGamePanel.SetActive(true);

            highscoresPanel.ShowHighscores();
        }
        
    }
}