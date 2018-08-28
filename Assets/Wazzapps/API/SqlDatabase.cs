using System;
using System.Collections.Generic;
using System.Text;
using Wazzapps.Crypto;
using Wazzapps.Sql;
using UnityEngine;

namespace Wazzapps.API
{
    public static class SqlDatabase
    {
        private const string authorizationServerAdress = "http://remote-wazbaas.com/api/request.php?";
        private const string dataServerAdress = "http://remote-wazbaas.com/api/request.php?";
        private const string httpRequestPrefix = "req=";
        #region crypto
        private const string BD_KEY_0 = "59e16266e2feaf8";
        private const string BD_KEY_1 = "GGs1sd7Fdst2Olf2";
        public static string GetSecurityPrefix(string data)
        {
            StringBuilder prefixBuilder = new StringBuilder();
            prefixBuilder.Append("&time=");
            string timeString = (DateTime.UtcNow).ToString("u");
            prefixBuilder.Append(timeString);
            prefixBuilder.Append("&hash=");
            string hash = "false+hash";
            prefixBuilder.Append(hash);
            return prefixBuilder.ToString();
        }
        #endregion
        #region sql manager
        public static void Send(string command, Action<string[], List<string[]>> callback = null, float timeOutDelay = 2f)
        {
            SqlCommand sqlCommand;
            string cryptoCommand = dataServerAdress + httpRequestPrefix + command + GetSecurityPrefix(command);
            if (callback == null)
            {
                sqlCommand = new SqlCommandSendOnly(cryptoCommand, null, timeOutDelay);
            }
            else
            {
                sqlCommand = new SqlCommandGetValues(cryptoCommand, callback, timeOutDelay);
            }
            SqlCommandManager.ExecuteSqlCommand(sqlCommand);
        }
        public static void Send(string command, string postData, Action<string[], List<string[]>> callback = null, float timeOutDelay = 2f)
        {
            SqlCommand sqlCommand;
            string cryptoCommand = dataServerAdress + GetSecurityPrefix(command);
            if (callback == null)
            {
                sqlCommand = new SqlCommandSendOnly(cryptoCommand, command, null, timeOutDelay);
            }
            else
            {
                sqlCommand = new SqlCommandGetValues(cryptoCommand, command, callback, timeOutDelay);
            }
            SqlCommandManager.ExecuteSqlCommand(sqlCommand);
        }
        #endregion
        #region authorization

        public static void Authenticate(string id, AutorizationType type, Action<string> onAuthenticate)
        {
            AutorizationType autorizationType = type;
            if (autorizationType != AutorizationType.device && string.IsNullOrEmpty(id))
            {
                Debug.LogWarning("Autorization ID is null");
                autorizationType = AutorizationType.device;
            }
            StringBuilder commandBuilder = new StringBuilder();
            commandBuilder.Append("select Login(\'");
            if (type != AutorizationType.device)
            {
                commandBuilder.Append(id);
            }
            commandBuilder.Append("\',\'");
            commandBuilder.Append(SystemInfo.deviceUniqueIdentifier);
            commandBuilder.Append("\',\'");
            commandBuilder.Append(autorizationType.ToString());
            commandBuilder.Append("\');");
            Send(commandBuilder.ToString(), (columns, data) => OnAuthenticate(columns, data, onAuthenticate));
        }
        private static void OnAuthenticate(string[] columns, List<string[]> data, Action<string> onAuthenticate)
        {
            string loginID = null;
            if (columns != null && data != null && data.Count > 0 && data[0].Length > 0 && !string.IsNullOrEmpty(data[0][0]))
            {
                loginID = data[0][0];
                Debug.Log("Wazzapps Authenticated as: " + loginID);
                if (loginID.Equals("ERROR") || loginID.Length < 2 || loginID[0] != 'w')
                {
                    loginID = "";
                }
            }
            if (onAuthenticate != null)
            {
                onAuthenticate(loginID);
            }
        }
        public enum AutorizationType
        {
            googleplay,
            googleplus,
            vkontakte,
            facebook,
            device
        }
        #endregion
    }
}