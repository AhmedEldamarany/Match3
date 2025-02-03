using UnityEngine;

public class TileFactory : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileSprites tileSprites;
    public int NumberOfTileTypes => tileSprites.sprites.Length;
    public Tile CreateTile(Vector3 position, int id, Transform parent = null)
    {
        Tile tile = Instantiate(tilePrefab, position, Quaternion.identity, parent);
        if (id >= 0 && id < tileSprites.sprites.Length)
        {
            tile.Initialize(id, tileSprites.sprites[id]);
        }
        else
        {
            Debug.LogWarning("Invalid tile ID, using default sprite.");
            tile.Initialize(id, null);
        }
        return tile;
    }
}
