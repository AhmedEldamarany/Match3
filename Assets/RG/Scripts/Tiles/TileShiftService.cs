using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TileShiftService
{
    public IEnumerator ShiftTilesDownWithAnimation(Tile[,] grid, Vector2Int position, int gridHeight, MonoBehaviour context, TileAnimationService animationService)
    {
        int maxY = gridHeight - 1;
        List<Vector2Int> affectedPositions = new List<Vector2Int>();

        for (int y = position.y; y < gridHeight - 1; y++)
        {
            if (grid[position.x, y + 1] != null)
            {
                grid[position.x, y] = grid[position.x, y + 1];
                if (grid[position.x, y] != null)
                {
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

        yield return context.StartCoroutine(animationService.MoveColumnWithBounce(grid, position.x, position.y, maxY));
    }

    public IEnumerator ShiftMultipleTilesDownWithAnimation(Tile[,] grid, Tile[] matchedTiles, int gridHeight, MonoBehaviour context, TileAnimationService animationService)
    {
        Dictionary<int, int> maxYDict = new Dictionary<int, int>();
        List<Vector2Int> affectedPositions = new List<Vector2Int>();

        foreach (var mTile in matchedTiles)
        {
            int x = mTile.position.x;
            int maxY = gridHeight - 1;

            for (int y = mTile.position.y; y < gridHeight - 1; y++)
            {
                if (grid[x, y + 1] != null)
                {
                    grid[x, y] = grid[x, y + 1];
                    if (grid[x, y] != null)
                    {
                        grid[x, y].position = new Vector2Int(x, y);
                        affectedPositions.Add(grid[x, y].position);
                    }
                }
                else
                {
                    maxY = y;
                    break;
                }
            }
            grid[x, maxY] = null;
            maxYDict[x] = maxY;
        }

        // Start bounce animations for each affected column.
        foreach (var kvp in maxYDict)
        {
            context.StartCoroutine(animationService.MoveColumnWithBounce(grid, kvp.Key, matchedTiles[0].position.y, kvp.Value));
        }
        yield return new WaitForSeconds(1f);
    }
}
