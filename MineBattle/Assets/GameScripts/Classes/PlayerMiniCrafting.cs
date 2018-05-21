using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data = System.Collections.Generic.KeyValuePair<int, int>;

public class PlayerMiniCrafting {

    public static List<List<Data>> Sets = new List<List<Data>>();

    public static void DoMiniCrafting(bool IsDone)
    {

        List<Data> dat = Check();

        if (dat[0].Key == -1)
        {
            GameManager._Instance.MCTGameObjs.B[4] = new BlockItem(0, 0);
            return;
        }

        if(IsDone)
        {
            for (int i = 0; i < 4; i++) 
            {
                GameManager._Instance.MCTGameObjs.B[i].Drop(dat[i].Value);
            }

            Player.PStatus.Points += 10;
            PlayerGUI.Refresh();
            DoMiniCrafting(false);
        }
        else
        {
            GameManager._Instance.MCTGameObjs.B[4] = new BlockItem(dat[4].Key, dat[4].Value);
        }
    }

    public static List<Data> Check()
    {
        foreach (List<Data> set in Sets)
        {
            int i = 0;
            foreach (Data cell in set)
            {
                if (i < 4)
                {
                    if (!(GameManager._Instance.MCTGameObjs.B[i].BlockID == cell.Key && GameManager._Instance.MCTGameObjs.B[i].Count >= cell.Value))
                    {
                        break;
                    }

                }

                if (i == 4)
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
        Sets = new List<List<Data>>()
        {
            //Planks Big Oak
            new List<Data>() { new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksbigoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbigoak"), 1), new Data(BlockRegistry.GID("Planksbigoak"), 4) },

            //Planks Acacia
            new List<Data>() { new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksacacia"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logacacia"), 1), new Data(BlockRegistry.GID("Planksacacia"), 4) },

            //Planks Birch
            new List<Data>() { new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksbirch"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logbirch"), 1), new Data(BlockRegistry.GID("Planksbirch"), 4) },

            //Planks Jungle
            new List<Data>() { new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksjungle"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logjungle"), 1), new Data(BlockRegistry.GID("Planksjungle"), 4) },

            //Planks Oak
            new List<Data>() { new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksoak"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logoak"), 1), new Data(BlockRegistry.GID("Planksoak"), 4) },

            //Planks Spruce
            new List<Data>() { new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(0, 0), new Data(BlockRegistry.GID("Planksspruce"), 4) },
            new List<Data>() { new Data(0, 0), new Data(0, 0), new Data(0, 0), new Data(BlockRegistry.GID("Logspruce"), 1), new Data(BlockRegistry.GID("Planksspruce"), 4) },

            //CraftingTable
            new List<Data>() { new Data(BlockRegistry.GID("Planksbigoak"), 1), new Data(BlockRegistry.GID("Planksbigoak"), 1), new Data(BlockRegistry.GID("Planksbigoak"), 1), new Data(BlockRegistry.GID("Planksbigoak"), 1), new Data(BlockRegistry.GID("Craftingtable"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("Planksacacia"), 1), new Data(BlockRegistry.GID("Planksacacia"), 1), new Data(BlockRegistry.GID("Planksacacia"), 1), new Data(BlockRegistry.GID("Planksacacia"), 1), new Data(BlockRegistry.GID("Craftingtable"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("Planksbirch"), 1), new Data(BlockRegistry.GID("Planksbirch"), 1), new Data(BlockRegistry.GID("Planksbirch"), 1), new Data(BlockRegistry.GID("Planksbirch"), 1), new Data(BlockRegistry.GID("Craftingtable"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("Planksjungle"), 1), new Data(BlockRegistry.GID("Planksjungle"), 1), new Data(BlockRegistry.GID("Planksjungle"), 1), new Data(BlockRegistry.GID("Planksjungle"), 1), new Data(BlockRegistry.GID("Craftingtable"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("Planksoak"), 1), new Data(BlockRegistry.GID("Planksoak"), 1), new Data(BlockRegistry.GID("Planksoak"), 1), new Data(BlockRegistry.GID("Planksoak"), 1), new Data(BlockRegistry.GID("Craftingtable"), 1) },
            new List<Data>() { new Data(BlockRegistry.GID("Planksspruce"), 1), new Data(BlockRegistry.GID("Planksspruce"), 1), new Data(BlockRegistry.GID("Planksspruce"), 1), new Data(BlockRegistry.GID("Planksspruce"), 1), new Data(BlockRegistry.GID("Craftingtable"), 1) }
        };

    }

}
