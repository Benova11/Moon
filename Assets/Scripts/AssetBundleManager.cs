using UnityEditor;
using UnityEngine;

public class AssetBundleManager : MonoBehaviour
{
  public static void BuildMapABs(string bundleName, string xIconName, string oIconName, string bgImgName)
  {
    AssetBundleBuild[] buildMap = new AssetBundleBuild[1];

    buildMap[0].assetBundleName = bundleName;

    string[] skinAssets = new string[3];
    skinAssets[0] = xIconName;
    skinAssets[1] = oIconName;
    skinAssets[2] = bgImgName;

    buildMap[0].assetNames = skinAssets;

    //$"{skinFilesDir + xIconName + "png"}";

    BuildPipeline.BuildAssetBundles("Assets/StreamingAssets/SkinBundles", buildMap, BuildAssetBundleOptions.None, BuildTarget.StandaloneOSX);
  }
}
