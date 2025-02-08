using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHandler
{
    private IMatchFinder matchFinder;

    public MatchHandler(IMatchFinder matchFinder)
    {
        this.matchFinder = matchFinder;
    }

    public IEnumerator CheckAndHandleMatches(GridController gridController, Vector2Int position, int gridHeight,
                                               MonoBehaviour context, TileShiftService tileShiftService, TileAnimationService tileAnimationService)
    {
        List<Vector2Int> affectedPositions = gridController.GetAffectedPositions(position);
        List<Tile> match = matchFinder.FindMatches(gridController.Grid, affectedPositions);
        if (match != null)
        {
            yield return context.StartCoroutine(RemoveMatches(match, gridController, tileAnimationService));
            yield return context.StartCoroutine(tileShiftService.ShiftMultipleTilesDownWithAnimation(
                gridController.Grid, match.ToArray(), gridHeight, context, tileAnimationService));
        }
    }

    private IEnumerator RemoveMatches(List<Tile> match, GridController gridController, TileAnimationService tileAnimationService)
    {
        EventManager.Raise(ActionType.TileMatched,true);
        yield return tileAnimationService.ScaleDownMultipleTiles(match.ToArray(), gridController);
    }
}
