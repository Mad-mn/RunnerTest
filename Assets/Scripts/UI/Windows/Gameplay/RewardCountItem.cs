using Configs.RewardConfigs;
using TMPro;
using UnityEngine;

namespace UI.Windows.Gameplay {
  public class RewardCountItem : MonoBehaviour {
    [SerializeField]
    private RewardType _rewardType;
    [SerializeField]
    private TMP_Text _countTxt;

    private int _currentAmount;

    public void SetToDefault() {
      _currentAmount = 0;
      UpdateTxt();
    }

    public void IncreaseCount() {
      _currentAmount++;
      UpdateTxt();
    }

    private void UpdateTxt() {
      _countTxt.text = _currentAmount.ToString();
    }

    public RewardType RewardType {
      get {
        return _rewardType;
      }
    }
  }
}
