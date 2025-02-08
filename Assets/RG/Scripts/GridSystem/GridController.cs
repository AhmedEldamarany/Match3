using System.Collections.Generic;

using UnityEngine;

public class GridController
{
    public Tile[,] Grid { get; private set; }

    private int width;
    private int height;
    private TileFactory tileFactory;
    private Transform parent;

    public GridController(int width, int height, TileFactory tileFactory, Transform parent)
    {
        this.width = width;
        this.height = height;
        this.tileFactory = tileFactory;
        this.parent = parent;
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        Grid = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int tileId = UnityEngine.Random.Range(0, tileFactory.NumberOfTileTypes);
                Tile tile = tileFactory.CreateTile(new Vector3(i, j, 0), tileId, parent);
                tile.position = new Vector2Int(i, j);
                Grid[i, j] = tile;
            }
        }
    }

    public void RemoveTileAt(Vector2Int position)
    {
        if (Grid[position.x, position.y] != null)
        {
            GameObject.Destroy(Grid[position.x, position.y].gameObject);
            Grid[position.x, position.y] = null;
        }
    }

    public List<Vector2Int> GetAffectedPositions(Vector2Int removedTilePosition)
    {
        List<Vector2Int> affectedPositions = new List<Vector2Int>();
        for (int y = removedTilePosition.y; y < height; y++)
        {
            if (Grid[removedTilePosition.x, y] != null)
            {
                affectedPositions.Add(new Vector2Int(removedTilePosition.x, y));
            }
        }
        return affectedPositions;
    }
}
