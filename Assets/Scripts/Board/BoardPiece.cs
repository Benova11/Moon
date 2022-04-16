﻿using UnityEngine;
using UnityEngine.UI;

public class BoardPiece : MonoBehaviour
{
  [SerializeField] Image icon;
  [SerializeField] float appeareDuration = 0.4f;

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
    icon.gameObject.transform.LeanScale(new Vector3(1, 1, 1), appeareDuration).setEaseOutBounce();
  }

  public void AnimateOnPartOfTriplet(float delay)
  {
    icon.gameObject.transform.LeanScale(new Vector3(1.4f, 1.4f, 1.4f), 0.14f)
      .setDelay(delay).setEaseInCirc().setOnComplete(()=>
      {
        icon.gameObject.transform.LeanScale(new Vector3(1f, 1f, 1f), 0.1f)
      .setDelay(0.04f);
      });
    icon.gameObject.transform.LeanRotateZ(180, 1.5f).setEaseOutElastic().setDelay(delay).setOnComplete(()=>
    {
      icon.gameObject.transform.LeanRotateZ(360, 1.5f).setEaseOutElastic().setDelay(delay/2);
    }
    );
  }
}
