using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus {

    public List<GameObject> ToolbarItems;
    public List<GameObject> CreativeToolbarItems;
    public List<GameObject> SurvivalToolbarItems;
    public List<GameObject> CraftingToolbarItems;
    public List<GameObject> SurvInventoryGOs;
    public List<GameObject> CraftInventoryGOs;
    
    public BlockItem ArmorBIT;

    public static List<BlockItem> CreativeInv = new List<BlockItem>();
    public List<BlockItem> SurvivalInv = new List<BlockItem>();

    public List<BlockItem> ToolBox9;
    public Dictionary<string, GameObject> Hands = new Dictionary<string, GameObject>();

    public int CurrentItem = 0;

    public float gameDifficulty;
    
    public bool Died = false;
    public bool InventoryVisible;
    public int Points;
    public int Health;
    public int Enemies;

    public PlayerStatus()
    {
        ToolbarItems = new List<GameObject>();
        CreativeToolbarItems = new List<GameObject>();
        SurvivalToolbarItems = new List<GameObject>();
        CraftingToolbarItems = new List<GameObject>();
        SurvInventoryGOs = new List<GameObject>();
        CraftInventoryGOs = new List<GameObject>();
        ArmorBIT = new BlockItem(BlockRegistry.GID("Chestplate"), 1, 0);

        GameManager._Instance.MCTGameObjs.B = new List<BlockItem>() { new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem() };
        GameManager._Instance.CraftTableData.B = new List<BlockItem>();
        for (int i=0; i<10; i++)
        {
            GameManager._Instance.CraftTableData.B.Add(new BlockItem());
        }
        ToolBox9 = new List<BlockItem>() { new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem(), new BlockItem() };
        
        InventoryVisible = false;
        Health = 100;
        Points = 0;
        Died = false;

        Enemies = Mathf.FloorToInt(12 * Mathf.Log(GameTime.Day + 2));

        gameDifficulty = 0.5f;
        if (GameTime.Day < 100)
            gameDifficulty = GameTime.Day / 100;
    }


    public void PrepareToolbox()
    {
        if(PlayerSettings.FromSave)
        {
            ToolBox9 = new List<BlockItem>(PlayerSettings.toolbox);
            PlayerGUI.UpdateMiniCraftingTable();
            PlayerGUI.UpdateToolbar();
        }
    }

    public void SetHands()
    {
        
        string name = "Arm";
        int bID = ToolBox9[CurrentItem].BlockID;
        if (bID == BlockRegistry.GID("Pickaxe"))
        {
            name = "Pickaxe";
        }
        else if (bID == BlockRegistry.GID("Sword"))
        {
            name = "Sword";
        }
        else if (bID == BlockRegistry.GID("Axe"))
        {
            name = "Axe";
        }
        else if (bID == BlockRegistry.GID("Shovel"))
        {
            name = "Shovel";
        }

        foreach (GameObject o in GameObject.FindGameObjectsWithTag("Arm"))
        {
            Hands.Add(o.name, o);

            if (o.name != name)
            {
                o.SetActive(false);
            }
        }
    }

    public void TakeDamage(int d)
    {
        
        if(d < 0)
        {
            Health -= d;

            if (Health > 100)
            {
                Health = 100;
            }

            PlayerGUI.UpdateHealth();

        }
        else if(!Died && Player._Instance.CanEnemyAttack)
        {
            Player._Instance.CanEnemyAttack = false;
            Health -= Mathf.FloorToInt(d * (1 - ArmorBIT.Level / 8));

            GameManager._Instance.AttackEffect.Play();

            if (Health <= 0)
            {
                Health = 0;
                Died = true;
                PlayerDeath();
            }

            if (Health > 100)
            {
                Health = 100;
            }

            PlayerGUI.UpdateHealth();
        }
        
    }

    public void PlayerDeath()
    {
        GameManager._Instance.BackgroundIMG.SetActive(true);
        GameManager._Instance.DiedText.SetActive(true);
        GameManager._Instance.DiedText.GetComponent<Text>().text = string.Format("You died!\nNick: {0}\nPoints: {1}\nNow you start playing in Creative Mode", FileManager.PlayerName, Points);
    }

    public static void InitializeSurvivalInventory()
    {

        for (int i=0; i<27; i++)
        {
            if(PlayerSettings.FromSave)
            {
                Player.PStatus.SurvivalInv.Add(new BlockItem(PlayerSettings.surv[i]));//zrobic podobnie dalej
            }
            else
            {
                Player.PStatus.SurvivalInv.Add(new BlockItem());
            }
        }

        if(PlayerSettings.FromSave)
        {
            GameManager._Instance.MCTGameObjs.B = new List<BlockItem>(PlayerSettings.craft)
            {
                new BlockItem()
            };

            Player.PStatus.ArmorBIT.SetLevel(PlayerSettings.ArmorLevel);
        }
    }

    public static void InitializeCreativeInventory()
    {
        foreach(Block b in BlockRegistry._RegisteredBlocks)
        {
            if(b.InCreativeInventory)
            {
                if (b.GetID() == BlockRegistry.GID("Pickaxe") || b.GetID() == BlockRegistry.GID("Chestplate") || b.GetID() == BlockRegistry.GID("Sword") || b.GetID() == BlockRegistry.GID("Axe") || b.GetID() == BlockRegistry.GID("Shovel"))
                {
                    CreativeInv.Add(new BlockItem(b.GetID(), 1, 1));
                }
                else
                {
                    CreativeInv.Add(new BlockItem(b.GetID(), 1));

                }
            }
        }
    }

    public static void AddBlock(int id)
    {
        Player.PStatus.AddBlockToInventory(id);
        PlayerGUI.UpdateToolbar();
    }

    private void AddBlockToInventory(int id)
    {
        
        if(IsInInventory(id))
        {
            foreach (BlockItem b in ToolBox9)
            {
                if (b.Equals(id) && b.Count < 64)
                {
                    b.IncrementBlocks();
                    return;
                }
            }

            foreach (BlockItem b in SurvivalInv)
            {
                if (b.Equals(id) && b.Count < 64)
                {
                    b.IncrementBlocks();
                    return;
                }
            }
        }
        else
        {
            foreach (BlockItem b in ToolBox9)
            {
                if (b.Equals(0))
                {
                    b.Set(id, 1);
                    return;
                }
            }

            foreach (BlockItem b in SurvivalInv)
            {
                if (b.Equals(0))
                {
                    b.Set(id, 1);
                    return;
                }
            }
        }

        

    }

    public void SetToolbarItems(List<GameObject> a, List<GameObject> b, List<GameObject> c, List<GameObject> d)
    {
        ToolbarItems = a;
        CreativeToolbarItems = b;
        SurvivalToolbarItems = c;
        CraftingToolbarItems = d;
        ToolbarItems[CurrentItem].GetComponent<Image>().color = GameManager._Instance.SelectedToolbar;
    }

    public void SetInventoryItems(List<GameObject> a, List<GameObject> b, List<GameObject> c)
    {
        for (int i = 0; i < 9; i++) 
        {
            SurvInventoryGOs.Add(a[i]);
        }

        for (int i = 0; i < 9; i++)
        {
            SurvInventoryGOs.Add(b[i]);
        }

        for (int i = 0; i < 9; i++)
        {
            SurvInventoryGOs.Add(c[i]);
        }
    }

    public void SetCraftingItems(List<GameObject> a, List<GameObject> b, List<GameObject> c)
    {
        for (int i = 0; i < 9; i++)
        {
            CraftInventoryGOs.Add(a[i]);
        }

        for (int i = 0; i < 9; i++)
        {
            CraftInventoryGOs.Add(b[i]);
        }

        for (int i = 0; i < 9; i++)
        {
            CraftInventoryGOs.Add(c[i]);
        }
    }

    public void DropOneItem()
    {
        ToolBox9[CurrentItem].DecrementBlocks();
        PlayerGUI.UpdateToolbar();
    }

    public void DropSetOfItem()
    {
        ToolBox9[CurrentItem].DropBlocks();
        PlayerGUI.UpdateToolbar();
    }

    public GameObject GetActiveHand()
    {
        foreach(KeyValuePair<string, GameObject> t in Hands)
        {
            if (t.Value.activeSelf)
                return t.Value;
        }

        return Hands["Arm"];
    }

    public int GetBlockIdFromToolbar(int ID = -1)
    {
        if (ID == -1)
            ID = CurrentItem;

        return ToolBox9[ID].BlockID;
    }

    public void ShowInventory()
    {
        InventoryVisible = !InventoryVisible;

        if(GameManager._Instance.ModeOfTheGame == GameManager.GameMode.CREATIVE)
        {
            GameManager._Instance.CreativeInventory.SetActive(InventoryVisible);

            if (InventoryVisible)
            {
                GameManager._Instance.StateOfTheGame = GameManager.GameState.ININVENTORY;
                Cursor.visible = true;
            }
            else
            {
                GameManager._Instance.StateOfTheGame = GameManager.GameState.RUNNING;
                Cursor.visible = false;
            }

            PlayerGUI.UpdateToolbar();
        }
        else
        {
            GameManager._Instance.SurvivalInventory.SetActive(InventoryVisible);

            if (InventoryVisible)
            {
                GameManager._Instance.StateOfTheGame = GameManager.GameState.ININVENTORY;
                Cursor.visible = true;
            }
            else
            {
                GameManager._Instance.StateOfTheGame = GameManager.GameState.RUNNING;
                Cursor.visible = false;
            }

            PlayerGUI.Refresh();
            PlayerGUI.UpdateSurvInv();
            PlayerGUI.UpdateCraftingInv();
            PlayerGUI.UpdateMiniCraftingTable();
            PlayerGUI.UpdateCraftingTable();
            PlayerGUI.UpdateToolbar();
            PlayerGUI.UpdateArmor();
        }
    }

    bool inv2 = false;
    public void ShowInventory2()
    {
        inv2 = !inv2;

        GameManager._Instance.CreativeInventory.SetActive(inv2);

        if (inv2)
        {
            GameManager._Instance.StateOfTheGame = GameManager.GameState.ININVENTORY;
            Cursor.visible = true;
        }
        else
        {
            GameManager._Instance.StateOfTheGame = GameManager.GameState.RUNNING;
            Cursor.visible = false;
        }

        PlayerGUI.UpdateToolbar();
    }

    public void UpdateBreakingProgress(bool active, float t = 0f)
    {

        if(active)
        {
            int l = Mathf.RoundToInt(Mathf.Min(100f, t*100));
            GameManager._Instance.BreakingTimeText.text = string.Format("{0}%", l);
        }
        else
        {
            GameManager._Instance.BreakingTimeText.text = "";
        }

    }

    public bool IsInInventory(int id)
    {
        foreach(BlockItem b in ToolBox9)
        {
            if(b.Equals(id) && b.Count < 64 && !b.isLevelType)
            {
                return true;
            }
        }

        foreach (BlockItem b in SurvivalInv)
        {
            if (b.Equals(id) && b.Count < 64 && !b.isLevelType)
            {
                return true;
            }
        }

        return false;
    }

}

