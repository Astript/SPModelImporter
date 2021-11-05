using System.IO;
using SPModelImporter.Editor;
using UnityEditor;
using UnityEngine;

namespace SPModelImporter.Editor
{
    public class SPModelImportWindow : EditorWindow
    {
        [MenuItem("SPModelImporter/Import Model")]
        public static void OpenFile()
        {
            SPModelSetting setting = SPModelSetting.GetOrCreate();
            // モデルファイルのパス
            var sourceFile = EditorUtility.OpenFilePanel("Select Model file", "", "fbx,obj");
            var sourceDir = Path.GetDirectoryName(sourceFile);
            var saveDir =
                $"{Application.dataPath}{setting.saveDir.Replace("Assets", "")}/{Path.GetFileNameWithoutExtension(sourceFile)}";
            var saveFile =
                $"{Application.dataPath}{setting.saveDir.Replace("Assets", "")}/{Path.GetFileNameWithoutExtension(sourceFile)}/{Path.GetFileName(sourceFile)}";
            var jsonPath = @$"{saveDir}/autogenerate.json";
            // モデル名のディレクトリの作成
            if (!Directory.Exists(saveDir)) Directory.CreateDirectory(saveDir);


            // Jsonデータの作成
            var json = JsonUtility.ToJson(
                new SPData(
                    sourceDir,
                    setting.baseColorPrefix,
                    setting.metallicPrefix,
                    setting.normalPrefix,
                    setting.heightPrefix,
                    setting.aoPrefix));

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