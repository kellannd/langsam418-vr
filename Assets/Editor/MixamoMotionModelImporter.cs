using UnityEditor;

public sealed class MixamoMotionModelImporter : AssetPostprocessor
{
    private const string MixamoMotionPath = "Assets/Avatars/movements/mixamo motions/";

    private void OnPreprocessModel()
    {
        if (!assetPath.StartsWith(MixamoMotionPath, System.StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        var importer = (ModelImporter)assetImporter;
        importer.importTangents = ModelImporterTangents.None;
    }
}
