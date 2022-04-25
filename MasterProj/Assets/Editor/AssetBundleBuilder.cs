using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

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
        thisWindow.maxSize = new Vector2(250, 300);
        thisWindow.Show();
    }

    bool atlas;
    bool prefabs;


    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        atlas = EditorGUILayout.Toggle(atlas, GUILayout.Width(20));
        if (GUILayout.Button("ͼ�����", GUILayout.Width(100)))
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
    /// �Զ�����·����ɾ��·���������ļ�
    /// </summary>
    /// <param name="path"></param>
    /// <param name="isOutputPath">����������·��������Ҫ���棬�Ҳ���ɾ�����ļ������κ���Դ</param>
    static void CheckPath(string path,bool isOutputPath = false)
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
