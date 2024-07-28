using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GridLayout", menuName = "Grid Layout")]
public class GridLayoutScript : ScriptableObject
{
    public int Width;
    public int Height;
    public TileType[] tiles;
}
