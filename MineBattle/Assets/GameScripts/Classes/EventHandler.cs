using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventHandler {

    public delegate void _Del();

    static event _Del KeyESC_Pressed = null;
    static event _Del KeyF3_Pressed = null;
    static event _Del KeyE_Pressed = null;
    static event _Del KeyQ_Pressed = null;
    static event _Del KeyR_Pressed = null;
    static event _Del KeyP_Pressed = null;

    public static void RegisterEvent(_Del method, KeyCode key)
    {
        if (key == KeyCode.Escape)
        {
            KeyESC_Pressed += method;
            return;
        }

        if (key == KeyCode.F3)
        {
            KeyF3_Pressed += method;
            return;
        }

        if (key == KeyCode.E)
        {
            KeyE_Pressed += method;
            return;
        }

        if (key == KeyCode.Q)
        {
            KeyQ_Pressed += method;
            return;
        }

        if (key == KeyCode.R)
        {
            KeyR_Pressed += method;
            return;
        }

        if (key == KeyCode.P)
        {
            KeyP_Pressed += method;
            return;
        }
    }

    public static void DeleteEvent(_Del method, KeyCode key)
    {
        if (key == KeyCode.Escape)
        {
            KeyESC_Pressed -= method;
            return;
        }

        if (key == KeyCode.F3)
        {
            KeyF3_Pressed -= method;
            return;
        }

        if (key == KeyCode.E)
        {
            KeyE_Pressed -= method;
            return;
        }

        if (key == KeyCode.Q)
        {
            KeyQ_Pressed -= method;
            return;
        }

        if (key == KeyCode.R)
        {
            KeyR_Pressed -= method;
            return;
        }

        if (key == KeyCode.P)
        {
            KeyP_Pressed -= method;
            return;
        }
    }

    public static void InvokeEvent(KeyCode key)
    {
        if (key == KeyCode.Escape)
        {
            if (KeyESC_Pressed != null)
            {
                KeyESC_Pressed();
            }
            Debug.Log("ESC");
            return;
        }

        if (key == KeyCode.F3)
        {
            if (KeyF3_Pressed != null)
            {
                KeyF3_Pressed();
            }
            Debug.Log("F3");
            return;
        }

        if (key == KeyCode.E)
        {
            if (KeyE_Pressed != null)
            {
                KeyE_Pressed();
            }
            Debug.Log("E");
            return;
        }

        if (key == KeyCode.Q)
        {
            if (KeyQ_Pressed != null)
            {
                KeyQ_Pressed();
            }
            Debug.Log("Q");
            return;
        }

        if (key == KeyCode.R)
        {
            if (KeyR_Pressed != null)
            {
                KeyR_Pressed();
            }
            Debug.Log("R");
            return;
        }

        if (key == KeyCode.P)
        {
            if (KeyP_Pressed != null)
            {
                KeyP_Pressed();
            }
            Debug.Log("P");
            return;
        }
    }

}
