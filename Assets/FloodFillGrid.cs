using System.Collections.Generic;
using UnityEngine;

public class FloodFillGrid : MonoBehaviour
{
    public GameObject cellPrefab; // Assign the cell sprite prefab here
    public int gridWidth = 10;
    public int gridHeight = 10;
    public float cellSize = 1.1f; // Space between cells
    public Color clearCellColor = Color.white;
    public Color mineCellColor = Color.red;

    private Cell[,] grid; // Grid of cells

    void Start()
    {
        GenerateGrid();
    }

    void GenerateGrid()
    {
        grid = new Cell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                // Create a new cell
                GameObject newCell = Instantiate(cellPrefab, new Vector3(x * cellSize - 5, y * cellSize - 5, 0), Quaternion.identity);
                newCell.transform.SetParent(transform);

                // Randomly decide if the cell is a mine or a clear cell
                bool isMine = Random.Range(0, 5) == 0; // 20% chance to be a mine

                // Set up the cell component
                Cell cell = newCell.AddComponent<Cell>();
                cell.Init(x, y, isMine, this);

                // Set cell color
                if (isMine)
                {
                    cell.SetColor(mineCellColor);
                }
                else
                {
                    cell.SetColor(clearCellColor);
                }

                // Store the cell in the grid
                grid[x, y] = cell;
            }
        }
    }

    public void FloodFill(int startX, int startY)
    {
        // Early exit if the start cell is invalid
        if (grid[startX, startY].isMine) return;

        // Flood fill algorithm using a stack
        Stack<Cell> stack = new Stack<Cell>();
        stack.Push(grid[startX, startY]);

        while (stack.Count > 0)
        {
            Cell cell = stack.Pop();
            if (cell.isRevealed || cell.isMine) continue;

            cell.Reveal();

            // Check all 4 directions (N, S, E, W)
            CheckAndAddCellToStack(cell.x + 1, cell.y, stack);
            CheckAndAddCellToStack(cell.x - 1, cell.y, stack);
            CheckAndAddCellToStack(cell.x, cell.y + 1, stack);
            CheckAndAddCellToStack(cell.x, cell.y - 1, stack);
        }
    }

    private void CheckAndAddCellToStack(int x, int y, Stack<Cell> stack)
    {
        if (x >= 0 && x < gridWidth && y >= 0 && y < gridHeight && !grid[x, y].isRevealed && !grid[x, y].isMine)
        {
            stack.Push(grid[x, y]);
        }
    }

    public int GetAdjacentMineCount(int x, int y)
{
    int mineCount = 0;

    // Loop over the surrounding 8 cells (including diagonals)
    for (int dx = -1; dx <= 1; dx++)
    {
        for (int dy = -1; dy <= 1; dy++)
        {
            // Skip the current cell itself
            if (dx == 0 && dy == 0) continue;

            int checkX = x + dx;
            int checkY = y + dy;

            // Check if the neighboring cell is within bounds
            if (checkX >= 0 && checkX < gridWidth && checkY >= 0 && checkY < gridHeight)
            {
                // If the cell is a mine, increment the count
                if (grid[checkX, checkY].isMine)
                {
                    mineCount++;
                }
            }
        }
    }

    return mineCount;
}

}