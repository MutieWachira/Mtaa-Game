using NUnit.Framework;
using UnityEngine;

public class PlayerMovementTests
{
    [Test]
    public void ZeroInput_ShouldProduceZeroMovement()
    {
        Vector2 input = Vector2.zero;

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        );

        Assert.That(movement, Is.EqualTo(Vector3.zero));
    }

    [Test]
    public void ForwardInput_ShouldProduceForwardMovement()
    {
        Vector2 input = Vector2.up;

        Vector3 movement = new Vector3(
            input.x,
            0f,
            input.y
        );

        Assert.That(movement, Is.EqualTo(Vector3.forward));
    }
}