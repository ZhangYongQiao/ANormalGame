using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

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
        thisWindow.maxSize = new Vector2(250, 300);
        thisWindow.Show();
    }

    bool atlas;
    bool prefabs;


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        atlas = EditorGUILayout.Toggle(atlas, GUILayout.Width(20));
        if (GUILayout.Button("图集打包", GUILayout.Width(100)))
        {
            BuildAtlas();
        }

        EditorGUILayout.EndHorizontal();
    }


    static void BuildAtlas()
    {
        CheckPath(atlasPath,false);

    }



    /// <summary>
    /// 自动生成路径并删除路径下所有文件
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isOutputPath">如果不是输出路径，就需要报告，且不能删除该文件夹下任何资源</param>
    static void CheckPath(string path,bool isOutputPath = false)
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
            if (filePath!=null && filePath.Length != 0)
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
        Debug.Log(Application.dataPath);
        Debug.Log(Application.streamingAssetsPath);
        Debug.Log(Application.persistentDataPath);
        Debug.Log(Application.consoleLogPath);
    }

}
