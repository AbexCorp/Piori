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

    //Inspector References

    //Code References

    //Fields & Properties
    [SerializeField]
    [Range(1f, 3f)]
    private float _moveSpeed = 1;

    private Vector2 _movement;


    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        gameObject.GetComponent<Rigidbody2D>().velocity = _movement * _moveSpeed;
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
}
