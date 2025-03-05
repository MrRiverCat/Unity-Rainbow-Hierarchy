using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;
using System.Text;
using System.Reflection;

namespace EditorExpand.HierarchyExpand
{
    /// <summary>
    /// �ʺ� Hierarchy
    /// </summary>
    [InitializeOnLoad]
    public class RainbowHierarchy : MonoBehaviour
    {
        static RainbowHierarchy()
        {

        }



        [Tooltip("�����ļ���·��")]
        public static string fileDataPath = "Assets/ScriptableObject Data/Rainbow Hierarchy Config";

        [InitializeOnLoadMethod]
        private static void RainbowView()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnRainbowGUI;
            EditorApplication.hierarchyWindowItemOnGUI += OnRainbowButton;
        }

        private static void OnRainbowGUI(int instanceID, Rect selectionRect)
        {
            // Debug.Log($"{instanceID} {selectionRect}");

            UnityEngine.Object target = EditorUtility.InstanceIDToObject(instanceID);
            if (target == null) return;


#if UNITY_2020_1_OR_NEWER
            var globalID = GlobalObjectId.GetGlobalObjectIdSlow(target).targetObjectId;
#elif UNITY_2018_1
            var globalID =  GetLocalIdentifierInFile(target);
#endif

            // ��ȡ�������ݱ�
            var fileName = EditorSceneManager.GetActiveScene().name;
            RainbowHierarchyConfig config = GetScriptableObject(fileName);

            if (config == null) return;

            List<HierarchyConfig> rainbowList = config.RainbowList;
            var unitConfig = rainbowList.Find(e => e.ID == globalID);

            if (unitConfig == null) return;

            #region Ӧ������
            Texture2D _Bg = CreateDefaultTexture2D(unitConfig);
            GUIStyle FontStyle = CreateGUIStyle(_Bg, unitConfig);
            #endregion

            EditorGUI.LabelField(selectionRect, unitConfig.GameObjectName, FontStyle);
        }

        private static void OnRainbowButton(int instanceID, Rect selectionRect)
        {
            // ���Scene
            UnityEngine.Object target = EditorUtility.InstanceIDToObject(instanceID);
            if (target == null) return;

     

            if (GUI.Button(new Rect(selectionRect.xMax, selectionRect.y, selectionRect.height, selectionRect.height), "K"))
            { 
                
            }

            if (GUI.Button(new Rect(selectionRect.xMax - selectionRect.height, selectionRect.y, selectionRect.height, selectionRect.height), "-"))
            {

            }

            if (GUI.Button(new Rect(selectionRect.xMax - selectionRect.height * 2, selectionRect.y, selectionRect.height, selectionRect.height), "+"))
            {

            }
        }



        #region Menu - GameObject
        [MenuItem("GameObject/�ʺ� Hierarchy/��Ӳʺ�", priority = 1)]
        private static void AddRainbowGUI()
        {
            // ��ȡ ScriptableObejct
            var fileName = GetFilePath(EditorSceneManager.GetActiveScene().name);
            var config = AssetDatabase.LoadAssetAtPath<RainbowHierarchyConfig>(fileName);

            if (config == null)
            {
                config = CreateScriptableObejct(EditorSceneManager.GetActiveScene().name);
            }

            var globalID = GlobalObjectId.GetGlobalObjectIdSlow(Selection.activeObject).targetObjectId;

            var rainbowList = config.RainbowList;
            var unitConfig = rainbowList.Find(e => e.ID == globalID);

            if (unitConfig == null)
            {
                HierarchyConfig newConfig = new()
                {
                    GameObjectName = Selection.activeGameObject.name,
                    ID = globalID,
                };

                rainbowList.Add(newConfig);
            }
            else
            {
                unitConfig.GameObjectName = Selection.activeGameObject.name;
            }

            EditorUtility.SetDirty(config);
        }

        [MenuItem("GameObject/�ʺ� Hierarchy/�Ƴ��ʺ�", priority = 2)]
        private static void RemoveRainbowGUI()
        {
            //var localID = GetLocalIdentifierInFile(Selection.activeGameObject);
            var globalID = GlobalObjectId.GetGlobalObjectIdSlow(Selection.activeObject).targetObjectId;


            // ��ȡScriptableObject�����ļ�
            RainbowHierarchyConfig config = GetScriptableObject(Selection.activeGameObject.scene.name);

            var rainbowList = config.RainbowList;
            var unitConfig = rainbowList.Find(e => e.ID == globalID);

            if (unitConfig != null)
            {
                rainbowList.Remove(unitConfig);
            }

            EditorUtility.SetDirty(config);
        }

        [MenuItem("GameObject/�ʺ� Hierarchy/����ʺ�", priority = 3)]
        private static void ClearAll()
        {
            RainbowHierarchyConfig config = GetScriptableObject(Selection.activeGameObject.scene.name);
            if (config == null) return;

            var rainbowList = config.RainbowList;
            rainbowList.Clear();

            EditorUtility.SetDirty(config);
        }
        #endregion

