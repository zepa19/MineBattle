using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoop : ILoop
{

    private static MainLoop _Instance;
    private List<ILoop> _RegisteredLoopes = new List<ILoop>();

    public static void Instantiate()
    {
        _Instance = new MainLoop();
        Logger.Instantiate();
        World.Instantiate();

        BlockRegistry.RegisterBlocks();
        PlayerMiniCrafting.Initialize();
        PlayerCrafting.Initialize();
        
        Player.Instantiate();
        DayNightCycle.Instantiate();
        // TODO
    }

    public void Start()
    {
        foreach (ILoop l in _RegisteredLoopes)
        {
            l.Start();
        }
    }

    public void Update()
    {
        foreach (ILoop l in _RegisteredLoopes)
        {
            l.Update();
        }
    }

    public void LateUpdate()
    {
        foreach (ILoop l in _RegisteredLoopes)
        {
            l.LateUpdate();
        }
    }

    public void OnApplicationQuit()
    {
        foreach (ILoop l in _RegisteredLoopes)
        {
            l.OnApplicationQuit();
        }
    }

    public void RegisterLoopes(ILoop l)
    {
        _RegisteredLoopes.Add(l);
    }

    public void DeRegisterLoopes(ILoop l)
    {
        _RegisteredLoopes.Remove(l);
    }

    public static MainLoop GetInstance()
    {
        return _Instance;
    }
}
