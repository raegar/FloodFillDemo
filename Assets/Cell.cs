using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public bool isMine;
    public bool isRevealed = false;

    private FloodFillGrid gridManager;
    private SpriteRenderer spriteRenderer;

    public void Init(int x, int y, bool isMine, FloodFillGrid gridManager)
    {
        this.x = x;
        this.y = y;
        this.isMine = isMine;
        this.gridManager = gridManager;

        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.AddComponent<BoxCollider2D>();
    }

    public void OnMouseDown()
    {
        if (isMine)
        {
            // Turn mine cell red
            SetColor(Color.red);
        }
        else
        {
            // Start flood fill from this cell
            gridManager.FloodFill(x, y);
        }
    }

    public void Reveal()
    {
        isRevealed = true;
        SetColor(Color.grey); // Change the color when revealed
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}