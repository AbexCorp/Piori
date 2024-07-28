using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Scriptable Tile")]
public class ScriptableTile : ScriptableObject
{
    public Tile TilePrefab;

    public string TileName = "NOT SET";
    public bool IsWalkable;
    public bool IsBuildable;
    public bool BlocksBullets;

    //Map Editor
    public TileType TileType;
    public Color TileColor = Color.white;
}

public enum TileType
{
    Default = 0,
    Ground = 1,
    Wall = 2
}
