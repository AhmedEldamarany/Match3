using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int Id { get; private set; }
    public Vector2Int position;
    public void Initialize(int id, Sprite sprite,Vector2Int position)
    {
        Id = id;
       this.position= position;
        spriteRenderer.sprite = sprite;
    }

   

    void OnMouseDown()
    {
        EventManager.Raise(ActionType.TileClicked, this);
    }

   
}
