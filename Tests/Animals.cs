using System.Runtime.InteropServices;
using NUnit.Framework;
using TechSession;

namespace Tests
{
    [TestFixture]
    public class AnimalsTests
    {
        [Test]
        public void CheckPolymorphicism()
        {
            var zebbie = new AnimalBuilder()
                .WithType(typeof (Zebra))
                .WithName("Zebbie")
                .Build();

            zebbie.PublicMakeNoise();
        }
    }
}
