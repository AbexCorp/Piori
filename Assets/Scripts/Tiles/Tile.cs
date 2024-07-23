using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Tile : MonoBehaviour
{
    [SerializeField]
    protected Color _primaryColor;
    [SerializeField]
    protected Color _secondaryColor;
    [SerializeField]
    protected SpriteRenderer _spriteRenderer;

    [SerializeField]
    protected GameObject _highlight;

    [SerializeField]
    protected bool _isWalkable;


    public BaseUnit OccupiedUnit;
    public bool Walkable => _isWalkable && OccupiedUnit == null;
    public string TileName;


    public virtual void Init(int x, int y)
    {
        bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        _spriteRenderer.color = isOffset ? _primaryColor : _secondaryColor;
    }

    public void SetUnit(BaseUnit unit)
    {
        if(unit.OccupiedTile != null)
            unit.OccupiedTile.OccupiedUnit = null;

        unit.transform.position = transform.position;
        OccupiedUnit = unit;
        unit.OccupiedTile = this;
    }

    void OnMouseEnter()
    {
        _highlight.SetActive(true);
        MenuManager.Instance.ShowTileInfo(this);
    }
    void OnMouseExit()
    {
        _highlight.SetActive(false);
        MenuManager.Instance.ShowTileInfo(null);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.GameState != GameState.HeroesTurn)
            return;

        if(OccupiedUnit != null) //Something exists on clicked tile
        {
            if (OccupiedUnit.Faction == Faction.Hero) //Something clicked is a hero
                UnitManager.Instance.SetSelectedHero((BaseHero)OccupiedUnit);
            else //Its an enemy
            {
                if (UnitManager.Instance.SelectedHero != null) //If you have hero selected already then kill
                {
                    var enemy = (BaseEnemy)OccupiedUnit;
                    Destroy(enemy.gameObject);
                    UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }
        else //Nothing is on tile
        {
            if(UnitManager.Instance.SelectedHero != null) //But already have hero selected then move
            {
                if (Walkable) //Not a wall
                {
                    SetUnit(UnitManager.Instance.SelectedHero);
                    UnitManager.Instance.SetSelectedHero(null);
                }
            }
        }
    }
}
