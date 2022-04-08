using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  PlayerType CurrentPlayerPiece { get { return GameManager.Instance.currentPlayerType; } }
  public int NumOfEmptyTiles { get { return currentBoardPieces.FindAll(x => x < 0).Count; } }
  bool IsGameActive { get { return GameManager.Instance.IsGameActive; } }

  [SerializeField] GameObject XPiecePrefab;
  [SerializeField] GameObject OPiecePrefab;

  [SerializeField] GameObject boardContainer;
  List<int> currentBoardPieces;

  void Start()
  {
    currentBoardPieces = new List<int> { -1, -2, -3, -4, -5, -6, -7, -8, -9 };
  }

  public void GenarateNextBoardPiece(int gridIndex)
  {
    if (currentBoardPieces[gridIndex] > 0 || NumOfEmptyTiles == 0)
    {
      return; //place already taken or board full
    }
    GameObject piecePrefabToInstantiate = CurrentPlayerPiece == PlayerType.X ? XPiecePrefab : OPiecePrefab;
    currentBoardPieces[gridIndex] = (int)CurrentPlayerPiece;
    Instantiate(piecePrefabToInstantiate, boardContainer.transform.GetChild(gridIndex));
    GameManager.Instance.OnPlayerMove(IsBoardOnWinState());
  }

  bool IsBoardOnWinState()
  {
    Debug.Log(NumOfEmptyTiles);
    return (NumOfEmptyTiles < 5) && (IsRowWin() || IsColumnWin() || IsDiagonalWin());
  }

  bool IsRowWin()
  {
    bool isRowWin = false;
    for (int i = 0; i < 9; i+=3)
    {
      if (currentBoardPieces[i] == currentBoardPieces[i + 1] && currentBoardPieces[i + 1] == currentBoardPieces[i + 2]) isRowWin = true;
    }
    return isRowWin;
  }

  bool IsColumnWin()
  {
    bool isColWin = false;
    for (int i = 0; i < 3; i ++)
    {
      if (currentBoardPieces[i] == currentBoardPieces[i + 3] && currentBoardPieces[i + 3] == currentBoardPieces[i + 6]) isColWin = true;
    }
    return isColWin;
  }

  bool IsDiagonalWin()
  {
    return (currentBoardPieces[0] == currentBoardPieces[4] && currentBoardPieces[4] == currentBoardPieces[8])
         ||(currentBoardPieces[2] == currentBoardPieces[4] && currentBoardPieces[4] == currentBoardPieces[6]);
  }
}
