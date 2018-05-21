using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using SimplexNoise;
using System.Linq;

public class World : ILoop
{

    public static World _Instance { get; private set; }
    public static Int3 PlayerPos { get; private set; }
    private static Vector3 PP;

    private Thread worldThread;

    private static readonly int RenderDistanceChunks = PlayerSettings.RenderDistance;
    public static readonly int ChunksInYAxis = 3;
    public static bool ChunksGenerated = false;

    private bool IsRunning;
    private bool PlayerSet = false;
    private bool PlayerCanBeLoaded = false;
    private bool RanOnce = false;
    private bool CanMakeATree = false;
    private bool firstCheck = false;

    private List<Chunk> _LoadedChunks = new List<Chunk>();

    public static void Instantiate()
    {
        _Instance = new World();
        MainLoop.GetInstance().RegisterLoopes(_Instance);
        System.Random r = new System.Random();
        PlayerPos = new Int3(r.Next(-1000, 1000), 120, r.Next(-1000, 1000));
        PP = Int3.ToVector3(PlayerPos);

        if (Serializer.CheckFileExists(FileManager.GetPlayerSaveFileName(1)))
        {
            PlayerSettings.PrepareGame(Serializer.Deserialize_From_File<float[]>(FileManager.GetPlayerSaveFileName(1)));
        }

        if (Serializer.CheckFileExists(FileManager.GetPlayerSaveFileName(2)))
        {
            PlayerSettings.PrepareGame(Serializer.Deserialize_From_File<int[]>(FileManager.GetPlayerSaveFileName(2)));
            PP = PlayerSettings.Position;
            PlayerPos = new Int3(PP);
        }

        if (Serializer.CheckFileExists(FileManager.GetSaveDirectory() + "game.dat"))
        {
            int[] tmpData = Serializer.Deserialize_From_File<int[]>(FileManager.GetSaveDirectory() + "game.dat");
            GameTime.Initialize((float)tmpData[0], tmpData[1]);
        }
        else
        {
            GameTime.Initialize();
        }
    }

    public void Start()
    {
        IsRunning = true;
        worldThread = new Thread(ThreadStart);
        worldThread.Start();
    }

    public void Update()
    {

        List<Chunk> tmp_chunks;
        lock (_LoadedChunks)
            tmp_chunks = new List<Chunk>(_LoadedChunks);

        foreach (Chunk c in tmp_chunks)
        {
            c.OnUnityUpdate();
        }

        if (PlayerCanBeLoaded && !PlayerSet)
        {
            GameManager._Instance.StartPlayer(PP);
            PlayerSet = true;
        }
    }

    public void OnApplicationQuit()
    {

        List<Chunk> tmp_chunks;
        lock (_LoadedChunks)
            tmp_chunks = new List<Chunk>(_LoadedChunks);

        foreach (Chunk c in tmp_chunks)
        {
            try
            {
                Serializer.Serialize_ToFile_FullPath<int[,,]>(FileManager.GetChunkString(c.PosX, c.PosY, c.PosZ), c.GetChunkSaveData());
            }
            catch (System.Exception e)
            {
                Debug.Log(e.ToString());
            }
        }

        Serializer.Serialize_ToFile<float[]>(FileManager.GetSaveDirectory(), FileManager.PlayerName + "1", "dat", PlayerSettings.GetSaveData());
        Serializer.Serialize_ToFile<int[]>(FileManager.GetSaveDirectory(), FileManager.PlayerName + "2", "dat", PlayerSettings.GetSaveData2());
        Serializer.Serialize_ToFile<int[]>(FileManager.GetSaveDirectory(), "game", "dat", GameTime.GetDataToArray());
        Serializer.Serialize_ToFile<string>(FileManager.GetSaveDirectory(), "general", "dat", DateTime.Now.ToString("G"));

        if (Serializer.CheckFileExists(FileManager.WorldsDirectory + "rank.dat"))
        {
            List<string> tmp = new List<string>(Serializer.Deserialize_From_File<string[]>(FileManager.WorldsDirectory + "rank.dat"));

            foreach(string t in new List<string>(tmp))
            {
                string[] data = t.Split('|');

                if(data[0].Equals(FileManager.GameName) && data[1].Equals(FileManager.PlayerName))
                {
                    tmp.Remove(t);
                    break;
                }
            }

            tmp.Add(string.Format("{0}|{1}|{2}", FileManager.GameName, FileManager.PlayerName, Player.PStatus.Points.ToString()));

            Serializer.Serialize_ToFile<string[]>(FileManager.WorldsDirectory, "rank", "dat", tmp.ToArray());
        }
        else
        {
            string[] tmp = new string[] { string.Format("{0}|{1}|{2}", FileManager.GameName, FileManager.PlayerName, Player.PStatus.Points.ToString()) };
            Serializer.Serialize_ToFile<string[]>(FileManager.WorldsDirectory, "rank", "dat", tmp);
        }

        IsRunning = false;
        Logger.Log("Stopping World Thread");
    }

