
/// <summary>
/// 心跳消息
/// </summary>
class HeartMsg : BaseMsg
{
    public override int GetID()
    {
        return MsgId.HeartMsg;
    }

    public override int GetBytesNum()
    {
        return 8;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        // 没有消息体
        return 0;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, 0, ref index);
        return bytes;
    }
}