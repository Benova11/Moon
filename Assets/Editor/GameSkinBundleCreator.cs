using System.IO;
using UnityEngine;
using UnityEditor;

public class GameSkinBundleCreator : EditorWindow
{
  string skinName;

  Texture2D xSymbolSkin;
  Texture2D oSymbolSkin;
  Texture2D backgroundImg;

  [MenuItem("Window/Bundle Skin")]
  public static void ShowWindow()
  {
    GetWindow(typeof(GameSkinBundleCreator));
  }

  void OnGUI()
  {
    GUILayout.Label("Bundle Properties", EditorStyles.boldLabel);
    EditorGUILayout.Space(4);

    skinName = EditorGUILayout.TextField("Bundle Name", skinName);
    EditorGUILayout.Space(10);

    EditorGUILayout.BeginVertical();
    xSymbolSkin = TextureField("X symbol", xSymbolSkin);
    oSymbolSkin = TextureField("O symbol", oSymbolSkin);
    backgroundImg = TextureField("Background", backgroundImg);
    SetCreateBundleButton();
    EditorGUILayout.EndVertical();
  }

  void SetCreateBundleButton()
  {
    if (GUI.Button(new Rect(175, 285, 100, 30), "Create Bundle"))
    {
      if (skinName != null && xSymbolSkin != null && oSymbolSkin != null && backgroundImg != null)
      {
        if (File.Exists(Application.streamingAssetsPath + "/skinName"))
        {
          Debug.LogWarning("Bundle already exists with that name, please choose another");
          return;
        }
        BuildMapABs(skinName, AssetDatabase.GetAssetPath(xSymbolSkin), AssetDatabase.GetAssetPath(oSymbolSkin), AssetDatabase.GetAssetPath(backgroundImg));
      }
    }
  }

  private static Texture2D TextureField(string name, Texture2D texture)
  {
    GUILayout.BeginHorizontal();
    var style = new GUIStyle(GUI.skin.label);
    style.alignment = TextAnchor.UpperCenter;
    style.fixedWidth = 70;
    GUILayout.Label(name, style);
    Texture2D selectedTexture = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
    GUILayout.EndHorizontal();
    return selectedTexture;
  }

  static void BuildMapABs(string bundleName, string xIconPath, string oIconPath, string bgImgPath)
  {
    AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

    buildMap[0].assetBundleName = bundleName;

    string[] skinAssets = new string[3];
    skinAssets[0] = xIconPath;
    skinAssets[1] = oIconPath;
    skinAssets[2] = bgImgPath;

    buildMap[0].assetNames = skinAssets;

    BuildTarget buildTarget = BuildTarget.StandaloneOSX;
#if !UNITY_STANDALONE_OSX
    buildTarget = BuildTarget.StandaloneWindows;
#endif

    BuildPipeline.BuildAssetBundles(Path.Combine(Application.streamingAssetsPath, "SkinBundles"), buildMap, BuildAssetBundleOptions.None, buildTarget);

  }
}