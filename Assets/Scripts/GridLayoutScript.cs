using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridLayout", menuName = "Grid Layout")]
public class GridLayoutScript : ScriptableObject
{
    public int Width;
    public int Height;
    public TileType[] Tiles;

    public Vector2Int SpawnPosition;


    public TileType GetTile(int y, int x)
    {
        return Tiles[y * Width + x];
    }
    public void SetTile(int y, int x, TileType tileType)
    {
        Tiles[y * Width + x] = tileType;
    }
}
