using UnityEngine.UI;

public class DebugPanel : BasePanel
{
    public Text txtDebug;

    private void OnEnable()
    {
        DebugModel.Instance.Message.OnValueChanged += Refresh;

        Refresh(DebugModel.Instance.Message.Value);
    }

    private void OnDisable()
    {
        DebugModel.Instance.Message.OnValueChanged -= Refresh;
    }

    private void Refresh(string value)
    {
        txtDebug.text = value;
    }
}
