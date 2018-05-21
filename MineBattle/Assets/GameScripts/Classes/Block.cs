using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block {

    private Vector2[] _UvMap_Top;
    private Vector2[] _UvMap_Bottom;
    private Vector2[] _UvMap_Front;
    private Vector2[] _UvMap_Side;
    protected Texture2D Icon;

    public enum Direction
    {
        SOUTH,
        EAST,
        NORTH,
        WEST
    }

    public enum BlockType
    {
        NONE,
        DIRT,
        WOOD,
        STONE
    }

    public bool InHand;

    //private Direction FacingAt;
    public BlockType BType;

    private string FileName;
    protected string BlockName;

    protected int ID;
    protected static int CurrentID;

    private bool IsTransparent;
    public bool InCreativeInventory;

    public Block(string Name, bool IsTransparent)
    {
        this.BlockName = Name;
        this.IsTransparent = IsTransparent;
        //this.FacingAt = Direction.NORTH;
        this.BType = BlockType.NONE;
        this.InCreativeInventory = false;
        this.InHand = false;

        ID = CurrentID;
        CurrentID++;
    }

    public Block(string Name, bool IsTransparent, string name, Texture2D Miniature)
    {
        this.BlockName = Name;
        this.IsTransparent = IsTransparent;
        this.FileName = name;
        this.Icon = Miniature;
        //this.FacingAt = Direction.NORTH;
        this.InCreativeInventory = true;
        this.InHand = false;

        if (this.FileName.Equals("furnaceon"))
            this.InCreativeInventory = false;

        foreach (UvMap u in UvMap.getUvMap(this.FileName))
        {
            if (u.typ.Equals(UvMap.Type.Top))
            {
                _UvMap_Top = u._UVMAP;
                continue;
            }

            if (u.typ.Equals(UvMap.Type.Bottom))
            {
                _UvMap_Bottom = u._UVMAP;
                continue;
            }

            if (u.typ.Equals(UvMap.Type.Front))
            {
                _UvMap_Front = u._UVMAP;
                continue;
            }

            if (u.typ.Equals(UvMap.Type.Side))
            {
                _UvMap_Side = u._UVMAP;
                continue;
            }
        }

        if (_UvMap_Top == null)
            _UvMap_Top = _UvMap_Side;

        if (_UvMap_Bottom == null)
            _UvMap_Bottom = _UvMap_Side;

        if (_UvMap_Front == null)
            _UvMap_Front = _UvMap_Side;

        if(InCreativeInventory && Icon == null)
        {
            Logger.Log(string.Format("Cant find an icon to {0}", FileName));
        }

        SetBlockType();

        ID = CurrentID;
        CurrentID++;
    }

    public Block(string Name, Texture2D icon)
    {
        this.BlockName = Name;
        this.IsTransparent = false;
        this.FileName = "";
        this.Icon = icon;
        //this.FacingAt = Direction.NORTH;
        this.BType = BlockType.NONE;
        this.InCreativeInventory = true;
        this.InHand = false;

        if (InCreativeInventory && Icon == null)
        {
            Logger.Log(string.Format("Cant find an icon to {0}", FileName));
        }

        ID = CurrentID;
        CurrentID++;
    }

    public string GetBlockName()
    {
        return BlockName;
    }

    public string GetFileName()
    {
        return FileName;
    }

    public int GetID()
    {
        return ID;
    }

    public Texture2D GetIcon()
    {
        return this.Icon;
    }

    public bool Istransparent()
    {
        return IsTransparent;
    }

    public virtual float GetBreakingTime(Block d, int level)
    {

        if(ID == 0)
            return 1000f;

        return 800f;
    }

    public virtual int GetHitDamage(int level = 1)
    {
        return 4;
    }

    private void SetBlockType()
    {
        this.BType = BlockType.NONE;

        if (this.BlockName.Equals("Bedrock"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Brick"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Clay"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Coalblock"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Cobblestone"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("CobblestoneMossy"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Craftingtable"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Diamond"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Dirt"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Endbricks"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Endstone"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Furnaceoff"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Furnaceon"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Goldblock"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Grass"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Gravel"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Ice"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Icepacked"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Ironblock"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Lapisblock"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Logacacia"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Logbigoak"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Logbirch"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Logjungle"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Logoak"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Logspruce"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Planksacacia"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Planksbigoak"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Planksbirch"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Planksjungle"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Planksoak"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Planksspruce"))
        {
            this.BType = BlockType.WOOD;
        }
        else if (this.BlockName.Equals("Sand"))
        {
            this.BType = BlockType.DIRT;
        }
        else if (this.BlockName.Equals("Sandstone"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Stone"))
        {
            this.BType = BlockType.STONE;
        }
        else if (this.BlockName.Equals("Stonebrick"))
        {
            this.BType = BlockType.STONE;
        }
    }

    public virtual MeshData Draw(Chunk chunk, Block[,,] _Blocks, int x, int y, int z)
    {
        if (this.ID.Equals(0))  // Air
            return new MeshData();

        return MathHelper.DrawCube(chunk, _Blocks, this, x, y, z, this._UvMap_Top, this._UvMap_Bottom, this._UvMap_Front, this._UvMap_Side);
    }

}
