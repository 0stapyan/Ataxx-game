namespace Ataxx.Model.Tests;

using Xunit;
using System;
using Ataxx.Model.Logic;
using Ataxx.Model.Data;

public class AtaxxEngineTests
{
    private readonly Player _playerX;
    private readonly Player _playerO;
    private readonly AtaxxEngine _engine;

    public AtaxxEngineTests()
    {
        _playerX = new Player("X");
        _playerO = new Player("O");
        _engine = new AtaxxEngine(_playerX, _playerO);
        _engine.StartGame();
    }
    
    [Fact]
    public void MakeMove_ValidJump_Succeeds()
    {
        var move = new Move(new Coordinates(0, 0), new Coordinates(2, 1));
        _engine.MakeMove(move);
        var field = _engine.GetFieldCopy();
        Assert.Null(field[0, 0].MarkedByPlayer);
        Assert.Equal(_playerX, field[2, 1].MarkedByPlayer);
    }

    [Fact]
    public void MakeMove_ValidClone_Succeeds()
    {
        var move = new Move(new Coordinates(0, 0), new Coordinates(1, 1));
        _engine.MakeMove(move);
        var field = _engine.GetFieldCopy();
        Assert.Equal(_playerX, field[0, 0].MarkedByPlayer);
        Assert.Equal(_playerX, field[1, 1].MarkedByPlayer);
    }

    [Fact]
    public void MakeMove_InvalidTooFar_ThrowsException()
    {
        var move = new Move(new Coordinates(0, 0), new Coordinates(3, 3));
        Assert.Throws<Exception>(() => _engine.MakeMove(move));
    }

    [Fact]
    public void CheckGameState_PlayerSwitch_Succeeds()
    {
        Assert.Equal(_playerX, _engine.CurrentPlayer);
        _engine.MakeMove(new Move(new Coordinates(0, 0), new Coordinates(1, 1)));
        Assert.Equal(_playerO, _engine.CurrentPlayer);
    }

    [Fact]
    public void EndGame_InitialState_IsNotEnded()
    {
        Assert.False(_engine.IsEnded);
        Assert.Null(_engine.Winner);
    }
    
    [Fact]
    public void MakeMove_ConvertsOpponentPieces()
    {
        var testEngine = new AtaxxEngine(_playerX, _playerO);
        testEngine.StartGame();

        testEngine.MakeMove(new Move(new Coordinates(0, 0), new Coordinates(1, 1)));
        testEngine.MakeMove(new Move(new Coordinates(0, 6), new Coordinates(1, 5)));
        testEngine.MakeMove(new Move(new Coordinates(1, 1), new Coordinates(2, 1)));
        testEngine.MakeMove(new Move(new Coordinates(6, 0),
            new Coordinates(5, 1)));
        testEngine.MakeMove(new Move(new Coordinates(2, 1),
            new Coordinates(3, 1)));
        testEngine.MakeMove(new Move(new Coordinates(1, 5), new Coordinates(2, 5)));
        testEngine.MakeMove(new Move(new Coordinates(6, 6), new Coordinates(5, 5)));
        
        testEngine.MakeMove(new Move(new Coordinates(2, 5), new Coordinates(3, 5)));
        
        testEngine.MakeMove(new Move(new Coordinates(3, 1), new Coordinates(4, 1)));
        
        var finalField = testEngine.GetFieldCopy();
        Assert.Equal(_playerX, finalField[5, 1].MarkedByPlayer);
    }
}
    