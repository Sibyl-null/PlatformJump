using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class RecordRankData
{
    private static RecordRankData instance = new RecordRankData();
    public static RecordRankData Instance => instance;

    // key:name, value:recordData
    private Dictionary<string, RecordData> recordDic = new Dictionary<string, RecordData>();

    private const string path = @"D:\Studio\C#\Server\Server\LocalData\RecordRankData.yang";

    private RecordRankData()
    {
        using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
        {
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);
            fs.Dispose();

            if (bytes.Length >= 4)
            {
                int index = 0;
                int size = BitConverter.ToInt32(bytes, index);
                index += 4;
                for (int i = 0; i < size; ++i)
                {
                    RecordData recordData = new RecordData();
                    index += recordData.Reading(bytes, index);

                    recordDic.Add(recordData.name, recordData);
                }
            }
        }

        foreach (RecordData data in recordDic.Values)
        {
            Console.WriteLine(data.name + "  " + data.record);
        }
    }

    private void SaveData()
    {
        using (FileStream fs = new FileStream(path, FileMode.Create))
        {
            fs.Write(BitConverter.GetBytes(recordDic.Count));
            foreach (RecordData data in recordDic.Values)
            {
                fs.Write(data.Writing());
            }
            fs.Flush();
            fs.Dispose();
        }
    }

    public void AddRecord(RecordData record)
    {
        if (recordDic.ContainsKey(record.name))
        {
            if (recordDic[record.name].record < record.record)
            {
                recordDic[record.name].record = record.record;
                SaveData();
            }
        }
        else
        {
            recordDic.Add(record.name, record);
            SaveData();
        }
    }

    public List<RecordData> GetData()
    {
        return recordDic.Values.ToList();
    }
}