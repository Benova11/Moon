using UnityEngine;
using TMPro;
using System.Collections;

public class UIManager : Singleton<UIManager>
{
  [SerializeField] TextMeshProUGUI timerText;
  [SerializeField] TextMeshProUGUI endGameStatusText;
  [SerializeField] GameObject endGamePanel;

  public void OnGameStarts()
  {
    if(endGamePanel.activeInHierarchy)
      endGamePanel.SetActive(false);
  }

  public void SetTimerText(string time)
  {
    timerText.text = time;
  }

  public void OnEndOfGame(string endOfGameMsg)
  {
    endGameStatusText.text = endOfGameMsg;
    StartCoroutine(OnEndOfGameRoutine());
  }

  IEnumerator OnEndOfGameRoutine()
  {
    yield return new WaitForSeconds(1);
    endGamePanel.SetActive(true);
  }
}