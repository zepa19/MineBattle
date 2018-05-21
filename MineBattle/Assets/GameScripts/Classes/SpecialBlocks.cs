using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialBlocks : Block
{
    bool ChestPlate = false;
    int level = 1;

    public SpecialBlocks(string Name, Texture2D icon, bool isChestPlate) : base(Name, icon)
    {
        ChestPlate = isChestPlate;
    }

    public int GetShieldPower()
    {
        if (ChestPlate)
            return level * 25;

        return 0;
    }

    public SpecialBlocks(string Name, bool IsTransparent) : base(Name, IsTransparent)
    {
    }

    

    public SpecialBlocks(string Name, bool IsTransparent, string name, Texture2D Miniature) : base(Name, IsTransparent, name, Miniature)
    {
    }
}
