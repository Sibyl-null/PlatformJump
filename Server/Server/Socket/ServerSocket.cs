using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server
{
    class ServerSocket
    {
        private Socket socket;

        // 监听的客户端socket，key是clientID
        private Dictionary<int, ClientSocket> clientDic = new Dictionary<int, ClientSocket>();

        /// <summary>
        /// 开启服务器
        /// </summary>
        /// <param name="ip"> 绑定的ip地址 </param>
        /// <param name="port"> 绑定的端口号 </param>
        /// <param name="listenNum"> 监听的数量 </param>
        public void Start(string ip, int port, int listenNum)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            try
            {
                socket.Bind(ipPoint);
                socket.Listen(listenNum);

                // 接受客户端连入
                socket.BeginAccept(AcceptCallBack, null);
            }
            catch (SocketException e)
            {
                Console.WriteLine("server start failed " + e.Message + " " + e.ErrorCode);
            }

            Console.WriteLine("server start!");
        }

        private void AcceptCallBack(IAsyncResult result)
        {
            try
            {
                Socket linkSocket = socket.EndAccept(result);
                ClientSocket clientSocket = new ClientSocket(linkSocket);

                clientDic.Add(clientSocket.clientID, clientSocket);

                // 继续接受客户端连入
                socket.BeginAccept(AcceptCallBack, null);
            }
            catch (SocketException e)
            {
                Console.WriteLine("client link failed " + e.Message + " " + e.ErrorCode);
            }
        }

        /// <summary>
        /// 广播消息
        /// </summary>
        public void Broadcast(BaseMsg msg)
        {
            foreach (ClientSocket clientSocket in clientDic.Values)
            {
                clientSocket.Send(msg);
            }
        }

        public void CloseClientSocket(ClientSocket socket)
        {
            lock (clientDic)
            {
                if (clientDic.ContainsKey(socket.clientID))
                {
                    clientDic.Remove(socket.clientID);
                    Console.WriteLine("客户端{0}断开链接", socket.clientID);
                    socket.Close();
                }
            }
        }
    }
}
