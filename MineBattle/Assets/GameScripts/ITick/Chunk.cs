using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimplexNoise;

public class Chunk : ITick
{

    private World World;

    public static readonly int ChunkWidth = 20;
    public static readonly int ChunkHeight = 20;

    private Block[,,] _Blocks;

    public int PosX { private set; get; }
    public int PosY { private set; get; }
    public int PosZ { private set; get; }

    protected bool HasGenerated = false;    // Chunk was generated
    protected bool HasDrawn = false;        // Chunk was drawn
    protected bool DrawnLock = false;       // Lock
    protected bool HasRendered = false;     // Chunk was rendered
    private bool RenderingLock = false;     // Lock
    private bool NeedToUpdate = false;      // Chunk must be updatet(rerendered)

    private MeshData data;
    private GameObject go;

    public virtual void Start()
    {
        if (HasGenerated)
            return;

        _Blocks = new Block[ChunkWidth, ChunkHeight, ChunkWidth];

        for (int x = 0; x < ChunkWidth; x++)
        {
            for (int y = 0; y < ChunkHeight; y++)
            {
                for (int z = 0; z < ChunkWidth; z++)
                {
                    float perlin = GetHeight(x, y, z);

                    if (perlin > GameManager.Scutoff)
                    {
                        _Blocks[x, y, z] = BlockRegistry.GetBlockFromBlockName("Air");             //Air
                    }
                    else if (perlin >= GameManager.Scutoff - 0.5f)
                    {
                        _Blocks[x, y, z] = BlockRegistry.GetBlockFromBlockName("Grass");
                    }
                    else if (perlin > GameManager.Scutoff * 0.8f)
                    {
                        _Blocks[x, y, z] = BlockRegistry.GetBlockFromBlockName("Dirt");
                    }
                    else
                    {
                        _Blocks[x, y, z] = BlockRegistry.GetBlockFromBlockName("Stone");
                    }

                    if (y < 1 && PosY == 0)
                    {
                        _Blocks[x, y, z] = BlockRegistry.GetBlockFromBlockName("Bedrock");             //Bedrock
                    }
                }
            }
        }
        
        HasGenerated = true;
    }

    public virtual void Update()
    {
        if (NeedToUpdate)
        {
            if (!DrawnLock && !RenderingLock)
            {
                HasDrawn = false;
                HasRendered = false;
                NeedToUpdate = false;
            }
        }

        if (!HasDrawn && HasGenerated && !DrawnLock)
        {
            DrawnLock = true;
            data = new MeshData();
            for (int x = 0; x < ChunkWidth; x++)
            {
                for (int y = 0; y < ChunkHeight; y++)
                {
                    for (int z = 0; z < ChunkWidth; z++)
                    {
                        data.Merge(_Blocks[x, y, z].Draw(this, _Blocks, x, y, z));
                    }
                }
            }
            DrawnLock = false;
            HasDrawn = true;
        }
    }

    public virtual void OnUnityUpdate()
    {
        if (HasGenerated && !HasRendered && HasDrawn && !RenderingLock)
        {
            HasRendered = true;
            RenderingLock = true;

            Mesh mesh = data.ToMesh();
            if (go == null)
            {
                go = new GameObject();
            }

            Transform t = go.transform;

            if (t.gameObject.GetComponent<MeshFilter>() == null)
            {
                t.gameObject.AddComponent<MeshFilter>();
                t.gameObject.AddComponent<MeshRenderer>();
                t.gameObject.AddComponent<MeshCollider>();
                t.transform.position = new Vector3(PosX * ChunkWidth, PosY * ChunkHeight, PosZ * ChunkWidth);
                Texture2D tmp = new Texture2D(0, 0);
                tmp.LoadImage(System.IO.File.ReadAllBytes("atlas.png"));
                tmp.filterMode = FilterMode.Point;
                t.gameObject.GetComponent<MeshRenderer>().material.mainTexture = tmp;

            }

            t.transform.GetComponent<MeshFilter>().sharedMesh = mesh;
            t.transform.GetComponent<MeshCollider>().sharedMesh = mesh;

            RenderingLock = false;
        }
    }

    public Chunk(int px, int py, int pz, World world)
    {
        PosX = px;
        PosY = py;
        PosZ = pz;
        this.World = world;
    }

    public Chunk(int px, int py, int pz, int[,,] _data, World world)
    {
        PosX = px;
        PosY = py;
        PosZ = pz;
        LoadChunkFromData(_data);
        HasGenerated = true;
        this.World = world;
    }

    public void Degenerate()
    {
        try
        {
            Serializer.Serialize_ToFile_FullPath<int[,,]>(FileManager.GetChunkString(PosX, PosY, PosZ), GetChunkSaveData());
        }
        catch (System.Exception e)
        {
            Debug.Log(e.ToString());
        }

        GameManager._Instance.RegisterDelegate(new Action(() => {
            GameObject.Destroy(go);

            //Debug.Log(new Int3(PosX, 0, PosZ).ToString());
        }));

        World.RemoveChunk(this);
    }

    public float GetHeight(float px, float py, float pz)
    {
        px += (PosX * ChunkWidth);
        py += (PosY * ChunkHeight);
        pz += (PosZ * ChunkWidth);

        float p1 = Noise.Generate(px / GameManager.Sdx, pz / GameManager.Sdz) * GameManager.Smul + GameManager.Smy;
        p1 *= (1 / GameManager.Sdy * py);

        return p1 > 0 ? p1 : -p1;

    }

    public int[,,] GetChunkSaveData()
    {
        return _Blocks.ToIntArray();
    }

    public void LoadChunkFromData(int[,,] _data)
    {
        _Blocks = _data.ToBlockArray();
    }

    internal void SetBlock(int x, int y, int z, Block block)
    {
        _Blocks[x, y, z] = block;
        NeedToUpdate = true;
    }

    internal Block GetBlock(int x, int y, int z)
    {
        return _Blocks[x, y, z];
    }

}
