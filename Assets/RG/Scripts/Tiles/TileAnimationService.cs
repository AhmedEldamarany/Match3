using System.Collections;

using UnityEngine;

public class TileAnimationService
{
    public IEnumerator ScaleDownTile(Tile tile)
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

    public IEnumerator ScaleDownMultipleTiles(Tile[] tiles, GridController gridController)
    {
        float duration = 0.5f;
        if (tiles.Length == 0) yield break;
        Vector3 originalScale = tiles[0].transform.localScale;
        Vector3 targetScale = Vector3.zero;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float scale = EasingFunctions.EaseInOutQuad(t);
            foreach (var tile in tiles)
            {
                tile.transform.localScale = Vector3.Lerp(originalScale, targetScale, scale);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        foreach (var tile in tiles)
        {
            tile.transform.localScale = targetScale;
            gridController.RemoveTileAt(tile.position);
        }
    }

    public IEnumerator MoveColumnWithBounce(Tile[,] grid, int column, int startRow, int endRow, float duration = 0.5f)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float bounce = EasingFunctions.EaseOutBounce(t);
            for (int y = startRow; y <= endRow; y++)
            {
                if (grid[column, y] != null)
                {
                    Vector3 originalPosition = new Vector3(column, y + 1, 0);
                    Vector3 targetPosition = new Vector3(column, y, 0);
                    grid[column, y].transform.position = Vector3.Lerp(originalPosition, targetPosition, bounce);
                }
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        for (int y = startRow; y <= endRow; y++)
        {
            if (grid[column, y] != null)
            {
                grid[column, y].transform.position = new Vector3(column, y, 0);
            }
        }
    }
}
