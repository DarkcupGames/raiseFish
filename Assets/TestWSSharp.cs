using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;
using WebSocketSharp.Server;

public class TestWSSharp : MonoBehaviour
{
    
    void Start()
    {
        Debug1.Log("starting web socket sharp");

        using (var ws = new WebSocket("wss://fishtankserver.herokuapp.com/ws"))
        {
            ws.OnMessage += (sender, e) =>
            Debug1.Log("Message receive: " + e.Data);
            //Console.WriteLine("Laputa says: " + e.Data);

            ws.Connect();
            ws.Send("BALUS");
 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
