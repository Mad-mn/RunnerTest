using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.ObjectPool {
  public interface IObjectPoolManager {
    UniTask Initialize();
    T GetFromPool<T>() where T : MonoBehaviour, IPoolable;
    T GetFromPool<T>(Type objType) where T : MonoBehaviour, IPoolable;
    void ReturnToPool<T>(T returnedObject) where T : MonoBehaviour, IPoolable;
  }
}
