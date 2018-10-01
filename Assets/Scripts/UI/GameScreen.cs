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
        
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private Text endPlayerScore;
        [SerializeField] private Text[] highScoreNames;
        [SerializeField] private Text[] highScoreTexts;

        [SerializeField] private GameObject endGamePanel;
        [SerializeField] private GameObject startGamePanel;
        
        private bool gameStarted;

        public override void Initialize()
        {
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
            if (!string.IsNullOrEmpty(playerNameInput.text))
            {
                string name = playerNameInput.text.ToLower();
                Wazzapps.API.HighScores.HighscoresManager.Instance.SetHighscore(name, GameController.Score);
            }

            StartGame();
        }

        public void ShowEndGame()
        {
            scoreText.gameObject.SetActive(false);
            multiplierText.gameObject.SetActive(false);
            endGamePanel.SetActive(true);

            endPlayerScore.text = GameController.Score.ToString().PadLeft(11, '0');

            Wazzapps.API.HighScores.HighscoresManager.Instance.GetHighscores((records) => {
                for (int i = 0; i < highScoreTexts.Length; i++)
                {
                    if (i >= records.Count)
                    {
                        highScoreNames[i].gameObject.SetActive(false);
                        highScoreTexts[i].gameObject.SetActive(false);
                    }
                    else
                    {
                        highScoreTexts[i].gameObject.SetActive(true);
                        highScoreNames[i].gameObject.SetActive(true);
                        var values = records[i].Split(',');
                        highScoreNames[i].text = values[0].TrimStart('(');
                        highScoreTexts[i].text = values[1].TrimEnd(')').PadLeft(11, '0');
                    }
                }
            });
        }
        
    }
}