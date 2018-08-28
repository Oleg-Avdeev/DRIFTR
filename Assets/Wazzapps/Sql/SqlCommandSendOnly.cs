using System;
using System.Collections.Generic;
using UnityEngine.Networking;

namespace Wazzapps.Sql
{
    public class SqlCommandSendOnly : SqlCommand
    {
        public SqlCommandSendOnly(string command, Action<string[], List<string[]>> onReadOver = null,
            float timeoutDelay = 2f) : base(command, onReadOver, timeoutDelay)
        {
        }

        public SqlCommandSendOnly(string command, string postData, Action<string[], List<string[]>> onReadOver = null,
            float timeoutDelay = 2f) : base(command, postData, onReadOver, timeoutDelay)
        {
        }

        protected override void RequestHandle(UnityWebRequest request)
        {
        }
    }
}