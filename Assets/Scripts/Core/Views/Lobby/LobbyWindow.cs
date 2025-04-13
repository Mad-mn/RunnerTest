using Core.Loaders.Scene;
using Core.Managers.UI;
using Core.SaveLoadDataSystem;
using Core.SaveLoadDataSystem.SavedData;
using Core.Views.Gameplay;
using Cysharp.Threading.Tasks;
using Tools.Constants;
using UI.Windows;
using UI.Windows.Lobby;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Core.Views.Lobby {
  public class LobbyWindow : BaseUIWindow {
    [SerializeField]
    private Button _playButton;
    [SerializeField]
    private ScoreBoard _scoreBoard;

    private IUIManager _uiManager;
    private ISceneLoader _sceneLoader;
    private IDataHandler _dataHandler;

    private void Start() {
      InitComponents();
    }

    private void OnEnable() {
      AddListeners();
    }

    private void OnDisable() {
      RemoveListeners();
    }

    public override async UniTask Initialize() {
      await InitScoreBoard();
    }

    public override void Show() {
      base.Show();
      _scoreBoard.UpdateScoreBoard(_dataHandler.GetData<PlayerData>().GetListOfGames());
    }

    private void OnPlayButton() {
      LoadPlayScene().Forget();
      _uiManager.HideWindow<LobbyWindow>();
      _uiManager.ShowWindow<GameplayWindow>();
    }

    private async UniTask InitScoreBoard() {
      await _scoreBoard.Initialize();
      PlayerData playerData = _dataHandler.GetData<PlayerData>();
      _scoreBoard.SetupScoreBoard(playerData.GetListOfGames());
    }

    private async UniTaskVoid LoadPlayScene() {
      await _sceneLoader.LoadSceneAsync(SceneNameConstants.GameplaySceneKey);
    }

    private void AddListeners() {
      _playButton.onClick.AddListener(OnPlayButton);
    }

    private void RemoveListeners() {
      _playButton.onClick.RemoveListener(OnPlayButton);
    }

    private void InitComponents() {
      DiContainer container = ProjectContext.Instance.Container;
      _sceneLoader = container.Resolve<ISceneLoader>();
      _uiManager = container.Resolve<IUIManager>();
      _dataHandler = container.Resolve<IDataHandler>();
    }
  }
}
