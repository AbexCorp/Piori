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
    [SerializeField]
    private Tile _groundTile;
    [SerializeField]
    private Tile _wallTile;

    private Dictionary<Vector2, Tile> _tiles;

    private void Awake()
    {
        Instance = this;
    }


    public void GenerateGrid()
    {
        _tiles = new Dictionary<Vector2, Tile>();
        for(int x = 0; x < _width; x++)
        {
            for(int y = 0; y < _height; y++)
            {
                var randomTile = Random.Range(0, 6) == 3 ? _wallTile : _groundTile;
                var createdTile = Instantiate(randomTile, new Vector3(x,y), Quaternion.identity);
                createdTile.name = $"Tile {x} {y}";

                createdTile.Init(x, y);

                _tiles[new Vector2(x, y)] = createdTile;
            }
        }

        _cameraPosition.transform.position = new Vector3((float)_width / 2 - 0.5f, (float)_height / 2 - 0.5f, -10);

        GameManager.Instance.ChangeState(GameState.SpawnHeroes);
    }

    public Tile GetHeroSpawnTile()
    {
        return _tiles.Where(t => t.Key.x < _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }
    public Tile GetEnemySpawnTile()
    {
        return _tiles.Where(t => t.Key.x > _width / 2 && t.Value.Walkable).OrderBy(t => Random.value).First().Value;
    }

    public Tile GetTileAtPosition(Vector2 position)
    {
        if(_tiles.TryGetValue(position, out Tile tile))
            return tile;
        return null;
    }
}
