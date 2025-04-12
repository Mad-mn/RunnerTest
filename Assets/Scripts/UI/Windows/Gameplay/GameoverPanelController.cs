using UnityEngine;

namespace UI.Windows.Gameplay {
  public class GameoverPanelController : MonoBehaviour {
    [SerializeField]
    private GameObject _gameOverPanel;

    private void OnDisable() {
      ChangePanelState(false);
    }

    public void GameOver() {
      ChangePanelState(true);
    }

    private void ChangePanelState(bool show) {
      _gameOverPanel.SetActive(show);
    }
  }
}
