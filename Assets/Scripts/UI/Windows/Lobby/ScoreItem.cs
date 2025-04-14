using TMPro;
using Tools.Constants;
using UnityEngine;

namespace UI.Windows.Lobby {
  public class ScoreItem : MonoBehaviour {
    [SerializeField]
    private TMP_Text _gameNumber;
    [SerializeField]
    private TMP_Text _scoreAmount;

    public void SetupData(int gameNumber, int score) {
      _gameNumber.text = $"{UIConstants.Game}{gameNumber}";
      _scoreAmount.text = score.ToString();
    }
  }
}
