using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tile", menuName = "Scriptable Tile")]
public class ScriptableTile : ScriptableObject
{
    public Tile TilePrefab;

    public string TileName;
    public bool IsWalkable;
    public bool IsBuildable;
    public bool BlocksBullets;
}
