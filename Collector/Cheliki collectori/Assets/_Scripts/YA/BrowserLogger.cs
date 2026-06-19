using UnityEngine;

public class BrowserLogger : MonoBehaviour
{
    void Awake()
    {
        Application.logMessageReceived += (condition, stackTrace, type) =>
        {
            string escaped = condition.Replace("\\", "\\\\").Replace("\"", "\\\"");
            string js = "";
            switch (type)
            {
                case LogType.Warning:
                    js = $"console.warn(\"Unity: {escaped}\")";
                    break;
                case LogType.Error:
                case LogType.Exception:
                    js = $"console.error(\"Unity: {escaped}\")";
                    break;
                default:
                    js = $"console.log(\"Unity: {escaped}\")";
                    break;
            }
            Application.ExternalEval(js);
        };
    }
}