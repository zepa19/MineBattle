using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public GameObject LoadingText;
    public Animation AttackEffect;
    public Text DbgText;
    public Text ArmorText;
    public GameObject DiedText;
    public Text InventoryText;
    public Text InventoryText2;
    public Text InventoryText3;
    public Text BreakingTimeText;
    public Text Points;
    public Color SelectedToolbar;
    public Sprite Toolbar_Empty;
    
    public GameObject MainMenu;
    public GameObject Toolbar;
    public GameObject CreativeInventory;
    public GameObject SurvivalInventory;
    public GameObject CraftingTablePanel;
    public GameObject Armor;

    public GameObject HeartsPanel;
    public Image Hearts;

    public Transform ToolbarParent;

    [System.Serializable]
    public class MiniCraftingTableData
    {
        public List<GameObject> P;

        [System.NonSerialized]
        public List<BlockItem> B;
    }

    [System.Serializable]
    public class CraftingTableData
    {
        public List<GameObject> P;

        [System.NonSerialized]
        public List<BlockItem> B;
    }

    public MiniCraftingTableData MCTGameObjs = new MiniCraftingTableData();
    public CraftingTableData CraftTableData = new CraftingTableData();

    [System.NonSerialized]
    public GameObject Arm;

    [System.NonSerialized]
    public GameObject CurrentMenuPanel;

    [System.NonSerialized]
    public bool IsPlayerLoaded = false;

    [System.NonSerialized]
    public Vector3 PlayerPosition;

    [System.NonSerialized]
    public GameObject MainCamera;

    [System.NonSerialized]
    public GameObject BackgroundIMG;

    [System.NonSerialized]
    public GameObject Player;

    [System.NonSerialized]
    public GameObject Eyes;

    public static float Sdx = 94.3f;
    public static float Sdy = 23.9f;
    public static float Sdz = 64.1f;
    public static float Smy = 4.13f;
    public static float Scutoff = 6.11f;
    public static float Smul = 1f;

    public enum GameState
    {
        LOADING,
        RUNNING,
        PAUSE,
        ININVENTORY
    }

    public enum GameMode
    {
        CREATIVE,
        SURVIVAL
    }

    public GameState StateOfTheGame;
    public GameMode ModeOfTheGame;
    public static GameManager _Instance;

    private bool DbgTxtEnabled = false;

    private MainLoop MainL;

    private List<System.Delegate> _Delegates = new List<System.Delegate>();

    void Start () {

        StateOfTheGame = GameState.LOADING;

        _Instance = this;

        FileManager.RegisterFiles();

        TextureAtlas._Instance.CreateAtlas();

        MainLoop.Instantiate();
        MainL = MainLoop.GetInstance();
        
        Cursor.visible = false;
        MainL.Start();

        StateOfTheGame = GameState.RUNNING;
        EventHandler.RegisterEvent(GameManager._Instance.HandleESCKey, KeyCode.Escape);

        DayNightCycle.DNCycle.cycleEnabled = true;
    }

    void Update () {

        HandleInputs();

		if(StateOfTheGame == GameState.RUNNING)
        {

            BreakingTimeText.text = "";

            if (Player != null)
            {
                PlayerPosition = Player.transform.transform.position;
                IsPlayerLoaded = true;
            }

            MainL.Update();

            foreach (System.Delegate d in new List<System.Delegate>(_Delegates))
            {
                d.DynamicInvoke();
                _Delegates.Remove(d);
            }

        }
	}

    private void LateUpdate()
    {
        if (StateOfTheGame == GameState.RUNNING)
        {

            MainL.LateUpdate();
        }
    }

    void OnApplicationQuit()
    {
        MainL.OnApplicationQuit();
    }

    public void RegisterDelegate(System.Delegate d)
    {
        _Delegates.Add(d);
    }

    private void HandleInputs()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EventHandler.InvokeEvent(KeyCode.Escape);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            EventHandler.InvokeEvent(KeyCode.F3);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            EventHandler.InvokeEvent(KeyCode.E);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventHandler.InvokeEvent(KeyCode.Q);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            EventHandler.InvokeEvent(KeyCode.R);
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            EventHandler.InvokeEvent(KeyCode.P);
        }
    }

    public void HandleESCKey()
    {
        HandlePause();
    }

    private void HandlePause()
    {
        if (StateOfTheGame == GameState.RUNNING)
        {
            StateOfTheGame = GameState.PAUSE;
            CurrentMenuPanel = MainMenu;
            Eyes.SetActive(false);
            Toolbar.SetActive(false);
            MainCamera.SetActive(true);
            CurrentMenuPanel.SetActive(true);
            HeartsPanel.SetActive(false);
            BackgroundIMG.SetActive(true);
            DbgTxtEnabled = DbgText.enabled;
            DbgText.enabled = false;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
        else if(StateOfTheGame == GameState.PAUSE)
        {
            StateOfTheGame = GameState.RUNNING;
            BackgroundIMG.SetActive(false);
            CurrentMenuPanel.SetActive(false);
            MainCamera.SetActive(false);
            Toolbar.SetActive(true);

            if(ModeOfTheGame == GameMode.SURVIVAL)
                HeartsPanel.SetActive(true);

            Eyes.SetActive(true);
            DbgText.enabled = DbgTxtEnabled;
            Cursor.visible = false;
            Time.timeScale = 1f;
        }
        else    // state = Inventory
        {
            StateOfTheGame = GameState.RUNNING;
        }
    }

    public void StartPlayer(Vector3 Pos)
    {
        GameObject.Destroy(LoadingText);

        MainCamera = GameObject.FindWithTag("StartCamera");
        BackgroundIMG = GameObject.FindWithTag("BackgroundIMG");

        MainCamera.SetActive(false);

        if(ModeOfTheGame == GameMode.SURVIVAL)
            HeartsPanel.SetActive(true);

        Player = Transform.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), Pos, Quaternion.identity) as GameObject;
        Eyes = GameObject.Find("Player(Clone)/Camera");
        Arm = GameObject.Find("Player(Clone)/Camera/Arm");

        Player.transform.position = Pos;

    }

    public void ExitGame1()
    {
        OnApplicationQuit();
    }

    internal static void ExitGame()
    {
        _Instance.ExitGame1();
    }

    internal static bool PlayerLoaded()
    {
        return _Instance.IsPlayerLoaded;
    }
}
