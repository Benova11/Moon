using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
  [SerializeField] TextMeshProUGUI timerText;
  [SerializeField] TextMeshProUGUI endGameStatusText;
  [SerializeField] GameObject endGamePanel;

  [SerializeField] ParticleSystem endGamePS;
  [SerializeField] Animator endGameAnimator;
  public Animator timerAnimator;

  public void OnGameStarts()
  {
    endGameAnimator.SetBool("Visible", false);
    timerAnimator.SetBool("TimerRuns", true);
  }

  public void SetTimerText(string time)
  {
    timerText.text = time;
  }

  public void OnEndOfGame(string endOfGameMsg, bool isWin)
  {
    timerAnimator.SetBool("TimerRuns", false);
    endGameStatusText.text = endOfGameMsg;
    StartCoroutine(OnEndOfGameRoutine(isWin));
  }

  IEnumerator OnEndOfGameRoutine(bool isWin)
  {
    yield return new WaitForSeconds(0.25f);
    if (isWin) endGamePS.Play();
    yield return new WaitForSeconds(0.75f);
    endGameAnimator.SetBool("Visible", true);
  }
}