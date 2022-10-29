using System;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

public class ProtoTool
{
    // Server文件夹位置
    private static string SERVER_PATH = Application.dataPath.Replace("Assets", "Server");
    // protobuf描述文件的文件夹位置
    private static string PROTO_PATH = SERVER_PATH + "/Scheme";
    // protobuf生成工具位置
    private static string PROTO_EXE_PATH = SERVER_PATH + "/protoc.exe";

    // 生成C#文件指令参数
    private static string CSHARP_ARG = "csharp_out";
    // 生成C#文件存放位置
    private static string CSHARP_PATH = Application.dataPath + "/Scripts/NetWork";
    
    [MenuItem("NetTool/ProtoTool/打开Server文件夹")]
    public static void OpenServerFolder()
    {
        EditorUtility.RevealInFinder(SERVER_PATH);
    }
    
    [MenuItem("NetTool/ProtoTool/生成C#代码")]
    private static void GenerateCSharp()
    {
        Generate(CSHARP_ARG, CSHARP_PATH);
    }

    private static void Generate(string outCmd, string outPath)
    {
        DirectoryInfo directoryInfo = Directory.CreateDirectory(PROTO_PATH);
        FileInfo[] fileInfos = directoryInfo.GetFiles();
        foreach (FileInfo fileInfo in fileInfos)
        {
            if (fileInfo.Extension == ".proto")
            {
                try
                {
                    Process cmd = new Process();
                    cmd.StartInfo.FileName = PROTO_EXE_PATH;
                    cmd.StartInfo.Arguments = $"-I={PROTO_PATH} --{outCmd}={outPath} {fileInfo}";
                    cmd.Start();
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log(fileInfo.Name + "生成失败 " + e.Message);
                }
            }
        }
        UnityEngine.Debug.Log("生成结束");
        AssetDatabase.Refresh();
    }
}
