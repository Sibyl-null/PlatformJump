using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Transform _canvasTrans;

    private Dictionary<string, BasePanel> _panelDic = new Dictionary<string, BasePanel>();

    private async void Awake()
    {
        ResourceRequest request = ResManager.Instance.LoadAsync<GameObject>(GlobalString.UICANVAS_PREFAB);
        await request;
        _canvasTrans = Instantiate(request.asset as GameObject).transform;
    }

    /// <summary>
    /// 同步加载panel
    /// </summary>
    /// <param name="panelPath"> panel资源路径 </param>
    public T ShowPanel<T>(string panelPath) where T : BasePanel
    {
        if (_panelDic.TryGetValue(panelPath, out BasePanel panel))
        {
            if (panel.gameObject.activeSelf == false) panel.gameObject.SetActive(true);
            return panel as T;
        }

        GameObject obj = ResManager.Instance.Load<GameObject>(panelPath);
        panel = Instantiate(obj).GetComponent<T>();
        obj.transform.SetParent(_canvasTrans);
        _panelDic.Add(panelPath, panel);
        
        return (T)panel;
    }

    /// <summary>
    /// 隐藏panel
    /// </summary>
    public void HidePanel(string panelPath)
    {
        if (_panelDic.TryGetValue(panelPath, out BasePanel panel))
        {
            panel.gameObject.SetActive(false);
        }
        else
            Debug.LogError("不存在该panel: " + panelPath);
    }

    /// <summary>
    /// 销毁panel
    /// </summary>
    public void DestroyPanel(string panelPath)
    {
        if (_panelDic.TryGetValue(panelPath, out BasePanel panel))
        {
            Destroy(panel.gameObject);
            _panelDic.Remove(panelPath);
        }
        else
            Debug.LogError("不存在该panel: " + panelPath);
    }
}
