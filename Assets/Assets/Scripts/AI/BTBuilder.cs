using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BTAI;

public enum BTNodeType
{
    Root,
    Sequence,
    Selector,
    RunCoroutine,
    Call,
    If,
    While,
    Condition,
    Repeat,
    Wait,
    Trigger,
    WaitForAnimatorState,
    SetBool,
    SetActive,
    WaitForAnimatorSignal,
    Terminate,
    RandomSequence,
}

[System.Serializable]
public class BTBuilderNode
{
    public BTNodeType type;
    public Condition conditional;
    public SerializableEvent callback;

    public BTBuilderNode[] children;
}

public class BTBuilder : MonoBehaviour
{
    public BTBuilderNode behaviourTree;

    private Root aiRoot;

    private void Awake()
    {
        aiRoot = BT.Root();
        aiRoot.OpenBranch(
            BT.If(True).OpenBranch(
                BT.Call(BuildTree),
                BT.Call(BuildTree)
            ),
            BT.Sequence().OpenBranch(
                BT.Call(BuildTree),
                BT.Wait(5.0f),
                BT.Call(BuildTree),
                BT.Wait(1.0f),
                BT.Call(BuildTree)
                )
            );
    }

	private void Update()
	{
        aiRoot.Tick();
	}

    private void BuildTree() {
        
    }

    private bool True() {
        return true;
    } 
}
