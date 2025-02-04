using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int height, width;
    [SerializeField] private TileFactory tileFactory;
    private Tile[,] grid;
    private IMatchFinder matchFinder;

    void Start()
    {
        matchFinder = new HorizontalMatchFinder();
        InitializeGrid();
    }

    private void InitializeGrid()
    {
        grid = new Tile[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                int tileId = UnityEngine.Random.Range(0, tileFactory.NumberOfTileTypes);
                grid[i, j] = tileFactory.CreateTile(new Vector3(i, j, 0), tileId, transform);
                grid[i, j].position = new Vector2Int(i, j);
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
            StartCoroutine(RemoveTileWithAnimation(tile, tilePosition));
        }
    }

    private IEnumerator RemoveTileWithAnimation(Tile tile, Vector2Int position)
    {
        yield return StartCoroutine(ScaleDownTile(tile));
        RemoveTile(position);
        yield return StartCoroutine(ShiftTilesDownWithAnimation(position));
        yield return new WaitForSeconds(0.5f); // Ensuring animations settle
        List<Vector2Int> affectedPositions = GetAffectedPositions(position);
        var matches = matchFinder.FindMatches(grid, affectedPositions);
        if (matches.Count > 0)
        {
            foreach (var match in matches)
            {
                foreach (var matchedTile in match)
                {
                    StartCoroutine(RemoveTileWithAnimation(matchedTile, matchedTile.position));
                }
            }
        }
    }

    private IEnumerator ScaleDownTile(Tile tile)
    {
        float duration = 0.5f;
        Vector3 originalScale = tile.transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scale = EasingFunctions.EaseInOutQuad(t);
            tile.transform.localScale = Vector3.Lerp(originalScale, targetScale, scale);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tile.transform.localScale = targetScale;
    }

    private void RemoveTile(Vector2Int position)
    {
        Destroy(grid[position.x, position.y].gameObject);
        grid[position.x, position.y] = null;
    }

    private IEnumerator ShiftTilesDownWithAnimation(Vector2Int position)
    {
        int maxY = height - 1;
        List<Vector2Int> affectedPositions = new List<Vector2Int>();

        for (int y = position.y; y < height - 1; y++)
        {
            if (grid[position.x, y + 1] != null)
            {
                grid[position.x, y] = grid[position.x, y + 1];
                if (grid[position.x, y] != null)
                {
                    StartCoroutine(MoveTileWithBounce(grid[position.x, y], new Vector3(position.x, y, 0)));
                    grid[position.x, y].position = new Vector2Int(position.x, y);
                    affectedPositions.Add(grid[position.x, y].position);
                }
            }
            else
            {
                maxY = y;
                break;
            }
        }
        grid[position.x, maxY] = null;

        yield return new WaitForSeconds(0.5f); // Ensure animations finish
    }

    private IEnumerator MoveTileWithBounce(Tile tile, Vector3 targetPosition)
    {
        float duration = 0.5f;
        Vector3 originalPosition = tile.transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float bounce = EasingFunctions.EaseOutBounce(t);
            tile.transform.position = Vector3.Lerp(originalPosition, targetPosition, bounce);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        tile.transform.position = targetPosition;
    }

    private List<Vector2Int> GetAffectedPositions(Vector2Int removedTilePosition)
    {
        List<Vector2Int> affectedPositions = new List<Vector2Int>();
        for (int y = removedTilePosition.y; y < height; y++)
        {
            if (grid[removedTilePosition.x, y] != null)
            {
                affectedPositions.Add(new Vector2Int(removedTilePosition.x, y));
            }
        }
        return affectedPositions;
    }
}
