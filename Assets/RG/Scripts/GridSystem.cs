using System;

using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int height, width;
    [SerializeField] private TileFactory tileFactory;
    private Tile[,] grid;

    void Start()
    {
        IntializeGrid();
    }

    private void IntializeGrid()
    {
        grid = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int tileId = UnityEngine.Random.Range(0, tileFactory.NumberOfTileTypes);
                grid[i, j] = tileFactory.CreateTile(new Vector3(i, j, 0), tileId, transform);
            }
        }
    }

    private void OnEnable()
    {
        EventManager.Subscribe<Tile>(ActionType.TileClicked, OnTileClicked);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe<Tile>(ActionType.TileClicked, OnTileClicked);
    }

    private void OnTileClicked(Tile tile)
    {
        Vector2Int tilePosition = tile.position;
        if (tilePosition != Vector2Int.one * -1)
        {
            RemoveTile(tilePosition);
            ShiftTilesDown(tilePosition);
        }
    }

    

    private void RemoveTile(Vector2Int position)
    {
        Destroy(grid[position.x, position.y].gameObject);
        grid[position.x, position.y] = null;
    }

    private void ShiftTilesDown(Vector2Int position)
    {
        for (int y = position.y; y < height - 1; y++)
        {
            grid[position.x, y] = grid[position.x, y + 1];
            if (grid[position.x, y] != null)
            {
                grid[position.x, y].transform.position = new Vector3(position.x, y, 0);
                grid[position.x, y].position = new Vector2Int(position.x, y);
            }
        }
        grid[position.x, height - 1] = null;
    }
}
