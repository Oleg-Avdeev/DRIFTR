using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace Wazzapps.Sql
{
    public class SqlCommandGetValues : SqlCommand
    {
        public SqlCommandGetValues(string command, Action<string[], List<string[]>> onReadOver = null,
            float timeoutDelay = 2f) : base(command, onReadOver, timeoutDelay)
        {
        }
        public SqlCommandGetValues(string command, string postData, Action<string[], List<string[]>> onReadOver = null,
            float timeoutDelay = 2f) : base(command, postData, onReadOver, timeoutDelay)
        {
        }

        protected override void RequestHandle(UnityWebRequest request)
        {
            string[] dataStrings = request.downloadHandler.text.Split('\n');
            if (dataStrings.Length < 1)
            {
                Debug.Log("SQL ERROR: data is empty");
                return;
            }
            columns = dataStrings[0].Split('\t');
            if (dataStrings.Length > 1)
            {
                data = new List<string[]>(dataStrings.Length - 1);
                for (int i = 1; i < dataStrings.Length; i++)
                {
                    if (string.IsNullOrEmpty(dataStrings[i]))
                    {
                        continue;
                    }
                    data.Add(dataStrings[i].Split('\t'));
                }
            }
        }
    }
}