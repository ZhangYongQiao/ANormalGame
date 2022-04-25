using System.Collections.Generic;
using UnityEngine;

public static class Tools
{
    public static T JsonConverter<T>(string json)
    {
        if (string.IsNullOrEmpty(json))
        {
            Debug.LogError("json is null");
            return default(T);
        }

        T instance = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        return instance;
    }

    /// <summary>
    /// 创建物体
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="originGo"></param>
    /// <param name="localPos"></param>
    /// <param name="parent"></param>
    /// <returns></returns>
    public static T CreateGameObject<T>(T originGo, Vector3 localPos, Transform parent = null) where T : Component
    {
        if (!originGo)
        {
            Debug.LogError("originGo不可为空");
            return default(T);
        }
        var curGo = GameObject.Instantiate<T>(originGo, Vector3.zero,Quaternion.identity);
        if (!curGo)
        {
            Debug.LogError("创建物体失败...");
            return default(T);
        }
        if (parent)
        {
            curGo.transform.SetParent(parent);
            curGo.transform.localPosition = localPos;
        }
        else
        {
            curGo.transform.localPosition = localPos;
        }

        T comp = curGo.GetComponent<T>();
        if (comp)
        {
            return comp;
        }
        Debug.LogWarning("curGo不含T组件...");
        return default(T);
    }


    public static void RandomLocalPos(Transform tran,int minX = -5,int maxX = 5,int minZ = -5,int maxZ = 5,float y = 0.5f)
    {
        System.Random random = new System.Random();
        int x = random.Next(minX, maxX);
        int z = random.Next(minZ, maxZ);
        tran.localPosition = new Vector3(x, y, z);
    }


}
