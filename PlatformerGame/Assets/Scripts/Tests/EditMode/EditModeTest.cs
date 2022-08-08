using NUnit.Framework;
using UnityEngine;

public class EditModeTest
{   
    [Test]
    public void NewTestScriptSimplePasses()
    {
        Assert.AreEqual(expected: false, actual: MovementController._isGrounded);
    }
}
