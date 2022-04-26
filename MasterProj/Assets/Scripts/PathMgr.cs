using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PathMgr
{   
    //Ab加载路径
    static readonly string LoadBase = Application.streamingAssetsPath;
    //预制体
    public static readonly string PrefabAbPath = Path.Combine(LoadBase, "Prefabs");
    //图集
    public static readonly string AtlasAbPath = Path.Combine(LoadBase, "Atlas");

}
