using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AI : MonoBehaviour
{
    public AIBaseInfo _AIBaseInfo;
    public TextMesh Content;
    public Animator SelfAnimator;
    public SpriteRenderer Renderer;

    public UserInfo _CurInfo { get; private set; }

    private void Awake()
    {
        SelfAnimator = GetComponent<Animator>();
        StartCoroutine(DownLoadImg("https://p3.douyinpic.com/aweme/100x100/aweme-avatar/tos-cn-avt-0015_fe30c9d48d44a36f179a5c5c4b021026.jpeg?from=4010531038"));
    }

    public void SetBaseInfo(UserInfo userInfo)
    {
        if (userInfo==null)
        {
            Debug.LogError("userInfo==null");
            return;  
        }
        _CurInfo = userInfo;
        Tools.RandomLocalPos(transform);
    }

    /// <summary>
    ///download img
    /// </summary>
    IEnumerator DownLoadImg(string imgUri)
    {
        UnityWebRequest uwr = UnityWebRequest.Get(imgUri);
        DownloadHandlerTexture handlerTexture = new DownloadHandlerTexture(true);
        uwr.downloadHandler = handlerTexture;
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.Success)
        {
            Sprite sprite = Sprite.Create(handlerTexture.texture, new Rect(Vector2.zero, new Vector2(handlerTexture.texture.width, handlerTexture.texture.height)), Vector2.zero);
            if (!sprite)
            {
                Debug.LogError("DownLoad sprite Failed...");
                yield return null;
            }
            Renderer.sprite = sprite;
        }
        StopCoroutine(DownLoadImg(imgUri));
    }

    #region Behaviour

    private void MoveTo(Vector3 destination)
    {
        var tweener = transform.DOMove(destination, 2f);

        MoveDir moveDir = GetMoveDirection(transform.position, destination);

        tweener.onPlay = () =>
        {
            SelfAnimator.enabled = true;
            switch (moveDir)
            {
                case MoveDir.Right:
                    SelfAnimator.Play(_AIBaseInfo.clips[3].name);
                    break;
                case MoveDir.Left:
                    SelfAnimator.Play(_AIBaseInfo.clips[2].name);
                    break;
                case MoveDir.Forward:
                    break;
                case MoveDir.Back:
                    break;
                default:
                    break;
            }
        };
        tweener.onComplete = () =>
        {
            SelfAnimator.Play(_AIBaseInfo.clips[4].name);
            Debug.Log($"移动到：{destination.ToString()}");
        };
    }

    private MoveDir GetMoveDirection(Vector3 originPos,Vector3 destinationPos)
    {
        return originPos.x>destinationPos.x?MoveDir.Left:MoveDir.Right;
    }


    /// <summary>
    /// 发送弹幕.后期需求可能需要解析弹幕
    /// </summary>
    /// <param name="content">弹幕内容</param>
    public void SendBulletChatContent(string content)
    {
        Debug.Log($"弹幕信息：id:{_CurInfo.GetId()} --> {content}");
        Content.text = content;
        Tweener tweener = Content.transform.DOLocalMoveY(2f, 0.5f);
        tweener.onComplete = () =>
        {
            Content.text = null;
            Vector3 pos = Content.transform.localPosition;
            Content.transform.localPosition = new Vector3(pos.x, 1f, pos.z);
        };
    }

    private void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    #endregion
}

public enum MoveDir
{
    Right,
    Left,
    Forward,
    Back
}