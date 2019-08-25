using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System.Text;
using Game;

namespace UI
{
    public class HighscoresPanel : Controller
    {
        [SerializeField] private InputField playerNameInput;
        [SerializeField] private Text endPlayerScore;
        [SerializeField] private Text[] highScoreNames;
        [SerializeField] private Text[] highScoreTexts;

        private long mask = 0x0000_0000_FFFF_FFFF;
        private byte shift = 32;
        private List<string> scoreList;
        private List<long> scoreValueList;


        public override void Initialize()
        {
            scoreList = GetScoreList();
            scoreValueList = new List<long>();
        }

        public override void Act() { }

        public void SetHighscores()
        {
            if (!string.IsNullOrEmpty(playerNameInput.text))
            {
                bool append = true;
                string name = playerNameInput.text.ToLower();
                
                for (int i = 0; i < scoreList.Count; i++)
                {
                    if (string.Compare(scoreList[i], name) == 0)
                    {
                        if (scoreValueList[i] > GameController.Score) return;
                        scoreValueList[i] = GameController.Score;
                        append = false;
                        break;
                    }
                }

                if (append)
                {
                    scoreList.Add(name);
                    scoreValueList.Add(GameController.Score);
                    SaveScoreList();
                }
                
                //Sorry, this is so stupid but fun
                PlayerPrefs.SetInt($"{name}_p1", (int)(GameController.Score & mask));
                PlayerPrefs.SetInt($"{name}_p2", (int)(GameController.Score >> shift & mask));
            }
        }

        public void ShowHighscores()
        {
            endPlayerScore.text = GameController.Score.ToString().PadLeft(11, '0');
            
            for (int i = 0; i < highScoreTexts.Length; i++)
            {
                if (i >= scoreList.Count)
                {
                    highScoreNames[i].gameObject.SetActive(false);
                    highScoreTexts[i].gameObject.SetActive(false);
                }
                else
                {
                    highScoreTexts[i].gameObject.SetActive(true);
                    highScoreNames[i].gameObject.SetActive(true);

                    if (i >= scoreValueList.Count) scoreValueList.Add(0);
                    scoreValueList[i] = GetScore(scoreList[i]);
                    highScoreNames[i].text = scoreList[i];
                    highScoreTexts[i].text = scoreValueList[i].ToString().PadLeft(11, '0');
                }
            }
        }

        private void SaveScoreList()
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < scoreList.Count; i++)
                builder.Append($"{scoreList[i]} ");
            PlayerPrefs.SetString("score_list", builder.ToString());
        }

        private List<string> GetScoreList()
        {
            string[] strings = PlayerPrefs.GetString("score_list", "").Split(' ');
            var list = new List<string>();
            for (int i = 0; i < strings.Length; i++) list.Add(strings[i]);
            return list;
        }

        private long GetScore(string name)
        {
            int p1 = PlayerPrefs.GetInt($"{name}_p1", 0);
            int p2 = PlayerPrefs.GetInt($"{name}_p2", 0);
            return (long)(p1 + p2 << shift);
        }
    }
}