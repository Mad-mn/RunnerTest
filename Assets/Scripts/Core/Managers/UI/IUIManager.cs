using Cysharp.Threading.Tasks;
using UI.Windows;

namespace Core.Managers.UI {
  public interface IUIManager {
    UniTask Initialize();

    T ShowWindow<T>() where T : BaseUIWindow;

    void HideWindow<T>() where T : BaseUIWindow;
  }
}