    public void ThreadStart()
    {
        Logger.Log("Initializing World Thread");

        while (IsRunning)
        {
            if (GameManager._Instance.StateOfTheGame == GameManager.GameState.RUNNING)
            {
                try
                {
                    CanMakeATree = false;
                    Int3 ChunkToTreePos = new Int3(0, 0, 0);

                    if (!RanOnce)
                    {
                        RanOnce = true;
                        for (int x = -RenderDistanceChunks; x < RenderDistanceChunks; x++)
                        {
                            for (int y = 0; y < ChunksInYAxis; y++)
                            {
                                for (int z = -RenderDistanceChunks; z < RenderDistanceChunks; z++)
                                {
                                    Int3 newchunkpos = new Int3(PlayerPos.x, 0, PlayerPos.z);
                                    newchunkpos.AddPos(new Int3(x * Chunk.ChunkWidth, y * Chunk.ChunkHeight, z * Chunk.ChunkWidth));
                                    newchunkpos.ToChunkCoordinates();

                                    if (System.IO.File.Exists(FileManager.GetChunkString(newchunkpos.x, newchunkpos.y, newchunkpos.z)))
                                    {
                                        try
                                        {
                                            _LoadedChunks.Add(new Chunk(newchunkpos.x, newchunkpos.y, newchunkpos.z, Serializer.Deserialize_From_File<int[,,]>(FileManager.GetChunkString(newchunkpos.x, newchunkpos.y, newchunkpos.z)), this));
                                        }
                                        catch (System.Exception e)
                                        {
                                            Debug.Log(e.ToString());
                                        }
                                    }
                                    else
                                    {
                                        CanMakeATree = true;
                                        _LoadedChunks.Add(new Chunk(newchunkpos.x, newchunkpos.y, newchunkpos.z, this));
                                        ChunkToTreePos = newchunkpos;
                                        //Debug.Log("Cant find " + FileManager.GetChunkString(x, y, z));
                                    }

                                }
                            }
                        }

                        foreach (Chunk c in _LoadedChunks)
                        {
                            c.Start();
                        }
                    }

                    if (!PlayerCanBeLoaded)
                        PlayerCanBeLoaded = true;

                    if (GameManager.PlayerLoaded())
                    {
                        PlayerPos = new Int3(GameManager._Instance.PlayerPosition);
                    }


                    foreach (Chunk c in new List<Chunk>(_LoadedChunks))
                    {
                        if (Vector2.Distance(new Vector2(c.PosX * Chunk.ChunkWidth, c.PosZ * Chunk.ChunkWidth), new Vector2(PlayerPos.x, PlayerPos.z)) > ((RenderDistanceChunks * 2) * Chunk.ChunkWidth))
                        {
                            c.Degenerate();
                        }
                    }

                    for (int x = -RenderDistanceChunks; x < RenderDistanceChunks; x++)
                    {
                        for (int y = 0; y < ChunksInYAxis; y++)
                        {
                            for (int z = -RenderDistanceChunks; z < RenderDistanceChunks; z++)
                            {
                                Int3 newchunkpos = new Int3(PlayerPos.x, 0, PlayerPos.z);
                                newchunkpos.AddPos(new Int3(x * Chunk.ChunkWidth, y * Chunk.ChunkHeight, z * Chunk.ChunkWidth));
                                newchunkpos.ToChunkCoordinates();
                                if (!ChunkExists(newchunkpos.x, newchunkpos.y, newchunkpos.z))
                                {

                                    //make function for this

                                    if (System.IO.File.Exists(FileManager.GetChunkString(newchunkpos.x, newchunkpos.y, newchunkpos.z)))
                                    {
                                        try
                                        {
                                            Chunk c = new Chunk(newchunkpos.x, newchunkpos.y, newchunkpos.z, Serializer.Deserialize_From_File<int[,,]>(FileManager.GetChunkString(newchunkpos.x, newchunkpos.y, newchunkpos.z)), this);
                                            c.Start();
                                            _LoadedChunks.Add(c);
                                        }
                                        catch (System.Exception e)
                                        {
                                            Debug.Log(e.ToString());
                                        }
                                    }
                                    else
                                    {
                                        Chunk c = new Chunk(newchunkpos.x, newchunkpos.y, newchunkpos.z, this);
                                        c.Start();
                                        _LoadedChunks.Add(c);
                                        CanMakeATree = true;
                                        ChunkToTreePos = newchunkpos;
                                        //Debug.Log("Cant find " + FileManager.GetChunkString(x, z));
                                    }

                                }
                            }
                        }
                    }

                    if(CanMakeATree)
                    {
                        GenerateTree(ChunkToTreePos);
                        CanMakeATree = false;
                    }

                    foreach (Chunk c in _LoadedChunks)
                    {
                        c.Update();
                    }

                    if(!firstCheck)
                    {
                        ChunksGenerated = true;
                        firstCheck = true;
                    }
                }
                catch (System.Exception e)
                {
                    UnityEngine.Debug.Log(e.StackTrace);
                    Logger.Log(e);
                }
            }
        }

        Logger.Log("Wordl Thread succesfully stopped");
        Logger.MainLog.Update();
    }

