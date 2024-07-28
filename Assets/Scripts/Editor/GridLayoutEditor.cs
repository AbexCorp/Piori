using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class GridLayoutEditor : EditorWindow
{
    private GridLayoutScript _gridLayout;
    private TileType _currentPaintType = TileType.Ground;
    private bool _isPainting = false;

    [MenuItem("Tools/Grid Layout Editor")]
    public static void ShowWindow()
    {
        GetWindow<GridLayoutEditor>("Grid Layout Editor");
    }

    private void OnEnable()
    {
        LoadTiles();
    }

    private void OnGUI()
    {
        LoadTiles();
        _gridLayout = (GridLayoutScript)EditorGUILayout.ObjectField("Grid Layout", _gridLayout, typeof(GridLayoutScript), false);

        if (_gridLayout == null)
        {
            if (GUILayout.Button("Create New Layout"))
            {
                _gridLayout = CreateInstance<GridLayoutScript>();
                _gridLayout.Width = 10; // Default values
                _gridLayout.Height = 10; // Default values
                _gridLayout.tiles = new TileType[_gridLayout.Width * _gridLayout.Height];
            }
            return;
        }

        _gridLayout.Width = EditorGUILayout.IntField("Width", _gridLayout.Width);
        _gridLayout.Height = EditorGUILayout.IntField("Height", _gridLayout.Height);

        if (_gridLayout.tiles == null || _gridLayout.tiles.Length != _gridLayout.Width * _gridLayout.Height)
        {
            _gridLayout.tiles = new TileType[_gridLayout.Width * _gridLayout.Height];
        }

        // Paint type selector
        _currentPaintType = (TileType)EditorGUILayout.EnumPopup("Paint Type", _currentPaintType);

        // Calculate the size of each tile to ensure they are square
        float availableWidth = EditorGUIUtility.currentViewWidth - 40; // 40 for padding and margins
        float availableHeight = position.height - 150; // 150 for controls and padding
        float tileSize = Mathf.Min(availableWidth / _gridLayout.Width, availableHeight / _gridLayout.Height);

        // Draw the grid
        for (int y = 0; y < _gridLayout.Height; y++)
        {
            GUILayout.BeginHorizontal();
            for (int x = 0; x < _gridLayout.Width; x++)
            {
                Rect tileRect = GUILayoutUtility.GetRect(tileSize, tileSize);
                EditorGUI.DrawRect(tileRect, GetColorForTileType(_gridLayout.tiles[y * _gridLayout.Width + x]));

                if (tileRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                    {
                        _isPainting = true;
                        _gridLayout.tiles[y * _gridLayout.Width + x] = _currentPaintType;
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.MouseDrag && _isPainting)
                    {
                        _gridLayout.tiles[y * _gridLayout.Width + x] = _currentPaintType;
                        Event.current.Use();
                    }
                    else if (Event.current.type == EventType.MouseUp && Event.current.button == 0)
                    {
                        _isPainting = false;
                    }
                }
            }
            GUILayout.EndHorizontal();
        }

        if (GUILayout.Button("Save Layout"))
        {
            string path = EditorUtility.SaveFilePanelInProject("Save Grid Layout", "NewGridLayout", "asset", "Save Grid Layout");
            if (path != "")
            {
                AssetDatabase.CreateAsset(_gridLayout, path);
                AssetDatabase.SaveAssets();
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_gridLayout);
        }
    }

    private Dictionary<TileType, ScriptableTile> _scriptedTiles = new();
    private Color GetColorForTileType(TileType tileType)
    {
        _scriptedTiles.TryGetValue(tileType, out ScriptableTile tile);
        if (tile == null)
            return Color.white;
        return tile.TileColor;
    }
    private void LoadTiles()
    {
        List<ScriptableTile> tiles = Resources.LoadAll<ScriptableTile>("Tiles").ToList();
        foreach(var tile in tiles)
        {
            if (_scriptedTiles.ContainsKey(tile.TileType))
                continue;
            _scriptedTiles[tile.TileType] = tile;
        }
    }
    //_units = Resources.LoadAll<ScriptableUnit>("Units").ToList(); //Load all sunits from resource folder
    //return (T)_units.Where(u => u.Faction == faction).OrderBy(o => Random.value).First().UnitPrefab;
}