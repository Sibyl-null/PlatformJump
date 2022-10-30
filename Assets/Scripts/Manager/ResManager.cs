using UnityEngine;

public class ResManager : BaseSingleton<ResManager>
{
    private ResManager(){}

    /// <summary>
    /// 异步加载资源
    /// </summary>
    public ResourceRequest LoadAsync<T>(string path) where T : Object
    {
        return Resources.LoadAsync<T>(path);
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    public T Load<T>(string path) where T : Object
    {
        return Resources.Load<T>(path);
    }
}
