using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathMgr
{   
    //Ab����·��
    static readonly string LoadBase = Application.streamingAssetsPath;
    //Ԥ����
    public static readonly string PrefabAbPath = Path.Combine(LoadBase, "Prefabs");
    //ͼ��
    public static readonly string AtlasAbPath = Path.Combine(LoadBase, "Atlas");

}
