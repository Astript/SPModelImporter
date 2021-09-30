using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SPModelImporter.Scripts.Editor
{
    public class SPModelImporter : AssetPostprocessor
    {
        private static readonly List<string> ProcessedList = new List<string>();


        /// <summary>
        /// アセットを読み込んだ時に発火する
        /// </summary>
        /// <param name="importedAssets"></param>
        /// <param name="deletedAssets"></param>
        /// <param name="movedAssets"></param>
        /// <param name="movedFromAssetPaths"></param>
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            foreach (var path in importedAssets)
            {
                if (path.EndsWith(".fbx"))
                {
                    if (ProcessedList.Contains(path))
                    {
                        ProcessedList.Remove(path);
                        continue;
                    }

                    // モデルデータのあるディレクトリ
                    var distModelFile = @$"{Application.dataPath}{path.Replace("Assets", "")}";
                    var distModelDir = Path.GetDirectoryName(distModelFile);
                    var modelName = Path.GetFileName(distModelFile);

                    // Jsonからデータのロード
                    var jsonPath = @$"{path.TrimEnd(modelName.ToCharArray())}autogenerate.json";
                    if (!File.Exists(jsonPath))
                    {
                        continue;
                    }

                    string json = File.ReadAllText(jsonPath);
                    var temp = JsonUtility.FromJson<SPTempData>(json);

                    if (string.IsNullOrEmpty(json))
                    {
                        Debug.LogWarning("Tempファイルの作成に失敗しました");
                        continue;
                    }

                    // jsonの削除
                    if (File.Exists(jsonPath)) File.Delete(jsonPath);
                    if (File.Exists(jsonPath + ".meta")) File.Delete(jsonPath + ".meta");
                    
                    // フォルダ
                    var setting = SPModelSetting.GetOrCreate();
                    var sourceTexDir = @$"{temp.SourcePath}\{setting.textureFolderName}";
                    var distTexDir = @$"{distModelDir}\Textures";
                    var distMatDir = @$"{distModelDir}\Materials";

                    // ディレクトリの作成
                    if (!Directory.Exists(distTexDir)) Directory.CreateDirectory(distTexDir);
                    if (!Directory.Exists(distMatDir)) Directory.CreateDirectory(distMatDir);

                    // モデルデータからマテリアルの抽出と割り当て
                    if (AssetImporter.GetAtPath(path) is ModelImporter importer)
                    {
                        SerializedObject modelImporterObj = new SerializedObject(importer);
                        SerializedProperty materials = modelImporterObj.FindProperty("m_Materials");
                        for (int i = 0; i < materials.arraySize; i++)
                        {
                            SerializedProperty id = materials.GetArrayElementAtIndex(i);
                            var matName = id.FindPropertyRelative("name").stringValue;
                            CreateMaterialAndSetTextures(sourceTexDir, distTexDir, distMatDir, modelName, temp,
                                matName);
                        }

                        importer.SearchAndRemapMaterials(ModelImporterMaterialName.BasedOnMaterialName,
                            ModelImporterMaterialSearch.Local);

                        ProcessedList.Add(path);
                        importer.SaveAndReimport();
                    }
                }
            }
        }


        static void CreateMaterialAndSetTextures(
            string sourceTexDir,
            string distTexDir,
            string distMatDir,
            string modelName,
            SPTempData spTemp,
            string matName)
        {
#if UNITY_RP_URP
            Material material = new Material(Shader.Find("Universal Render Pipeline/Lit")) { name = matName };
            var sourceBaseMap = ConvertPrefix(@$"{sourceTexDir}\{spTemp.BaseColorPrefix}", modelName, matName);
            var sourceHeightMap = ConvertPrefix(@$"{sourceTexDir}\{spTemp.MetallicPrefix}", modelName, matName);
            var sourceMetallicMap = ConvertPrefix(@$"{sourceTexDir}\{spTemp.MetallicPrefix}", modelName, matName);
            var sourceNormalMap = ConvertPrefix(@$"{sourceTexDir}\{spTemp.NormalPrefix}", modelName, matName);
            var sourceRoughnessMap = ConvertPrefix(@$"{sourceTexDir}\{spTemp.HeightPrefix}", modelName, matName);
            var sourceAoMap = ConvertPrefix(@$"{sourceTexDir}\{spTemp.AoPrefix}", modelName, matName);


            // マテリアルごとにフォルダを作成
            if (!Directory.Exists(@$"{distTexDir}\{matName}"))
                Directory.CreateDirectory(@$"{distTexDir}\{matName}");

            var distBaseMap = ConvertPrefix(@$"{distTexDir}\{matName}\{spTemp.BaseColorPrefix}", modelName, matName);
            var distHeightMap = ConvertPrefix(@$"{distTexDir}\{matName}\{spTemp.MetallicPrefix}", modelName, matName);
            var distMetallicMap = ConvertPrefix(@$"{distTexDir}\{matName}\{spTemp.MetallicPrefix}", modelName, matName);
            var distNormalMap = ConvertPrefix(@$"{distTexDir}\{matName}\{spTemp.NormalPrefix}", modelName, matName);
            var distRoughnessMap = ConvertPrefix(@$"{distTexDir}\{matName}\{spTemp.HeightPrefix}", modelName, matName);
            var distAoMap = ConvertPrefix(@$"{distTexDir}\{matName}\{spTemp.AoPrefix}", modelName, matName);
            List<Tuple<string, string, int>> maps = new List<Tuple<string, string, int>>();
            maps.Add(new Tuple<string, string, int>(sourceBaseMap, distBaseMap, ShaderProperty.BaseMap));
            maps.Add(new Tuple<string, string, int>(sourceHeightMap, distHeightMap, ShaderProperty.HeightMap));
            maps.Add(new Tuple<string, string, int>(sourceMetallicMap, distMetallicMap, ShaderProperty.MetallicMap));
            maps.Add(new Tuple<string, string, int>(sourceNormalMap, distNormalMap, ShaderProperty.NormalMap));
            maps.Add(new Tuple<string, string, int>(sourceRoughnessMap, distRoughnessMap, ShaderProperty.RoughnesMap));
            maps.Add(new Tuple<string, string, int>(sourceAoMap, distAoMap, ShaderProperty.AOMap));


            foreach (var map in maps)
            {
                var sourceMap = map.Item1;
                var distMap = map.Item2;
                var key = map.Item3;
                if (File.Exists(sourceMap))
                {
                    File.Copy(sourceMap, distMap, true);
                    UnityEditor.EditorApplication.delayCall += () => Apply(material, distMap, key);
                }
            }

            AssetDatabase.CreateAsset(material, FullPathToAsset(@$"{distMatDir}\{matName}.mat"));
#endif
        }

        private static void Apply(Material material, string map, int key)
        {
            var tex = AssetDatabase.LoadAssetAtPath<Texture2D>($"{FullPathToAsset(map)}");
            material.SetTexture(key, tex);
            EditorApplication.delayCall -= () => Apply(material, map, key);
        }

        private static string FullPathToAsset(string fullPath)
        {
            return $"{fullPath.Replace(@"\", "/").Replace(Application.dataPath, "Assets")}";
        }


        private static string ConvertPrefix(string path, string modelName, string materialName) =>
            path.Replace("{MODEL_NAME}", Path.GetFileNameWithoutExtension(modelName))
                .Replace("{MATERIAL_NAME}", materialName);
    }
}