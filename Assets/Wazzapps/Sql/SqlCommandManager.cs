using System.Collections.Generic;
using UnityEngine;

namespace Wazzapps.Sql
{
    public static class SqlCommandManager
    {
        private static SqlCommand currentCommand;
        private static List<SqlCommand> commandsQueue = new List<SqlCommand>();

        #region ManageSQLCommands

        public static void ExecuteSqlCommand(SqlCommand command)
        {
            if (currentCommand == null)
            {
                Debug.Log("SQL command send");
                currentCommand = command;
                currentCommand.Execute();
                Debug.Log("SQL command send over");
            }
            else
            {
                Debug.Log("SQL command added to queue");
                commandsQueue.Add(command);
            }
        }

        public static void SqlCommandOver(SqlCommand command)
        {
            currentCommand = null;
            if (commandsQueue.Count > 0)
            {
                currentCommand = commandsQueue[0];
                commandsQueue.RemoveAt(0);
                currentCommand.Execute();
            }
        }

        #endregion
    }
}