using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logger : ILoop
{

    public static Logger MainLog = new Logger();
    private List<string> Logs = new List<string>();
    
    public static void Instantiate()
    {
        MainLoop.GetInstance().RegisterLoopes(MainLog);
    }

    public static void Log(string l)
    {
        MainLog.log(l);
    }

    public static void Log(System.Exception e)
    {
        MainLog.log(e);
    }

    private void log(string l)
    {
        Logs.Add(l);
    }

    private void log(System.Exception e)
    {
        Logs.Add(e.StackTrace.ToString());
    }

    public void Update()
    {
        System.IO.File.WriteAllLines("Log.txt", new List<string>(Logs).ToArray());
    }

    public void Start()
    {

    }

    public void OnApplicationQuit()
    {

    }

    public void LateUpdate()
    {

    }
}