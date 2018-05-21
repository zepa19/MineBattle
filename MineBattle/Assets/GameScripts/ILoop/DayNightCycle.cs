using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : ILoop
{

    public static DayNightCycle DNCycle = new DayNightCycle();

    public bool cycleEnabled;
    public Light Sun;
    public Light Moon;

    private float intensity;
    private Color fogDay = Color.gray;
    private Color fogNight = Color.black;

    public int speed = 1000;

    public bool IsDay;
    int _days;
    int distance = 600;
    float rotAngle;

    public static void Instantiate()
    {
        MainLoop.GetInstance().RegisterLoopes(DNCycle);
    }

    public void Start()
    {
        Sun = GameObject.Find("Sun").GetComponent<Light>();
        Moon = GameObject.Find("Moon").GetComponent<Light>();

        float angle = (GameTime.Seconds - 21600) / 86400 * 360;

        Sun.transform.position = new Vector3(distance * Mathf.Cos(angle * Mathf.Deg2Rad), distance * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        Sun.transform.LookAt(Vector3.zero);

        Moon.transform.position = new Vector3(-distance * Mathf.Cos(angle * Mathf.Deg2Rad), -distance * Mathf.Sin(angle * Mathf.Deg2Rad), 0);
        Moon.transform.LookAt(Vector3.zero);

        IsDay = false;
        if (GameTime.Seconds > 21600 && GameTime.Seconds < 64800)
        {
            IsDay = true;
        }

        if (Sun == null || Moon == null)
        {
            Logger.Log("Failed to set Sun and Moon GameObjects");
        }
        else
        {
            Logger.Log("Loading Sun and Moon.");
            cycleEnabled = true;
        }
    }

    public void Update()
    {
        if (cycleEnabled)
        {
            GameTime.Update(Time.deltaTime);
            ChangeTime();
        }
    }

    public void OnApplicationQuit()
    {
        cycleEnabled = false;
    }

    public void ChangeTime()
    {

        rotAngle = GameTime.deltaTime / 240;

        Sun.transform.RotateAround(Vector3.zero, Vector3.forward, rotAngle);
        Sun.transform.LookAt(Vector3.zero);

        Moon.transform.RotateAround(Vector3.zero, Vector3.forward, rotAngle);
        Moon.transform.LookAt(Vector3.zero);

        if (GameTime.Seconds < 43200)
        {
            intensity = 1 - (43200 - GameTime.Seconds) / 43200;
        }
        else
        {
            intensity = 1 - ((43200 - GameTime.Seconds) / 43200 * -1);
        }

        if (Player._Instance.PlayerInitialized && GameTime.Seconds > 21600 && GameTime.Seconds < 64800 && !IsDay)
        {
            IsDay = true;
            Player._Instance.Day();
            RenderSettings.fogDensity = 0.01f;
        }
        else if (Player._Instance.PlayerInitialized && GameTime.Seconds > 64800 && IsDay)
        {
            IsDay = false;
            Player._Instance.Night();
            RenderSettings.fogDensity = 0.06f;
        }

        RenderSettings.fogColor = Color.Lerp(fogNight, fogDay, intensity * intensity);

        if (GameTime.Seconds < 19400 || GameTime.Seconds > 67000)
            Sun.intensity = 0;
        else
            Sun.intensity = intensity;
    }

    public bool Day()
    {
        return IsDay;
    }

    public bool Night()
    {
        return !IsDay;
    }

    public void LateUpdate()
    {

    }
}
