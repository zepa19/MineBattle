using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools : Block
{

    public float power;
    public float attack;
    
    public int durability;

    public enum Type
    {
        NONE,
        DIRT,
        STONE,
        WOOD,
        SWORD
    }

    public Type ToolType;

    public Tools(string Name, Texture2D Icon) : base(Name, Icon)
    {
        durability = 100;
        ToolType = Type.NONE;
        base.InHand = true;

        if (Name.Equals("Sword"))
        {
            power = 1f;
            attack = 7f;
            ToolType = Type.SWORD;
        }
        else
        {
            power = 5f;
            attack = 2f;

            if(Name.Equals("Axe"))
            {
                ToolType = Type.WOOD;
            }
            else if (Name.Equals("Shovel"))
            {
                ToolType = Type.DIRT;
            }
            else if(Name.Equals("Pickaxe"))
            {
                ToolType = Type.STONE;
            }
        }

    }

    public override float GetBreakingTime(Block d, int level)
    {

        float MaxBreakingTime = 1000f;

        if(ToolType == Type.NONE)
        {
            return MaxBreakingTime;
        }


        if(ToolType == Type.SWORD)
        {
            return 100000f;
        }
        
        if((ToolType == Type.STONE && d.BType == BlockType.STONE) || (ToolType == Type.WOOD && d.BType == BlockType.WOOD) || (ToolType == Type.DIRT && d.BType == BlockType.DIRT))
        {
            return MaxBreakingTime / 2 - level * 100;
        }

        return MaxBreakingTime * 0.6f;

    }

    public override int GetHitDamage(int level = 1)
    {
        if (ToolType == Type.NONE)
            return base.GetHitDamage(level);

        if (ToolType == Type.SWORD)
            return level * 9;

        return level * 4;
    }

    public float GetDamage()
    {
        return 1f;
    }

    public Tools(string Name, bool IsTransparent) : base(Name, IsTransparent)
    {

    }
    public Tools(string Name, bool IsTransparent, string name, Texture2D Miniature) : base(Name, IsTransparent, name, Miniature)
    {

    }
}
