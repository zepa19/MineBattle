using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerSettings {

    public static float MasterVolume = 10f;
    public static float MusicVolume = 10f;
    public static float SFXVolume = 10f;

    private static int renderdist = 3;

    public static bool FromSave = false;

    public static List<BlockItem> surv = new List<BlockItem>();
    public static List<BlockItem> craft = new List<BlockItem>();
    public static List<BlockItem> toolbox = new List<BlockItem>();
    public static int ArmorLevel = 0;

    public static bool RenderDistanceChanged = false;
    public static int RenderDistance
    {
        get
        {
            return renderdist;
        }
        set
        {
            if (value < 3)
                renderdist = 3;
            else if (value > 6)
                renderdist = 6;
            else
                renderdist = value;
        }
    }

    public static Vector3 Position;

    public static float[] GetSaveData()
    {
        float[] data = new float[6];

        data[0] = MasterVolume;
        data[1] = MusicVolume;
        data[2] = SFXVolume;
        data[3] = GameManager._Instance.PlayerPosition.x;
        data[4] = GameManager._Instance.PlayerPosition.y;
        data[5] = GameManager._Instance.PlayerPosition.z;

        return data;
    }

    public static int[] GetSaveData2()
    {
        int[] data = new int[133];
        int x = 3;

        data[0] = RenderDistance;

        data[1] = 0;
        if (GameManager._Instance.ModeOfTheGame == GameManager.GameMode.SURVIVAL)
            data[1] = 1;

        data[2] = Player.PStatus.Points;

        for (int i = 0; i < 9; i++, x += 3) 
        {
            data[x] = Player.PStatus.ToolBox9[i].BlockID;
            data[x + 1] = Player.PStatus.ToolBox9[i].Count;
            data[x + 2] = Player.PStatus.ToolBox9[i].Level;
        }

        if(data[1] == 1)
        {
            data[x++] = Player.PStatus.ArmorBIT.Level; //armor

            for (int i = 0; i < 27; i++, x += 3)  //inventory
            {
                data[x] = Player.PStatus.SurvivalInv[i].BlockID;
                data[x + 1] = Player.PStatus.SurvivalInv[i].Count;
                data[x + 2] = Player.PStatus.SurvivalInv[i].Level;
            }

            for (int i = 0; i < 4; i++, x += 3) //in crafting items
            {
                data[x] = GameManager._Instance.MCTGameObjs.B[i].BlockID;
                data[x + 1] = GameManager._Instance.MCTGameObjs.B[i].Count;
                data[x + 2] = GameManager._Instance.MCTGameObjs.B[i].Level;
            }
        }

        return data;
    }

    public static void PrepareGame(float[] data)
    {
        MasterVolume = data[0];
        MusicVolume = data[1];
        SFXVolume = data[2];

        Position = new Vector3(data[3], data[4], data[5]);
    }

    public static void PrepareGame(int[] data)
    {
        FromSave = true;

        if (!RenderDistanceChanged)
            RenderDistance = data[0];

        GameManager._Instance.ModeOfTheGame = GameManager.GameMode.CREATIVE;
        if (data[1] == 1)
            GameManager._Instance.ModeOfTheGame = GameManager.GameMode.SURVIVAL;

        Player.PStatus.Points = data[2];

        int x = 3;
        for (int i = 0; i < 9; i++, x += 3)
        {
            toolbox.Add(new BlockItem(data[x], data[x+1], data[x+2]));
        }

        if (data[1] == 1)
        {
            ArmorLevel = data[x++]; //armor

            for (int i = 0; i < 27; i++, x += 3)  //inventory
            {
                surv.Add(new BlockItem(data[x], data[x + 1], data[x + 2]));
            }

            for (int i = 0; i < 4; i++, x += 3) //in crafting items
            {
                craft.Add(new BlockItem(data[x], data[x + 1], data[x + 2]));
            }
        }

    }

}