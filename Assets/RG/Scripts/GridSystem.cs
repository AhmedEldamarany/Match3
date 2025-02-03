using System;

using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int height, width;
    [SerializeField] private float paddingx, paddingy;
    [SerializeField] private TileFactory tileFactory;
    private Tile[,] grid;

    void Start()
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
        Debug.Log($"Tile clicked {tile.transform.position.x} {tile.transform.position.y}");
    }
}
