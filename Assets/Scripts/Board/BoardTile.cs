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
    if(GameManager.Instance.IsGameActive)
      GameManager.Instance.boardManager.GenarateNextBoardPiece(gridIndex);
  }
}
