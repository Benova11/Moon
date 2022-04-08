using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  enum PlayerType { X, O }
  PlayerType currentPlayerType = PlayerType.X;
  int CurrentPlayerPieceValue { get { return (int)currentPlayerType; } }
  int NumOfEmptyTiles { get { return currentBoardPieces.FindAll(x => x == -1).Count; } }

  [SerializeField] GameObject XPiecePrefab;
  [SerializeField] GameObject OPiecePrefab;

  [SerializeField] GameObject boardContainer;
  List<int> currentBoardPieces;

  void Start()
  {
    currentBoardPieces = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1, -1 };
  }

  public void GenarateNextBoardPiece(int gridIndex)
  {
    if (currentBoardPieces[gridIndex] != -1 || NumOfEmptyTiles == 0)
    {
      return; //place already taken or board full
    }
    GameObject piecePrefabToInstantiate = currentPlayerType == PlayerType.X ? XPiecePrefab : OPiecePrefab;
    currentBoardPieces[gridIndex] = CurrentPlayerPieceValue;
    currentPlayerType = (PlayerType)Mathf.Abs(1 - CurrentPlayerPieceValue);
    Instantiate(piecePrefabToInstantiate, boardContainer.transform.GetChild(gridIndex));
    CheckBoardState();
  }

  void CheckBoardState()
  {
    if (NumOfEmptyTiles > 5) return;

    for (int i = 1; i <= 3; i++)
    {
      if (currentBoardPieces[i - 1] == (int)PlayerType.X)
        Debug.Log("");
      else if (currentBoardPieces[i - 1] == (int)PlayerType.O)
        Debug.Log("");

      if (currentBoardPieces[i*3 - 1] == (int)PlayerType.X)
        Debug.Log("");
      else if (currentBoardPieces[i*3 - 1] == (int)PlayerType.O)
        Debug.Log("");

      if (i < 3)
      {
        if (currentBoardPieces[i * 4 - 1] == (int)PlayerType.X)
          Debug.Log("");
        else if (currentBoardPieces[i * 4 - 1] == (int)PlayerType.O)
          Debug.Log("");
      }

    }
  }

  //  if (currentBoardPieces[i * 1 - 1] == (int) PlayerType.X)
  //else if (currentBoardPieces[i + 1] == (int) PlayerType.O)

}
