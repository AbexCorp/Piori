using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    //Components
    [SerializeField]
    protected Rigidbody2D _rigidBody;

    //Inspector References
    public ScriptableEnemy ScriptableEnemy;


    //Code References


    //Fields & Properties
    public string Name => ScriptableEnemy.Name;
    public int PointCost => ScriptableEnemy.PointCost;
    public float Speed => ScriptableEnemy.Speed;
    public int Health => ScriptableEnemy.Health;
    public int Damage => ScriptableEnemy.Damage;
    public float AttackSpeed => ScriptableEnemy.AttackSpeed;

    protected Tile _position;
    protected Vector2 _destination;
    protected float _distanceToPlayer;


    protected void Awake()
    {
        if( _rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();
    }
    protected void Update()
    {
        _distanceToPlayer = Vector2.Distance(transform.position, PlayerController.Instance.Position);
        FindPath();
        Move();
    }
    protected void FindPath()
    {
        if (_distanceToPlayer > 1)
        {
            UpdateGridPosition();
            if (_position == null || GridManager.Instance.PlayerTile == null)
                return;

            //List<NavigationNode> path = Pathfinding.FindPath(_position.NavigationNode, GridManager.Instance.PlayerTile.NavigationNode);
            path = Pathfinding.FindPath(_position.NavigationNode, GridManager.Instance.PlayerTile.NavigationNode);
            //Debug.Log(path.Last().Tile.gameObject.name);
            //Debug.Log(path.Last().Tile.TileCoordinates.Position);
            //Debug.Break();
            _destination = path.Last().Tile.TileCoordinates.Position;
        }
        else if (_distanceToPlayer <= 1)
            _destination = PlayerController.Instance.Position;
    }
    protected void Move()
    {
        Vector2 direction = (_destination - (Vector2)transform.position).normalized;
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
            _position = tile;
        }
    }

    List<NavigationNode> path = new();
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        if (path == null)
            return;

        for (int i = 0; i < path.Count - 1; i++)
        {
            var p1 = path[i].Tile.TileCoordinates.Position;
            var p2 = path[i + 1].Tile.TileCoordinates.Position;
            var thickness = 30;
            Handles.DrawBezier(p1, p2, p1, p2, Color.white, null, thickness);
        }
    }
}
