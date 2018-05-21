using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour {

    public Texture2D crossHairIMG;

    private void OnGUI()
    {
        float xMin = (Screen.width / 2) - (crossHairIMG.width / 2);
        float yMin = (Screen.height / 2) - (crossHairIMG.height / 2);

        if (GameManager._Instance.StateOfTheGame == GameManager.GameState.RUNNING)
        {
            GUI.DrawTexture(new Rect(xMin, yMin, crossHairIMG.width, crossHairIMG.height), crossHairIMG);
        }
    }
}
