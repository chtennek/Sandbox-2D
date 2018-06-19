using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringUnityEvent : UnityEvent<string> { }

[System.Serializable]
public class FloatUnityEvent : UnityEvent<float> { }

[System.Serializable]
public class BoolUnityEvent : UnityEvent<bool> { }

[System.Serializable]
public class TransformUnityEvent : UnityEvent<Transform> { }

[System.Serializable]
public class Int2UnityEvent : UnityEvent<int, int> { }

[System.Serializable]
public class Int3UnityEvent : UnityEvent<int, int, int> { }

[System.Serializable]
public class Float2UnityEvent : UnityEvent<float, float> { }

[System.Serializable]
public class Float3UnityEvent : UnityEvent<float, float, float> { }