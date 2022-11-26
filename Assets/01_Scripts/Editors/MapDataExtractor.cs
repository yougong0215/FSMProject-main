using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapDataExtractor
{
    [MenuItem("tools/exasdlnfl")]
    public static void ExtractMapData()
    {
        GameObject tilemap = GameObject.Find("Tilemap");

        if (tilemap == null)
        {
            Debug.LogError("'tilemap' 이라느 ㄴ이름이 업ㅁ서요");
            return;
        }

        Tilemap collision = tilemap.transform.Find("Collisions").GetComponent<Tilemap>();

        collision.CompressBounds(); // 외각 경계선 제거

        BoundsInt bounds = collision.cellBounds;

        using (StreamWriter writer = File.CreateText($"../Assets/Resources/Map/{tilemap.name}.txt"))
        {
    

        writer.WriteLine(bounds.xMin);
        writer.WriteLine(bounds.xMax);
        writer.WriteLine(bounds.yMin);
        writer.WriteLine(bounds.yMax);
            for(int y = bounds.yMax -1; y >=bounds.yMin; y--)
            {
                for(int x = bounds.xMin; x<=bounds.xMax-1; x++)
                {
                    Vector3Int tilePos = new Vector3Int(x, y, 0);
                    TileBase tile = collision.GetTile(tilePos);

                    if(tile != null)
                    {
                        writer.Write("1");
                    }
                    else
                    {
                        writer.Write("0");
                    }
                }
                writer.WriteLine("");
            }
        }
    }
}
