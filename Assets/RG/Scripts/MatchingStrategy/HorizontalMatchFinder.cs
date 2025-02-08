using System;
using System.Collections.Generic;
using UnityEngine;

public interface IMatchFinder//Strategy Design Pattern
{
    List<Tile> FindMatches(Tile[,] grid, List<Vector2Int> affectedPositions);
}

public class HorizontalMatchFinder : IMatchFinder
{
    private int matchLength;

    public HorizontalMatchFinder(int matchLength = 3)
    {
        this.matchLength = matchLength;
    }

    public List<Tile> FindMatches(Tile[,] grid, List<Vector2Int> affectedPositions)
    {
        List<List<Tile>> matches = new List<List<Tile>>();

        foreach (var pos in affectedPositions)
        {
            List<Tile> match = CheckHorizontalMatch(grid, pos);

            if (match.Count >= matchLength)
            {
                return match;
            }
        }
        return null;
    }

    private List<Tile> CheckHorizontalMatch(Tile[,] grid, Vector2Int startPos)
    {
        List<Tile> match = new List<Tile>();
        int width = grid.GetLength(0);
        Tile startTile = grid[startPos.x, startPos.y];
        if (startTile == null) return match;

        match.Add(startTile);
        int tileId = startTile.Id;

        for (int x = startPos.x - 1; x >= 0; x--)
        {
            if (grid[x, startPos.y] != null && grid[x, startPos.y].Id == tileId)
                match.Insert(0, grid[x, startPos.y]);
            else
                break;
        }

        for (int x = startPos.x + 1; x < width; x++)
        {
            if (grid[x, startPos.y] != null && grid[x, startPos.y].Id == tileId)
                match.Add(grid[x, startPos.y]);
            else
                break;
        }

        return match;
    }
}