        #region Menu - AssetDatabase
        [MenuItem("AssetDatabase/�ʺ� Hierarchy/���������ļ���")]
        private static void CreateFolder()
        {
            string[] path = fileDataPath.Split('/');

            if (!path[0].Equals("Assets"))
            {
                Debug.Log("[Error Create] You must use \"Assets\" to start filePath");
                return;
            }

            string parentPath = path[0];
            string folderName;

            for (int i = 1; i < path.Length; i++)
            {
                folderName = path[i];

                if (!AssetDatabase.IsValidFolder($"{parentPath}/{folderName}"))
                {
                    AssetDatabase.CreateFolder(parentPath, folderName);
                }

                parentPath = $"{parentPath}/{folderName}";
            }

            AssetDatabase.Refresh();
        }

        [MenuItem("AssetDatabase/�ʺ� Hierarchy/���������ļ�")]
        private static void InitScriptObject()
        {
            string dataPath = GetFilePath(Selection.activeGameObject.scene.name);

            string[] assets = AssetDatabase.FindAssets(dataPath);

            if (assets.Length.Equals(0))
            {
                var config = ScriptableObject.CreateInstance<RainbowHierarchyConfig>();

                AssetDatabase.CreateAsset(config, dataPath);
                //AssetDatabase.SaveAssets();
            }

            AssetDatabase.Refresh();
        }
        #endregion

        #region others

        private static Texture2D CreateDefaultTexture2D(HierarchyConfig config)
        {
            Texture2D _Bg = new(1, 1, TextureFormat.RGBAFloat, false);
            _Bg.SetPixel(0, 0, config.BackgroundColor);
            _Bg.Apply();

            return _Bg;
        }

        private static GUIStyle CreateGUIStyle(Texture2D backgorund, HierarchyConfig config)
        {
            GUIStyle FontStyle = new GUIStyle
            {
                alignment = TextAnchor.MiddleCenter
            };

            FontStyle.normal.background = backgorund;
            FontStyle.normal.textColor = config.FontColor;
            FontStyle.fontSize = config.FontSize;
            FontStyle.fontStyle = UnityEngine.FontStyle.Bold;

            return FontStyle;
        }

        /// <summary>
        /// �ļ�·��
        /// </summary>
        /// <param name="name">�ļ���</param>
        /// <returns></returns>
        private static string GetFilePath(string name)
        {
            // ��ע��δ�� name �Ƿ�Ϊ�յ��жϣ������ò�ȷ���ԣ�Ӧ�ڵ��ô˷���ǰ�����жϷ�����
            return $"{fileDataPath}/Rainbow_{name}";
        }

        /// <summary>
        /// ScriptObject RainbowHierarchy���ݻ�ȡ
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="filePath">�ļ�·����Ĭ��ΪNULL��������ʹ������·�����������ļ���·����ַ���ɣ�</param>
        /// <returns></returns>
        private static RainbowHierarchyConfig GetScriptableObject(string fileName, string filePath = null)
        {
            // ��Դ�ļ�·����ַ
            filePath ??= GetFilePath(fileName + ".asset");

            // �����Դ�ļ���Ŀ¼�Ƿ����
            if (!AssetDatabase.IsValidFolder(fileDataPath))
            {
                CreateFolder();
            }

            // ����·���ļ�
            RainbowHierarchyConfig config = AssetDatabase.LoadAssetAtPath<RainbowHierarchyConfig>(filePath);

            // ���򷵻أ����򴴽�
            config = config != null ? config : CreateScriptableObejct(fileName, filePath);

            return config;
        }

        /// <summary>
        /// ScriptableObject RainbowHeirarchyConfig ����
        /// </summary>
        /// <param name="fileName">�ļ���</param>
        /// <param name="filePath">�ļ�·����Ĭ��ΪNULL��������ʹ������·�����������ļ���·����ַ���ɣ�</param>
        /// <returns></returns>
        private static RainbowHierarchyConfig CreateScriptableObejct(string fileName, string filePath = null)
        {
            // ��Դ·����ַ
            filePath ??= GetFilePath(fileName + ".asset");

            var config = AssetDatabase.LoadAssetAtPath<RainbowHierarchyConfig>(filePath);

            if (config == null)
            {
                var instance = ScriptableObject.CreateInstance<RainbowHierarchyConfig>();

                AssetDatabase.CreateAsset(instance, filePath);
            }

            AssetDatabase.Refresh();

            return config;
        }

        /// <summary>
        /// ��ȡ�����ļ�GUID
        /// </summary>
        private static int GetLocalIdentifierInFile(GameObject target)
        {
            PropertyInfo insepectorInfo = typeof(SerializedObject).GetProperty("inspectorMode", BindingFlags.NonPublic | BindingFlags.Instance);

            SerializedObject serializedObject = new SerializedObject(target);
            insepectorInfo.SetValue(serializedObject, InspectorMode.Debug, null);
            SerializedProperty localIdentifierInFile = serializedObject.FindProperty("m_LocalIdentfierInFile");

            return localIdentifierInFile.intValue;
        }
        #endregion
    }
}
