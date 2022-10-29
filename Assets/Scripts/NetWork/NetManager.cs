using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace NetWork
{
    public class NetManager : BaseSingleton<NetManager>
    {
        private Socket _socket;

        private byte[] _cacheBytes = new byte[1024];
        private int _cacheNum = 0;

        private NetManager(){}
        
        /// <summary>
        /// 链接服务器
        /// </summary>
        /// <param name="ip"> 目标ip地址 </param>
        /// <param name="port"> 目标端口号 </param>
        public void Connect(string ip, int port)
        {
            if (_socket != null && _socket.Connected) return;

            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            SocketAsyncEventArgs args = new SocketAsyncEventArgs();
            args.RemoteEndPoint = ipPoint;
            args.Completed += ConnectCallBack;

            _socket.ConnectAsync(args);
        }

        private void ConnectCallBack(object obj, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Debug.Log("connect success");
                
                // 开始接受消息
                SocketAsyncEventArgs receiveArgs = new SocketAsyncEventArgs();
                receiveArgs.SetBuffer(_cacheBytes, 0, _cacheBytes.Length);
                receiveArgs.Completed += ReceiveCallBack;

                _socket.ReceiveAsync(receiveArgs);
            }
            else
            {
                Debug.Log("connect failed " + args.SocketError);
            }
        }

        private void ReceiveCallBack(object obj, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                Debug.Log(Encoding.UTF8.GetString(args.Buffer, 0, args.BytesTransferred));
                
                // 继续接受消息
                args.SetBuffer(0, args.Buffer.Length);
                if (_socket != null && _socket.Connected)
                    _socket.ReceiveAsync(args);
            }
            else
            {
                Debug.Log("receive failed " + args.SocketError);
                Close();
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void Send(string str)
        {
            if (_socket != null && _socket.Connected)
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);

                SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                sendArgs.SetBuffer(bytes, 0, bytes.Length);
                sendArgs.Completed += (obj, args) =>
                {
                    if (args.SocketError != SocketError.Success)
                    {
                        Debug.Log("send failed " + args.SocketError);
                        Close();
                    }
                };
                
                _socket.SendAsync(sendArgs);
            }
            else
            {
                Close();
            }
        }

        /// <summary>
        /// 关闭客户端链接
        /// </summary>
        public void Close()
        {
            if (_socket != null)
            {
                _socket.Shutdown(SocketShutdown.Both);
                _socket.Disconnect(false);
                _socket.Close();
                _socket = null;
            }
        }
    }
}

