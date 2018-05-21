using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextureAtlas {

    public static readonly TextureAtlas _Instance = new TextureAtlas();
    public static Texture2D _Atlas { get; private set; }
    public static Texture2D _Icons { get; private set; }

    public void CreateAtlas()
    {
        string[] _Images = Directory.GetFiles("textures/blocks/");
        string[] _MiniatureImages = Directory.GetFiles("textures/block_icons/");
        List<string> _BlockNames = new List<string>();
        bool exists = false;

        int PixelWidth = 64;
        int PixelHeight = 64;
        int atlasWidth = Mathf.CeilToInt(Mathf.Sqrt(_Images.Length) + 1) * PixelWidth;
        int atlasHeight = Mathf.CeilToInt(Mathf.Sqrt(_Images.Length) + 1) * PixelHeight;
        Texture2D Atlas = new Texture2D(atlasWidth, atlasHeight);
        int count = 0;

        for (int x = 0; x < atlasWidth / PixelWidth; x++)
        {

            for (int y = 0; y < atlasHeight / PixelHeight; y++)
            {
                if (count >= _Images.Length - 1)
                    goto End;

                Texture2D temp = new Texture2D(0, 0);
                temp.LoadImage(File.ReadAllBytes(_Images[count]));

                try
                {
                    if (!_Images[count].Split('.')[1].Equals("ini"))
                    {
                        Atlas.SetPixels(x * PixelWidth, y * PixelHeight, PixelWidth, PixelHeight, temp.GetPixels());
                    }

                }
                catch (System.Exception e)
                {
                    Debug.Log(e.StackTrace);
                }

                float startX = x * PixelWidth;
                float startY = y * PixelHeight;
                float perPixelRatioX = 1.0f / (float)Atlas.width;
                float perPixelRatioY = 1.0f / (float)Atlas.height;
                startX *= perPixelRatioX;
                startY *= perPixelRatioY;
                float endX = startX + (perPixelRatioX * PixelWidth);
                float endY = startY + (perPixelRatioY * PixelHeight);

                UvMap m = new UvMap(_Images[count], new Vector2[] {
                    new Vector2(startX, startY),
                    new Vector2(startX, endY),
                    new Vector2(endX, startY),
                    new Vector2(endX, endY)
                });

                m.Register();
                exists = false;

                foreach (string s in _BlockNames)
                {
                    string s1 = _Images[count].Split('.')[0].Split('/')[2].Split('_')[0];
                    if (s1.Equals(s.Split('.')[0].Split('/')[2].Split('_')[0]) || s1.Equals("desktop"))
                        exists = true;
                }

                if (!exists)
                    _BlockNames.Add(_Images[count]);

                count++;
            }
        }

        End:

        BlockRegistry.RegisterBlock(new Block("Air", true));
        bool MinExists;

        foreach (string s in _BlockNames)
        {
            MinExists = false;
            string name = s.Split('.')[0].Split('/')[2].Split('_')[0];
            Texture2D icon = new Texture2D(50, 50);

            foreach (string s2 in _MiniatureImages)
            {
                string m_name = s2.Split('.')[0].Split('/')[2];

                if (name.Equals(m_name))
                {
                    MinExists = true;
                    icon.LoadImage(File.ReadAllBytes(s2));

                    break;
                }
            }

            if (MinExists)
                BlockRegistry.RegisterBlock(new Block(char.ToUpper(name[0]) + name.Substring(1), false, name, icon));
            else
                BlockRegistry.RegisterBlock(new Block(char.ToUpper(name[0]) + name.Substring(1), false, name, null));
        }

        CreateTools();
        CreateSpecialItems();

        _Atlas = Atlas;
        File.WriteAllBytes("atlas.png", Atlas.EncodeToPNG());
    }

    private void CreateTools()
    {
        string[] _MiniatureImages = Directory.GetFiles("textures/tool_icons/");

        foreach(string s in _MiniatureImages)
        {
            string name = s.Split('.')[0].Split('/')[2];
            Texture2D icon = new Texture2D(50, 50);
            icon.LoadImage(File.ReadAllBytes(s));

            BlockRegistry.RegisterBlock(new Tools(char.ToUpper(name[0]) + name.Substring(1), icon));
        }
        
    }

    private void CreateSpecialItems()
    {

        string[] _MiniatureImages = Directory.GetFiles("textures/special_blocks/");

        foreach (string s in _MiniatureImages)
        {
            string name = s.Split('.')[0].Split('/')[2];
            name = char.ToUpper(name[0]) + name.Substring(1);
            Texture2D icon = new Texture2D(50, 50);
            icon.LoadImage(File.ReadAllBytes(s));
            if (name.Equals("Chestplate"))
                BlockRegistry.RegisterBlock(new SpecialBlocks(char.ToUpper(name[0]) + name.Substring(1), icon, true));
            else
                BlockRegistry.RegisterBlock(new SpecialBlocks(char.ToUpper(name[0]) + name.Substring(1), icon, false));
        }

    }

}
