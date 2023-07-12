using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Pokemon.SceneManagement.Editor
{
    public class SceneManagementModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath) {
                SceneGraph sceneGraph = AssetDatabase.LoadMainAssetAtPath(sourcePath) as SceneGraph;
                if (sceneGraph == null) {
                    return AssetMoveResult.DidNotMove;
                }
                // Move from one directory to another
                if(Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath)) {
                    return AssetMoveResult.DidNotMove;
                }

                // Change due to rename in order to rename the sceneGraph without any bug
                sceneGraph.name = Path.GetFileNameWithoutExtension(destinationPath);

                return AssetMoveResult.DidNotMove;
            }
    }
}
