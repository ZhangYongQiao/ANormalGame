using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System;
using System.Text;
using WebSocketSharp;
using System.Threading;

public class NetClient : MonoBehaviour
{
    const string DomainName = "ws://iuqaq.com:9545/ws/message?device=a";
    private WebSocket webSocket;

    //const int MAX_BUFFER_SIZE = 255;
    //const int ServePort = 9545;
    //private byte[] buffer = new byte[255];
    //TcpClient tcpClient;

    private void Awake()
    {
        //ConnectServe();
        //WebSocketConnect();
        Debug.Log("已关闭网络...");
    }

    #region Useless
    /*
    void ConnectServe()
    {
        tcpClient = new TcpClient(AddressFamily.InterNetwork);

        //IPHostEntry apas = Dns.GetHostEntry("ws://iuqaq.com:9545/ws/message?device=a");
        IPAddress[] apas = Dns.GetHostAddresses("39.105.146.150");

        tcpClient.BeginConnect(apas[0], ServePort, BeginConnectHandler, tcpClient);
    }

    private void BeginConnectHandler(IAsyncResult ar)
    {
        try
        {
            TcpClient client = ar.AsyncState as TcpClient;
            client.EndConnect(ar);

            NetworkStream stream = client.GetStream();

            if (stream == null)
            {
                Debug.Log("stream == null");
                return;
            }
            string str = "ddddddddddddddddddddddddddddddddddddddd";
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            stream.BeginWrite(bytes, 0, bytes.Length, (ar) =>
            {
                stream.EndWrite(ar);
                Debug.Log("dddddddddddddd");

            }, null);
            Debug.Log("Connect success..");
            stream.BeginRead(buffer, 0, MAX_BUFFER_SIZE, BeginReadHandler, stream);
        }
        catch (Exception)
        {
            Debug.LogError("Connect failed...");
            Disconnect();
        }


    }

    private void BeginReadHandler(IAsyncResult ar)
    {
        NetworkStream stream = ar.AsyncState as NetworkStream;
        int count = stream.EndRead(ar);

        if (count < 0)
        {
            Disconnect();
            return;
        }
        if (count > MAX_BUFFER_SIZE)
        {
            Debug.LogError($"接受字节数量大于了每次最大接受字节数,丢弃。。。len:{count}");
            stream.BeginRead(buffer, 0, MAX_BUFFER_SIZE, BeginReadHandler, stream);
        }
        else
        {
            string json = Encoding.UTF8.GetString(buffer, 0, MAX_BUFFER_SIZE);
            Debug.Log($"Get Received Info...  bytes len --> {count}");
            if (!string.IsNullOrEmpty(json))
            {
                //TODO解析Json
                UserInfo behaviourMsg = Tools.JsonConverter<UserInfo>(json);
                AIManager.Instance.MsgQueue.Enqueue(behaviourMsg);
            }

            stream.BeginRead(buffer, 0, MAX_BUFFER_SIZE, BeginReadHandler, stream);
        }
    }

    void Disconnect()
    {
        tcpClient?.Dispose();
        tcpClient?.Close();
        Debug.Log("关闭连接.....");
    }
    */

    #endregion

    private void OnDisable()
    {

    }

    private void OnDestroy()
    {   
        webSocket?.Close();
    }

    public void WebSocketConnect()
    {
        webSocket = new WebSocket(DomainName);

        webSocket.ConnectAsync();
        Debug.Log("Connect success...");

        // 接收到消息并处理
        webSocket.OnMessage += (sender, e) =>
        {
            string json =  e.Data;
            UserInfo userInfo = Tools.JsonConverter<UserInfo>(json);

            if(userInfo != null)
            {   
                AIManager.Instance.MsgQueue.Enqueue(userInfo);
            }
            else
            {
                Debug.Log("DeserializeObject Failed...");
            }
        };

        webSocket.OnClose += (sender, e) =>
        {
            Debug.LogWarning("Socket Disconnect...");
        };

        webSocket.OnError += (sender, e) =>
        {   
            Debug.LogError($"{webSocket.ReadyState}  ConnectState Error...  --> {e.Message}");
        };
    }


}
