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
    private Dictionary<TileType, ScriptableTile> _scriptedTiles = new();
    private Dictionary<Vector2, Tile> _tiles;
    private List<Tile> _borderTiles;
    public Tile PlayerTile {  get; private set; }
    public Action OnPlayerChangeTile;
    public Action OnMapChange;



    private void Awake()
    {
        Instance = this;
        LoadTiles();
        layout = Resources.LoadAll<GridLayoutScript>("GridLayouts").ToList().FirstOrDefault(); //TEMP
    }
    GridLayoutScript layout; /////////////////////////////////// REEEEEEEEEEMOVVVVVVVVE THISSSSSSSS


    #region >>> Generation <<<

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

    private void SpawnTilesFromLayout(GridLayoutScript layout)
    {
        _tiles = new Dictionary<Vector2, Tile>();

        for (int y = 0; y < layout.Height; y++)
        {
            for (int x = 0; x < layout.Width; x++)
            {
                Tile createdTile = Instantiate(GetPrefabForTileType(layout.GetTile(y, x)), new Vector3(x, -y + layout.Height - 1), Quaternion.identity);
                //Tile createdTile = Instantiate(GetPrefabForTileType(layout.GetTile(y, x)), new Vector3(x, y), Quaternion.identity);
                createdTile.name = $"Tile {x} {y}";

                createdTile.Init(x, y);
                _tiles[new Vector2(x, y)] = createdTile;
            }
        }

        _camera.transform.position = new Vector3((float)layout.Width / 2 - 0.5f, (float)layout.Height / 2 - 0.5f, -10);
        _camera.orthographicSize = layout.Width >= layout.Height ? layout.Height / 2 : layout.Width / 2;
        LoadedMap = layout;
    }
    private void FindBorderTiles()
    {
        _borderTiles = new();
        for(int x = 0; x < LoadedMap.Width; x++)
        {
            _borderTiles.Add(GetTileAtPosition(new Vector2(x,0)));
            _borderTiles.Add(GetTileAtPosition(new Vector2(x, LoadedMap.Height - 1)));
        }
        for(int y = 1; y < LoadedMap.Height -1; y++)
        {
            _borderTiles.Add(GetTileAtPosition(new Vector2(0,y)));
            _borderTiles.Add(GetTileAtPosition(new Vector2(LoadedMap.Width -1, y)));
        }
    }

    public void GenerateGrid()
    {
        SpawnTilesFromLayout(layout);
        FindBorderTiles();
        foreach (var tile in _tiles)
            tile.Value.NavigationNode.ConnectNeighboringTiles();


        ////GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    #endregion


    public Tile GetTileAtPosition(Vector2 position)
    {
        if(_tiles.TryGetValue(position, out Tile tile))
            return tile;
        return null;
    }
    public Tile GetRandomWalkableTile()
    {
        Tile tile = _tiles.Where(t => t.Value.IsWalkable).OrderBy(o => UnityEngine.Random.value).FirstOrDefault().Value;
        if(tile == null)
            return null;
        return tile;
    }
    public Tile GetRandomBorderTile()
    {
        return _borderTiles?.ToList()?.OrderBy(o => UnityEngine.Random.value)?.First();
    }
    public Tile GetRandomBorderTileWalkable()
    {
        return _borderTiles?.ToList()?.Where(t => t.IsWalkable).OrderBy(o => UnityEngine.Random.value).FirstOrDefault();
    }
    public bool IsBorderTile(Tile tile) => _borderTiles.Contains(tile);
    public void SetPlayerTile(Tile tile)
    {
        if (tile == null)
            return;
        if (tile != PlayerTile)
            OnPlayerChangeTile?.Invoke();
        PlayerTile = tile;
    }
}
