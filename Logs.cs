using System;
using System.IO;

namespace Client_Management_2._1
{
    class Logs
    {
        public static void CreateLog(Exception ex)
        {
            File.AppendAllText("log.txt", ex.Message);
            File.AppendAllText("log.txt", Environment.NewLine);
            File.AppendAllText("log.txt", DateTime.Now.ToString("dd.MM.yyyy HH:mm"));
            File.AppendAllText("log.txt", Environment.NewLine + "****************************" + Environment.NewLine);
        }
    }
}
