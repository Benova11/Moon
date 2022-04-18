using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace Tests
{
  public class GameFeatures
  {
    [Test]
    public void TestWin()
    {
      bool isWin = IsBoardWinCondition(new GameObject());
      Assert.AreEqual(isWin, true);
    }

    [Test]
    public void TestDraw()
    {
      var gameObject = new GameObject();
      var boardManager = gameObject.AddComponent<BoardManager>();
      boardManager.SetPreMadeBoardPieces(new List<PieceType> { PieceType.X, PieceType.O, PieceType.O, PieceType.O, PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O });
      bool isDraw = !boardManager.IsBoardOnWinState() && boardManager.NumOfEmptyTiles == 0;
      Assert.AreEqual(isDraw, true);
    }

    [Test]
    public void TestLose()
    {
      var gameObject = new GameObject();
      bool isBoardWinCondition = IsBoardWinCondition(gameObject);
      BoardManager boardManager = gameObject.GetComponent <BoardManager>();
      boardManager.SetPVCPlayerType(PieceType.O);
      bool isLost = isBoardWinCondition && (int)boardManager.PlayerInPVCMode != boardManager.winningTriplet.first;
      Assert.AreEqual(isLost, true);
    }

    [Test]
    public void TestUndo()
    {
      var gameObject = new GameObject();
      var boardManager = gameObject.AddComponent<BoardManager>();
      boardManager.SetPreMadeBoardPieces(new List<PieceType> { PieceType.X, PieceType.O, PieceType.O, PieceType.O, PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.Empty });
      List<PieceType> expectedUndoPieceTypeList = new List<PieceType> { PieceType.Empty, PieceType.Empty, PieceType.O, PieceType.O, PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.Empty };
      boardManager.UndoLastTurnTiles();
      Assert.AreEqual(expectedUndoPieceTypeList, boardManager.CurrentBoardPiecesValues);
    }

    bool IsBoardWinCondition(GameObject newGameObject)
    {
      var gameObject = newGameObject;
      var boardManager = gameObject.AddComponent<BoardManager>();
      return
             boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
          && boardManager.IsColumnWin(new List<PieceType> { PieceType.X, PieceType.O, PieceType.O, PieceType.X, PieceType.Empty, PieceType.Empty, PieceType.X, PieceType.Empty, PieceType.Empty })
          && boardManager.IsDiagonalWin(new List<PieceType> { PieceType.X, PieceType.O, PieceType.O, PieceType.O, PieceType.X, PieceType.O, PieceType.X, PieceType.O, PieceType.X });
    }
  }
}
