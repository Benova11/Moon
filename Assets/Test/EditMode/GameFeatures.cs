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
      var gameObject = new GameObject();
      var boardManager = gameObject.AddComponent<BoardManager>();
      //bool isWin =
      //       boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.O, PieceType.O, PieceType.O, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty })
      //    && boardManager.IsRowWin(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty });

      boardManager.SetPreMadeBoardPieces(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty });
      Assert.AreEqual(boardManager.IsBoardOnWinState(), true);
      //Assert.AreEqual(isWin, true);

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
      var boardManager = gameObject.AddComponent<BoardManager>();
      boardManager.SetPreMadeBoardPieces(new List<PieceType> { PieceType.X, PieceType.X, PieceType.X, PieceType.O, PieceType.O, PieceType.Empty, PieceType.Empty, PieceType.Empty, PieceType.Empty });
      Assert.AreEqual(boardManager.IsBoardOnWinState(), true);
    }
  }
}
