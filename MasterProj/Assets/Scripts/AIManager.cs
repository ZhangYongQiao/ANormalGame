using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    private static AIManager _Instance;
    public static AIManager Instance
    {
        get
        {
            if (_Instance == null)
            {
                Debug.LogError("AIManager == null");
                return null;
            }
            return _Instance;
        }
    }

    public AI AIModel;
    //TODO 使用对象池
    public Transform CreatedContainer;

    public AIBaseInfo AiBaseInfo;

    private void Awake()
    {
        _Instance = this;
    }

    //所有推送事件
    public Queue<UserInfo> MsgQueue  { get; private set; }  = new Queue<UserInfo>();

    //保存所有当前用户对应的游戏对象信息
    public Dictionary<ulong,AI> UserDic { get; private set; } = new Dictionary<ulong, AI>();

    /// <summary>
    /// 固定处理产生的消息
    /// </summary>
    private void FixedUpdate()
    {
        if (MsgQueue.Count > 0)
        {
            while (MsgQueue.Count > 0)
            {
                UserInfo curMsg = MsgQueue.Dequeue();
                if (curMsg != null && int.Parse(curMsg.user.id) != 0)
                {
                    MsgEnQueue(curMsg);
                }
            }
        }
    }

    public void MsgEnQueue(UserInfo userInfo)
    {
        switch ((InstructionType)userInfo.type)
        {
            case InstructionType.Enter:
                NewUserHandler(userInfo);
                break;
            case InstructionType.GetGift:
                GetGiftHandler(userInfo);
                break;
            case InstructionType.Chat:
                SendChatHandler(userInfo);
                break;
            default:
                Debug.LogError($"未知type类型 type :{userInfo.type}");
                break;
        }
    }

    /// <summary>
    /// 新用户处理
    /// </summary>
    private void NewUserHandler(UserInfo userInfo)
    {
        ulong id = userInfo.GetId();
        if (id != 0)
        {
            //这里只对唯一id做了判断，可能还有其它的，比如头像等,后期优化可以在id已存在的情况下，更新user里面的其他数据
            if (!UserDic.ContainsKey(id))
            {
                Debug.Log(Thread.CurrentThread.ManagedThreadId);

                //新建用户实例-->后期需求可能不会让新用户及时加载，可能会保存到加载队列里面
                var ai = Tools.CreateGameObject<AI>(AIModel,Vector2.zero, CreatedContainer);
                if (ai)
                {
                    ai.GetComponent<AI>().SetBaseInfo(userInfo);
                    UserDic.Add(id, ai.GetComponent<AI>());
                }
            }
            else
            {
                //TODO 更新头像等数据
            }
        }
        else
        {
            Debug.LogError("用户Id解析失败...");
        }
    }

    /// <summary>
    /// 送礼用户处理
    /// </summary>
    private void GetGiftHandler(UserInfo userInfo)
    {
        Debug.Log($"id:{userInfo.GetId()} 送了礼物...");
    }

    /// <summary>
    /// 发送弹幕处理
    /// </summary>
    /// <param name="userInfo"></param>
    private void SendChatHandler(UserInfo userInfo)
    {
        ulong id = userInfo.GetId();
        if (!UserDic.ContainsKey(id))
        {
            NewUserHandler(userInfo);
        }
        AI ai = UserDic[id];
        //如果对应对象是空就直接移除
        if (!ai)
        {
            UserDic.Remove(id);
            Debug.LogError("发送弹幕用户不存在对应游戏实例，请检查...");
        }
        else
        {
            ai.SendBulletChatContent(userInfo.content);
        }
    }

}
