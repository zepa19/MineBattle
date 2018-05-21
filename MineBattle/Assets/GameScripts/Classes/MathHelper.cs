using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathHelper {

    public static MeshData DrawCube(Chunk chunk, Block[,,] _Blocks, Block block, int x, int y, int z, Vector2[] _UvMap_Top, Vector2[] _UvMap_Bottom, Vector2[] _UvMap_Front, Vector2[] _UvMap_Side)
    {

        MeshData d = new MeshData();

        if (block.GetBlockName() == "Air")
        {
            return new MeshData();
        }

        if (y - 1 <= 0 || _Blocks[x, y - 1, z].Istransparent())
        {
            d.Merge(
                new MeshData(           // Bottom face
                    new List<Vector3>()
                    {
                        new Vector3(0, 0, 0),
                        new Vector3(0, 0, 1),
                        new Vector3(1, 0, 0),
                        new Vector3(1, 0, 1)
                    },
                    new List<int>()
                    {
                        0, 2, 1, 3, 1, 2
                    },
                    _UvMap_Bottom
                ));
        }

        if (y + 1 >= Chunk.ChunkHeight || _Blocks[x, y + 1, z].Istransparent())
        {
            d.Merge(
                new MeshData(           // Top face
                    new List<Vector3>()
                    {
                        new Vector3(0, 1, 0),
                        new Vector3(0, 1, 1),
                        new Vector3(1, 1, 0),
                        new Vector3(1, 1, 1)
                    },
                    new List<int>()
                    {
                        0, 1, 2, 3, 2, 1
                    },
                    _UvMap_Top
                ));
        }

        if (x + 1 >= Chunk.ChunkWidth || _Blocks[x + 1, y, z].Istransparent())
        {
            d.Merge(
                new MeshData(           // Back face
                    new List<Vector3>()
                    {
                        new Vector3(1, 0, 1),
                        new Vector3(1, 1, 1),
                        new Vector3(1, 0, 0),
                        new Vector3(1, 1, 0)
                    },
                    new List<int>()
                    {
                        0, 3, 1, 0, 2, 3
                    },
                    _UvMap_Side
                ));
        }

        if (x - 1 <= 0 || _Blocks[x - 1, y, z].Istransparent())
        {
            d.Merge(
                new MeshData(           // Front face
                    new List<Vector3>()
                    {
                        new Vector3(0, 0, 1),
                        new Vector3(0, 1, 1),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 1, 0)
                    },
                    new List<int>()
                    {
                        0, 1, 2, 3, 2, 1
                    },
                    _UvMap_Front
                ));
        }

        if (z + 1 >= Chunk.ChunkWidth || _Blocks[x, y, z + 1].Istransparent())
        {
            d.Merge(
                new MeshData(           // Right face
                    new List<Vector3>()
                    {
                        new Vector3(1, 0, 1),
                        new Vector3(1, 1, 1),
                        new Vector3(0, 0, 1),
                        new Vector3(0, 1, 1)
                    },
                    new List<int>()
                    {
                        0, 1, 2, 3, 2, 1
                    },
                    _UvMap_Side
                ));
        }

        if (z - 1 <= 0 || _Blocks[x, y, z - 1].Istransparent())
        {
            d.Merge(
                new MeshData(           // Left face
                    new List<Vector3>()
                    {
                        new Vector3(1, 0, 0),
                        new Vector3(1, 1, 0),
                        new Vector3(0, 0, 0),
                        new Vector3(0, 1, 0)
                    },
                    new List<int>()
                    {
                        0, 2, 1, 3, 1, 2
                    },
                    _UvMap_Side
                ));
        }

        d.AddPos(new Vector3(x, y, z));

        return d;
    }

    internal static string[] GetBlockNameIDFromHitPointPosition(Vector3 pos)
    {
        int ChunkPosX = Mathf.FloorToInt(pos.x / Chunk.ChunkWidth);
        int ChunkPosY = Mathf.FloorToInt(pos.y / Chunk.ChunkHeight);
        int ChunkPosZ = Mathf.FloorToInt(pos.z / Chunk.ChunkWidth);
        Chunk currentchunk;
        Block b;

        try
        {
            currentchunk = World._Instance.GetChunk(ChunkPosX, ChunkPosY, ChunkPosZ);

            if (currentchunk.GetType().Equals(typeof(ErroredChunk)))
            {
                Debug.Log("Current chunk is errored " + ":" + pos.ToString());
            }

            int x = (int)(pos.x - ChunkPosX * Chunk.ChunkWidth);
            int y = (int)(pos.y - ChunkPosY * Chunk.ChunkHeight);
            int z = (int)(pos.z - ChunkPosZ * Chunk.ChunkWidth);

            b = currentchunk.GetBlock(x, y, z);
            return new string[] { b.GetBlockName(), string.Format("{0}", b.GetID()) };

        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message.ToString());
        }

        return new string[] { "Unknown", "-1" };

    }

    internal static void AddBlock(Vector3 roundedPosition, Block block, Block.Direction direction = Block.Direction.NORTH, bool isBreak = false, bool Adding = false)
    {

        if (block.GetType().Equals(typeof(Tools)) || block.GetType().Equals(typeof(SpecialBlocks)))
            return;

        if (roundedPosition.y >= Chunk.ChunkHeight * World.ChunksInYAxis)
            return;

        int ChunkPosX = Mathf.FloorToInt(roundedPosition.x / Chunk.ChunkWidth);
        int ChunkPosY = Mathf.FloorToInt(roundedPosition.y / Chunk.ChunkHeight);
        int ChunkPosZ = Mathf.FloorToInt(roundedPosition.z / Chunk.ChunkWidth);
        Chunk currentchunk;

        try
        {
            currentchunk = World._Instance.GetChunk(ChunkPosX, ChunkPosY, ChunkPosZ);

            if (currentchunk.GetType().Equals(typeof(ErroredChunk)))
            {
                Debug.Log("Current chunk is errored " + ":" + roundedPosition.ToString());
                return;
            }

            int x = (int)(roundedPosition.x - ChunkPosX * Chunk.ChunkWidth);
            int y = (int)(roundedPosition.y - ChunkPosY * Chunk.ChunkHeight);
            int z = (int)(roundedPosition.z - ChunkPosZ * Chunk.ChunkWidth);

            if (isBreak)
            {
                //GameObject c = Transform.Instantiate(Resources.Load<GameObject>("Prefabs/Cube"), roundedPosition + new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity) as GameObject;
                //c.GetComponent<CubeHandler>().ID = currentchunk.GetBlock(x, y, z).GetID();
                //Debug.Log(roundedPosition.ToString());
                PlayerStatus.AddBlock(currentchunk.GetBlock(x, y, z).GetID());
                Player.PStatus.Points += 1;
            }

            currentchunk.SetBlock(x, y, z, block);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message.ToString());
        }

    }

    internal static Block GetBlockAtPosition(Vector3 pos)
    {
        int ChunkPosX = Mathf.FloorToInt(pos.x / Chunk.ChunkWidth);
        int ChunkPosY = Mathf.FloorToInt(pos.y / Chunk.ChunkHeight);
        int ChunkPosZ = Mathf.FloorToInt(pos.z / Chunk.ChunkWidth);
        Chunk currentchunk = World._Instance.GetChunk(ChunkPosX, ChunkPosY, ChunkPosZ);
        int x = (int)(pos.x - ChunkPosX * Chunk.ChunkWidth);
        int y = (int)(pos.y - ChunkPosY * Chunk.ChunkHeight);
        int z = (int)(pos.z - ChunkPosZ * Chunk.ChunkWidth);

        return currentchunk.GetBlock(x, y, z);
    }

    public static int MyRoundingFunctionToInt(float number)
    {
        float epsilon = 0.1f;
        float ceil;
        
        ceil = Mathf.Ceil(number);

        if(ceil - number < epsilon)
        {
            return Mathf.CeilToInt(number);
        }

        return Mathf.FloorToInt(number);
    }

}
