using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  [SerializeField] BoardPiece XPiecePrefab;
  [SerializeField] BoardPiece OPiecePrefab;
  [SerializeField] GameObject boardContainer;

  List<int> lastMoves;
  List<PieceType> currentBoardPieces;
  PieceType playerInPVCMode;
  BoardPiece hintPiece;
  (int first, int second, int third) winningTriplet;

  public PieceType CurrentPlayerPiece { get { return GameManager.Instance.CurrentPlayerTypePiece; } }
  public PieceType PlayerInPVCMode { get { return playerInPVCMode; } }
  public List<PieceType> CurrentBoardPiecesValues { get { return currentBoardPieces; } }
  public (int first, int second, int third) WinningTriplet { get { return winningTriplet; } }

  public int NumOfEmptyTiles
  {
    get {
      if (currentBoardPieces == null)
        return -1;
      else
        return currentBoardPieces.FindAll(boardPiece => boardPiece == PieceType.Empty).Count;
    }
  }

  public void InitBoard(PieceType firstPlayerType)
  {
    playerInPVCMode = firstPlayerType;
    InitCurrentBoardPiecesList();
    lastMoves = new List<int>();
  }

  void InitCurrentBoardPiecesList()
  {
    currentBoardPieces = new List<PieceType>();
    for(int i = 0; i < 9; i++)
      currentBoardPieces.Add(PieceType.Empty);
  }

  public void SetPVCPlayerType(PieceType pieceType)
  {
    playerInPVCMode = pieceType;
  }

  public void GenarateNextBoardPiece(int listIndex,bool isBot = false)
  {
    if (!isBot && !GameManager.Instance.IsPlayerTurnAvailable) return;
    if (currentBoardPieces[listIndex] != PieceType.Empty) return;
    BoardPiece piecePrefabToInstantiate = CurrentPlayerPiece == PieceType.X ? XPiecePrefab : OPiecePrefab;
    currentBoardPieces[listIndex] = CurrentPlayerPiece;
    BoardPiece boardPiece = Instantiate(piecePrefabToInstantiate, boardContainer.transform.GetChild(listIndex));
    boardPiece.OnPlacingPiece(CurrentPlayerPiece == PieceType.O ? GameManager.Instance.oPlayerIcon : GameManager.Instance.xPlayerIcon);
    lastMoves.Insert(0, listIndex);
    GameManager.Instance.OnBoardMove(IsBoardOnWinState(), isBot);
    if (GetBoardPieceObject(listIndex) != null && GetBoardPieceObject(listIndex).IsHint)
    {
      Destroy(hintPiece.gameObject);
      hintPiece = null;
    }
  }

  public int GenarateHintBoardPiece()
  {
    if (hintPiece != null) return -1;
    (int xindex, int yIndex) nextMove = BoardMoveHelper.GetHint(playerInPVCMode, 1 - playerInPVCMode, CurrentBoardPiecesValues);
    int listIndex = nextMove.yIndex * 3 + nextMove.xindex;
    BoardPiece piecePrefabToInstantiate = PlayerInPVCMode == PieceType.X ? XPiecePrefab : OPiecePrefab;
    if(piecePrefabToInstantiate != null)
    {
      hintPiece = Instantiate(piecePrefabToInstantiate, boardContainer.transform.GetChild(listIndex));
      hintPiece.OnPlacingPiece(CurrentPlayerPiece == PieceType.O ? GameManager.Instance.oPlayerIcon : GameManager.Instance.xPlayerIcon,true);
    }
    return listIndex;
  }

  public void SetPreMadeBoardPieces(List<PieceType> preMadeBoard)
  {
    currentBoardPieces = preMadeBoard;
    lastMoves = new List<int>();
    foreach (PieceType pieceType in currentBoardPieces)
      if(pieceType != PieceType.Empty)
        lastMoves.Add((int)pieceType);
  }

  public void UndoLastTurnTiles()
  {
    if (lastMoves.Count > 0 && lastMoves.Count % 2 == 0)
      for (int i = 0; i < 2; i++)
      {
        int lastTurnIndex = lastMoves[0];
        lastMoves.Remove(lastTurnIndex);
        currentBoardPieces[lastTurnIndex] = PieceType.Empty;
        if(boardContainer!=null && boardContainer.transform.childCount > 0)
          Destroy(boardContainer.transform.GetChild(lastTurnIndex).GetChild(0).gameObject);
      }
  }

  public bool IsBoardOnWinState()
    => (NumOfEmptyTiles < 5) && (IsRowWin() || IsColumnWin() || IsDiagonalWin());
  

  public bool IsRowWin()
  {
    bool isRowWin = false;
    for (int i = 0; i < 9; i+=3)
    {
      if (currentBoardPieces[i] == PieceType.Empty) continue;
      if (currentBoardPieces[i] == currentBoardPieces[i + 1] && currentBoardPieces[i + 1] == currentBoardPieces[i + 2])
      {
        winningTriplet = (i, i + 1, i + 2);
        isRowWin = true;
        break;
      }
    }
    return isRowWin;
  }

  public bool IsRowWin(List<PieceType> preMadeBoard)
  {
    bool isRowWin = false;
    for (int i = 0; i < 9; i += 3)
    {
      if (preMadeBoard[i] == PieceType.Empty) continue;
      if (preMadeBoard[i] == preMadeBoard[i + 1] && preMadeBoard[i + 1] == preMadeBoard[i + 2])
      {
        winningTriplet = (i, i + 1, i + 2);
        isRowWin = true;
        break;
      }
    }
    return isRowWin;
  }

  public bool IsColumnWin()
  {
    bool isColWin = false;
    for (int i = 0; i < 3; i++)
    {
      if (currentBoardPieces[i] == PieceType.Empty) continue;
      if (currentBoardPieces[i] == currentBoardPieces[i + 3] && currentBoardPieces[i + 3] == currentBoardPieces[i + 6])
      {
        winningTriplet = (i, i + 3, i + 6);
        isColWin = true;
        break;
      }
    }
    return isColWin;
  }

  public bool IsColumnWin(List<PieceType> preMadeBoard)
  {
    bool isColWin = false;
    for (int i = 0; i < 3; i++)
    {
      if (preMadeBoard[i] == PieceType.Empty) continue;
      if (preMadeBoard[i] == preMadeBoard[i + 3] && preMadeBoard[i + 3] == preMadeBoard[i + 6])
      {
        winningTriplet = (i, i + 3, i + 6);
        isColWin = true;
        break;
      }
    }
    return isColWin;
  }

  public bool IsDiagonalWin()
  {
    if (currentBoardPieces[4] == PieceType.Empty) return false;
    if (currentBoardPieces[0] == currentBoardPieces[4] && currentBoardPieces[4] == currentBoardPieces[8])
    {
      winningTriplet = (0, 4, 8);
      return true;
    }
    else if (currentBoardPieces[2] == currentBoardPieces[4] && currentBoardPieces[4] == currentBoardPieces[6])
    {
      winningTriplet = (2, 4, 6);
      return true;
    }
    else return false;
  }

  public bool IsDiagonalWin(List<PieceType> preMadeBoard)
  {
    if (preMadeBoard[4] == PieceType.Empty) return false;
    if (preMadeBoard[0] == preMadeBoard[4] && preMadeBoard[4] == preMadeBoard[8])
    {
      winningTriplet = (0, 4, 8);
      return true;
    }
    else if (preMadeBoard[2] == preMadeBoard[4] && preMadeBoard[4] == preMadeBoard[6])
    {
      winningTriplet = (2, 4, 6);
      return true;
    }
    else return false;
  }

  public void ClearBoard()
  {
    winningTriplet = (-1,-1,-1);
    int numOfCurrentBoardPieces = 9 - NumOfEmptyTiles;
    for (int i = 0; i < numOfCurrentBoardPieces; i++)
    {
      int lastTurnIndex = lastMoves[0];
      lastMoves.Remove(lastTurnIndex);
      currentBoardPieces[lastTurnIndex] = PieceType.Empty;
      Destroy(boardContainer.transform.GetChild(lastTurnIndex).GetChild(0).gameObject);
    }
  }

  public BoardPiece GetBoardPieceObject(int index)
  {
    return boardContainer.transform.GetChild(index).GetChild(0)
            .gameObject.GetComponent<BoardPiece>();
  }

  public List<int> GetCurrentEmptyTilesIndexs()
  {
    List<int> currentEmptyTilesIndexs = new List<int>();
    for(int i=0; i< currentBoardPieces.Count;i++)
    {
      if (currentBoardPieces[i] == PieceType.Empty)
        currentEmptyTilesIndexs.Add(i);
    }
    return currentEmptyTilesIndexs;
  }
}
