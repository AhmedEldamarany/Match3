using UnityEngine;

public class TileFactory : MonoBehaviour
{
    [SerializeField] private Tile tilePrefab;
    [SerializeField] private TileSpritesSO tileSprites;
    public int NumberOfTileTypes => tileSprites.sprites.Length;
    public Tile CreateTile(Vector3 position, int id, Transform parent = null)
    {
        Tile tile = Instantiate(tilePrefab, position, Quaternion.identity, parent);
        
            tile.Initialize(id, tileSprites.sprites[id],new Vector2Int((int)position.x,(int)position.y));
        
        
        return tile;
    }
}