public class BlockItem
{
    public int BlockID { get; private set; }
    public int Count { get; private set; }
    public int Level { get; private set; }
    public bool isLevelType { get; private set; }

    public BlockItem()
    {
        BlockID = 0;
        Count = 0;
        Level = 0;
        isLevelType = false;
    }

    public BlockItem(BlockItem a)
    {
        BlockID = a.BlockID;
        Count = a.Count;
        Level = a.Level;
        isLevelType = a.isLevelType;
    }

    public BlockItem(int a, int b)
    {
        BlockID = a;
        Count = b;
        DetermineLevel();
    }

    public BlockItem(int a, int b, int c)
    {
        BlockID = a;
        Count = b;
        DetermineLevel();
        Level = c;
    }

    private void DetermineLevel()
    {
        isLevelType = false;

        if (BlockID == BlockRegistry.GID("Pickaxe") || BlockID == BlockRegistry.GID("Chestplate") || BlockID == BlockRegistry.GID("Sword") || BlockID == BlockRegistry.GID("Axe") || BlockID == BlockRegistry.GID("Shovel"))
        {
            isLevelType = true;
            Level = 1;
        }
    }

    public bool IncrementLevel()
    {
        if(Level == 4)
        {
            return false;
        }
        Level++;
        return true;
    }