    public void GenerateTree(Int3 Pos)
    {
        System.Random rnd = new System.Random();
        int count = rnd.Next(3, 100) % 3 + 1;

        int treeheight = 6;

        for (int i = 0; i < count; i++)
        {
            int x = rnd.Next(3, Chunk.ChunkWidth - 4);
            int z = rnd.Next(3, Chunk.ChunkWidth - 4);

            Int3 Tree = new Int3(x, 0, z);
            Chunk ch;

            if(GetSurface(ref Pos, out ch, ref Tree))
            {
                for (int k = 0; k < treeheight + 1; k++) 
                {
                    if(k < 3)
                    {
                        ch.SetBlock(Tree.x, Tree.y, Tree.z, BlockRegistry.GetBlockFromBlockName("Logbigoak"));

                    }
                    else if(k >= 3 && k <= 5)
                    {
                        ch.SetBlock(Tree.x - 1, Tree.y, Tree.z, BlockRegistry.GetBlockFromBlockName("Leaves"));
                        ch.SetBlock(Tree.x, Tree.y, Tree.z, BlockRegistry.GetBlockFromBlockName("Logbigoak"));
                        ch.SetBlock(Tree.x + 1, Tree.y, Tree.z, BlockRegistry.GetBlockFromBlockName("Leaves"));

                        ch.SetBlock(Tree.x - 1, Tree.y, Tree.z - 1, BlockRegistry.GetBlockFromBlockName("Leaves"));
                        ch.SetBlock(Tree.x, Tree.y, Tree.z - 1, BlockRegistry.GetBlockFromBlockName("Leaves"));
                        ch.SetBlock(Tree.x + 1, Tree.y, Tree.z - 1, BlockRegistry.GetBlockFromBlockName("Leaves"));

                        ch.SetBlock(Tree.x - 1, Tree.y, Tree.z + 1, BlockRegistry.GetBlockFromBlockName("Leaves"));
                        ch.SetBlock(Tree.x, Tree.y, Tree.z + 1, BlockRegistry.GetBlockFromBlockName("Leaves"));
                        ch.SetBlock(Tree.x + 1, Tree.y, Tree.z + 1, BlockRegistry.GetBlockFromBlockName("Leaves"));
                    }
                    else
                    {
                        ch.SetBlock(Tree.x, Tree.y, Tree.z, BlockRegistry.GetBlockFromBlockName("Leaves"));
                    }



                    Tree.y++;
                    if(Tree.y >= Chunk.ChunkHeight)
                    {
                        Pos.y++;
                        ch = GetChunk(Pos.x, Pos.y, Pos.z);
                        Tree.y = 0;
                    }
                }
            }

        }
    }

