using System.Collections;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
  [SerializeField] TextMeshProUGUI timerText;
  [SerializeField] TextMeshProUGUI endGameStatusText;
  [SerializeField] GameObject endGamePanel;
  [SerializeField] GameObject restartButton;

  [SerializeField] ParticleSystem endGamePS;
  [SerializeField] Animator endGameAnimator;
  public Animator timerAnimator;

  Coroutine endGameCoroutine;

  public void OnGameStarts()
  {
    if (endGameCoroutine != null) StopCoroutine(endGameCoroutine);
    if (endGamePS.isPlaying) endGamePS.Stop();
    LeanTween.cancelAll();
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
    endGameCoroutine = StartCoroutine(OnEndOfGameRoutine(isWin));
  }

  IEnumerator OnEndOfGameRoutine(bool isWin)
  {
    endGameCoroutine = null;
    yield return new WaitForSeconds(0.5f);
    if (isWin) endGamePS.Play();
    yield return new WaitForSeconds(0.65f);
    endGameAnimator.SetBool("Visible", true);
    yield return new WaitForSeconds(1);
    AnimateRestartButton();
  }

  void AnimateRestartButton()
  {
    restartButton.transform.LeanScale(new Vector3(1.2f, 1.2f, 1.2f), 0.5f)
    .setEaseInCirc().setLoopPingPong();
  }
}