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
    [SerializeField]
    protected BoxCollider2D _collider;


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



    protected void Awake()
    {
        if(_spriteRenderer == null)
            _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if(_collider == null)
            _collider = gameObject.GetComponent<BoxCollider2D>();
    }
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


    #region >>> Building <<<

    //public void SetTower(BaseTower tower) //Move tower here
    //{
    //    //if (tower.OccupiedTile != null) //If tile was set somwhere already, will be used when moving unit
    //        //tower.OccupiedTile.OccupiedUnit = null;

    //    tower.transform.position = transform.position;
    //    OccuppyingTower = tower;
    //    tower.SetOccupiedTile(this);
    //}
    public void BuildTower(BaseTower towerPrefab)
    {
        BaseTower tower = TowerManager.Instance.SpawnTower(towerPrefab);
        tower.transform.position = transform.position;
        OccuppyingTower = tower;
        tower.SetOccupiedTile(this);
        EnableCollisions(fullObstacle: BlocksBullets);
        MenuManager.Instance.ShowTileHoverInfo(this);
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
    protected bool BuildingIsObstructedByPlayerOrEnemy()
    {
        if (GridManager.Instance.PlayerTile == this)
            return true;
        Collider2D col = Physics2D.OverlapBox(point:_collider.bounds.center, size:_collider.bounds.extents * 0.9f, angle:0, layerMask: LayerMask.GetMask(new string[] {"Player", "Enemy"}));
        if(col != null)
            return true;
        return false;
    }

    #endregion


    #region >>> Sprite <<<

    [Space]
    [Header("Sprites")]
    [SerializeField]
    protected Sprite _topLeft;
    [SerializeField]
    protected Sprite _topCenter;
    [SerializeField]
    protected Sprite _topRight;

    [SerializeField]
    protected Sprite _middleLeft;
    [SerializeField]
    protected Sprite _middleCenter;
    [SerializeField]
    protected Sprite _middleRight;

    [SerializeField]
    protected Sprite _bottomLeft;
    [SerializeField]
    protected Sprite _bottomCenter;
    [SerializeField]
    protected Sprite _bottomRight;


    public void SelectSprite()
    {
        bool top;
        bool left;
        bool right;
        bool down;

        top = CompareTileTypeAt(GridManager.GridUp + TileCoordinates.GridPosition);
        left = CompareTileTypeAt(GridManager.GridLeft + TileCoordinates.GridPosition);
        right = CompareTileTypeAt(GridManager.GridRight + TileCoordinates.GridPosition);
        down = CompareTileTypeAt(GridManager.GridDown + TileCoordinates.GridPosition);


        if ((top && right && left && down) || (!top && !right && !left && !down))
            _spriteRenderer.sprite = _middleCenter;

        else if (right && down && !left && !top)
            _spriteRenderer.sprite = _topLeft;
        else if (left && down && !top && !right)
            _spriteRenderer.sprite = _topRight;
        else if (top && right && !left && !down)
            _spriteRenderer.sprite = _bottomLeft;
        else if (top && left && !right && !down)
            _spriteRenderer.sprite = _bottomRight;

        else if (top && right && down && !left)
            _spriteRenderer.sprite = _middleLeft;
        else if (left && down && right && !top)
            _spriteRenderer.sprite = _topCenter;
        else if (top && left && down && !right)
            _spriteRenderer.sprite = _middleRight;
        else if (left && top && right && !down)
            _spriteRenderer.sprite = _bottomCenter;

        //add others here
        else
            _spriteRenderer.color = Color.red;
        if (_spriteRenderer.sprite == null)
            _spriteRenderer.sprite = _middleCenter;

    }
    protected bool CompareTileTypeAt(Vector2 position)
    {
        Tile t = GridManager.Instance.GetTileAtPosition(position);
        if(t == null || t._scriptableTile == _scriptableTile)
            return true;
        return false;
    }

    #endregion


    void IMouseInteractions.OnMouseHoverEnter()
    {
        _highlight?.SetActive(true);
        MenuManager.Instance.ShowTileHoverInfo(this);
    }

    void IMouseInteractions.OnMouseHoverLeave()
    {
        _highlight?.SetActive(false);
        MenuManager.Instance.ShowTileHoverInfo(null);
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

            TowerManager.Instance.ChangeCurrency( (int)(TowerManager.TowerSellingReturnPercentage * (float)OccuppyingTower.Cost) / 100 );
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
                //Debug.LogWarning("Cannot create enclosed spaces");
                MenuManager.Instance.ShowWarning("Cannot create enclosed spaces");
                return;
            }
            if (BuildingIsObstructedByPlayerOrEnemy())
            {
                //Debug.LogWarning("Blocked by player or enemy");
                MenuManager.Instance.ShowWarning("Space blocked by player or enemy");
                return;
            }
            BuildTower(TowerManager.Instance.SelectedTowerPrefabToBuy);
            GridManager.Instance.OnMapChange?.Invoke();
            TowerManager.Instance.ChangeCurrency( -TowerManager.Instance.SelectedTowerPrefabToBuy.Cost );
            MenuManager.Instance.StopAllCommands();
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
