using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo
{
    public int type;
    public User user;
    public string content;

    public ulong GetId()
    {   
        if(ulong.TryParse(user.id, out ulong id))
        {
            return id;
        }
        return 0;
    }
}

public class User
{
    public string id { get; set; }
    public string shortId { get; set; }
    public string nickname { get; set; }
    public int gender { get; set; }
    public int level { get; set; }
}


public enum InstructionType
{
    Enter = 1,
    GetGift = 2,
    Chat = 3
}