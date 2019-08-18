using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WizardsCode
{
    public abstract class AbstractDatabase : MonoBehaviour
    {

        public string modelsProjectFolder = "";

        public void Initialize()
        {
            modelsProjectFolder = AssetDatabase.GetAssetPath(MonoScript.FromMonoBehaviour(this));
            modelsProjectFolder = modelsProjectFolder.Substring(0, modelsProjectFolder.IndexOf("/Scripts/"));
        }
    }
}
