using UnityEngine.UI;

public class VictoryPanel : BasePanel
{
    public Button btnPush;
    public Button btnBack;
    public InputField inputName;
    public Text txtTime;

    private void Start()
    {
        btnPush.onClick.AddListener(() =>
        {
            
        });
        
        btnBack.onClick.AddListener(() =>
        {
            
        });
    }

    public void InitTime(float time)
    {
        int minute = (int)(time / 60);
        int second = (int)time % 60;

        txtTime.text = Helper.FormatTime(minute) + ":" + Helper.FormatTime(second);
    }
}
