using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Pokemon.Dialogue.Editor
{
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath) {
            Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue;
            if (dialogue == null) {
                return AssetMoveResult.DidNotMove;
            }
            // Move from one directory to another
            if(Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath)) {
                return AssetMoveResult.DidNotMove;
            }

            // Change due to rename in order to rename the dialogue without any bug
            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath);

            return AssetMoveResult.DidNotMove;
        }
    }
}
