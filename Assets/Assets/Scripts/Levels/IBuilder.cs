using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Levels;

public interface IBuilder {
    void Save();
    void Load(Level data);
    void Reload();
    void Clear();
    Level GetCurrent();
}
