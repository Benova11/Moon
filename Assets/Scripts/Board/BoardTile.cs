using UnityEngine;

public class BoardTile : MonoBehaviour
{
  [SerializeField] int listIndex;

  public void OnTileClicked()
  {
    if(GameManager.Instance.CurrentGameMode != GameMode.CVC)
      PlaceBoardPiece();
  }

  void PlaceBoardPiece()
  {
    if(GameManager.Instance.IsGameActive)
      GameManager.Instance.CurrentBoardManager.GenarateNextBoardPiece(listIndex);
  }
}
