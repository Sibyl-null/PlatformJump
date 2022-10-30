using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    private Transform _canvasTrans;

    private Dictionary<string, BasePanel> _panelDic = new Dictionary<string, BasePanel>();

    private bool _hasInit = false;

    public void Init()
    {
        _canvasTrans = Instantiate(ResManager.Instance.Load<GameObject>(GlobalString.UICANVAS_PREFAB)).transform;
        DontDestroyOnLoad(_canvasTrans.gameObject);
        _hasInit = true;
    }

    /// <summary>
    /// 同步加载panel
    /// </summary>
    /// <param name="panelPath"> panel资源路径 </param>
    public T ShowPanel<T>(string panelPath) where T : BasePanel
    {
        if (!_hasInit) Init();
        if (_panelDic.TryGetValue(panelPath, out BasePanel panel))
        {
            if (panel.gameObject.activeSelf == false) panel.gameObject.SetActive(true);
            return panel as T;
        }

        GameObject obj = Instantiate(ResManager.Instance.Load<GameObject>(panelPath), _canvasTrans, false);
        panel = obj.GetComponent<T>();
        _panelDic.Add(panelPath, panel);
        
        return (T)panel;
    }

    /// <summary>
    /// 隐藏panel
    /// </summary>
    public void HidePanel(string panelPath)
    {
        if (!_hasInit) Init();
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
        if (!_hasInit) Init();
        if (_panelDic.TryGetValue(panelPath, out BasePanel panel))
        {
            Destroy(panel.gameObject);
            _panelDic.Remove(panelPath);
        }
        else
            Debug.LogError("不存在该panel: " + panelPath);
    }

    /// <summary>
    /// 获取panel
    /// </summary>
    public T GetPanel<T>(string panelPath) where T : BasePanel
    {
        if (!_hasInit) Init();
        if (_panelDic.TryGetValue(panelPath, out BasePanel panel))
        {
            return panel as T;
        }
        else
        {
            Debug.LogError("不存在该panel: " + panelPath);
            return default(T);
        }
    }

    /// <summary>
    /// 清空当前所有panel
    /// </summary>
    public void DestroyAllPanel()
    {
        foreach (BasePanel panel in _panelDic.Values)
        {
            Destroy(panel.gameObject);
        }
        _panelDic.Clear();
    }
}
