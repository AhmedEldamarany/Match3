using System;

using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int height, width;
    [SerializeField] private float paddingx, paddingy;
    [SerializeField] private Tile TilePrefab;
    private Tile[,] grid;
    void Start()
    {
        grid = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
              grid[i,j]= Instantiate(TilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                grid[i, j].AssignColor(Color.green);
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
        Debug.Log($"Tile clicked {tile.transform.position.x } {tile.transform.position.y}");
    }

    
}
