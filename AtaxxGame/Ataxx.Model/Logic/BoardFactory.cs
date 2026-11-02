namespace Ataxx.Model.Logic;

using System;
using System.Collections.Generic;
using Ataxx.Model.Data;

public class BoardFactory
{
    private readonly Random _random = new Random();
    
    public AtaxxBoard CreateRandomLayoutBoard()
    {
        int size = 7; 
        var board = new AtaxxBoard(size);

        int layoutIndex = _random.Next(1, 4);

        switch (layoutIndex)
        {
            case 1:
                //classic
                break;
            case 2:
                ApplyLayoutCross(board);
                break;
            case 3:
                ApplyLayoutCenterBlock(board);
                break;
        }

        return board;
    }
    
    private void ApplyLayoutCross(AtaxxBoard board)
    {
        
        board.GetCell(3, 1).SetWall();
        board.GetCell(3, 2).SetWall();
        board.GetCell(3, 4).SetWall();
        board.GetCell(3, 5).SetWall();
        
        board.GetCell(0, 3).SetWall();
        board.GetCell(1, 3).SetWall();
        board.GetCell(2, 3).SetWall();
        board.GetCell(4, 3).SetWall();
        board.GetCell(5, 3).SetWall();
        board.GetCell(6, 3).SetWall();
    }
    
    private void ApplyLayoutCenterBlock(AtaxxBoard board)
    {

        board.GetCell(3, 1).SetWall();
            
        board.GetCell(2, 2).SetWall();
        board.GetCell(4, 2).SetWall();
            
        board.GetCell(2, 3).SetWall();
        board.GetCell(4, 3).SetWall();
            
        board.GetCell(2, 4).SetWall();
        board.GetCell(4, 4).SetWall();
            
        board.GetCell(3, 5).SetWall();
    }
}