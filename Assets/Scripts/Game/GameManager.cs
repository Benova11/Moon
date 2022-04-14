using UnityEngine;
using UnityEngine.UI;

public enum GameMode { PVP, PVC }
public enum PieceType { X, O, Empty }

public class GameManager : Singleton<GameManager>
{
  bool isGameActive = true;
  float currentTurnTimeRemaining;
  [SerializeField] float turnTimeInterval;

  public bool IsGameActive { get { return isGameActive; } }

  public BoardManager boardManager;

  public PieceType currentPlayerType = PieceType.X;
  //bool turnPlayed = false;
  bool isPVCMode = true;

  public Sprite xPlayerIcon;
  public Sprite oPlayerIcon;
  [SerializeField] Image gameBg;

  public void StartGame(GameMode selectedGameMode, Sprite selectedXIcon, Sprite selectedOIcon, Sprite selectedBg)
  {
    isPVCMode = selectedGameMode == GameMode.PVC;
    UIManager.Instance.OnGameStarts();
    ResertTimer();
    SetGameSkin(selectedXIcon, selectedOIcon, selectedBg);
  }

  void LateUpdate()
  {
    if (IsGameActive)
    {
      currentTurnTimeRemaining -= Time.deltaTime;
      UIManager.Instance.SetTimerText(Mathf.RoundToInt(currentTurnTimeRemaining).ToString());
      if (currentTurnTimeRemaining <= 0)
        OnTimesUp();
    }
  }

  void SetGameSkin(Sprite selectedXIcon, Sprite selectedOIcon, Sprite selectedBg)
  {
    if (selectedXIcon != null)
      xPlayerIcon = selectedXIcon;
    if (selectedOIcon != null)
      oPlayerIcon = selectedOIcon;
    if (selectedBg != null)
      gameBg.sprite = selectedBg;
  }

  public void OnPlayerMove(bool isWon = false,bool isBotTurn = false)
  {
    ResertTimer();
    if (!isWon)
    {
      SwitchPlayer();
      if (isPVCMode && !isBotTurn)
        PlayBotTurn();
    }
    else if (boardManager.NumOfEmptyTiles != 0)
      OnPlayerWon();
    else
      OnDraw();
  }

  public void PlayBotTurn()
  {
    int indexToPlayOn = boardManager.CurrentBoardPieces.FindIndex(pieceType => pieceType == PieceType.Empty);
    boardManager.GenarateNextBoardPiece(indexToPlayOn, true);
  }

  public void UndoLastTurn()
  {
    if (isGameActive)
    {
      boardManager.ResetLastTurnTiles();
      ResertTimer();
    }
  }

  void OnPlayerWon()
  {
    isGameActive = false;
    UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() +" wins");
  }

  void OnTimesUp()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    SwitchPlayer();
    UIManager.Instance.OnEndOfGame("Player " + ((int)currentPlayerType + 1).ToString() + " wins");
  }

  void OnDraw()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 0;
    UIManager.Instance.OnEndOfGame("Draw");
  }

  void SwitchPlayer()
  {
    currentPlayerType = currentPlayerType == PieceType.X ? PieceType.O : PieceType.X;
  }

  void ResertTimer()
  {
    currentTurnTimeRemaining = turnTimeInterval;
  }

  public void RestartGame()
  {
    isGameActive = false;
    currentTurnTimeRemaining = 5;
    boardManager.ClearBoard();
    currentPlayerType = (PieceType)Random.Range(0, 2);
    UIManager.Instance.OnGameStarts();
    isGameActive = true;
  }

  // public void LoadSkinBundle(string bundleName)
  //{
  //  string bundlePathToLoad = Path.Combine(Application.streamingAssetsPath, "SkinBundles", bundleName);
  //  if (File.Exists(bundlePathToLoad))
  //  {
  //    AssetBundle myLoadedAssetBundle = AssetBundle.LoadFromFile(bundlePathToLoad);
  //    Sprite[] sprites = myLoadedAssetBundle.LoadAllAssets<Sprite>();
  //    xPlayerIcon = sprites[0];
  //    oPlayerIcon = sprites[1];
  //    gameBg = sprites[2];
  //  }
  //  else
  //  {
  //    Debug.LogWarning($"Bundle with the path {bundlePathToLoad} does not exists.");
  //  }
  //}
}
