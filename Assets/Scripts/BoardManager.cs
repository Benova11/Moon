using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
  enum PlayerType { X, O }
  PlayerType currentPlayerType = PlayerType.X;
  int CurrentPlayerPieceValue { get { return (int)currentPlayerType; } }

  [SerializeField] GameObject XPiecePrefab;
  [SerializeField] GameObject OPiecePrefab;

  [SerializeField] GameObject boardContainer;
  List<int> currentBoardPieces;

  void Start()
  {
    currentBoardPieces = new List<int> { -1,-1,-1,-1,-1,-1,-1,-1,-1};
  }

  public GameObject GenarateNextBoardPiece(int gridIndex)
  {
    if (currentBoardPieces[gridIndex] != -1)
    {
      Debug.Log("Place already taken");
      return null; //place already taken
    }
    GameObject piecePrefabToInstantiate = currentPlayerType == PlayerType.X ? XPiecePrefab : OPiecePrefab;
    currentBoardPieces[gridIndex] = CurrentPlayerPieceValue;
    currentPlayerType = (PlayerType)Mathf.Abs(1 - CurrentPlayerPieceValue);

    return Instantiate(piecePrefabToInstantiate, boardContainer.transform.GetChild(gridIndex));
  }
}
