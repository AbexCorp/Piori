using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;

    [SerializeField]
    private int _width;
    [SerializeField]
    private int _height;

    [SerializeField]
    private Transform _cameraPosition;

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
        for(int x = 0; x < layout.Width; x++)
        {
            for(int y = 0; y < layout.Height; y++)
            {
                Tile createdTile = Instantiate(GetPrefabForTileType(layout.tiles[((y*layout.Width)) +x]), new Vector3(x,y), Quaternion.identity);
                createdTile.name = $"Tile {x} {y}";

                createdTile.Init(x,y);

                _tiles[new Vector2(x,y)] = createdTile;
            }
        }

        _cameraPosition.transform.position = new Vector3((float)layout.Width / 2 - 0.5f, (float)layout.Width / 2 - 0.5f, -10);
    }

    public void GenerateGrid()
    {
        Gener(layout);
        //_tiles = new Dictionary<Vector2, Tile>();
        //for(int x = 0; x < _width; x++)
        //{
        //    for(int y = 0; y < _height; y++)
        //    {
        //        var randomTile = Random.Range(0, 6) == 3 ? _wallTile : _groundTile;
        //        var createdTile = Instantiate(randomTile, new Vector3(x,y), Quaternion.identity);
        //        createdTile.name = $"Tile {x} {y}";

        //        createdTile.Init(x, y);

        //        _tiles[new Vector2(x, y)] = createdTile;
        //    }
        //}

        //_cameraPosition.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        ////GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.IsWalkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if(_tiles.TryGetValue(position, out Tile tile))
            return tile;
        return null;
    }
}
