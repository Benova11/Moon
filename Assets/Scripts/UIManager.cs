using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager>
{
  [SerializeField] TextMeshProUGUI timerText;

  public void SetTimerText(string time)
  {
    timerText.text = time;
  }
}
