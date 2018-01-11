using UnityEngine;
using System.Runtime.InteropServices;
using System.Collections.Generic;


namespace PWL
{
    public class MenuController : MonoBehaviour
    {

        public string sessionId;
        private List<ISessionListener> sessionListeners;

        [DllImport("__Internal")]
        private static extern void setSessionID(string sessionId);
        public void SetSessionId(string sessionId)
        {
            this.sessionId = sessionId;
        }

        public void addListener(ISessionListener listener)
        {
            sessionListeners.Add(listener);
        }

        public void removeListener(ISessionListener listener)
        {
            sessionListeners.Remove(listener);
        }
    }

    public interface ISessionListener
    {
        void setSessionId();
    }
}
