using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = System.Collections.Generic.KeyValuePair<int, int>;

public class PlayerCrafting {

    public static List<List<Data>> Sets = new List<List<Data>>();

    public static void DoCrafting(bool IsDone)
    {
        List<Data> dat = Check();

        if (dat[0].Key == -1)
        {
            GameManager._Instance.CraftTableData.B[9] = new BlockItem(0, 0);
            return;
        }

        if (IsDone)
        {
            for (int i = 0; i < 9; i++)
            {
                int Bid = GameManager._Instance.CraftTableData.B[i].BlockID;
                if (Bid == BlockRegistry.GID("Pickaxe") || Bid == BlockRegistry.GID("Chestplate") || Bid == BlockRegistry.GID("Sword") || Bid == BlockRegistry.GID("Axe") || Bid == BlockRegistry.GID("Shovel"))
                {
                    GameManager._Instance.CraftTableData.B[i].Drop(1);
                }
                else
                {
                    GameManager._Instance.CraftTableData.B[i].Drop(dat[i].Value);
                }
            }

            Player.PStatus.Points += 10;
            DoCrafting(false);
        }
        else
        {
            int Bid = dat[9].Key;
            if (Bid == BlockRegistry.GID("Pickaxe") || Bid == BlockRegistry.GID("Chestplate") || Bid == BlockRegistry.GID("Sword") || Bid == BlockRegistry.GID("Axe") || Bid == BlockRegistry.GID("Shovel"))
            {
                GameManager._Instance.CraftTableData.B[9] = new BlockItem(dat[9].Key, 1, dat[9].Value);
                
            }
            else
            {
                GameManager._Instance.CraftTableData.B[9] = new BlockItem(dat[9].Key, dat[9].Value);
            }
            
        }
    }

    public static List<Data> Check()
    {
        foreach (List<Data> set in Sets)
        {
            int i = 0;
            foreach (Data cell in set)
            {
                if (i < 9)
                {
                    int Bid = GameManager._Instance.CraftTableData.B[i].BlockID;
                    if (Bid == BlockRegistry.GID("Pickaxe") || Bid == BlockRegistry.GID("Chestplate") || Bid == BlockRegistry.GID("Sword") || Bid == BlockRegistry.GID("Axe") || Bid == BlockRegistry.GID("Shovel"))
                    {
                        if(!(Bid == cell.Key && GameManager._Instance.CraftTableData.B[i].Level == cell.Value))
                        {
                            break;
                        }
                    }
                    else
                    {
                        if (!(GameManager._Instance.CraftTableData.B[i].BlockID == cell.Key && GameManager._Instance.CraftTableData.B[i].Count >= cell.Value))
                        {
                            break;
                        }
                    }

                }

                if (i == 9)
                {
                    return set;
                }

                i++;
            }
        }

        return new List<Data>() { new Data(-1, -1) };
    }

    public static void Initialize()
    {

        //new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0) },

        Sets = new List<List<Data>>()
        {
            //Planks Big Oak
            new List<Data>() { new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(BlockRegistry.GID("Planksbigoak"), 4) },

            //Planks Acacia
            new List<Data>() { new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(BlockRegistry.GID("Planksacacia"), 4) },

            //Planks Birch
            new List<Data>() { new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(BlockRegistry.GID("Planksbirch"), 4) },

            //Planks Jungle
            new List<Data>() { new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(BlockRegistry.GID("Planksjungle"), 4) },

            //Planks Oak
            new List<Data>() { new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(BlockRegistry.GID("Planksoak"), 4) },

            //Planks Spruce
            new List<Data>() { new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            
            //Sticks
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 4) },

            //Tools
            new List<Data>() { new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 1) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 1) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 1) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stick"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("Stone"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Stone"), 1), new Data(BlockRegistry.GID("Chestplate"), 1) },

            //Upgrade from 1 to 2 level
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("IronS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("CoalS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 2) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("IronS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("CoalS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 2) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("IronS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("CoalS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 2) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("IronS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("CoalS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 2) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Chestplate"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("IronS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("CoalS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Chestplate"), 2) },
            
            //Upgrade from 2 to 3 level
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 2), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapis"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Redstone"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 3) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 2), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapis"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Redstone"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 3) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 2), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapis"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Redstone"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 3) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 2), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapis"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Redstone"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 3) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Chestplate"), 2), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapis"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Redstone"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Chestplate"), 3) },
            
            //Upgrade from 3 to 4 level
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("DiamondS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("EmeraldS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Pickaxe"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("DiamondS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("EmeraldS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Shovel"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("DiamondS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("EmeraldS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Sword"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("DiamondS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("EmeraldS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Axe"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Chestplate"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("DiamondS"), 3), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("EmeraldS"), 3), new Data(0, 0), new Data(BlockRegistry.GID("Chestplate"), 4) },

            //blocks
            new List<Data>() { new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapis"), 1), new Data(BlockRegistry.GID("Lapisblock"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("IronS"), 1), new Data(BlockRegistry.GID("Ironblock"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("GoldS"), 1), new Data(BlockRegistry.GID("Goldblock"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("DiamondS"), 1), new Data(BlockRegistry.GID("Diamond"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("CoalS"), 1), new Data(BlockRegistry.GID("Coalblock"), 1) },

            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapisblock"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Lapis"), 9) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Ironblock"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("IronS"), 9) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Goldblock"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("GoldS"), 9) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Diamond"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("DiamondS"), 9) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Coalblock"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("CoalS"), 9) },



        };

    }

}
