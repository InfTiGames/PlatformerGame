using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayModeTest
{
    [UnityTest]
    public IEnumerator MoveCalculate()
    {
        var go = new GameObject();
        MovementController moveControll = go.AddComponent<MovementController>();

        moveControll.Move(move: 15f, true, true);

        yield return null;

    }
}

