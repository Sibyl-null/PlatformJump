using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace NetWork
{
    public class NetManager : BaseSingleton<NetManager>
    {
        private Socket _socket;

        private byte[] _cacheBytes = new byte[1024];
        private int _cacheNum = 0;

        private Queue<BaseMsg> _receiveQueue = new Queue<BaseMsg>();
        
        //发送心跳消息的间隔时间
        private double SEND_HEART_MSG_TIME = 5;
        private HeartMsg hearMsg = new HeartMsg();
        
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
                
                SendHeartMsgAsync().Forget();    //开始发送心跳消息
                MsgHandleAsync().Forget();    //开始分帧处理消息
                
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
                HandleReceiveMsg(args.BytesTransferred);

                // 继续接受消息
                args.SetBuffer(_cacheNum, args.Buffer.Length - _cacheNum);
                if (_socket != null && _socket.Connected)
                    _socket.ReceiveAsync(args);
                else
                    Close();
            }
            else
            {
                Debug.Log("receive failed " + args.SocketError);
                Close();
            }
        }

        /// <summary>
        /// 发送消息异步
        /// </summary>
        public void Send(BaseMsg msg, Action action = null)
        {
            if (_socket != null && _socket.Connected)
            {
                byte[] bytes = msg.Writing();

                SocketAsyncEventArgs sendArgs = new SocketAsyncEventArgs();
                sendArgs.SetBuffer(bytes, 0, bytes.Length);
                sendArgs.Completed += (obj, args) =>
                {
                    if (args.SocketError != SocketError.Success)
                    {
                        Debug.Log("send failed " + args.SocketError);
                        Close();
                    }
                    else
                    {
                        action?.Invoke();
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
        /// 同步发送消息
        /// </summary>
        public void SendNoAsync(BaseMsg msg)
        {
            if (_socket != null && _socket.Connected)
            {
                byte[] bytes = msg.Writing();
                try
                {
                    _socket.Send(bytes);
                }
                catch (SocketException e)
                {
                    Debug.Log("SendNoAsync failed" + e.Message + " " + e.ErrorCode);
                    Close();
                }
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

        /// <summary>
        /// 定时给服务端发送心跳消息
        /// </summary>
        private async UniTask SendHeartMsgAsync()
        {
            while (_socket != null && _socket.Connected)
            {
                Send(hearMsg);
                await UniTask.Delay(TimeSpan.FromSeconds(SEND_HEART_MSG_TIME));
            }
        }
        
        // 处理分包，粘包
        private void HandleReceiveMsg(int receiveNum)
        {
            Debug.Log("dadada");
            int msgID = 0;
            int msgLength = 0;
            int nowIndex = 0;

            _cacheNum += receiveNum;

            while (true)
            {
                msgLength = -1;
                
                if (_cacheNum - nowIndex >= 8)  //处理消息头
                {
                    msgID = BitConverter.ToInt32(_cacheBytes, nowIndex);
                    nowIndex += 4;
                    
                    msgLength = BitConverter.ToInt32(_cacheBytes, nowIndex);
                    nowIndex += 4;
                }

                if (_cacheNum - nowIndex >= msgLength && msgLength != -1)
                {
                    //解析消息体
                    BaseMsg baseMsg = MsgBytesHandle(msgID, nowIndex);
                    
                    if (baseMsg != null) _receiveQueue.Enqueue(baseMsg);
                    
                    nowIndex += msgLength;
                    if (nowIndex == _cacheNum)
                    {
                        _cacheNum = 0;
                        break;
                    }
                }
                else
                {
                    if (msgLength != -1) nowIndex -= 8;
                    Array.Copy(_cacheBytes, nowIndex, _cacheBytes, 0, _cacheNum - nowIndex);
                    _cacheNum = _cacheNum - nowIndex;
                    break;
                }
            }
        }
        
        /// <summary>
        /// 解析消息体
        /// </summary>
        private BaseMsg MsgBytesHandle(int msgID, int nowIndex)
        {
            BaseMsg baseMsg = null;
            switch (msgID)
            {
                case MsgId.HeartMsg:
                    baseMsg = new HeartMsg();
                    break;
            }
            return baseMsg;
        }

        // 分帧处理消息队列里的消息
        private async UniTask MsgHandleAsync()
        {
            while (_socket != null && _socket.Connected)
            {
                if (_receiveQueue.Count > 0)
                {
                    BaseMsg baseMsg = _receiveQueue.Dequeue();
                    
                    switch (baseMsg)
                    {
                        
                    }
                }
                await UniTask.Yield();
            }
        }
    }
}

