using UnityEngine;

public class UISingleton<T> : MonoBehaviour
 where T : Component
{
  private static int m_referenceCount = 0;

  private static T m_instance;

  public static T Instance
  {
    get
    {
      if (m_instance == null)
      {
        m_instance = FindObjectOfType<T>();
      }
      return m_instance;
    }
  }

  void Awake()
  {
    m_referenceCount++;

    m_instance = this as T;

  }

  void OnDestroy()
  {
    m_referenceCount--;
    if (m_referenceCount == 0)
    {
      m_instance = null;
    }
  }
}
