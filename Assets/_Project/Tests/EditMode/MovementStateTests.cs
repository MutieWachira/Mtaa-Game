using NUnit.Framework;

public class MovementStateTests
{
    [Test]
    public void IdleState_ShouldExist()
    {
        Assert.That(
            MovementState.Idle,
            Is.EqualTo(MovementState.Idle)
        );
    }

    [Test]
    public void WalkingState_ShouldExist()
    {
        Assert.That(
            MovementState.Walking,
            Is.EqualTo(MovementState.Walking)
        );
    }

    [Test]
    public void RunningState_ShouldExist()
    {
        Assert.That(
            MovementState.Running,
            Is.EqualTo(MovementState.Running)
        );
    }

    [Test]
    public void JumpingState_ShouldExist()
    {
        Assert.That(
            MovementState.Jumping,
            Is.EqualTo(MovementState.Jumping)
        );
    }

    [Test]
    public void FallingState_ShouldExist()
    {
        Assert.That(
            MovementState.Falling,
            Is.EqualTo(MovementState.Falling)
        );
    }
}