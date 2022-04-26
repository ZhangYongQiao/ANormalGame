using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Timers;
using System;

public class AssetBundleBuilder : EditorWindow
{
    //******���¾����ļ���·�� output ��ʾ���������·��
    const string basePath = "Assets/GameResources";
    static readonly string baseOutputPath = Application.streamingAssetsPath;

    static readonly string atlasPath = Path.Combine(basePath, "Atlas");
    static readonly string atlasOutputPath = Path.Combine(baseOutputPath, "Atlas");

    static readonly string prefabsPath = Path.Combine(basePath, "Prefabs");
    static readonly string prefabsOutputPath = Path.Combine(baseOutputPath, "Prefabs");

    static AssetBundleBuilder thisWindow;

    [MenuItem("Tools/ABBuilder")]
    static void Open()
    {
        thisWindow = EditorWindow.GetWindow<AssetBundleBuilder>();
        thisWindow.minSize = new Vector2(250, 300);
        thisWindow.Show();
    }

    static bool atlas;
    static bool prefabs;


    private void OnGUI()
    {
        float toggleWidth = 20;
        float baseWidth = thisWindow.minSize.x - 30;

        EditorGUILayout.BeginHorizontal();

        atlas = EditorGUILayout.Toggle(atlas, GUILayout.Width(toggleWidth));
        if (GUILayout.Button("ͼ�����", GUILayout.Width(baseWidth)))
        {
            BuildAtlas();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        prefabs = EditorGUILayout.Toggle(prefabs, GUILayout.Width(toggleWidth));
        if (GUILayout.Button("Ԥ������", GUILayout.Width(baseWidth)))
        {
            BuildPrefabs();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(30);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("ȫѡ", GUILayout.Width(100)))
        {
            atlas = true;
            prefabs = true;
            //Add
        }
        if (GUILayout.Button("ȫ��ѡ", GUILayout.Width(100)))
        {
            atlas = false;
            prefabs = false;
            //Add
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("���", GUILayout.Width(thisWindow.minSize.x), GUILayout.Height(50)))
        {
            BuildAtlas();
            BuildPrefabs();
            //Other...

        }
        EditorGUILayout.EndHorizontal();

    }

    /// <summary>
    /// Ԥ������
    /// </summary>
    static void BuildPrefabs()
    {
        if (!prefabs) return;
        CheckPath(prefabsPath);
        BuildPart("t:prefab", new string[] { prefabsPath }, "prefabsab", prefabsOutputPath);
    }

    /// <summary> ͼ����� </summary>
    static void BuildAtlas()
    {
        if (!atlas) return;
        CheckPath(atlasPath);
        BuildPart("t:texture", new string[] { atlasPath }, "atlasab", atlasOutputPath);
    }

    static void BuildPart(string filter, string[] findPaths, string abName, string outputPath)
    {
        CheckPath(outputPath, true);
        string[] guids = AssetDatabase.FindAssets(filter, findPaths);
        if (guids == null || guids.Length <= 0)
        {
            Debug.LogError("δ�ҵ�ƥ����Դ : texture");
            return;
        }
        int len = guids.Length;
        string[] paths = new string[len];

        for (int i = 0; i < len; ++i)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            if (path != null)
            {
                paths[i] = path;
            }
        }
        AssetBundleBuild abb = new AssetBundleBuild();
        abb.assetBundleName = abName;
        abb.assetNames = paths;
        BuildPipeline.BuildAssetBundles(outputPath, new AssetBundleBuild[] { abb }, BuildAssetBundleOptions.UncompressedAssetBundle, BuildTarget.StandaloneWindows64);
        AssetDatabase.Refresh();
    }


    /// <summary>
    /// �Զ�����·����ɾ��·���������ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isOutputPath">����������·��������Ҫ���棬�Ҳ���ɾ�����ļ������κ���Դ</param>
    static void CheckPath(string path, bool isOutputPath = false)
    {
        if (!Directory.Exists(path))
        {
            if (!isOutputPath)
            {
                Debug.LogError("��δ�ҵ���Ӧ·��,������Դ����·��.");
                return;
            }
            Directory.CreateDirectory(path);
        }
        else
        {
            if (!isOutputPath)
            {
                return;
            }
            string[] filePath = Directory.GetFiles(path);
            if (filePath != null && filePath.Length != 0)
            {
                int len = filePath.Length;
                for (int i = 0; i < len; i++)
                {
                    File.Delete(filePath[i]);
                }
            }
            string[] dirPath = Directory.GetDirectories(path);
            if (dirPath != null && dirPath.Length != 0)
            {
                int len = dirPath.Length;
                for (int i = 0; i < len; i++)
                {
                    Directory.Delete(dirPath[i]);
                }
            }
        }
    }


    [MenuItem("Tools/PrintPath")]
    static void PrintPaths()
    {
        Debug.Log("dataPath : " + Application.dataPath);
        Debug.Log("streamingAssetsPath : " + Application.streamingAssetsPath);
        Debug.Log("persistentDataPath : " + Application.persistentDataPath);
        Debug.Log("consoleLogPath : " + Application.consoleLogPath);
    }

}
