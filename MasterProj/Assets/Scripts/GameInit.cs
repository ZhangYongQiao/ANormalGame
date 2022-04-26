using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(LoadAb());
        StartCoroutine(LoadAtlas());
    }

    IEnumerator LoadAb()
    {
        string path = PathMgr.PrefabAbPath + "/prefabsab";
        AssetBundleCreateRequest abcr = AssetBundle.LoadFromFileAsync(path);
        yield return abcr;
        if (abcr.isDone)
        {
            var ab = abcr.assetBundle;
            GameObject go = ab.LoadAsset<GameObject>("UserGo");
            if (!go)
            {
                Debug.LogError($"º”‘ÿUserGo ß∞‹");
                yield return null;
            }
            EventManager.Send(EventName.LoadAIModelCompleted, new EventParam(go));
        }
        StopCoroutine(LoadAb());
    }

    IEnumerator LoadAtlas()
    {
        string path = PathMgr.AtlasAbPath + "/atlasab";

    }

}
