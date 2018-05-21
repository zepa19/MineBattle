using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour, IPointerClickHandler
{
    public int ButtonID = -1;

    public bool SurvivalInventoryToolbar = false;
    public bool SurvivalInventory = false;
    public bool MiniCraftingTableIN = false;
    public bool MiniCraftingTableOUT = false;
    public bool Armor = false;

    public bool CreativeInventoryToolbar = false;
    public bool CreativeInventory = false;

    public bool CraftingInventoryToolbar = false;
    public bool CraftingInventory = false;
    public bool CraftingTableIN = false;
    public bool CraftingTableOUT = false;

    public static BlockItem CopyOfItem = null;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (SurvivalInventoryToolbar)
                SurvivalInventoryToolbarLeftClick();
            else if (SurvivalInventory)
                SurvivalInventoryLeftClick();
            else if (MiniCraftingTableIN)
                MiniCraftingTableINLeftClick();
            else if (MiniCraftingTableOUT)
                MiniCraftingTableOUTLeftClick();
            else if (Armor)
                ArmorLeftClick();
            else if (CreativeInventoryToolbar)
                CreativeInventoryToolbarLeftClick();
            else if (CreativeInventory)
                CreativeInventoryLeftClick();
            else if (CraftingInventoryToolbar)
                CraftingInventoryToolbarLeftClick();
            else if (CraftingInventory)
                CraftingInventoryLeftClick();
            else if (CraftingTableIN)
                CraftingTableINLeftClick();
            else if (CraftingTableOUT)
                CraftingTableOUTLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (SurvivalInventoryToolbar)
                SurvivalInventoryToolbarRightClick();
            else if (SurvivalInventory)
                SurvivalInventoryRightClick();
            else if (MiniCraftingTableIN)
                MiniCraftingTableINRightClick();
            else if (CreativeInventoryToolbar)
                CreativeInventoryToolbarRightClick();
            else if (CraftingInventoryToolbar)
                CraftingInventoryToolbarRightClick();
            else if (CraftingInventory)
                CraftingInventoryRightClick();
            else if (CraftingTableIN)
                CraftingTableINRightClick();
        }
    }
    
    private void MiniCraftingTableOUTLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (GameManager._Instance.MCTGameObjs.B[ButtonID].BlockID != 0)
            {
                CopyOfItem = GameManager._Instance.MCTGameObjs.B[ButtonID];
                GameManager._Instance.MCTGameObjs.B[ButtonID] = new BlockItem();
            }
        }

        PlayerMiniCrafting.DoMiniCrafting(true);
        PlayerGUI.UpdateMiniCraftingTable();
        SetTextTemp2();

    }

    private void CraftingTableOUTLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (GameManager._Instance.CraftTableData.B[ButtonID].BlockID != 0)
            {
                CopyOfItem = GameManager._Instance.CraftTableData.B[ButtonID];
                GameManager._Instance.CraftTableData.B[ButtonID] = new BlockItem();
            }
        }

        PlayerCrafting.DoCrafting(true);
        PlayerGUI.UpdateCraftingTable();
        SetTextTemp3();

    }

    private void MiniCraftingTableINLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (GameManager._Instance.MCTGameObjs.B[ButtonID].BlockID != 0)
            {
                CopyOfItem = GameManager._Instance.MCTGameObjs.B[ButtonID];
                GameManager._Instance.MCTGameObjs.B[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (GameManager._Instance.MCTGameObjs.B[ButtonID].BlockID != 0)
            {
                BlockItem tmp = GameManager._Instance.MCTGameObjs.B[ButtonID];
                GameManager._Instance.MCTGameObjs.B[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                GameManager._Instance.MCTGameObjs.B[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerMiniCrafting.DoMiniCrafting(false);
        PlayerGUI.UpdateMiniCraftingTable();
        SetTextTemp2();
    }
    

    private void CraftingTableINLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (GameManager._Instance.CraftTableData.B[ButtonID].BlockID != 0)
            {
                CopyOfItem = GameManager._Instance.CraftTableData.B[ButtonID];
                GameManager._Instance.CraftTableData.B[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (GameManager._Instance.CraftTableData.B[ButtonID].BlockID != 0)
            {
                BlockItem tmp = GameManager._Instance.CraftTableData.B[ButtonID];
                GameManager._Instance.CraftTableData.B[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                GameManager._Instance.CraftTableData.B[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerCrafting.DoCrafting(false);
        PlayerGUI.UpdateCraftingTable();
        SetTextTemp3();
    }

    private void MiniCraftingTableINRightClick()
    {

        if (CopyOfItem != null)
        {
            if (GameManager._Instance.MCTGameObjs.B[ButtonID].BlockID != 0)
            {
                if (GameManager._Instance.MCTGameObjs.B[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (GameManager._Instance.MCTGameObjs.B[ButtonID].Count < 64)
                    {
                        if(GameManager._Instance.MCTGameObjs.B[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }
                    }
                }
            }
            else
            {
                GameManager._Instance.MCTGameObjs.B[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }

        PlayerMiniCrafting.DoMiniCrafting(false);
        PlayerGUI.UpdateMiniCraftingTable();
        SetTextTemp2();

    }
    
    private void CraftingTableINRightClick()
    {

        if (CopyOfItem != null)
        {
            if (GameManager._Instance.CraftTableData.B[ButtonID].BlockID != 0)
            {
                if (GameManager._Instance.CraftTableData.B[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (GameManager._Instance.CraftTableData.B[ButtonID].Count < 64)
                    {

                        if (GameManager._Instance.CraftTableData.B[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }
                    }

                }
            }
            else
            {
                GameManager._Instance.CraftTableData.B[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }
        
        PlayerCrafting.DoCrafting(false);
        PlayerGUI.UpdateCraftingTable();
        SetTextTemp3();

    }

    private void SurvivalInventoryRightClick()
    {

        if (CopyOfItem != null)
        {
            if (Player.PStatus.SurvivalInv[ButtonID].BlockID != 0)
            {
                if (Player.PStatus.SurvivalInv[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (Player.PStatus.SurvivalInv[ButtonID].Count < 64)
                    {

                        if (Player.PStatus.SurvivalInv[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }

                    }
                }
            }
            else
            {
                Player.PStatus.SurvivalInv[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }

        PlayerGUI.UpdateSurvInv();
        SetTextTemp2();
    }

    private void CraftingInventoryRightClick()
    {

        if (CopyOfItem != null)
        {
            if (Player.PStatus.SurvivalInv[ButtonID].BlockID != 0)
            {
                if (Player.PStatus.SurvivalInv[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (Player.PStatus.SurvivalInv[ButtonID].Count < 64)
                    {

                        if (Player.PStatus.SurvivalInv[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }
                    }
                }
            }
            else
            {
                Player.PStatus.SurvivalInv[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }

        PlayerGUI.UpdateCraftingInv();
        SetTextTemp3();
    }

    private void SurvivalInventoryToolbarRightClick()
    {
        if (CopyOfItem != null)
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                if (Player.PStatus.ToolBox9[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (Player.PStatus.ToolBox9[ButtonID].Count < 64)
                    {
                        if (Player.PStatus.ToolBox9[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }
                    }
                }
            }
            else
            {
                Player.PStatus.ToolBox9[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }

        PlayerGUI.UpdateToolbar();
        SetTextTemp2();
    }

    private void CraftingInventoryToolbarRightClick()
    {
        if (CopyOfItem != null)
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                if (Player.PStatus.ToolBox9[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (Player.PStatus.ToolBox9[ButtonID].Count < 64)
                    {
                        if (Player.PStatus.ToolBox9[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }
                    }
                }
            }
            else
            {
                Player.PStatus.ToolBox9[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }

        PlayerGUI.UpdateToolbar();
        SetTextTemp3();
    }

    private void CreativeInventoryToolbarRightClick()
    {

        if(CopyOfItem != null)
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                if(Player.PStatus.ToolBox9[ButtonID].BlockID == CopyOfItem.BlockID)
                {
                    if (Player.PStatus.ToolBox9[ButtonID].Count < 64)
                    {
                        if (Player.PStatus.ToolBox9[ButtonID].IncrementBlocks())
                        {
                            CopyOfItem.DecrementBlocks();

                            if (CopyOfItem.Count == 0)
                            {
                                CopyOfItem = null;
                            }
                        }
                    }
                }
            }
            else
            {
                Player.PStatus.ToolBox9[ButtonID].Set(CopyOfItem.BlockID, 1, CopyOfItem.Level);
                CopyOfItem.DecrementBlocks();

                if (CopyOfItem.Count == 0)
                {
                    CopyOfItem = null;
                }
            }
        }

        PlayerGUI.UpdateToolbar();
        SetTextTemp();
    }

    private void CreativeInventoryLeftClick()
    {
        if (this.GetComponent<Button>().interactable)
        {
            if (CopyOfItem == null)
            {
                int ID = Int32.Parse(gameObject.GetComponentsInChildren<Text>()[1].text);
                CopyOfItem = new BlockItem(PlayerStatus.CreativeInv[ID]);
            }
            else
            {
                CopyOfItem = null;
            }
        }
        else
        {
            if (CopyOfItem != null)
            {
                CopyOfItem = null;
            }
        }
        
        SetTextTemp();
    }

    private void CreativeInventoryToolbarLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                CopyOfItem = Player.PStatus.ToolBox9[ButtonID];
                Player.PStatus.ToolBox9[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                BlockItem tmp = Player.PStatus.ToolBox9[ButtonID];
                Player.PStatus.ToolBox9[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                Player.PStatus.ToolBox9[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerGUI.UpdateToolbar();
        SetTextTemp();
    }

    private void ArmorLeftClick()
    {
        Debug.Log("jestem");
        if (CopyOfItem == null)
        {
            if (Player.PStatus.ArmorBIT.BlockID != 0)
            {
                CopyOfItem = Player.PStatus.ArmorBIT;
                Player.PStatus.ArmorBIT = new BlockItem(BlockRegistry.GID("Chestplate"), 1, 0);
            }
        }
        else
        {
            if (Player.PStatus.ArmorBIT.Level == 0)
            {
                Player.PStatus.ArmorBIT.SetLevel(CopyOfItem.Level);
                CopyOfItem = null;
            }
        }

        PlayerGUI.UpdateArmor();
        SetTextTemp2();
    }


    private void SurvivalInventoryLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (Player.PStatus.SurvivalInv[ButtonID].BlockID != 0)
            {
                CopyOfItem = Player.PStatus.SurvivalInv[ButtonID];
                Player.PStatus.SurvivalInv[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (Player.PStatus.SurvivalInv[ButtonID].BlockID != 0)
            {
                BlockItem tmp = Player.PStatus.SurvivalInv[ButtonID];
                Player.PStatus.SurvivalInv[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                Player.PStatus.SurvivalInv[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerGUI.UpdateSurvInv();
        SetTextTemp2();
    }

    private void CraftingInventoryLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (Player.PStatus.SurvivalInv[ButtonID].BlockID != 0)
            {
                CopyOfItem = Player.PStatus.SurvivalInv[ButtonID];
                Player.PStatus.SurvivalInv[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (Player.PStatus.SurvivalInv[ButtonID].BlockID != 0)
            {
                BlockItem tmp = Player.PStatus.SurvivalInv[ButtonID];
                Player.PStatus.SurvivalInv[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                Player.PStatus.SurvivalInv[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerGUI.UpdateCraftingInv();
        SetTextTemp3();
    }

    private void CraftingInventoryToolbarLeftClick()
    {
        if (CopyOfItem == null)
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                CopyOfItem = Player.PStatus.ToolBox9[ButtonID];
                Player.PStatus.ToolBox9[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                BlockItem tmp = Player.PStatus.ToolBox9[ButtonID];
                Player.PStatus.ToolBox9[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                Player.PStatus.ToolBox9[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerGUI.UpdateToolbar();
        SetTextTemp3();
    }

    private void SurvivalInventoryToolbarLeftClick()
    {

        if(CopyOfItem == null)
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                CopyOfItem = Player.PStatus.ToolBox9[ButtonID];
                Player.PStatus.ToolBox9[ButtonID] = new BlockItem();
            }
        }
        else
        {
            if (Player.PStatus.ToolBox9[ButtonID].BlockID != 0)
            {
                BlockItem tmp = Player.PStatus.ToolBox9[ButtonID];
                Player.PStatus.ToolBox9[ButtonID] = CopyOfItem;
                CopyOfItem = tmp;
            }
            else
            {
                Player.PStatus.ToolBox9[ButtonID] = CopyOfItem;
                CopyOfItem = null;
            }
        }

        PlayerGUI.UpdateToolbar();
        SetTextTemp2();
    }

    void SetTextTemp()
    {
        //GameManager._Instance.InfText.text = string.Format("ItemID: {0}\nCopyOfItem: {1}", ItemID, (CopyOfItem == null) ? 0 : 1);
        if (CopyOfItem != null)
        {
            if (CopyOfItem.isLevelType)
            {
                GameManager._Instance.InventoryText.text = string.Format("{0} - Lvl: {1}", BlockRegistry._RegisteredBlocks[CopyOfItem.BlockID].GetBlockName(), CopyOfItem.Level);
            }
            else
            {
                GameManager._Instance.InventoryText.text = string.Format("{0}x {1}", CopyOfItem.Count, BlockRegistry._RegisteredBlocks[CopyOfItem.BlockID].GetBlockName());
            }
        }
        else
        {
            GameManager._Instance.InventoryText.text = "";
        }
    }

    void SetTextTemp2()
    {
        //GameManager._Instance.InfText.text = string.Format("ItemID: {0}\nCopyOfItem: {1}", ItemID, (CopyOfItem == null) ? 0 : 1);
        if (CopyOfItem != null)
        {

            if (CopyOfItem.isLevelType)
            {
                GameManager._Instance.InventoryText2.text = string.Format("{0} - Lvl: {1}", BlockRegistry._RegisteredBlocks[CopyOfItem.BlockID].GetBlockName(), CopyOfItem.Level);
            }
            else
            {
                GameManager._Instance.InventoryText2.text = string.Format("{0}x {1}", CopyOfItem.Count, BlockRegistry._RegisteredBlocks[CopyOfItem.BlockID].GetBlockName());
            }
        }
        else
        {
            GameManager._Instance.InventoryText2.text = "";
        }
    }

    void SetTextTemp3()
    {
        //GameManager._Instance.InfText.text = string.Format("ItemID: {0}\nCopyOfItem: {1}", ItemID, (CopyOfItem == null) ? 0 : 1);
        if (CopyOfItem != null)
        {
            if (CopyOfItem.isLevelType)
            {
                GameManager._Instance.InventoryText3.text = string.Format("{0} - Lvl: {1}", BlockRegistry._RegisteredBlocks[CopyOfItem.BlockID].GetBlockName(), CopyOfItem.Level);
            }
            else
            {
                GameManager._Instance.InventoryText3.text = string.Format("{0}x {1}", CopyOfItem.Count, BlockRegistry._RegisteredBlocks[CopyOfItem.BlockID].GetBlockName());
            }
        }
        else
        {
            GameManager._Instance.InventoryText3.text = "";
        }
    }

}
