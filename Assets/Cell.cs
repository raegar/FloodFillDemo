using UnityEngine;

public class Cell : MonoBehaviour
{
    public int x, y;
    public bool isMine;
    public bool isRevealed = false;

    private FloodFillGrid gridManager;
    private SpriteRenderer spriteRenderer;
    private TextMesh textMesh; // TextMesh component for displaying the number

    public void Init(int x, int y, bool isMine, FloodFillGrid gridManager)
    {
        this.x = x;
        this.y = y;
        this.isMine = isMine;
        this.gridManager = gridManager;

        spriteRenderer = GetComponent<SpriteRenderer>();
        gameObject.AddComponent<BoxCollider2D>();

        // Add a TextMesh component for displaying numbers on the cell
        textMesh = new GameObject("Text").AddComponent<TextMesh>();
        textMesh.transform.SetParent(transform); // Set TextMesh as a child of the cell
        textMesh.transform.localPosition = Vector3.zero; // Center it on the cell
        textMesh.alignment = TextAlignment.Center;
        textMesh.anchor = TextAnchor.MiddleCenter;
        textMesh.characterSize = 0.2f; // Adjust the size as needed
        textMesh.color = Color.white;
        textMesh.text = ""; // Start with an empty string
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

        // Get the number of adjacent mines
        int adjacentMines = gridManager.GetAdjacentMineCount(x, y);

        // Display the number if there are any adjacent mines
        if (adjacentMines > 0)
        {
            textMesh.text = adjacentMines.ToString();
        }
    }

    public void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }
}
