using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILoop
{
    void Start();
    void Update();
    void LateUpdate();
    void OnApplicationQuit();

}