using System;
using System.Collections.Generic;
using NUnit.Framework;
using TechSession;

namespace Tests
{
    [TestFixture]
    public class Generix
    {
        // I've got this class inheritance hierarchy
        // LifeForm <- Animal <- Zebra
        //                    <- Giraffe

        public static void DoSomethingWithAnimals(IList<Animal> animals)
        {
            foreach (var animal in animals)
            {
                Console.WriteLine(animal.GetName());
            }
        }

        [Test]
        public void ThisWorks()
        {
            // Heterogenous collection
            var myAnimals = new List<Animal>
            {
                new Giraffe("Jerry"),
                new Zebra("Zebbie")
            };

            DoSomethingWithAnimals(myAnimals);
        }


        [Test]
        public void WhyCantIDoThis()
        {
            // Homogeneous collection
            var myAnimals = new List<Giraffe>
            {
                new Giraffe("Jerry"),
                new Giraffe("Melman")
            };

            // DoSomethingWithAnimals(myAnimals);
            // I know better ...
            DoSomethingWithAnimals((IList<Animal>)myAnimals);

            // Pop Quiz - If I could do this in Java, would I get a runtime exception?


            // Is myAnimals a reference type, or a value type ?
            // Is myAnimals passed to DoSomethingWithAnimals by reference, or by value ?

            // are ref and out good things? How else can we do this?

            // Let's get malicious with DoSomethingWithAnimals
        }

        public void PerformZebraAction(Action<Zebra> zebraAction)
        {
            var zebraToAction = new Zebra("Melman");
            zebraAction(zebraToAction);
        }

        [Test]
        public void IsThisMagicDownCasting()
        {
            var writeAnimal = new Action<Zebra>(a => Console.WriteLine($"My name is {a.GetName()}. I'm a zebra"));
            // var writeAnimal = new Action<Animal>(a => Console.WriteLine("I'm an animal"));
            // var writeAnimal = new Action<Giraffe>(a => Console.WriteLine("Giraffe!"));
            // var writeAnimal = new Action<LifeForm>(a => Console.WriteLine("Amoeba"));

            PerformZebraAction(writeAnimal);

            // Stoopid compiler. I know better
            // PerformZebraAction((Action<Zebra>)writeAnimal);
            // PerformZebraAction((Action<Animal>)writeAnimal);
        }





        // In Summary 
        // Mutable types like List<T> are Invariant - we have to match types, exactly. Not Less, not More
        // Immutable types like IEnumerable<T> are Covariant - any subtype will do
        // Action<T>, and Func<T, U> is Contravariant for T (not U). What looks to be magic downcasting, isn't at all. It's just inverted.
    }
}
