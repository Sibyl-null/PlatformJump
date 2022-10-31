using System;
using System.Collections.Generic;
using System.Text;

public class RecordData : BaseData
{
    public string name;
    public int record;

    public override int GetBytesNum()
    {
        return 4 + 4 + Encoding.UTF8.GetBytes(name).Length;
    }

    public override int Reading(byte[] bytes, int beginIndex = 0)
    {
        int index = beginIndex;
        name = ReadString(bytes, ref index);
        record = ReadInt(bytes, ref index);
        return index - beginIndex;
    }

    public override byte[] Writing()
    {
        int index = 0;
        byte[] bytes = new byte[GetBytesNum()];
        WriteString(bytes, name, ref index);
        WriteInt(bytes, record, ref index);
        return bytes;
    }
}