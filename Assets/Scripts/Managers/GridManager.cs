using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    //Components

    //Inspector References
    [SerializeField]
    private Camera _camera;

    //Code References
    [HideInInspector]
    public GridLayoutScript LoadedMap;

    //Fields & Properties



    //<<<Tiles>>>
    private Dictionary<TileType, ScriptableTile> _scriptedTiles = new();
    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
        LoadTiles();
        layout = Resources.LoadAll<GridLayoutScript>("GridLayouts").ToList().FirstOrDefault();
    }

    private void LoadTiles()
    {
        List<ScriptableTile> tiles = Resources.LoadAll<ScriptableTile>("Tiles").ToList();
        foreach(var tile in tiles)
        {
            if (_scriptedTiles.ContainsKey(tile.TileType))
                continue;
            _scriptedTiles[tile.TileType] = tile;
        }
    }
    private Tile GetPrefabForTileType(TileType tileType)
    {
        _scriptedTiles.TryGetValue(tileType, out ScriptableTile tile);
        if (tile == null)
            return null;
        return tile.TilePrefab;
    }


    GridLayoutScript layout; /////////////////////////////////// REEEEEEEEEEMOVVVVVVVVE THISSSSSSSS
    public void Gener(GridLayoutScript layout)
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int y = 0; y < layout.Height; y++)
        {
            for (int x = 0; x < layout.Width; x++)
            {
                Tile createdTile = Instantiate(GetPrefabForTileType(layout.GetTile(y, x)), new Vector3(x, -y + layout.Height - 1), Quaternion.identity);
                createdTile.name = $"Tile {y} {x}";

                createdTile.Init(x, y);
                _tiles[new Vector2(x, y)] = createdTile;
            }
        }

        _camera.transform.position = new Vector3((float)layout.Width / 2 - 0.5f, (float)layout.Height / 2 - 0.5f, -10);
        _camera.orthographicSize = layout.Width >= layout.Height ? layout.Height / 2 : layout.Width / 2;
        LoadedMap = layout;
    }

    public void GenerateGrid()
    {
        Gener(layout);
        
        ////GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    //public Tile GetHeroSpawnTile()
    //{
    //    return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.IsWalkable).OrderBy(t => Random.value).First().Value;
    //}

    public Tile GetTileAtPosition(Vector2 position)
    {
        if(_tiles.TryGetValue(position, out Tile tile))
            return tile;
        return null;
    }
    public Tile GetRandomWalkableTile()
    {
        Tile tile = _tiles.Where(t => t.Value.IsWalkable).FirstOrDefault().Value;
        if(tile == null)
            return null;
        return tile;
    }
}
