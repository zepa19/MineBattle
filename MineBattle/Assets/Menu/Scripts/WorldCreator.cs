using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class WorldCreator : MonoBehaviour {

    public GameObject ItemPrefab;
    public Transform ParentPanel;
    public Transform CurrentPanel;

    public Color HighlightenedColor;
    public Color NormalColor;

    public static string SelectedWorld = "DevWorld";

    public void LoadScenes()
    {
        Serializer.Check_Gen_Folder(FileManager.WorldsDirectory);
        List<string> dirs = new List<string>(Directory.GetDirectories(FileManager.WorldsDirectory));

        foreach (string dir in dirs)
        {

            string WorldName = dir.Split('/')[2];
            string LastDate = "";

            try
            {
                LastDate = Serializer.Deserialize_From_File<string>(dir + "/general.dat");
            }
            catch (System.Exception e)
            {
                Logger.Log(e.ToString());
            }

            GameObject a = Instantiate(ItemPrefab);
            Text p = a.GetComponentInChildren(typeof(Text)) as Text;
            p.text = string.Format("{0}\n{1}", WorldName, LastDate);

            a.transform.SetParent(ParentPanel.transform, false);

            Button world = a.GetComponent<Button>();
            world.onClick.AddListener(delegate { SelectButton(world, dir); });

        }
    }

    public void SelectButton(Button thisB, string dir)
    {
        SelectedWorld = dir.Split('/')[2];

        foreach (GameObject o in new List<GameObject>(GameObject.FindGameObjectsWithTag("WorldButtons")))
        {
            Button tmp_B = o.GetComponent<Button>();
            ColorBlock tmp_CB = tmp_B.colors;
            tmp_CB.normalColor = NormalColor;
            tmp_B.colors = tmp_CB;
        }

        ColorBlock cb = thisB.colors;
        cb.normalColor = HighlightenedColor;
        thisB.colors = cb;

        foreach (GameObject o in new List<GameObject>(GameObject.FindGameObjectsWithTag("ButtonToggler")))
        {
            o.GetComponent<Button>().interactable = true;
        }
    }

    public void GoToMainMenu()
    {

        foreach (Transform child in ParentPanel.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        foreach (Transform child in CurrentPanel.transform)
        {
            if (child.gameObject.tag.Equals("ButtonToggler"))
                child.gameObject.GetComponent<Button>().interactable = false;
        }

        SelectedWorld = "DevWorld";
    }

    public void DeleteSelectedWorld()
    {

        try
        {
            Directory.Delete(FileManager.WorldsDirectory + SelectedWorld, true);
        }
        catch (System.Exception e)
        {
            Logger.Log(e.ToString());
        }

        GoToMainMenu();
        LoadScenes();
    }

}
