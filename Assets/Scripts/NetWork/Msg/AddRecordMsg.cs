using System;
using System.Collections.Generic;
using System.Text;

class AddRecordMsg : BaseMsg
{
    public RecordData recordData = new RecordData();

    public override int GetID()
    {
        return MsgId.AddRecordMsg;
    }

    public override int GetBytesNum()
    {
        // 消息id + 消息大小 + 数据体
        return 4 + 4 + recordData.GetBytesNum();
    }

    public override byte[] Writing()
    {
        int index = 0;
        int bytesNum = GetBytesNum();
        byte[] bytes = new byte[bytesNum];

        WriteInt(bytes, GetID(), ref index);    //消息id
        WriteInt(bytes, GetBytesNum() - 8, ref index);   //消息大小
        WriteData(bytes, recordData, ref index);

        return bytes;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        recordData = ReadData<RecordData>(bytes, ref index);
        return index - beginIndex;
    }
}