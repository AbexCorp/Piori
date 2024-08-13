using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NavigationNode
{
    public Tile Tile { get; private set; }
    public List<NavigationNode> Neighbors { get; private set; }

    public NavigationNode Connection { get; private set; }
    public int StepsFromStart { get; private set; }
    public int G => StepsFromStart;
    public int StepsToEnd {  get; private set; }
    public int H => StepsToEnd;
    public int Score => StepsFromStart + StepsToEnd;
    public int F => Score;

    private List<Vector2> Directions = new() { Vector2.up, Vector2.right, Vector2.down, Vector2.left };

    public NavigationNode(Tile tile)
    {
        Tile = tile;
    }

    public void SetConnection(NavigationNode connection)
    {
        Connection = connection;
    }
    public void SetStepsFromStart(int g)
    {
        StepsFromStart = g;
    }
    public void SetStepsToEnd(int h)
    {
        StepsToEnd = h;
    }

    public void ConnectNeighboringTiles()
    {
        Neighbors = new();
        foreach(var direction in Directions)
        {
            Tile t = GridManager.Instance.GetTileAtPosition(direction + Tile.TileCoordinates.Position);
            if(t != null)
                Neighbors.Add(t.NavigationNode);
        }
    }

    public static int GetDistanceTo(NavigationNode from, NavigationNode to)
    {
        return Mathf.Abs(from.Tile.TileCoordinates.X - to.Tile.TileCoordinates.X) + Mathf.Abs(from.Tile.TileCoordinates.Y - to.Tile.TileCoordinates.Y);
    }
}
