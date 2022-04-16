using NUnit.Framework;

namespace Tests
{
    public class GameFeatures
    {
    int x = 0;
        [Test]
        public void GameFeaturesSimplePasses()
        {
      Assert.AreEqual(x, 0);
        }
    }
}
