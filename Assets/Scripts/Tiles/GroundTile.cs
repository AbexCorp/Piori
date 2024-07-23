using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundTile : Tile
{
    public override void Init(int x, int y)
    {
        bool isOffset = (x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0);
        _spriteRenderer.color = isOffset ? _primaryColor : _secondaryColor;
    }
}
