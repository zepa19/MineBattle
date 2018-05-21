using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{

    public Text ErrorText;
    public GameObject NextPanel;
    public GameObject CurrentPanel;
    
    public void CurrentMenu(GameObject o)
    {
        GameManager._Instance.CurrentMenuPanel = o;
    }

    public void ResumeGame()
    {
        GameManager._Instance.HandleESCKey();
    }

    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void LoadByIndex(int sceneID)
    {
        if (sceneID == 1)
        {
            FileManager.GameName = WorldCreator.SelectedWorld;
        }
        else if (sceneID == 0)
        {
            GameManager.ExitGame();
            Quit();
        }

        SceneManager.LoadScene(sceneID);
    }

    public void CreateNewWorldHandler(InputField NameField)
    {
        ErrorText.enabled = false;

        if (Directory.Exists(FileManager.WorldsDirectory + NameField.text + "/"))
        {
            ErrorText.enabled = true;
        }
        else
        {
            FileManager.GameName = NameField.text;
            SceneManager.LoadScene(1);
        }
    }

    public void RenderDistanceSliderHandler(Text RText)
    {
        PlayerSettings.RenderDistanceChanged = true;
        int rDist = Mathf.RoundToInt(gameObject.GetComponent<Slider>().value);
        PlayerSettings.RenderDistance = rDist;
        RText.text = string.Format("Render distance - {0}", rDist);
    }

    public void DefaultButtonHandler()
    {
        NextPanel.SetActive(true);
        CurrentPanel.SetActive(false);
    }

    public void LoginButtonHandler(InputField LoginField)
    {
        ErrorText.enabled = false;

        if(LoginField.text.Equals(""))
        {
            ErrorText.enabled = true;
        }
        else
        {
            FileManager.PlayerName = LoginField.text;
            NextPanel.SetActive(true);
            CurrentPanel.SetActive(false);
        }
    }

}
