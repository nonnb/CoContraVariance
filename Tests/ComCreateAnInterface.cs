using System.Diagnostics;
using System.Runtime.InteropServices;
using NUnit.Framework;

namespace Tests
{
    public interface ImAnInterface
    {
        void Boo();
    }

    public class SomeClass : ImAnInterface
    {
        public void Boo()
        {
            Debug.WriteLine("Boo!");
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void SomeTest()
        {
            // new ImAnInterface().Boo();
        }
    }
}





//[ComImport, Guid("00000000-2222-1111-3333-000000000000")]
//[CoClass(typeof(SomeClass))]




