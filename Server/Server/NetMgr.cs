using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    class NetMgr
    {
        private static NetMgr instance = new NetMgr();
        public static NetMgr Instance => instance;

        private ServerSocket serverSocket;

        public void StartServer(string ip, int port, int num)
        {
            serverSocket = new ServerSocket();
            serverSocket.Start(ip, port, num);
        }

        public void CloseClient(ClientSocket socket)
        {
            serverSocket.CloseClientSocket(socket);
        }
    }
}
