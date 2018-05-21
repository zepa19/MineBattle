using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
public class RankingManager : MonoBehaviour {

    public List<Text> RankList;

	public void LoadRanking()
    {
        Serializer.Check_Gen_Folder(FileManager.WorldsDirectory);

        if(Serializer.CheckFileExists(FileManager.WorldsDirectory + "rank.dat"))
        {
            List<string> tmp = new List<string>(Serializer.Deserialize_From_File<string[]>(FileManager.WorldsDirectory + "rank.dat"));
            List<KeyValuePair<int, string>> players = new List<KeyValuePair<int, string>>();

            foreach (string pos in tmp)
            {
                string[] t = pos.Split('|');
                players.Add(new KeyValuePair<int, string>(Int32.Parse(t[2]), (t[0] + "|" + t[1])));
            }

            players.Sort((x, y) => -x.Key.CompareTo(y.Key));

            for (int i = 0; i < 10; i++)
            {
                if (i < players.Count)
                {
                    char c = 's';

                    if (players[i].Key < 2)
                        c = ' ';

                    string[] t = players[i].Value.Split('|');
                    RankList[i].text = string.Format("{0}. \"{1}\" from \"{2}\" {3} point{4}", i + 1, t[1], t[0], players[i].Key, c);
                }
                else
                {
                    RankList[i].text = string.Format("{0}.", i + 1);
                }
            }
        }
        else
        {
            for (int i = 0; i < 10; i++)
            {
                RankList[i].text = string.Format("{0}.", i + 1);
            }
        }

    }


}
