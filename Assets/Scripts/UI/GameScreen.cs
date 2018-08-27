using UnityEngine.UI;
using UnityEngine;
using System;
using Game;

namespace UI
{
    public class GameScreen : Controller
    {
        public event Action OnGameStarted;

        [SerializeField] private Text scoreText;
        [SerializeField] private Text multiplierText;
        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private GameObject startGamePanel;

        public override void Initialize()
        {

        }
        
        public override void Update()
        {
            scoreText.text = string.Format("Score: {0}", GameController.Score.ToString().PadLeft(10, '0'));
            multiplierText.text = string.Format("MULTIPLIER: <color=#FFB900>{0:0.00}x</color>", GameController.Multiplier);
        }

        public void StartGame()
        {
            scoreText.gameObject.SetActive(true);
            multiplierText.gameObject.SetActive(true);
            startGamePanel.SetActive(false);
            endGamePanel.SetActive(false);
            OnGameStarted?.Invoke();
        }

        public void ShowEndGame()
        {
            scoreText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
            endGamePanel.SetActive(true);
        }
        
    }
}