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