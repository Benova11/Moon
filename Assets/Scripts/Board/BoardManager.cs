using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  PieceType CurrentPlayerPiece { get { return GameManager.Instance.currentPlayerType; } }
  public int NumOfEmptyTiles { get { return currentBoardPieces.FindAll(boardPiece => boardPiece == PieceType.Empty).Count; } }
  //bool IsGameActive { get { return GameManager.Instance.IsGameActive; } }

  [SerializeField] BoardPiece XPiecePrefab;
  [SerializeField] BoardPiece OPiecePrefab;

  [SerializeField] GameObject boardContainer;
  List<PieceType> currentBoardPieces;
  List<int> lastMoves;

  public List<PieceType> CurrentBoardPieces { get { return currentBoardPieces; } }

  void Start()
  {
    InitCurrentBoardPiecesList();
    lastMoves = new List<int>();
  }

  void InitCurrentBoardPiecesList()
  {
    currentBoardPieces = new List<PieceType>();
    for(int i=0; i<9; i++)
      currentBoardPieces.Add(PieceType.Empty);
  }

  public void GenarateNextBoardPiece(int gridIndex,bool isBot = false)
  {
    if (currentBoardPieces[gridIndex] != PieceType.Empty || NumOfEmptyTiles == 0)
    {
      return; //place already taken or board full
    }
    BoardPiece piecePrefabToInstantiate = CurrentPlayerPiece == PieceType.X ? XPiecePrefab : OPiecePrefab;
    currentBoardPieces[gridIndex] = CurrentPlayerPiece;
    BoardPiece boardPiece = Instantiate(piecePrefabToInstantiate, boardContainer.transform.GetChild(gridIndex));
    boardPiece.SetIconImage(CurrentPlayerPiece == PieceType.O ? GameManager.Instance.oPlayerIcon : GameManager.Instance.xPlayerIcon);
    lastMoves.Insert(0, gridIndex);
    GameManager.Instance.OnPlayerMove(IsBoardOnWinState(), isBot);
  }

  public void ResetLastTurnTiles()
  {
    if (lastMoves.Count > 0 && lastMoves.Count % 2 == 0)
    {
      for (int i = 0; i < 2; i++)
      {
        int lastTurnIndex = lastMoves[0];
        lastMoves.Remove(lastTurnIndex);
        currentBoardPieces[lastTurnIndex] = PieceType.Empty;
        Destroy(boardContainer.transform.GetChild(lastTurnIndex).GetChild(0).gameObject);
      }
    }
  }

  bool IsBoardOnWinState()
  {
    return (NumOfEmptyTiles < 5) && (IsRowWin() || IsColumnWin() || IsDiagonalWin());
  }

  bool IsRowWin()
  {
    bool isRowWin = false;
    for (int i = 0; i < 9; i+=3)
    {
      if (currentBoardPieces[i] == PieceType.Empty) break;
      if (currentBoardPieces[i] == currentBoardPieces[i + 1] && currentBoardPieces[i + 1] == currentBoardPieces[i + 2]) isRowWin = true;
    }
    return isRowWin;
  }

  bool IsColumnWin()
  {
    bool isColWin = false;
    for (int i = 0; i < 3; i ++)
    {
      if (currentBoardPieces[i] == PieceType.Empty) break;
      if (currentBoardPieces[i] == currentBoardPieces[i + 3] && currentBoardPieces[i + 3] == currentBoardPieces[i + 6]) isColWin = true;
    }
    return isColWin;
  }

  bool IsDiagonalWin()
  {
    if (currentBoardPieces[4] == PieceType.Empty) return false;
    return (currentBoardPieces[0] == currentBoardPieces[4] && currentBoardPieces[4] == currentBoardPieces[8])
         ||(currentBoardPieces[2] == currentBoardPieces[4] && currentBoardPieces[4] == currentBoardPieces[6]);
  }

  public void ClearBoard()
  {
    int numOfCurrentBoardPieces = 9 - NumOfEmptyTiles;
    for (int i = 0; i < numOfCurrentBoardPieces; i++)
    {
      int lastTurnIndex = lastMoves[0];
      lastMoves.Remove(lastTurnIndex);
      currentBoardPieces[lastTurnIndex] = PieceType.Empty;
      Destroy(boardContainer.transform.GetChild(lastTurnIndex).GetChild(0).gameObject);
    }
  }
}
