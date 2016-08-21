using UnityEditor;
using System.Linq;

namespace WakaTime
{
    public class AssetChangedChecker : AssetPostprocessor
    {
        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var assetName in importedAssets)
            {
                Main.OnAssetSaved(assetName);
            }

            var assetNames = deletedAssets.Concat(movedAssets);
            assetNames = assetNames.Concat(movedFromAssetPaths);
            assetNames = assetNames.Distinct();

            foreach (var assetName in assetNames)
            {
                Main.OnAssetChanged(assetName);
            }
        }
    }

}