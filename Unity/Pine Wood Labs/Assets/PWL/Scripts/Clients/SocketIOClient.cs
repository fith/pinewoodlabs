using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;


public class SocketIOClient : MonoBehaviour
{
    private SocketManager manager;
    // Use this for initialization
    void Awake() {
        this.manager = new SocketManager(new Uri("http://socket.pinewoodlabs.xyz/socket.io/"));
        this.manager.Open();
    }
    void Start()
    {
        //Socket sockChat = manager.GetSocket("/socket.io"); 
        //manager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) => Debug.LogError(string.Format("Error: {0}", args[0].ToString())));
        this.manager.Socket.On(SocketIOEventTypes.Connect, OnServerConnect);
        this.manager.Socket.On(SocketIOEventTypes.Disconnect, OnServerDisconnect);
        this.manager.Socket.On(SocketIOEventTypes.Error, OnError);
    }
    
    public void On(string socket_event, SocketIOCallback socket_callback) {
        //Debug.Log("Registering: " + socket_event);
        manager.Socket.On(socket_event, socket_callback);
    }

    public void Emit(string socket_event, object args = null) {
        //Debug.Log(socket_event);
        manager.Socket.Emit(socket_event, args);
    }

    void OnServerConnect(Socket socket, Packet packet, params object[] args)
    {
        //Debug.Log("Connected");
    }

    void OnServerDisconnect(Socket socket, Packet packet, params object[] args)
    {
        //Debug.Log("Disconnected");
    }

    void OnError(Socket socket, Packet packet, params object[] args)
    {
        Error error = args[0] as Error;
        switch (error.Code)
        {
            case SocketIOErrors.User:
                Debug.LogWarning("Exception in an event handler!");
                break;
            case SocketIOErrors.Internal:
                Debug.LogWarning("Internal error!");
                break;
            default:
                Debug.LogWarning("server error!");
                break;
        }
    }
}