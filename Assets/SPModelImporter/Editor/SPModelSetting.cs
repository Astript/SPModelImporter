using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SPModelImporter.Editor
{
    [CreateAssetMenu(menuName = "SPModelImporter/Setting", fileName = "SPModelSetting")]
    public class SPModelSetting : ScriptableObject
    {
        public string saveDir = $"Assets/Model/";
        public string textureFolderName;

        public string baseColorPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_AlbedoTransparency.png";
        public string metallicPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_MetallicSmoothness.png";
        public string normalPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_Normal.png";
        public string heightPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_Height.png";
        public string aoPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_AO.png";



        public void Init()
        {
            this.saveDir = $"Assets/Model/";
            this.textureFolderName = "tex";
            this.baseColorPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_AlbedoTransparency.png";
            this.metallicPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_MetallicSmoothness.png";
            this.normalPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_Normal.png";
            this.heightPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_Height.png";
            this.aoPrefix = "{MODEL_NAME}_{MATERIAL_NAME}_AO.png";
        }

        internal static SPModelSetting GetOrCreate()
        {
            SPModelSetting settings = null;
            var path = $"{Application.dataPath}/Settings/";
            if (Directory.Exists(path))
                settings = AssetDatabase.LoadAssetAtPath<SPModelSetting>(SPSettingsProvider.PATH);
            else
                Directory.CreateDirectory(path);
            

            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<SPModelSetting>();
           
                settings.Init();
                AssetDatabase.CreateAsset(settings, SPSettingsProvider.PATH);
                AssetDatabase.SaveAssets();
            }

            return settings;
        }
    }


    class SPSettingsProvider : SettingsProvider
    {
        public const string PATH = "Assets/Settings/SPSetting.asset";
        private SPModelSetting ModelSetting;

        private SPSettingsProvider(string path, SettingsScope scopes = SettingsScope.Project,
            IEnumerable<string> keywords = null) : base(path, scopes, keywords)
        {
        }


        public override void OnGUI(string searchContext)
        {
            if (!ModelSetting) return;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Folder name of Texture ");
            ModelSetting.textureFolderName = EditorGUILayout.TextField(ModelSetting.textureFolderName);

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginVertical();
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Save Model Directory");
            ModelSetting.saveDir = EditorGUILayout.TextField(ModelSetting.saveDir);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.LabelField("Prefixes");
            ModelSetting.baseColorPrefix = EditorGUILayout.TextField(ModelSetting.baseColorPrefix);
            ModelSetting.metallicPrefix = EditorGUILayout.TextField(ModelSetting.metallicPrefix);
            ModelSetting.normalPrefix = EditorGUILayout.TextField(ModelSetting.normalPrefix);
            ModelSetting.heightPrefix = EditorGUILayout.TextField(ModelSetting.heightPrefix);
            ModelSetting.aoPrefix = EditorGUILayout.TextField(ModelSetting.aoPrefix);
            UnityEditor.EditorGUILayout.EndVertical();
        }

        public override void OnActivate(string searchContext, VisualElement rootElement)
        {
            ModelSetting = SPModelSetting.GetOrCreate();
        }

        [SettingsProvider]
        public static SettingsProvider CreateProvider()
        {
            var provider = new SPSettingsProvider("Project/SPModelImporter", SettingsScope.Project);
            return provider;
        }
    }
}