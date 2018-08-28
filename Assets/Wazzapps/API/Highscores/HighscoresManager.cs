using System.Collections.Generic;
using Wazzapps.Sql;
using Wazzapps.API;
using UnityEngine;
using System;

namespace Wazzapps.API.HighScores
{
    public interface IHighscoresManager
    {
        void SetHighscore(string player, long highscore);
        void GetHighscores(Action<List<string>> onDataRecievedCallback);
    }

    public class HighscoresManager : IHighscoresManager
    {
        private static IHighscoresManager instance;
        public static IHighscoresManager Instance
        {
            get 
            {
                if (instance == null) instance = new HighscoresManager();
                return instance;
            }
        }

        public void GetHighscores(Action<List<string>> onDataRecievedCallback)
        {
            SqlDatabase.Send("SELECT get_highscores()", GetDataHandler(onDataRecievedCallback));
        }

        public void SetHighscore(string player, long highscore)
        {
            SqlDatabase.Send(string.Format("SELECT create_or_set_highscore(\'{0}\', {1})", player, highscore));
        }

        private Action<string[], List<string[]>> GetDataHandler(Action<List<string>> onDataRecievedCallback)
        {
            return (columns, rows) => {
                
                List<string> list = new List<string>();

                if (rows != null)
                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i] != null)
                    for (int j = 0; j < rows[i].Length; j++)
                    {
                        list.Add(rows[i][j]);
                    }
                }

                onDataRecievedCallback.Invoke(list);
            };
        }
    }
}