using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    [SerializeField]
    private GameObject _selectedHeroObject;
    [SerializeField]
    private GameObject _tileObject;
    [SerializeField]
    private GameObject _unitObject;

    private void Awake()
    {
        Instance = this;
    }

    public void ShowSelectedHero(BaseHero hero)
    {
        if(hero == null)
        {
            _selectedHeroObject.SetActive(false);
            return;
        }

        _selectedHeroObject.GetComponentInChildren<Text>().text = hero.UnitName;
        _selectedHeroObject.SetActive(true);
    }
    public void ShowTileInfo(Tile tile)
    {
        if(tile == null)
        {
            _tileObject.SetActive(false);
            _unitObject.SetActive(false);
            return;
        }

        _tileObject.GetComponentInChildren<Text>().text = tile.TileName;
        _tileObject.SetActive(true);

        if(tile.OccupiedUnit)
        {
            _unitObject.GetComponentInChildren<Text>().text = tile.OccupiedUnit.UnitName;
            _unitObject.SetActive(true);
        }
    }
}
