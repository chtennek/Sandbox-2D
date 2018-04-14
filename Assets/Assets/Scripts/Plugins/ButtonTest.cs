using UnityEngine;
using System.Reflection;

public class ButtonTest : MonoBehaviour {

    [Button("Method2", "Method2 button", true, BindingFlags.NonPublic | BindingFlags.Instance)] public int test1;

    [Button("Method2", true, BindingFlags.NonPublic | BindingFlags.Instance)] public int test2;

    [Button("Method1", true)] public int test3;

    [Button("Method4", "Method4 button", BindingFlags.NonPublic | BindingFlags.Instance)] public int test4;

    [Button("Method3", "Method3 button")] public int test5;

    [Button("Method4", BindingFlags.NonPublic | BindingFlags.Instance)] public int test6;

    [Button("Method3")] public int test7;

    public void Method1(int i) {
        Debug.Log("Public Method with Value: "+i.ToString());
    }

    void Method2(int i) {
        Debug.Log("Private Method with Value: "+i.ToString());
    }

    public void Method3() {
        Debug.Log("Public Method without Value.");
    }

    void Method4() {
        Debug.Log("Private Method without Value.");
    }
}