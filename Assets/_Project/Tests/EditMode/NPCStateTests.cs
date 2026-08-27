using NUnit.Framework;

public class NPCStateTests
{
    [Test]
    public void IdleState_ShouldExist()
    {
        Assert.That(
            NPCState.Idle,
            Is.EqualTo(NPCState.Idle)
        );
    }

    [Test]
    public void DestinationState_ShouldExist()
    {
        Assert.That(
            NPCState.GoingToDestination,
            Is.EqualTo(
                NPCState.GoingToDestination
            )
        );
    }

    [Test]
    public void WorkingState_ShouldExist()
    {
        Assert.That(
            NPCState.Working,
            Is.EqualTo(NPCState.Working)
        );
    }

    [Test]
    public void ShoppingState_ShouldExist()
    {
        Assert.That(
            NPCState.Shopping,
            Is.EqualTo(NPCState.Shopping)
        );
    }

    [Test]
    public void GoingHomeState_ShouldExist()
    {
        Assert.That(
            NPCState.GoingHome,
            Is.EqualTo(NPCState.GoingHome)
        );
    }

    [Test]
    public void FleeingState_ShouldExist()
    {
        Assert.That(
            NPCState.Fleeing,
            Is.EqualTo(NPCState.Fleeing)
        );
    }
}