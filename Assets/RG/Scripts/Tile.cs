using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public int Id { get; private set; }

    public void Initialize(int id, Sprite sprite)
    {
        Id = id;
        spriteRenderer.sprite = sprite;
    }

    public void AssignColor(Color color)
    {
        spriteRenderer.color = color;
    }

    void OnMouseDown()
    {
        EventManager.Raise(ActionType.TileClicked, this);
        Debug.Log("Mouse button pressed on the tile.");
        AssignColor(Color.black);
    }

   
}
