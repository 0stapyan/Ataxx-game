namespace Ataxx.Model.Logic;

using System;
using System.IO;
using Ataxx.Model.Data;

public class AtaxxBoard
{
    private readonly Cell[,] _cells;
    public int Size { get; }

    public AtaxxBoard(int size)
    {
        if (size < 5)
        {
            throw new Exception("Board size must be at least 5.");
        }

        Size = size;
        _cells = new Cell[size, size];
        InitializeEmptyBoard();
    }

    private void InitializeEmptyBoard()
    {
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                _cells[x, y] = new Cell(isWall: false);
            }
        }
    }

    public Cell GetCell(int x, int y)
    {
        if (!IsOnBoard(x, y))
        {
            throw new Exception("Coordinates are outside the board boundaries");
        }

        return _cells[x, y];
    }

    public bool IsOnBoard(int x, int y)
    {
        return x >= 0 && x < Size && y >= 0 && y < Size;
    }

    public Cell[,] GetFieldCopy()
    {
        return (_cells.Clone() as Cell[,])!;
    }
    
    internal AtaxxBoard(Cell[,] existingField)
    {
        _cells = existingField;
        Size = existingField.GetLength(0);
    }
}