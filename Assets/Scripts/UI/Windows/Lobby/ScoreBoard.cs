using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UI.Windows.Lobby {
  public class ScoreBoard : MonoBehaviour {
    [SerializeField]
    private RectTransform _container;
    [SerializeField]
    private AssetReference _scoreItemReference;

    private GameObject _scoreItemPrefab;
    private List<ScoreItem> _scoreItems;
    private List<int> _scoreList;
    private List<KeyValuePair<int, int>> _pointsSortedList;

    public async UniTask Initialize() {
      _scoreItemPrefab = await _scoreItemReference.LoadAssetAsync<GameObject>();
    }

    public void SetupScoreBoard(List<int> gamesPoints) {
      _scoreList = gamesPoints.ToList();
      _scoreItems = new List<ScoreItem>();
      _pointsSortedList = GetSortedList(gamesPoints);
      foreach (KeyValuePair<int, int> keyValuePair in _pointsSortedList) {
        ScoreItem scoreItem = CreateItem(keyValuePair.Key + 1, keyValuePair.Value);
        scoreItem.gameObject.transform.SetSiblingIndex(_scoreItems.Count);
        _scoreItems.Add(scoreItem);
      }
    }

    public void UpdateScoreBoard(List<int> gamesPoints) {
      if (gamesPoints.Count == 0 || gamesPoints.Count == _scoreList.Count) {
        return;
      }

      int addedValue = gamesPoints[^1];
      _scoreList.Add(addedValue);
      KeyValuePair<int, int> newPair = new KeyValuePair<int, int>(_scoreList.Count, addedValue);

      int insertIndex = _pointsSortedList.FindIndex(pair => newPair.Value > pair.Value);
      ScoreItem scoreItem = CreateItem(newPair.Key, newPair.Value);
      if (insertIndex == -1) {
        _pointsSortedList.Add(newPair);
      } else {
        scoreItem.gameObject.transform.SetSiblingIndex(insertIndex);
        _pointsSortedList.Insert(insertIndex, newPair);
      }
    }

    private List<KeyValuePair<int, int>> GetSortedList(List<int> gamesPoints) {
      List<KeyValuePair<int, int>> sortedPairs = gamesPoints.Select((value, index) => new KeyValuePair<int, int>(index, value)).OrderByDescending(pair => pair.Value).ToList();
      return sortedPairs;
    }

    private ScoreItem CreateItem(int gameNumber, int points) {
      GameObject scoreItemObj = Instantiate(_scoreItemPrefab, _container);
      ScoreItem scoreItem = scoreItemObj.GetComponent<ScoreItem>();
      scoreItem.SetupData(gameNumber, points);
      return scoreItem;
    }
  }

}
