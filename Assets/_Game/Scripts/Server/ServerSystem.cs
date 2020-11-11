using NativeWebSocket;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ServerSystem : MonoBehaviour
{
    public static string SERVER_URL = "ws://fishtankserver.herokuapp.com/ws";
    public static ServerSystem Instance;
    public static Dictionary<string, SyncPosition> dic;
    public static bool sendRequest = false;
    public static string playerid;

    public Joystick joystick;
    public GameObject player;
    public float speed = 4f;

    WebSocket websocket;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        dic = new Dictionary<string, SyncPosition>();
        OnlineData data = new OnlineData();
        data.id = playerid;
        data.desirePos = player.transform.position;
        data.speed = 4f;
        player.GetComponent<SyncPosition>().data = data;
        player.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerid;
        dic.Add(playerid, player.GetComponent<SyncPosition>());
    }

    async void Start()
    {
        websocket = new WebSocket(SERVER_URL);

        websocket.OnOpen += () =>
        {
            Debug1.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug1.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug1.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            var message = System.Text.Encoding.UTF8.GetString(bytes);

            OnlineData sync = JsonUtility.FromJson<OnlineData>(message);

            if (!dic.ContainsKey(sync.id))
            {
                GameObject go = Instantiate(player, sync.desirePos, Quaternion.identity);
                var comp = go.GetComponent<SyncPosition>().data;
                comp.id = sync.id;
                comp.desirePos = sync.desirePos;
                comp.state = sync.state;
                comp.speed = sync.speed;

                go.transform.GetChild(0).GetComponent<TextMeshPro>().text = sync.id;

                dic.Add(sync.id, go.GetComponent<SyncPosition>());
            }
            else
            {
                if (sync.id != playerid)
                {
                    dic[sync.id].data.desirePos = sync.desirePos;
                    dic[sync.id].data.state = sync.state;
                }
                    
            }
        };

        InvokeRepeating("SendWebSocketMessage", 0.0f, 0.3f);

        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        websocket.DispatchMessageQueue();
#endif

        if (!sendRequest)
            return;

        if (dic.ContainsKey(playerid))
        {
            if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                dic[playerid].data.state = "stand";
            }
            else
            {
                dic[playerid].data.state = "walk";
                Vector3 pos = new Vector3(joystick.Horizontal, joystick.Vertical).normalized * 4f * Time.deltaTime;

                dic[playerid].transform.position += pos;
                dic[playerid].data.desirePos = dic[playerid].transform.position;
            }
        }
    }

    async void SendWebSocketMessage()
    {
        if (!sendRequest)
            return;

        if (websocket.State == WebSocketState.Open)
        {
            await websocket.SendText(JsonUtility.ToJson(dic[playerid].data));
        }
    }

    private async void OnApplicationQuit()
    {
        await websocket.Close();
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                     