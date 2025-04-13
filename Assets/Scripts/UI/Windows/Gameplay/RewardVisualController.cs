using System.Collections.Generic;
using Configs;
using Configs.RewardConfigs;
using TMPro;
using UnityEngine;
using Zenject;

namespace UI.Windows.Gameplay {
  public class RewardVisualController : MonoBehaviour {
    [SerializeField]
    private List<RewardCountItem> _rewardCountItems;
    [SerializeField]
    private TMP_Text _totalPointTxt;

    private RewardConfig _rewardConfig;

    private int _totalPoint;

    private void Awake() {
      InitComponents();
    }

    public void SetToDefault() {
      foreach (RewardCountItem item in _rewardCountItems) {
        item.SetToDefault();
      }

      _totalPoint = 0;
      SetTotalPointTxt();
    }

    public void OnPlayerCatchReward(RewardType rewardType) {
      RewardCountItem item = _rewardCountItems.Find(item => item.RewardType == rewardType);
      item.IncreaseCount();
      UpdateTotalPoint(rewardType);
    }

    private void UpdateTotalPoint(RewardType rewardType) {
      int addedPoints = _rewardConfig.GetPointsForItem(rewardType);
      _totalPoint += addedPoints;
      SetTotalPointTxt();
    }

    private void SetTotalPointTxt() {
      _totalPointTxt.text = $"Total: {_totalPoint}";
    }

    private void InitComponents() {
      _rewardConfig = ProjectContext.Instance.Container.Resolve<IConfigManager>().GetConfig<RewardConfig>();
    }
  }
}
