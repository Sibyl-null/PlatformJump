using System.Collections.Generic;
using NetWork;
using UnityEngine;
using UnityEngine.UI;

public class RankPanel : BasePanel
{
    public Button btnBack;
    public Button btnRefresh;
    public ScrollRect svRank;
    public GameObject recordObj;

    private List<Text> _texts = new List<Text>();

    private void Start()
    {
        GenerateItem();
        
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.DestroyPanel(GlobalString.RANK_PANEL);
            UIManager.Instance.ShowPanel<StartPanel>(GlobalString.START_PANEL);
        });
        
        btnRefresh.onClick.AddListener(() =>
        {
            GetRankMsg msg = new GetRankMsg();
            NetManager.Instance.SendNoAsync(msg);
            Refresh();
        });
    }

    private void Refresh()
    {
        foreach (Text text in _texts)
        {
            Destroy(text.gameObject);
        }
        _texts.Clear();

        GenerateItem();
    }

    private void GenerateItem()
    {
        foreach (RecordData record in RecordRankModel.Instance.records)
        {
            GameObject obj = Instantiate(recordObj, svRank.content, false);
            obj.SetActive(true);
            Text txtRecord = obj.GetComponent<Text>();
            txtRecord.text = record.name + "  " + record.record + "s";
            _texts.Add(txtRecord);
        }
    }
}
