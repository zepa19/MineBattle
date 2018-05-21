using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Player : ILoop
{
    public static Player _Instance = new Player();
    public static PlayerStatus PStatus = new PlayerStatus();

    Transform Select;
    Camera Eyes;

    public Vector3 PlayerIOPosition;
    public Vector3 BlockIOPosition;
    public Vector3 HitNormal;
    public Int3 HitPoint;
    public Int3 ChunkPos;

    public List<GameObject> Enemies;
    public GameObject LastAttacked;
    
    private bool CanPlaceABlock = true;
    private bool CanRemoveABlock = true;
    private bool FirstRun = true;
    private bool InCraftingTable = false;
    private bool CanAttack = false;
    private bool EnemiesInitialized = false;
    private bool FirstEnemyCheck = false;
    public bool CanEnemyAttack = false;
    public bool PlayerInitialized = false;
    public bool HitBlockChanged = false;
    public bool DebugStatus = false;
    public bool LookingAtBlock = false;
    public bool LookingAtEnemy = false;

    public bool LeftMousePressed = false;
    public bool RightMousePressed = false;

    private int[] Zombie_IDS = new int[] { BlockRegistry.GID("GoldS"), BlockRegistry.GID("Redstone"), BlockRegistry.GID("CoalS") };
    private int[] Creeper_IDS = new int[] { BlockRegistry.GID("EmeraldS"), BlockRegistry.GID("IronS"), BlockRegistry.GID("CoalS") };
    private int[] Enderman_IDS = new int[] { BlockRegistry.GID("Lapis"), BlockRegistry.GID("Redstone"), BlockRegistry.GID("DiamondS") };

    float LastBlock = 5f;
    float LastBlockRem = 5f;
    float LastRegeneration = 0f;
    float LastAttack = 0f;
    float SinceDeath = 0f;
    float breakingTimeLeft;
    public float LastEnemyAtack = 0f;

    System.Random rnd = new System.Random();

    public static void Instantiate()
    {
        MainLoop.GetInstance().RegisterLoopes(_Instance);
        PlayerGUI.Inventory();
    }

    public void Start()
    {
        GameManager._Instance.DbgText.enabled = DebugStatus;
        Select = Transform.Instantiate(Resources.Load<Transform>("Prefabs/Select"), GameManager._Instance.PlayerPosition, Quaternion.identity) as Transform;
        Select.transform.GetComponent<MeshRenderer>().enabled = false;
        Enemies = new List<GameObject>();
        LastAttacked = null;
    }

    public void Update()
    {
        if (!PlayerInitialized && GameManager.PlayerLoaded())
        {
            Eyes = GameObject.Find("Player(Clone)/Camera").GetComponent<Camera>();
            PStatus.SetHands();
            GameManager._Instance.DbgText.gameObject.SetActive(true);
            GameManager._Instance.Toolbar.SetActive(true);
            SetToolbarItems();
            PlayerInitialized = true;
            GameManager._Instance.BackgroundIMG.SetActive(false);
            EventHandler.RegisterEvent(HandleF3Event, KeyCode.F3);
            EventHandler.RegisterEvent(HandleEEvent, KeyCode.E);
            EventHandler.RegisterEvent(HandleQEvent, KeyCode.Q);
            //EventHandler.RegisterEvent(HandleREvent, KeyCode.R);
            //EventHandler.RegisterEvent(HandlePEvent, KeyCode.P);
            PlayerGUI.UpdateHealth();
        }

        if(PStatus.Died)
        {
            SinceDeath += Time.deltaTime;

            if(SinceDeath > 5f)
            {
                GameManager._Instance.ModeOfTheGame = GameManager.GameMode.CREATIVE;
                SinceDeath = 0f;
                GameManager._Instance.BackgroundIMG.SetActive(false);
                GameManager._Instance.DiedText.SetActive(false);
                PStatus.Died = false;
            }
        }

        if (!CanPlaceABlock)
        {
            LastBlock += Time.deltaTime;

            if (LastBlock > 0.14f)
            {
                CanPlaceABlock = true;
                LastBlock = 0f;
            }
        }

        if (!CanRemoveABlock)
        {
            LastBlockRem += Time.deltaTime;

            if (LastBlockRem > 0.14f)
            {
                CanRemoveABlock = true;
                LastBlockRem = 0f;
            }
        }

        if(!CanAttack)
        {
            LastAttack += Time.deltaTime;

            if(LastAttack > 1f)
            {
                CanAttack = true;
                LastAttack = 0f;
            }
        }

        if(!CanEnemyAttack)
        {
            LastEnemyAtack += Time.deltaTime;

            if(LastEnemyAtack > 1.5f)
            {
                CanEnemyAttack = true;
                LastEnemyAtack = 0f;
            }
        }

        if (PlayerInitialized)
        {

            if(Input.GetMouseButton(0))
            {
                LeftMousePressed = true;
            }
            else
            {
                breakingTimeLeft = 0f;
                LeftMousePressed = false;
            }

            if (Input.GetMouseButton(1))
            {
                RightMousePressed = true;
            }
            else
            {
                RightMousePressed = false;
            }

            if(LeftMousePressed || RightMousePressed)
            {
                PStatus.GetActiveHand().GetComponent<Animation>().Play("Arm");
            }
            else
            {
                PStatus.GetActiveHand().GetComponent<Animation>().Stop();
            }
                
            PlayerIOPosition = GameManager._Instance.PlayerPosition;
            PlayerIOPosition.y -= 1f;
            //BlockIOPosition = new Vector3(Mathf.FloorToInt(PlayerIOPosition.x), Mathf.FloorToInt(PlayerIOPosition.y), Mathf.FloorToInt(PlayerIOPosition.z));
            BlockIOPosition = new Vector3(Mathf.FloorToInt(PlayerIOPosition.x), MathHelper.MyRoundingFunctionToInt(PlayerIOPosition.y), Mathf.FloorToInt(PlayerIOPosition.z));
            ChunkPos = new Int3(Mathf.FloorToInt(BlockIOPosition.x / Chunk.ChunkWidth), Mathf.FloorToInt(BlockIOPosition.y / Chunk.ChunkHeight), Mathf.FloorToInt(BlockIOPosition.z / Chunk.ChunkWidth));

            GenerateHitPoints();
            PlayerGUI.Debugging();
            PlayerGUI.ItemSelecting(ref PStatus.CurrentItem);
            CheckRegeneration();
            CheckEnemies();

            if (LookingAtBlock)
            {
                if(IsEnemyAtHitPosition())
                {
                    Select.transform.GetComponent<MeshRenderer>().enabled = false;
                    Attack();
                }
                else
                {
                    Select.transform.GetComponent<MeshRenderer>().enabled = true;
                    Select.transform.position = new Vector3(HitPoint.x + 0.5f, HitPoint.y + 0.5f, HitPoint.z + 0.5f);

                    Block destBlock = MathHelper.GetBlockAtPosition(Int3.ToVector3(HitPoint));
                    if (RightMousePressed && destBlock.GetBlockName().Equals("Craftingtable"))
                    {
                        CratingTable();
                    }
                    else if (!(!DayNightCycle.DNCycle.IsDay && GameManager._Instance.ModeOfTheGame == GameManager.GameMode.SURVIVAL))
                    {
                        BreakingAndPlacing();
                    }
                }
                
            }
            else
            {
                Select.transform.GetComponent<MeshRenderer>().enabled = false;
            }

            
        }
    }

    public void LateUpdate()
    {
        if (World.ChunksGenerated && !FirstEnemyCheck)
        {
            if (DayNightCycle.DNCycle.IsDay)
            {
                Day();
            }
            else
            {
                Night();
            }
            FirstEnemyCheck = true;
        }
    }

    public void OnApplicationQuit()
    {

    }

    void Attack()
    {
        GameObject e = GetAttackedEnemy();
        if(e.GetComponent<Enemy>().Script.GetType().Equals(typeof(Zombie)))
        {
            Zombie enemy = e.GetComponent<Enemy>().Script as Zombie;

            if (LeftMousePressed)
            {
                
                if(CanAttack && (GameManager._Instance.PlayerPosition - e.transform.position).magnitude < 2f)
                {
                    int dam = BlockRegistry._RegisteredBlocks[PStatus.ToolBox9[PStatus.CurrentItem].BlockID].GetHitDamage(PStatus.ToolBox9[PStatus.CurrentItem].Level);
                    if(LastAttacked != null && !LastAttacked.Equals(e))
                    {
                        if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(Zombie)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as Zombie).DisableHealthBar();
                        }
                        else if(LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(Creeper)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as Creeper).DisableHealthBar();
                        }
                        else if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(EnderMan)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as EnderMan).DisableHealthBar();
                        }
                        //dodac kolejne jak beda
                    }

                    if(enemy.TakeDamage(dam))
                    {
                        PStatus.Points += 50;
                        
                        PlayerStatus.AddBlock(Zombie_IDS[new System.Random().Next(0, Zombie_IDS.Length)]);
                    }
                    LastAttacked = e;
                    CanAttack = false;
                }
                
            }
        }
        else if (e.GetComponent<Enemy>().Script.GetType().Equals(typeof(Creeper)))
        {
            Creeper enemy = e.GetComponent<Enemy>().Script as Creeper;

            if (LeftMousePressed)
            {

                if (CanAttack && (GameManager._Instance.PlayerPosition - e.transform.position).magnitude < 2f)
                {
                    int dam = BlockRegistry._RegisteredBlocks[PStatus.ToolBox9[PStatus.CurrentItem].BlockID].GetHitDamage(PStatus.ToolBox9[PStatus.CurrentItem].Level);
                    if (LastAttacked != null && !LastAttacked.Equals(e))
                    {
                        if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(Zombie)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as Zombie).DisableHealthBar();
                        }
                        else if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(Creeper)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as Creeper).DisableHealthBar();
                        }
                        else if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(EnderMan)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as EnderMan).DisableHealthBar();
                        }
                        //dodac kolejne jak beda
                    }

                    if (enemy.TakeDamage(dam))
                    {
                        PStatus.Points += 50;

                        PlayerStatus.AddBlock(Creeper_IDS[new System.Random().Next(0, Creeper_IDS.Length)]);
                    }
                    LastAttacked = e;
                    CanAttack = false;
                }

            }
        }
        else if (e.GetComponent<Enemy>().Script.GetType().Equals(typeof(EnderMan)))
        {
            EnderMan enemy = e.GetComponent<Enemy>().Script as EnderMan;

            if (LeftMousePressed)
            {

                if (CanAttack && (GameManager._Instance.PlayerPosition - e.transform.position).magnitude < 2f)
                {
                    int dam = BlockRegistry._RegisteredBlocks[PStatus.ToolBox9[PStatus.CurrentItem].BlockID].GetHitDamage(PStatus.ToolBox9[PStatus.CurrentItem].Level);
                    if (LastAttacked != null && !LastAttacked.Equals(e))
                    {
                        if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(Zombie)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as Zombie).DisableHealthBar();
                        }
                        else if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(Creeper)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as Creeper).DisableHealthBar();
                        }
                        else if (LastAttacked.GetComponent<Enemy>().Script.GetType().Equals(typeof(EnderMan)))
                        {
                            (LastAttacked.GetComponent<Enemy>().Script as EnderMan).DisableHealthBar();
                        }
                        //dodac kolejne jak beda
                    }

                    if (enemy.TakeDamage(dam))
                    {
                        PStatus.Points += 50;

                        PlayerStatus.AddBlock(Enderman_IDS[new System.Random().Next(0, Enderman_IDS.Length)]);
                    }
                    LastAttacked = e;
                    CanAttack = false;
                }

            }
        }

    }

    void BreakingAndPlacing()
    {
        if (GameManager._Instance.StateOfTheGame != GameManager.GameState.ININVENTORY)
        {
            if(GameManager._Instance.ModeOfTheGame == GameManager.GameMode.CREATIVE)
            {
                if(LeftMousePressed)
                {
                    if (CanRemoveABlock)
                    {
                        MathHelper.AddBlock(Int3.ToVector3(HitPoint), BlockRegistry.GetBlockFromBlockName("Air"));
                        CanRemoveABlock = false;
                    }
                       
                }
                
                if(RightMousePressed)
                {

                    if (CanPlaceABlock)
                    {
                        int bID = PStatus.ToolBox9[PStatus.CurrentItem].BlockID;
                        if (bID != 0 && !(BlockRegistry.GetBlockFromID(bID).GetType().Equals(typeof(Tools)) || BlockRegistry.GetBlockFromID(bID).GetType().Equals(typeof(SpecialBlocks))))
                        {
                            Vector3 NewBlockPos = Int3.ToVector3(HitPoint) + HitNormal;
                            if (NewBlockPos != BlockIOPosition && NewBlockPos != BlockIOPosition + new Vector3(0f, 1f, 0f))
                            {
                                MathHelper.AddBlock(NewBlockPos, BlockRegistry.GetBlockFromID(bID), GetFacingDirection(), false, true);
                                CanPlaceABlock = false;
                            }
                        }
                    }
                    
                }
            }
            else
            {
                if(LeftMousePressed)
                {
                    
                    if (!HitBlockChanged)
                    {
                        breakingTimeLeft += GameTime.deltaTime;
                        Block destBlock = MathHelper.GetBlockAtPosition(Int3.ToVector3(HitPoint));
                        float lt = BlockRegistry._RegisteredBlocks[PStatus.GetBlockIdFromToolbar()].GetBreakingTime(destBlock, PStatus.ToolBox9[PStatus.CurrentItem].Level);

                        PStatus.UpdateBreakingProgress(true, breakingTimeLeft / lt);

                        if (breakingTimeLeft >= lt)
                        {
                            breakingTimeLeft = 0f;
                            MathHelper.AddBlock(Int3.ToVector3(HitPoint), BlockRegistry.GetBlockFromBlockName("Air"), Block.Direction.NORTH, true);
                        }
                    }
                    else
                    {
                        breakingTimeLeft = 0f;
                    }
                }
                else
                {
                    PStatus.UpdateBreakingProgress(false);
                }

                if(RightMousePressed)
                {

                    if (CanPlaceABlock)
                    {
                        int bID = PStatus.ToolBox9[PStatus.CurrentItem].BlockID;
                        int cID = PStatus.ToolBox9[PStatus.CurrentItem].Count;

                        if (bID != 0 && cID != 0 && !(BlockRegistry.GetBlockFromID(bID).GetType().Equals(typeof(Tools)) || BlockRegistry.GetBlockFromID(bID).GetType().Equals(typeof(SpecialBlocks))))
                        {
                            Vector3 NewBlockPos = Int3.ToVector3(HitPoint) + HitNormal;
                            if (NewBlockPos != BlockIOPosition && NewBlockPos != BlockIOPosition + new Vector3(0f, 1f, 0f))
                            {
                                MathHelper.AddBlock(NewBlockPos, BlockRegistry.GetBlockFromID(bID), GetFacingDirection(), false, true);
                                PStatus.DropOneItem();
                                CanPlaceABlock = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private GameObject GetAttackedEnemy()
    {
        foreach (GameObject e in Enemies)
        {
            Vector3 EnemyPos = new Vector3(Mathf.FloorToInt(e.transform.position.x), MathHelper.MyRoundingFunctionToInt(e.transform.position.y), Mathf.FloorToInt(e.transform.position.z));
            if (EnemyPos == Int3.ToVector3(HitPoint) || EnemyPos == Int3.ToVector3(HitPoint) - Vector3.up)
            {
                return e;
            }
        }
        return new GameObject();
    }

    public void CheckEnemies()
    {
        if(EnemiesInitialized)
        {

            foreach(GameObject e in new List<GameObject>(Enemies))
            {
                Vector3 chaseDir = e.transform.position - GameManager._Instance.PlayerPosition;
                chaseDir.y = 0;
                if (chaseDir.magnitude > 40)
                {
                    GameObject.Destroy(e);
                    Enemies.Remove(e);
                }
            }

            if(Enemies.Count != PStatus.Enemies)
            {
                
                for (int i = 0; i < PStatus.Enemies - Enemies.Count; i++)
                {
                    CreateEnemy();
                }
            }
        }
    }

    public void CreateEnemy()
    {
        Vector3 EnemyPosition = new Vector3();

        GameObject[] en = new GameObject[] { Resources.Load<GameObject>("Prefabs/Zombie"), Resources.Load<GameObject>("Prefabs/Creeper"), Resources.Load<GameObject>("Prefabs/Enderman") };

        if (World._Instance.GetNewEnemyPosition(ref EnemyPosition))
        {
            Enemies.Add(Transform.Instantiate(en[rnd.Next(0, en.Length)], EnemyPosition, Quaternion.identity) as GameObject);
        }
    }

    public void Night()
    {
        if(!EnemiesInitialized)
        {
            for (int i = 0; i < PStatus.Enemies; i++)
            {
                CreateEnemy();
            }
        }
        
        EnemiesInitialized = true;
    }

    public void Day()
    {
        foreach(GameObject e in new List<GameObject>(Enemies))
        {
            GameObject.Destroy(e);
        }
        Enemies = new List<GameObject>();

        EnemiesInitialized = false;
    }

    private bool IsEnemyAtHitPosition()
    {

        foreach(GameObject e in Enemies)
        {
            Vector3 EnemyPos = new Vector3(Mathf.FloorToInt(e.transform.position.x), MathHelper.MyRoundingFunctionToInt(e.transform.position.y), Mathf.FloorToInt(e.transform.position.z));
            if (EnemyPos == Int3.ToVector3(HitPoint) || EnemyPos == Int3.ToVector3(HitPoint) - Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
    
    public void HandlePEvent()
    {
        Vector3 EnemyPosition = new Vector3();
        if(World._Instance.GetNewEnemyPosition(ref EnemyPosition))
        {
            Enemies.Add(Transform.Instantiate(Resources.Load<GameObject>("Prefabs/Zombie"), EnemyPosition, Quaternion.identity) as GameObject);
        }
        
    }

    public void CheckRegeneration()
    {
        if(GameManager._Instance.ModeOfTheGame == GameManager.GameMode.SURVIVAL)
        {
            if(PStatus.Health < 100)
            {
                LastRegeneration += Time.deltaTime;

                if(LastRegeneration > 10)
                {
                    LastRegeneration = 0;
                    PStatus.TakeDamage(-10);
                }
            }
        }
    }

    void GenerateHitPoints()
    {
        Int3 old = HitPoint;

        RaycastHit hit;
        Ray ray = Eyes.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        if (Physics.Raycast(ray, out hit, 6))
        {
            Player._Instance.LookingAtBlock = true;

            HitPoint = new Int3(Mathf.FloorToInt(hit.point.x - 0.5f * hit.normal.x), Mathf.CeilToInt(hit.point.y - 0.5f * hit.normal.y - 1f), Mathf.FloorToInt(hit.point.z - 0.5f * hit.normal.z));
            HitNormal = new Vector3(hit.normal.x, hit.normal.y, hit.normal.z);

            if(FirstRun)
            {
                FirstRun = false;
            }
            else
            {
                if (HitPoint != old)
                {
                    HitBlockChanged = true;
                }
                else
                {
                    HitBlockChanged = false;
                }
            }
        }
        else
        {
            Player._Instance.LookingAtBlock = false;
            Player._Instance.LookingAtEnemy = false;
            HitBlockChanged = false;
        }

    }

    private void SetToolbarItems()
    {
        GameObject[] Toolbar = GameObject.FindGameObjectsWithTag("ToolbarItem");
        Array.Sort(Toolbar, CompareObjectNames);

        GameManager._Instance.CreativeInventory.SetActive(true);
        GameObject[] CreativeToolbar = GameObject.FindGameObjectsWithTag("InvToolbarItem");
        GameManager._Instance.CreativeInventory.SetActive(false);
        Array.Sort(CreativeToolbar, CompareObjectNames);

        GameManager._Instance.SurvivalInventory.SetActive(true);

        GameObject[] SurvivalToolbar = GameObject.FindGameObjectsWithTag("SurvToolbarItem");
        Array.Sort(SurvivalToolbar, CompareObjectNames);

        GameObject[] SurvivalInv1 = GameObject.FindGameObjectsWithTag("SROW0");
        Array.Sort(SurvivalInv1, CompareObjectNames);

        GameObject[] SurvivalInv2 = GameObject.FindGameObjectsWithTag("SROW1");
        Array.Sort(SurvivalInv2, CompareObjectNames);

        GameObject[] SurvivalInv3 = GameObject.FindGameObjectsWithTag("SROW2");
        Array.Sort(SurvivalInv3, CompareObjectNames);

        GameManager._Instance.SurvivalInventory.SetActive(false);

        GameManager._Instance.CraftingTablePanel.SetActive(true);

        GameObject[] CraftingToolbar = GameObject.FindGameObjectsWithTag("CraftToolbar");
        Array.Sort(CraftingToolbar, CompareObjectNames);

        GameObject[] CraftingInv1 = GameObject.FindGameObjectsWithTag("Crow1");
        Array.Sort(CraftingInv1, CompareObjectNames);

        GameObject[] CraftingInv2 = GameObject.FindGameObjectsWithTag("Crow2");
        Array.Sort(CraftingInv2, CompareObjectNames);

        GameObject[] CraftingInv3 = GameObject.FindGameObjectsWithTag("Crow3");
        Array.Sort(CraftingInv3, CompareObjectNames);

        GameManager._Instance.CraftingTablePanel.SetActive(false);

        PStatus.SetToolbarItems(new List<GameObject>(Toolbar), new List<GameObject>(CreativeToolbar), new List<GameObject>(SurvivalToolbar), new List<GameObject>(CraftingToolbar));
        PStatus.SetInventoryItems(new List<GameObject>(SurvivalInv1), new List<GameObject>(SurvivalInv2), new List<GameObject>(SurvivalInv3));
        PStatus.SetCraftingItems(new List<GameObject>(CraftingInv1), new List<GameObject>(CraftingInv2), new List<GameObject>(CraftingInv3));
        PStatus.PrepareToolbox();
    }

    public int CompareObjectNames(GameObject a, GameObject b)
    {
        return a.name.CompareTo(b.name);
    }
    
    public void CratingTable()
    {
        if(!InCraftingTable)
        {
            Debug.Log("InCraftingTable");
            InCraftingTable = true;
            EventHandler.RegisterEvent(QuitCraftingTable, KeyCode.Escape);
            EventHandler.DeleteEvent(GameManager._Instance.HandleESCKey, KeyCode.Escape);
            EventHandler.DeleteEvent(HandleEEvent, KeyCode.E);
            GameManager._Instance.CraftingTablePanel.SetActive(true);
            PStatus.GetActiveHand().GetComponent<Animation>().Stop();
            PlayerGUI.Refresh();
            PlayerGUI.UpdateSurvInv();
            PlayerGUI.UpdateCraftingInv();
            PlayerGUI.UpdateMiniCraftingTable();
            PlayerGUI.UpdateCraftingTable();
            PlayerGUI.UpdateToolbar();
            GameManager._Instance.StateOfTheGame = GameManager.GameState.ININVENTORY;
            Cursor.visible = true;
        }
    }

    public void QuitCraftingTable()
    {
        if(InCraftingTable)
        {
            Debug.Log("QuitCraftingTable");
            InCraftingTable = false;
            EventHandler.RegisterEvent(GameManager._Instance.HandleESCKey, KeyCode.Escape);
            EventHandler.RegisterEvent(HandleEEvent, KeyCode.E);
            EventHandler.DeleteEvent(QuitCraftingTable, KeyCode.Escape);
            GameManager._Instance.CraftingTablePanel.SetActive(false);
            GameManager._Instance.StateOfTheGame = GameManager.GameState.RUNNING;
            Cursor.visible = false;
        }
    }

    public Block.Direction GetFacingDirection()
    {
        float rY = GameManager._Instance.Player.transform.eulerAngles.y;
        float rX = GameObject.FindWithTag("Eyes").transform.eulerAngles.x;

        if (rX > 180.0)
            rX -= (float)360.0;

        if (rY > 180.0)
            rY -= (float)360.0;

        if (rY > -45.0 && rY <= 45.0)
            return Block.Direction.NORTH;
        else if (rY > 45.0 && rY <= 135.0)
            return Block.Direction.WEST;
        else if (rY > 135.0 && rY <= 180.0 || rY >= -180.0 && rY <= -135.0)
            return Block.Direction.SOUTH;
        else
            return Block.Direction.EAST;
    }

    public void HandleF3Event()
    {
        DebugStatus = !DebugStatus;
        GameManager._Instance.DbgText.enabled = DebugStatus;
    }

    public void HandleQEvent()
    {
        PStatus.DropOneItem();
    }

    public void HandleEEvent()
    {
        PStatus.ShowInventory();
    }

    public void HandleREvent()
    {
        if(GameManager._Instance.ModeOfTheGame == GameManager.GameMode.SURVIVAL)
            PStatus.ShowInventory2();
    }

}