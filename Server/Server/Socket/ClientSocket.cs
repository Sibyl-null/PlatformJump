using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server
{
    class ClientSocket
    {
        private static int CLIENT_NEXT_ID = 1;

        public Socket socket;
        public int clientID;

        private byte[] cacheBytes = new byte[1024];
        private int cacheNum = 0;   //当前缓冲区中有多少字节

        private long preTime = -1;   //上一次收到消息的时间
        private static int TIME_OUT_TIME = 10;   //超时时间

        public ClientSocket(Socket socket)
        {
            this.socket = socket;
            clientID = CLIENT_NEXT_ID++;

            // 开始接受消息
            this.socket.BeginReceive(cacheBytes, cacheNum, cacheBytes.Length, 
                                    SocketFlags.None, ReceiveCallBack, null);
            // 开启定时检测
            ThreadPool.QueueUserWorkItem(CheckTimeOut);
        }

        /// <summary>
        /// 定时检测，超时断开连接
        /// </summary>
        private void CheckTimeOut(object obj)
        {
            while (socket != null && socket.Connected)
            {
                if (preTime != -1 && DateTime.Now.Ticks / TimeSpan.TicksPerSecond - preTime >= TIME_OUT_TIME)
                {
                    NetMgr.Instance.CloseClient(this);
                    break;
                }
            }
            Thread.Sleep(5000);
        }

        private void ReceiveCallBack(IAsyncResult result)
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    int num = socket.EndReceive(result);
                    HandleReceiveMsg(num);  // 处理分包粘包
                    // 继续接受消息
                    socket.BeginReceive(cacheBytes, cacheNum, cacheBytes.Length,
                                            SocketFlags.None, ReceiveCallBack, null);
                }
                else
                {
                    NetMgr.Instance.CloseClient(this);
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("receive message failed " + e.Message + " " + e.ErrorCode);
                NetMgr.Instance.CloseClient(this);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        public void Send(BaseMsg msg)
        {
            if (socket.Connected)
            {
                byte[] bytes = msg.Writing();
                socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, SendCallBack, null);
            }
            else
                NetMgr.Instance.CloseClient(this);
        } 

        private void SendCallBack(IAsyncResult result)
        {
            try
            {
                socket.EndSend(result);
                Console.WriteLine("send success");
            }
            catch (SocketException e)
            {
                Console.WriteLine("send message failed " + e.Message + " " + e.ErrorCode);
                NetMgr.Instance.CloseClient(this);
            }
        }

        /// <summary>
        /// 处理分包粘包
        /// </summary>
        /// <param name="num"> 收到消息长度 </param>
        private void HandleReceiveMsg(int receiveNum)
        {
            int msgID = 0;
            int msgLength = 0;
            int nowIndex = 0;

            cacheNum += receiveNum;

            while (true)   // 循环处理粘包
            {
                msgLength = -1;

                if (cacheNum - nowIndex >= 8)   // 处理消息头
                {
                    msgID = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;

                    msgLength = BitConverter.ToInt32(cacheBytes, nowIndex);
                    nowIndex += 4;
                }

                // 消息完整的情况
                if (msgLength != -1 && cacheNum - nowIndex >= msgLength)
                {
                    //解析消息体
                    BaseMsg baseMsg = MsgBytesHandle(msgID, nowIndex);

                    if (baseMsg != null) ThreadPool.QueueUserWorkItem(MsgHandle, baseMsg);
                    nowIndex += msgLength;

                    if (nowIndex == cacheNum)
                    {
                        cacheNum = 0;
                        break;
                    }
                }
                else  // 消息不完整，有分包
                {
                    // 解析了消息头，但消息体不足。下标回退
                    if (msgLength != -1) nowIndex -= 8;

                    // 移到前面来，下次消息完整了再处理
                    Array.Copy(cacheBytes, nowIndex, cacheBytes, 0, cacheNum - nowIndex);
                    cacheNum = cacheNum - nowIndex;
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
                case MsgId.AddRecordMsg:
                    baseMsg = new AddRecordMsg();
                    baseMsg.Reading(cacheBytes, nowIndex);
                    break;
                case MsgId.GetRankMsg:
                    baseMsg = new GetRankMsg();
                    break;
                case MsgId.QuitMsg:
                    baseMsg = new QuitMsg();
                    break;
                case MsgId.HeartMsg:
                    baseMsg = new HeartMsg();
                    break;
            }
            return baseMsg;
        }

        /// <summary>
        /// 消息处理
        /// </summary>
        /// <param name="obj"> 实际消息 </param>
        private void MsgHandle(object obj)
        {
            switch (obj)
            {
                case AddRecordMsg msg:
                    msg.ServerHanle();
                    break;
                case GetRankMsg msg:
                    GiveRankMsg giveRankMsg = new GiveRankMsg();
                    giveRankMsg.records = RecordRankData.Instance.GetData();
                    Send(giveRankMsg);
                    break;
                case QuitMsg msg:
                    NetMgr.Instance.CloseClient(this);
                    break;
                case HeartMsg msg:
                    preTime = DateTime.Now.Ticks / TimeSpan.TicksPerSecond;
                    Console.WriteLine("client {0} heart", clientID);
                    break;
            }
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
                socket = null;
            }
        }
    }
}
