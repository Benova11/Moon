using UnityEngine;

public class BoardTile : MonoBehaviour
{
  public int gridIndex;

  public void OnTileClicked()
  {
    PlaceBoardPiece();
  }

  void PlaceBoardPiece()
  {
    GameManager.Instance.boardManager.GenarateNextBoardPiece(gridIndex);
  }
}
