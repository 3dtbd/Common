using UnityEditor;
using UnityEngine;

namespace U3Dtbd
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
