using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            RecordRankData recordRankData = RecordRankData.Instance;

            NetMgr.Instance.StartServer("127.0.0.1", 8080, 128);

            while (true)
            {
                string input = Console.ReadLine();
            }
        }
    }
}
