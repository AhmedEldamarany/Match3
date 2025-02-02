using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    public void AssignColor(Color color)
    {      
       spriteRenderer.color= color;
      
    }
    void OnMouseDown()
    {
        EventManager.Raise(ActionType.TileClicked, this);
        Debug.Log("Mouse button pressed on the tile.");
        AssignColor(Color.black);
    }

   
}
