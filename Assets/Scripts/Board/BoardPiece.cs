﻿using UnityEngine;
using UnityEngine.UI;

public class BoardPiece : MonoBehaviour
{
  [SerializeField] Image icon;

  public void SetIconImage(Sprite selectedIcon)
  {
    if (selectedIcon == null) return;
    icon.sprite = selectedIcon;
  }
}
