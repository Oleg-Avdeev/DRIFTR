using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace Wazzapps.Sql
{
    public abstract class SqlCommand
    {
        public string CommandText;
        public Action<string[], List<string[]>> OnReadOver;
        public string PostData;

        private DateTime startTime;
        private MonoBehaviour sqlRequestMonoBehaviour;
        protected string[] columns = null;
        protected List<string[]> data = null;
        protected float timeOutDelay = 2f;
        protected bool usePost;

        public SqlCommand(string command, Action<string[], List<string[]>> onReadOver = null, float timeOutDelay = 2f)
        {
            CommandText = command;
            OnReadOver = onReadOver;
            PostData = null;
            this.timeOutDelay = timeOutDelay;
        }
        public SqlCommand(string command, string postData, Action<string[], List<string[]>> onReadOver = null, float timeOutDelay = 2f)
        {
            CommandText = command;
            OnReadOver = onReadOver;
            PostData = postData;
            this.timeOutDelay = timeOutDelay;
        }
        public void Execute()
        {
            startTime = DateTime.Now;
            if (sqlRequestMonoBehaviour == null)
            {
                sqlRequestMonoBehaviour = new GameObject("sqlRequestMonoBehaviour").AddComponent<EmptyBehaviour>();
                Object.DontDestroyOnLoad(sqlRequestMonoBehaviour.gameObject);
            }
            sqlRequestMonoBehaviour.StartCoroutine(GetRequest());
        }

        private IEnumerator GetRequest()
        {
            UnityWebRequest request;
            if (PostData != null)
            {
                WWWForm postForm = new WWWForm();
                postForm.AddField("req", PostData);
                request = UnityWebRequest.Post(CommandText, postForm);
            }
            else
            {
                request = UnityWebRequest.Get(CommandText);
            }
            request.timeout = (int)Math.Ceiling(timeOutDelay);
            yield return request.SendWebRequest();
            if (!request.isNetworkError)
            {
                RequestHandle(request);
            }
            else
            {
                Debug.Log("SQL ERROR request: " + CommandText);
                Debug.Log("SQL ERROR: " + request.error);
            }
            if (OnReadOver != null)
            {
                OnReadOver(columns, data);
            }
            if (sqlRequestMonoBehaviour != null) { Object.Destroy(sqlRequestMonoBehaviour.gameObject); sqlRequestMonoBehaviour = null; }
            Debug.Log("sqlCommand completed in " + (DateTime.Now - startTime).TotalMilliseconds.ToString("F0") + "ms");
            SqlCommandManager.SqlCommandOver(this);
        }

        protected abstract void RequestHandle(UnityWebRequest request);
    }
}
