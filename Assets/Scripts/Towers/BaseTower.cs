using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTower : MonoBehaviour
{
    //Components
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;

    //Inspector References
    [SerializeField]
    protected ScriptableTower _scriptableTower;

    //Code References
    public Tile OccupiedTile { get; protected set; }

    //Properties
    public ScriptableTower ScriptableTower { get { return _scriptableTower; } }


    public void SetOccupiedTile(Tile tile)
    {
        OccupiedTile = tile;
    }
}
