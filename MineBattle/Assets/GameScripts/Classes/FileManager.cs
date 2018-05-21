using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FileManager
{

    public static readonly string WorldsDirectory = "Data/World/";
    public static string PlayerName = "Player";
    public static string GameName = "DevWorld";

    public static void RegisterFiles()
    {
        Serializer.Check_Gen_Folder(WorldsDirectory);
        Serializer.Check_Gen_Folder(GetSaveDirectory());
        Serializer.Check_Gen_Folder(GetSaveDirectory() + "Chunks/");
    }

    public static string GetChunkString(int x, int y, int z)
    {
        return string.Format("{0}C{1}_{2}_{3}.chk", GetSaveDirectory() + "Chunks/", x, y, z);
    }

    public static string GetSaveDirectory()
    {
        return WorldsDirectory + GameName + "/";
    }

    public static string GetPlayerSaveFileName(int i)
    {
        return string.Format("{0}{1}{2}.dat", GetSaveDirectory(), PlayerName, i);
    }

}