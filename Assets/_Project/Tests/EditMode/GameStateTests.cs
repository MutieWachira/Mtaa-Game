using NUnit.Framework;

public sealed class GameStateTests
{
    [Test]
    public void GameState_ShouldContainPlayingState()
    {
        Assert.That(
            GameState.Playing,
            Is.EqualTo(GameState.Playing)
        );
    }
}