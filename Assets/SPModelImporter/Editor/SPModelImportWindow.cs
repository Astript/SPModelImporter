using System.IO;
using UnityEditor;
using UnityEngine;

namespace SPModelImporter.Editor
{
    public class SPModelImportWindow : EditorWindow
    {
        [MenuItem("SPModelImporter/Import Model")]
        public static void OpenFile()
        {
            SPModelSetting Setting = SPModelSetting.GetOrCreate();
            // モデルファイルのパス
            var sourceFile = EditorUtility.OpenFilePanel("Select Model file", "", "fbx");
            var sourceDir = Path.GetDirectoryName(sourceFile);
            var saveDir = $"{Application.dataPath}{Setting.saveDir.Replace("Assets", "")}/{Path.GetFileNameWithoutExtension(sourceFile)}";
            var saveFile = $"{Application.dataPath}{Setting.saveDir.Replace("Assets", "")}/{Path.GetFileNameWithoutExtension(sourceFile)}/{Path.GetFileName(sourceFile)}";
            var jsonPath = @$"{saveDir}/autogenerate.json";
            // モデル名のディレクトリの作成
            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);


            // Jsonデータの作成
            var json = JsonUtility.ToJson(
                new SPTempData(
                    sourceDir,
                    Setting.baseColorPrefix,
                    Setting.metallicPrefix,
                    Setting.normalPrefix,
                    Setting.heightPrefix,
                    Setting.aoPrefix));
            
            // モデルのファイルをコピーしてくる
            if (File.Exists(saveFile))
            {
                if (UnityEditor.EditorUtility.DisplayDialog("Warning", "オーバーライドしますがよろしいですか？", "ok", "cancel"))
                {
                    File.Delete(saveFile);
                    File.Delete(saveFile + ".meta");
  
                }
                else
                    return;
            }
            File.WriteAllText(jsonPath, json);
            File.Copy(sourceFile, saveFile);

            AssetDatabase.Refresh();
        }
    }
}