    public void SetLevel(int l)
    {
        Level = l;
    }

    public void DecrementBlocks()
    {
        Count--;
        if (Count <= 0)
        {
            BlockID = 0;
            Count = 0;
            Level = 1;
        }
    }

    public bool IncrementBlocks()
    {
        if(!isLevelType)
        {
            if(Count >= 64)
            {
                return false;
            }

            Count++;
            return true;
        }
        return false;
    }

    public void DropBlocks()
    {
        BlockID = 0;
        Count = 0;
        Level = 1;
    }

    public void Set(int a, int b, int lvl = -1)
    {
        BlockID = a;
        Count = b;

        if (BlockID == BlockRegistry.GID("Pickaxe") || BlockID == BlockRegistry.GID("Chestplate") || BlockID == BlockRegistry.GID("Sword") || BlockID == BlockRegistry.GID("Axe") || BlockID == BlockRegistry.GID("Shovel"))
        {
            isLevelType = true;
            Level = 1;
            if (lvl != -1)
                Level = lvl;
        }
        else
        {
            isLevelType = false;
        }
    }

    public void Drop(int b)
    {
        Count -= b;

        if(Count <= 0)
        {
            BlockID = 0;
            Count = 0;
        }
    }

    public bool Equals(int ID)
    {
        if (BlockID == ID)
            return true;

        return false;
    }
}
