using Cysharp.Threading.Tasks;
using UI.Windows;

namespace Core.Managers.UI {
  public interface IUIManager {
    UniTask Initialize();
    T GetWindow<T>() where T : BaseUIWindow;
    T ShowWindow<T>() where T : BaseUIWindow;
    void HideWindow<T>() where T : BaseUIWindow;
  }
}
