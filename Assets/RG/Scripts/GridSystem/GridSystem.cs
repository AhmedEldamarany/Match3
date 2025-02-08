using System.Collections;

using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private int height, width;
    [SerializeField] private TileFactory tileFactory;

    private IMatchFinder matchFinder;

    private GridController gridController;
    private TileAnimationService tileAnimationService;
    private TileShiftService tileShiftService;
    private MatchHandler matchHandler;

    private bool isInteractable = true;
    void Start()
    {
        // Create the match finder (or assign via inspector if you prefer)
        matchFinder = new HorizontalMatchFinder();

        // Initialize helper classes.
        gridController = new GridController(width, height, tileFactory, transform);
        tileAnimationService = new TileAnimationService();
        tileShiftService = new TileShiftService();
        matchHandler = new MatchHandler(matchFinder);
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
    { if(!isInteractable) return;
        Vector2Int tilePosition = tile.position;
        // A tile with a negative position is considered “invalid”
        if (tilePosition != Vector2Int.one * -1)
        {
            StartCoroutine(HandleTileClick(tile, tilePosition));
            isInteractable = false;
        }
    }

    private IEnumerator HandleTileClick(Tile tile, Vector2Int position)
    {
        yield return StartCoroutine(tileAnimationService.ScaleDownTile(tile));
        gridController.RemoveTileAt(position);

        yield return StartCoroutine(tileShiftService.ShiftTilesDownWithAnimation(
            gridController.Grid, position, height, this, tileAnimationService));

        yield return StartCoroutine(matchHandler.CheckAndHandleMatches(
            gridController, position, height, this, tileShiftService, tileAnimationService));
        isInteractable = true;
    }
}
