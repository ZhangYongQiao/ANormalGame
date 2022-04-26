using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Timers;
using System;

public class AssetBundleBuilder : EditorWindow
{
    //******以下均是文件夹路径 output 表示打包后的输出路径
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
        if (GUILayout.Button("图集打包", GUILayout.Width(baseWidth)))
        {
            BuildAtlas();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        prefabs = EditorGUILayout.Toggle(prefabs, GUILayout.Width(toggleWidth));
        if (GUILayout.Button("预制体打包", GUILayout.Width(baseWidth)))
        {
            BuildPrefabs();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(30);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("全选", GUILayout.Width(100)))
        {
            atlas = true;
            prefabs = true;
            //Add
        }
        if (GUILayout.Button("全不选", GUILayout.Width(100)))
        {
            atlas = false;
            prefabs = false;
            //Add
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(20);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("打包", GUILayout.Width(thisWindow.minSize.x), GUILayout.Height(50)))
        {
            BuildAtlas();
            BuildPrefabs();
            //Other...

        }
        EditorGUILayout.EndHorizontal();

    }

    /// <summary>
    /// 预制体打包
    /// </summary>
    static void BuildPrefabs()
    {
        if (!prefabs) return;
        CheckPath(prefabsPath);
        BuildPart("t:prefab", new string[] { prefabsPath }, "prefabsab", prefabsOutputPath);
    }

    /// <summary> 图集打包 </summary>
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
            Debug.LogError("未找到匹配资源 : texture");
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
    /// 自动生成路径并删除路径下所有文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isOutputPath">如果不是输出路径，就需要报告，且不能删除该文件夹下任何资源</param>
    static void CheckPath(string path, bool isOutputPath = false)
    {
        if (!Directory.Exists(path))
        {
            if (!isOutputPath)
            {
                Debug.LogError("并未找到对应路径,请检查资源所在路径.");
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