    public bool GetSurface(ref Int3 Pos, out Chunk chunk, ref Int3 tree)
    {
        chunk = GetChunk(Pos.x, 0, Pos.z);

        for(int i = 0; i < ChunksInYAxis; i++)
        {
            chunk = GetChunk(Pos.x, i, Pos.z);
            Pos.y = i;

            for (int j = 0; j < Chunk.ChunkHeight; j++)
            {
                if(chunk.GetBlock(tree.x, j, tree.z).GetID() == 0)
                {
                    tree.y = j;
                    return true;
                }
            }
        }

        return false;
        
    }

    public bool GetNewEnemyPosition(ref Vector3 EnemyPosition)
    {
        System.Random rnd = new System.Random();
        int x = rnd.Next(-(RenderDistanceChunks - 1) * 20, (RenderDistanceChunks - 1) * 20),
            z = rnd.Next(-(RenderDistanceChunks - 1) * 20, (RenderDistanceChunks - 1) * 20);

        Int3 ChunkPos = new Int3(GameManager._Instance.PlayerPosition) + new Int3(x, 0, z);

        if(GetYCoordinate(ChunkPos, ref EnemyPosition))
        {
            return true;
        }

        EnemyPosition = Vector3.zero;
        return false;

    }

    public bool GetYCoordinate(Int3 c, ref Vector3 result)
    {
        System.Random rnd = new System.Random();
        c.ToChunkCoordinates();
        int x = rnd.Next(0, Chunk.ChunkWidth - 1), z = rnd.Next(0, Chunk.ChunkWidth - 1);

        Chunk chunk = GetChunk(c.x, 0, c.z);

        for (int i = 0; i < ChunksInYAxis; i++)
        {
            chunk = GetChunk(c.x, i, c.z);
            c.y = i;

            for (int j = 0; j < Chunk.ChunkHeight; j++)
            {
                if (chunk.GetBlock(x, j, z).GetID() == 0)
                {
                    result = new Vector3(c.x * Chunk.ChunkWidth + x, c.y * Chunk.ChunkHeight + j, c.z * Chunk.ChunkWidth + z) + new Vector3(0.5f, 0.5f, 0.5f);
                    return true;
                }
            }
        }

        return false;
    }

    public bool ChunkExists(int posX, int posY, int posZ)
    {

        List<Chunk> tmp_chunks;
        lock (_LoadedChunks)
            tmp_chunks = new List<Chunk>(_LoadedChunks);

        foreach (Chunk c in tmp_chunks)
        {
            if (c.PosX.Equals(posX) && c.PosY.Equals(posY) && c.PosZ.Equals(posZ))
            {
                return true;
            }
        }
        return false;
    }

    public Chunk GetChunk(int posX, int posY, int posZ)
    {
        List<Chunk> tmp_chunks;
        lock (_LoadedChunks)
            tmp_chunks = new List<Chunk>(_LoadedChunks);

        foreach (Chunk c in new List<Chunk>(tmp_chunks))
        {
            if (c.PosX.Equals(posX) && c.PosY.Equals(posY) && c.PosZ.Equals(posZ))
            {
                return c;
            }
        }

        return new ErroredChunk(0, 0, 0, this);
    }

    internal void RemoveChunk(Chunk chunk)
    {
        _LoadedChunks.Remove(chunk);
    }

    public void LateUpdate()
    {

    }
}
