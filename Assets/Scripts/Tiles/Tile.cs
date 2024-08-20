using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Tile : MonoBehaviour, IMouseInteractions
{
    //Components
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;
    [SerializeField]
    protected GameObject _highlight;
    [SerializeField]
    protected GameObject _collision;


    //Inspector References
    public ScriptableTile ScriptableTile { get { return _scriptableTile; } }
    [SerializeField]
    protected ScriptableTile _scriptableTile;


    //Code References
    [HideInInspector]
    public BaseTower OccuppyingTower { get; private set; }
    [HideInInspector]
    public NavigationNode NavigationNode { get; private set; }


    //Fields & Properties
    public string TileName => ScriptableTile.TileName;
    public bool IsWalkable => ScriptableTile.IsWalkable && OccuppyingTower == null && !_isCheckingForEnclosure;
    public bool IsBuildable => ScriptableTile.IsBuildable && OccuppyingTower == null && !GridManager.Instance.IsBorderTile(this);
    public bool BlocksBullets => ScriptableTile.BlocksBullets; // || OccypyingTower is tall???
    public TileType TileType => ScriptableTile.TileType;
    public Color TileColor => ScriptableTile.TileColor;
    public Coordinates TileCoordinates { get; private set; }

    public struct Coordinates
    {
        public int X { get { return (int)GridPosition.x; } }
        public int Y { get { return (int)GridPosition.y; } }
        public Vector2 GridPosition { get; private set; }
        public Vector2 WorldPosition { get; private set; }
        public Coordinates(int x, int y, Vector2 worldPosition)
        {
            GridPosition = new Vector2(x, y);
            WorldPosition = worldPosition;
        }
    }

    protected bool _isCheckingForEnclosure = false;




    public virtual void Init(int x, int y)
    {
        //bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        //_spriteRenderer.color = isOffset ? _primaryColor : _secondaryColor;

        if (BlocksBullets)
            EnableCollisions(fullObstacle: true);
        else if (!IsWalkable)
            EnableCollisions(fullObstacle: false);
        TileCoordinates = new Coordinates(x, y, transform.position);
        NavigationNode = new NavigationNode(this);
    }

    public void SetTower(BaseTower tower) //Move tower here
    {
        //if (tower.OccupiedTile != null) //If tile was set somwhere already, will be used when moving unit
            //tower.OccupiedTile.OccupiedUnit = null;

        tower.transform.position = transform.position;
        OccuppyingTower = tower;
        tower.SetOccupiedTile(this);
    }
    public void BuildTower(BaseTower towerPrefab)
    {
        BaseTower tower = TowerManager.Instance.SpawnTower(towerPrefab);
        tower.transform.position = transform.position;
        OccuppyingTower = tower;
        tower.SetOccupiedTile(this);
        EnableCollisions(fullObstacle: BlocksBullets);
    }
    //public BaseTower TakeTower() { } Move tower from here
    public void DestroyTower()
    {
        OccuppyingTower.SetOccupiedTile(null);
        Destroy(OccuppyingTower.gameObject);
        OccuppyingTower = null;
        DisableCollisions();
    }
    protected bool BuildingHereCreatesEnclosedArea()
    {
        _isCheckingForEnclosure = true;
        foreach(NavigationNode node in NavigationNode.Neighbors)
        {
            if (!node.Tile.IsWalkable)
                continue;
            List<NavigationNode> path = Pathfinding.FindPath(node, GridManager.Instance.GetRandomBorderTileWalkable().NavigationNode);
            if (path == null)
            {
                _isCheckingForEnclosure = false;
                return true;
            }
        }
        _isCheckingForEnclosure = false;
        return false;
    }

    

    void IMouseInteractions.OnMouseHoverEnter()
    {
        _highlight?.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }

    void IMouseInteractions.OnMouseHoverLeave()
    {
        _highlight?.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    void IMouseInteractions.OnMouseLeftClick(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;
        //if (GameManager.Instance.GameState != GameState.BuyPhase)
        //    return;
        if(TowerManager.Instance.IsSelling == true) //Selling
        {
            if (IsBuildable || OccuppyingTower == null)
                return;

            DestroyTower();
            GridManager.Instance.OnMapChange?.Invoke();
            MenuManager.Instance.StopSelling();
        }
        else if(TowerManager.Instance.SelectedTowerPrefabToBuy != null) //Building
        {
            if (OccuppyingTower != null || !IsBuildable)
                return;

            if(BuildingHereCreatesEnclosedArea())
            {
                Debug.LogWarning("Encloesd");
                return;
            }
            BuildTower(TowerManager.Instance.SelectedTowerPrefabToBuy);
            GridManager.Instance.OnMapChange?.Invoke();
            MenuManager.Instance.ClearTower();
        }
    }


    public void EnableCollisions(bool fullObstacle)
    {
        if(_collision == null)
        {
            Debug.LogError($"{gameObject.name} \"{TileName}\" has no collision object");
            return;
        }
        _collision.SetActive(true);
        _collision.layer = fullObstacle ? LayerMask.NameToLayer("ObstacleFull") : LayerMask.NameToLayer("ObstacleHalf");
    }
    public void DisableCollisions()
    {
        if(_collision == null)
        {
            Debug.LogError($"{gameObject.name} \"{TileName}\" has no collision object");
            return;
        }
        _collision.SetActive(false);
    }
}
