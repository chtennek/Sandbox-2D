using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuilder<T> where T : ScriptableObject {
    void Save();
    void Load(T data);
    void Reload();
    void Clear();
    T GetCurrent();
}
