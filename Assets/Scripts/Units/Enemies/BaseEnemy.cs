using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public ScriptableEnemy ScriptableEnemy;


    //Components
    [SerializeField]
    protected Rigidbody2D _rigidBody;
    [SerializeField]
    protected CircleCollider2D _collider;

    [SerializeField]
    protected GameObject _canvas;
    [SerializeField]
    protected UnityEngine.UI.Image _healthBar;

    //Inspector References


    //Code References


    //Fields & Properties
    public string Name => ScriptableEnemy.Name;
    public int PointCost => ScriptableEnemy.PointCost;
    public int CurrencyOnKill => ScriptableEnemy.CurrencyOnKill;
    public float Speed => ScriptableEnemy.Speed;
    public int Health => ScriptableEnemy.Health;
    public int CurrentHealth => _currentHealth;
    public int Damage => ScriptableEnemy.Damage;
    public float AttackSpeed => ScriptableEnemy.AttackSpeed;
    public float AttackRange => ScriptableEnemy.AttackRange;
    public bool AttackIsRanged => ScriptableEnemy.AttackIsRanged;
    public bool StopsMovingOnAttackCooldown => ScriptableEnemy.StopsMovingOnAttackCooldown;

    //Pathfinding
    protected float _pathfindingFrequency = 0.25f;
    protected List<NavigationNode> _pathToPlayer = new();
    protected Tile _position;
    protected Vector2? _destination = null;
    protected float _distanceToPlayer;
    protected bool _playerMoved = true;
    protected bool _mapChanged = true;
    protected bool _switchedTile = true;

    //Combat
    protected float _attackCheckFrequency = 0.05f;
    protected int _currentHealth = 1;
    protected bool _attackIsOnCooldown = false;


    protected void Awake()
    {
        if( _rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();
        if(_collider == null)
            _collider = GetComponent<CircleCollider2D>();

        _obstacleLayer = LayerMask.GetMask("ObstacleFull", "ObstacleHalf");
        GridManager.Instance.OnPlayerChangeTile += PlayerMoved;
        GridManager.Instance.OnMapChange += MapChanged;

        _currentHealth = Health;
    }
    protected void Start()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.Position);
        StartCoroutine(Pathfind());
        StartCoroutine(TargetPlayer());
        _canvas.SetActive(false);
    }
    protected void Update()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.Position);
        Move();
    }


    #region >>> Pathfinding <<<

    protected int _obstacleLayer = 0;

    protected IEnumerator Pathfind()
    {
        YieldInstruction yield = new WaitForSeconds(_pathfindingFrequency);
        while (true)
        {
            FindPath();
            yield return yield;
        }
    }

    protected void FindPath()
    {
        //Fix second raycast. Try to raycast to player tile or reduce the radius to stop collisions with walls touched by player
        if (TryDirectPath())
            return;
        //Todo: Improve pathfinding by adding current tile to path. Right now the enemies move fine when continuing to walk on path, but get stuck
        //on walls while cutting corners when using a new path. Cause is not including the current tile when more than half way on the corner tile. 
        if (TryComplexPath())
            return;
        if (TryContinueComplexPath())
            return;

        _destination = null;
    }
    protected bool TryDirectPath()
    {
        if(_distanceToPlayer <= 1)
        {
            _destination = PlayerController.Instance.Position;
            return true;
        }

        Vector2 direction = GridManager.Instance.PlayerTile.TileCoordinates.WorldPosition - (Vector2)gameObject.transform.position;
        float distance = Vector2.Distance(gameObject.transform.position, GridManager.Instance.PlayerTile.TileCoordinates.WorldPosition);

        RaycastHit2D hit1 = Physics2D.Raycast(origin: gameObject.transform.position, direction: direction, layerMask: _obstacleLayer, distance: distance);
        if (hit1 == false)
        {
            RaycastHit2D hit2 = Physics2D.CircleCast(origin: gameObject.transform.position, radius: _collider.radius,
                direction: direction, distance: distance, layerMask: _obstacleLayer);
            if (hit2 == false)
            {
                _destination = GridManager.Instance.PlayerTile.TileCoordinates.WorldPosition;
                return true;
            }
        }

        return false;
    }
    protected bool TryComplexPath()
    {
        if(_distanceToPlayer <= 1)
            return false;
        UpdateGridPosition();
        if (_position == null || GridManager.Instance.PlayerTile == null)
            return false;

        bool wasSearchingforPath = false;
        if (_pathToPlayer == null || _pathToPlayer.Count == 0 || _playerMoved || _mapChanged)
        {
            _pathToPlayer = Pathfinding.FindPath(_position.NavigationNode, GridManager.Instance.PlayerTile.NavigationNode);
            _playerMoved = false;
            _mapChanged = false;
            wasSearchingforPath = true;
        }
        if (_pathToPlayer != null && _pathToPlayer.Count > 0 && wasSearchingforPath)
        {
            _destination = _pathToPlayer.Last().Tile.TileCoordinates.WorldPosition;
            return true;
        }

        return false;
    }
    protected bool TryContinueComplexPath()
    {
        if(_pathToPlayer == null)
            return false;
        if(_switchedTile && Vector2.Distance(transform.position, _pathToPlayer.Last().Tile.TileCoordinates.WorldPosition) < 0.2f)
        {
            _pathToPlayer.Remove(_pathToPlayer.Last());
            if (_pathToPlayer.Count == 0)
                return false;
            _destination = _pathToPlayer.Last().Tile.TileCoordinates.WorldPosition;
            _switchedTile = false;
            return true;
        }
        return false;
    }

    protected void Move()
    {
        if(_destination == null)
        {
            _rigidBody.velocity += Vector2.zero;
            return;
        }
        Vector2 direction = ((Vector2)_destination - (Vector2)transform.position).normalized;
        _rigidBody.velocity = direction * Speed;
    }

    protected void UpdateGridPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(origin:transform.position, direction:Vector2.zero, distance:1, layerMask:LayerMask.GetMask("Tiles"));
        if (hit)
        {
            hit.collider.TryGetComponent<Tile>(out Tile tile);
            if (tile == null)
                return;
            if (tile != _position)
                _switchedTile = true;
            _position = tile;
        }
    }
    private void PlayerMoved() => _playerMoved = true;
    private void MapChanged() => _mapChanged = true;

    #endregion


    #region >>> Combat <<<
    
    public void GetDamaged(int damage)
    {
        if (_currentHealth <= 0)
            return;
        _currentHealth -= damage;
        if (_currentHealth <= 0)
            Die();
        UpdateHealthBar();
    }
    protected void UpdateHealthBar()
    {
        _canvas.SetActive(true);
        _healthBar.fillAmount = CurrentHealth / (float)Health;
    }
    protected void Die()
    {
        UnitManager.Instance.SpawnedEnemies.Remove(this);
        TowerManager.Instance.ChangeCurrency(CurrencyOnKill);
        Destroy(gameObject);
    }
    private IEnumerator TargetPlayer()
    {
        YieldInstruction yield = new WaitForSeconds(1);
        while (true)
        {
            if (_distanceToPlayer < AttackRange && !_attackIsOnCooldown)
                AttackPlayer();

            yield return yield;
        }
    }
    private void AttackPlayer()
    {
        if (_attackIsOnCooldown || _distanceToPlayer > AttackRange)
            return;
        PlayerController.Instance.DamagePlayer(Damage);
        _attackIsOnCooldown = true;
        StartCoroutine(AttackCooldown());
    }
    private IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(AttackSpeed);
        _attackIsOnCooldown = false;
    }

    #endregion


    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (_pathToPlayer == null)
            return;

        for (int i = 0; i < _pathToPlayer.Count - 1; i++)
        {
            var p1 = _pathToPlayer[i].Tile.TileCoordinates.WorldPosition;
            var p2 = _pathToPlayer[i + 1].Tile.TileCoordinates.WorldPosition;
            var thickness = 8;
            Handles.DrawBezier(p1, p2, p1, p2, Color.red, null, thickness);
        }
    }
    #endif
}
