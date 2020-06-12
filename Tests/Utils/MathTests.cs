using System.Collections;
using NUnit.Framework;
using Sand.Navigation.Utils;
using UnityEngine.TestTools;

namespace Tests
{
    public class MathTests
    {
        [Test]
        public void NavigationGridTestsWithEnumeratorPasses()
        {
            Int2 result = Math.CalculateIndexFromPosition(
                new UnityEngine.Vector2(0.5f, 0.8f),
                new UnityEngine.Vector2(2f, 2.4f)
            );

            Assert.AreEqual(result.x, 4);
            Assert.AreEqual(result.y, 3);
        }
    }
}
