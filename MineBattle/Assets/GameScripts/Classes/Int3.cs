using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Int3
{
    public int x, y, z;

    public Int3(int x, int y, int z)
    {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    public Int3(Int3 a)
    {
        this.x = a.x;
        this.y = a.y;
        this.z = a.z;
    }

    public Int3(Vector3 pos, bool round = false)
    {
        if (round)
        {
            this.x = Mathf.RoundToInt(pos.x);
            this.y = Mathf.RoundToInt(pos.y);
            this.z = Mathf.RoundToInt(pos.z);
        }
        else
        {
            this.x = (int)pos.x;
            this.y = (int)pos.y;
            this.z = (int)pos.z;
        }
    }

    public static Int3 operator +(Int3 a, Int3 b)
    {
        Int3 c = new Int3(a);
        c.x += b.x;
        c.y += b.y;
        c.z += b.z;

        return c;
    }

    public static Int3 operator -(Int3 a, Int3 b)
    {
        Int3 c = new Int3(a);
        c.x -= b.x;
        c.y -= b.y;
        c.z -= b.z;

        return c;
    }

    public static bool operator ==(Int3 a, Int3 b)
    {
        if (a.x == b.x && a.y == b.y && a.z == b.z)
            return true;

        return false;

    }

    public static bool operator !=(Int3 a, Int3 b)
    {
        return !(a == b);

    }

    public static Vector3 ToVector3(Int3 a)
    {
        return new Vector3(a.x, a.y, a.z);
    }

    public override string ToString()
    {
        return string.Format("x:{0}, y{1}, z{2}", x, y, z);
    }

    internal void AddPos(Int3 int3)
    {
        this.x += int3.x;
        this.y += int3.y;
        this.z += int3.z;
    }

    internal void ToChunkCoordinates()
    {
        this.x = Mathf.FloorToInt(x / Chunk.ChunkWidth);
        this.y = Mathf.FloorToInt(y / Chunk.ChunkHeight);
        this.z = Mathf.FloorToInt(z / Chunk.ChunkWidth);
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}