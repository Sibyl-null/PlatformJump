using System.Collections.Generic;

class GiveRankMsg : BaseMsg
{
    public List<RecordData> records = new List<RecordData>();

    public override int GetID()
    {
        return MsgId.GiveRankMsg;
    }

    public override int GetBytesNum()
    {
        // 消息id + 消息大小 + 元素个数
        int res = 4 + 4 + 4;
        foreach (RecordData data in records)
        {
            res += data.GetBytesNum();
        }
        return res;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;

        int size = ReadInt(bytes, ref index);
        for (int i = 0; i < size; ++i)
        {
            RecordData recordData = ReadData<RecordData>(bytes, ref index);
            records.Add(recordData);
        }

        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteInt(bytes, GetID(), ref index);
        WriteInt(bytes, GetBytesNum(), ref index);
        WriteInt(bytes, records.Count, ref index);

        foreach (RecordData data in records)
        {
            WriteData(bytes, data, ref index);
        }
        return bytes;
    }
}