using UnityEngine;
using UnityEngine.UI;

public class BoardPiece : MonoBehaviour
{
  [SerializeField] Image icon;

  void Awake()
  {
    icon.gameObject.transform.localScale = new Vector3(0, 0, 0);
  }

  public void SetIconImage(Sprite selectedIcon)
  {
    if (selectedIcon != null) icon.sprite = selectedIcon;
  }

  public void OnPlacingPiece(Sprite selectedIcon)
  {
    SetIconImage(selectedIcon);
    icon.gameObject.transform.LeanScale(new Vector3(1, 1, 1), 0.25f).setEaseInOutBounce();
  }
}
