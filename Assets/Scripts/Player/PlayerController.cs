using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    //Components
    [SerializeField]
    private SpriteRenderer _spriteRenderer;
    [SerializeField]
    private Rigidbody2D _rigidBody;

    //Inspector References

    //Code References

    //Fields & Properties
    [SerializeField]
    [Range(1f, 3f)]
    private float _moveSpeed = 1;
    [HideInInspector]
    public Vector2 Position => transform.position;

    private Vector2 _movement;


    private void Awake()
    {
        Instance = this;
        if(_spriteRenderer == null)
            _spriteRenderer = GetComponent<SpriteRenderer>();
        if( _rigidBody == null)
            _rigidBody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        _rigidBody.velocity = _movement * _moveSpeed;
        UpdateGridPosition();
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        _movement = context.ReadValue<Vector2>();
    }

    public void Spawn()
    {
        Tile tileToSpawn = GridManager.Instance.GetTileAtPosition(GridManager.Instance.LoadedMap.SpawnPosition);
        if(tileToSpawn == null || tileToSpawn.IsWalkable == false)
        {
            Debug.LogWarning("The set map spawn point was not suitable, trying to use a random tile");
            tileToSpawn = GridManager.Instance.GetRandomWalkableTile();
            if(tileToSpawn == null)
            {
                Debug.LogError("No suitable tile for spawn was found on this map");
                return;
            }
        }

        gameObject.transform.position = tileToSpawn.transform.position;
        Physics2D.SyncTransforms();
    }

    private void UpdateGridPosition()
    {
        RaycastHit2D hit = Physics2D.Raycast(origin:transform.position, direction:Vector2.zero, distance:1, layerMask:LayerMask.GetMask("Tiles"));
        if (hit)
        {
            hit.collider.TryGetComponent<Tile>(out Tile tile);
            if (tile == null)
                return;
            GridManager.Instance.SetPlayerTile(tile);
        }
    }
}
