using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using BestHTTP;
using BestHTTP.SocketIO;
using BestHTTP.SocketIO.Events;

namespace PWL
{
    public class SocketIOClient : MonoBehaviour
    {
        private SocketManager manager;
        private Socket socket;
        // Use this for initialization
        void Connect(string socketName)
        {
            manager = new SocketManager(new Uri("https://socket.pinewoodlabs.xyz/socket.io/"));
            socket = manager.GetSocket("/" + socketName);
            InitSocketEvents();
        }

        void InitSocketEvents()
        {
            socket.On(SocketIOEventTypes.Error, OnError);
        }

        public void On(string socket_event, SocketIOCallback socket_callback)
        {
            socket.On(socket_event, socket_callback);
        }

        public void Emit(string socket_event, object args = null)
        {   
            socket.Emit(socket_event, args);
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
}