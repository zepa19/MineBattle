using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class PlayerGUI
{

    public static void ItemSelecting(ref int Item_idx)
    {

        int old_item = Item_idx;
        bool changed = false;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            Item_idx++;
            Item_idx %= 9;
            changed = true;
        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            Item_idx--;
            if (Item_idx < 0)
                Item_idx += 9;

            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Item_idx = 0;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Item_idx = 1;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Item_idx = 2;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Item_idx = 3;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Item_idx = 4;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Item_idx = 5;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Item_idx = 6;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            Item_idx = 7;
            changed = true;
        }

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Item_idx = 8;
            changed = true;
        }

        if(changed)
        {
            Player.PStatus.ToolbarItems[old_item].GetComponent<Image>().color = Color.white;
            Player.PStatus.ToolbarItems[Item_idx].GetComponent<Image>().color = GameManager._Instance.SelectedToolbar;
            UpdateArm();
        }
    }

    public static void UpdateSurvInv()
    {
        for (int i = 0; i < 27; i++) 
        {
            int bID = Player.PStatus.SurvivalInv[i].BlockID;
            int cID = Player.PStatus.SurvivalInv[i].Count;

            if(bID != 0)
            {
                Player.PStatus.SurvInventoryGOs[i].GetComponent<Image>().sprite = Sprite.Create(
                    BlockRegistry.GetBlockFromID(bID).GetIcon(),
                    new Rect(0, 0, 50, 50),
                    new Vector2(0.5f, 0.5f)
                );

                if (cID == 1)
                {
                    Player.PStatus.SurvInventoryGOs[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                }
                else
                {
                    Player.PStatus.SurvInventoryGOs[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    Player.PStatus.SurvInventoryGOs[i].GetComponentInChildren<Text>().text = cID.ToString();
                }
            }
            else
            {
                Player.PStatus.SurvInventoryGOs[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                Player.PStatus.SurvInventoryGOs[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public static void UpdateArmor()
    {

        if(Player.PStatus.ArmorBIT.Level == 0)
        {
            GameManager._Instance.Armor.GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
            GameManager._Instance.ArmorText.text = "Lvl: 0";
        }
        else
        {
            GameManager._Instance.Armor.GetComponent<Image>().sprite = Sprite.Create(
                    BlockRegistry.GetBlockFromID(Player.PStatus.ArmorBIT.BlockID).GetIcon(),
                    new Rect(0, 0, 50, 50),
                    new Vector2(0.5f, 0.5f)
                );
            GameManager._Instance.ArmorText.text = "Lvl: " + Player.PStatus.ArmorBIT.Level.ToString();
        }
    }

    public static void UpdateCraftingInv()
    {
        for (int i = 0; i < 27; i++)
        {
            int bID = Player.PStatus.SurvivalInv[i].BlockID;
            int cID = Player.PStatus.SurvivalInv[i].Count;

            if (bID != 0)
            {
                Player.PStatus.CraftInventoryGOs[i].GetComponent<Image>().sprite = Sprite.Create(
                    BlockRegistry.GetBlockFromID(bID).GetIcon(),
                    new Rect(0, 0, 50, 50),
                    new Vector2(0.5f, 0.5f)
                );

                if (cID == 1)
                {
                    Player.PStatus.CraftInventoryGOs[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                }
                else
                {
                    Player.PStatus.CraftInventoryGOs[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    Player.PStatus.CraftInventoryGOs[i].GetComponentInChildren<Text>().text = cID.ToString();
                }
            }
            else
            {
                Player.PStatus.CraftInventoryGOs[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                Player.PStatus.CraftInventoryGOs[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public static void UpdateMiniCraftingTable()
    {
        for (int i = 0; i < 5; i++)
        {
            int bID = GameManager._Instance.MCTGameObjs.B[i].BlockID;
            int cID = GameManager._Instance.MCTGameObjs.B[i].Count;

            if (bID != 0)
            {
                GameManager._Instance.MCTGameObjs.P[i].GetComponent<Image>().sprite = Sprite.Create(
                    BlockRegistry.GetBlockFromID(bID).GetIcon(),
                    new Rect(0, 0, 50, 50),
                    new Vector2(0.5f, 0.5f)
                );

                if (cID == 1)
                {
                    GameManager._Instance.MCTGameObjs.P[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                }
                else
                {
                    GameManager._Instance.MCTGameObjs.P[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    GameManager._Instance.MCTGameObjs.P[i].GetComponentInChildren<Text>().text = cID.ToString();
                }
            }
            else
            {
                GameManager._Instance.MCTGameObjs.P[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                GameManager._Instance.MCTGameObjs.P[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public static void UpdateCraftingTable()
    {
        for (int i = 0; i < 10; i++)
        {
            int bID = GameManager._Instance.CraftTableData.B[i].BlockID;
            int cID = GameManager._Instance.CraftTableData.B[i].Count;

            if (bID != 0)
            {
                GameManager._Instance.CraftTableData.P[i].GetComponent<Image>().sprite = Sprite.Create(
                    BlockRegistry.GetBlockFromID(bID).GetIcon(),
                    new Rect(0, 0, 50, 50),
                    new Vector2(0.5f, 0.5f)
                );

                if (cID == 1)
                {
                    GameManager._Instance.CraftTableData.P[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                }
                else
                {
                    GameManager._Instance.CraftTableData.P[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    GameManager._Instance.CraftTableData.P[i].GetComponentInChildren<Text>().text = cID.ToString();
                }
            }
            else
            {
                GameManager._Instance.CraftTableData.P[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                GameManager._Instance.CraftTableData.P[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
            }
        }
    }

    public static void UpdateToolbar()  // drop or sth
    {
        UpdateArm();
        for (int i = 0; i < 9; i++)
        {
            int bID = Player.PStatus.ToolBox9[i].BlockID;
            int cID = Player.PStatus.ToolBox9[i].Count;

            if (bID != 0)
            {
                Sprite tmp = Sprite.Create(
                    BlockRegistry.GetBlockFromID(bID).GetIcon(),
                    new Rect(0, 0, 50, 50),
                    new Vector2(0.5f, 0.5f)
                );

                Player.PStatus.ToolbarItems[i].GetComponent<Image>().sprite = tmp;
                Player.PStatus.CreativeToolbarItems[i].GetComponent<Image>().sprite = tmp;
                Player.PStatus.SurvivalToolbarItems[i].GetComponent<Image>().sprite = tmp;
                Player.PStatus.CraftingToolbarItems[i].GetComponent<Image>().sprite = tmp;

                if (cID == 1)
                {
                    Player.PStatus.ToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                    Player.PStatus.CreativeToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                    Player.PStatus.SurvivalToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                    Player.PStatus.CraftingToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
                }
                else
                {
                    Player.PStatus.ToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    Player.PStatus.ToolbarItems[i].GetComponentInChildren<Text>().text = cID.ToString();

                    Player.PStatus.CreativeToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    Player.PStatus.CreativeToolbarItems[i].GetComponentInChildren<Text>().text = cID.ToString();

                    Player.PStatus.SurvivalToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    Player.PStatus.SurvivalToolbarItems[i].GetComponentInChildren<Text>().text = cID.ToString();

                    Player.PStatus.CraftingToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    Player.PStatus.CraftingToolbarItems[i].GetComponentInChildren<Text>().text = cID.ToString();
                }

            }
            else
            {
                Player.PStatus.ToolbarItems[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                Player.PStatus.ToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);

                Player.PStatus.CreativeToolbarItems[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                Player.PStatus.CreativeToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);

                Player.PStatus.SurvivalToolbarItems[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                Player.PStatus.SurvivalToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);

                Player.PStatus.CraftingToolbarItems[i].GetComponent<Image>().sprite = GameManager._Instance.Toolbar_Empty;
                Player.PStatus.CraftingToolbarItems[i].GetComponentInChildren<Text>().color = new Color(0, 0, 0, 0);
            }
            
        }
    }

    public static void Refresh()
    {
        GameManager._Instance.Points.text = string.Format("Points: {0}", Player.PStatus.Points);
    }

    public static void Inventory()
    {

        if(GameManager._Instance.ModeOfTheGame == GameManager.GameMode.SURVIVAL)
            PlayerStatus.InitializeSurvivalInventory();
        PlayerStatus.InitializeCreativeInventory();

        int NumOfBlocks = PlayerStatus.CreativeInv.Count;
        int Count = Math.Max(Mathf.CeilToInt((float)NumOfBlocks / 9.0f), 5);

        for (int i = 0; i < Count; i++) 
        {

            Transform tmpGO = Transform.Instantiate(Resources.Load<Transform>("Prefabs/RowOfButtons"), GameManager._Instance.ToolbarParent);

            for (int j = 0; j < 9; j++) 
            {
                if (i * 9 + j < NumOfBlocks)
                {
                    Button t = tmpGO.GetChild(j).GetComponent<Button>();
                    t.interactable = true;
                    Texture2D icon = BlockRegistry._RegisteredBlocks[PlayerStatus.CreativeInv[i * 9 + j].BlockID].GetIcon();
                    t.GetComponentsInChildren<Text>()[0].text = "";
                    t.GetComponentsInChildren<Text>()[0].color = new Color(0.196f, 0.196f, 0.196f, 1f);
                    t.GetComponentsInChildren<Text>()[1].text = (i * 9 + j).ToString();
                    tmpGO.GetChild(j).GetComponent<Image>().sprite = Sprite.Create(icon, new Rect(0, 0, 50, 50), new Vector2(0.5f, 0.5f));
                }
            }
        }
        
    }

    public static void Debugging()
    {
        if (Player._Instance.DebugStatus)
        {

            Chunk c = World._Instance.GetChunk(Player._Instance.ChunkPos.x, Player._Instance.ChunkPos.y, Player._Instance.ChunkPos.z);
            string ChunkErrored = "";
            string LookingAtStr = "";

            if (c.GetType().Equals(typeof(ErroredChunk)))
                ChunkErrored = " (Errored Chunk)";

            if (Player._Instance.LookingAtBlock)
            {
                string[] tmp = MathHelper.GetBlockNameIDFromHitPointPosition(Int3.ToVector3(Player._Instance.HitPoint));
                LookingAtStr = "Looking at: " + string.Format("{0} {1} {2}\n", Player._Instance.HitPoint.x, Player._Instance.HitPoint.y, Player._Instance.HitPoint.z);
                LookingAtStr += "Block type: " + tmp[0] + " (" + tmp[1] + ")";
            }

            string gamemode = (GameManager._Instance.ModeOfTheGame == GameManager.GameMode.CREATIVE) ? "Creative" : "Survival";

            string gameInfo = string.Format("Welcome {0} in MineBattle\nCurrent Save: {1}\nCurrent GameMode: {2}", FileManager.PlayerName, FileManager.GameName, gamemode);
            GameManager._Instance.DbgText.text = string.Format(
                gameInfo + "\n\n" + GameTime.ToString() +
                "\nXYZ: {0:F3} / {1:F3} / {2:F3}\n" +
                "Block: {3} {4} {5}\n" +
                "Chunk: {6} {7} {8} in {9} {10} {11}" +
                ChunkErrored + "\nFacing: " + GenFacing() + LookingAtStr,
                Player._Instance.PlayerIOPosition.x, Player._Instance.PlayerIOPosition.y, Player._Instance.PlayerIOPosition.z,
                Player._Instance.BlockIOPosition.x, Player._Instance.BlockIOPosition.y, Player._Instance.BlockIOPosition.z,
                Player._Instance.BlockIOPosition.x - Player._Instance.ChunkPos.x * Chunk.ChunkWidth,
                Player._Instance.BlockIOPosition.y - Player._Instance.ChunkPos.y * Chunk.ChunkHeight,
                Player._Instance.BlockIOPosition.z - Player._Instance.ChunkPos.z * Chunk.ChunkWidth,
                Player._Instance.ChunkPos.x, Player._Instance.ChunkPos.y, Player._Instance.ChunkPos.z
            );
        }
    }

    public static void UpdateArm()
    {
        string name = "Arm";
        int bID = Player.PStatus.ToolBox9[Player.PStatus.CurrentItem].BlockID;
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

        Player.PStatus.Hands["Arm"].SetActive(false);
        Player.PStatus.Hands["Pickaxe"].SetActive(false);
        Player.PStatus.Hands["Sword"].SetActive(false);
        Player.PStatus.Hands["Axe"].SetActive(false);
        Player.PStatus.Hands["Shovel"].SetActive(false);
        Player.PStatus.Hands[name].SetActive(true);

    }

    public static void UpdateHealth()
    {
        GameManager._Instance.Hearts.fillAmount = (float)Player.PStatus.Health / 100f;
    }

    public static string GenFacing()
    {
        string GeneratedString = "";
        float rY = GameManager._Instance.Player.transform.eulerAngles.y;
        float rX = GameObject.FindWithTag("Eyes").transform.eulerAngles.x;

        if (rX > 180.0)
            rX -= (float)360.0;

        if (rY > 180.0)
            rY -= (float)360.0;

        if (rY > -45.0 && rY <= 45.0)
            GeneratedString = "south (Towards positive Z) ";
        else if (rY > 45.0 && rY <= 135.0)
            GeneratedString = "east (Towards positive X) ";
        else if (rY > 135.0 && rY <= 180.0 || rY >= -180.0 && rY <= -135.0)
            GeneratedString = "north (Towards negative Z) ";
        else
            GeneratedString = "west (Towards negative X) ";

        return GeneratedString + string.Format("({0:F2} / {1:F2})\n", rY, rX);
    }

}
