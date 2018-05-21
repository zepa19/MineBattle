using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UvMap
{

    private static List<UvMap> _Maps = new List<UvMap>();
    public string name;
    public Type typ;

    public enum Type
    {
        Top,
        Bottom,
        Front,
        Side
    };

    public Vector2[] _UVMAP;

    public UvMap(string name, Vector2[] _UVMAP)
    {
        string[] nam = name.Split('.')[0].Split('/')[2].Split('_');

        if (nam.Length == 1)
        {
            typ = Type.Side;
        }
        else
        {
            switch (nam[1])
            {
                case "top":
                    typ = Type.Top;
                    break;
                case "side":
                    typ = Type.Side;
                    break;
                case "bottom":
                    typ = Type.Bottom;
                    break;
                case "front":
                    typ = Type.Front;
                    break;
                default:
                    typ = Type.Side;
                    break;
            }
        }

        this.name = nam[0];
        this._UVMAP = _UVMAP;
    }

    public void Register()
    {
        _Maps.Add(this);
    }

    public static List<UvMap> getUvMap(string name)
    {

        List<UvMap> BlockUvMap = new List<UvMap>();

        foreach (UvMap m in _Maps)
        {
            if (m.name.Equals(name))
            {
                BlockUvMap.Add(m);
            }
        }

        if (BlockUvMap.Count > 0)
            return BlockUvMap;


        GameManager.ExitGame();

        return new List<UvMap> { _Maps[0] };
    }

    public static List<UvMap> GUV()
    {

        return _Maps;
    }
}
