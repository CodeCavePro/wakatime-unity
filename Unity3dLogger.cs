using UnityEngine;

namespace WakaTime.Unity
{
    public class Unity3dLogger : ILogService
    {
        public void Log(string message)
        {
            Debug.Log(message);
        }
    }
}
