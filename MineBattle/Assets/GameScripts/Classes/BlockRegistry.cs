using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockRegistry {

    private static readonly bool DebugMode = true;
    public static List<Block> _RegisteredBlocks = new List<Block>();


    public static void RegisterBlock(Block b)
    {
        _RegisteredBlocks.Add(b);
    }

    public static void RegisterBlocks()
    {
        if (DebugMode)
        {
            int i = 0;
            List<string> _names = new List<string>();

            foreach (Block b in _RegisteredBlocks)
            {
                _names.Add(string.Format("CurrentID: {0}, BlockName: {1}, BlockID: {2}", i, b.GetBlockName(), b.GetID()));
                i++;
            }

            System.IO.File.WriteAllLines("BlockRegistry.txt", _names.ToArray());
        }

    }

    public static bool CheckIfExists(string FileName)
    {
        foreach (Block b in _RegisteredBlocks)
        {
            if (b.GetFileName().Equals(FileName))
            {
                return true;
            }
        }

        return false;
    }

    internal static Block GetBlockFromBlockName(string name)
    {
        try
        {
            foreach (Block b in _RegisteredBlocks)
            {
                if (b.GetBlockName().Equals(name))
                    return b;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return null;
    }

    internal static int GID(string name)
    {
        try
        {
            foreach (Block b in _RegisteredBlocks)
            {
                if (b.GetBlockName().Equals(name))
                    return b.GetID();
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return 0;
    }

    internal static Block GetBlockFromID(int v)
    {
        try
        {
            foreach (Block b in _RegisteredBlocks)
            {
                if (b.GetID().Equals(v))
                    return b;
            }
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        return null;
    }

}
