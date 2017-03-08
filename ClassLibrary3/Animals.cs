using System;
using System.Collections.Generic;
using System.Linq;

namespace TechSession
{
    // Lots of unnecessary inheritance hierarchy
    public abstract class LifeForm
    {
    }

    public abstract class Animal : LifeForm
    {
        protected readonly string animalName;
        // 20 other attributes here

        // Initializing Ctor. Be prepared for many, many parameters!
        public Animal(string animalName)
        {
            // Lots of spurious use of this and base (it was cool!)
            this.animalName = animalName;
        }

        // The famed copy constructor (which won't actually work)
        public Animal(Animal animalToClone)
        {
            // Would usually also implement an assignment operator
            // Lulz I can get at your privates coz your the same class!
            this.animalName = animalToClone.animalName;
        }

        // Life before garbage collection was rough
        ~Animal() // Destructors typically were virtual. Any idea why?
        {
           // Ok, we're safe - we're not in C++ land
        } 

        public abstract string Type { get; }

        // No properties actually - would have likely been lots of getters and setters
        public string GetName() => animalName;

        // Can you name a few awesome v-table based flow control frameworks (often referred to as 'hooks')
        protected virtual void InternalMakeNoise()
        {
            Console.WriteLine($"My Name is {animalName} and I make a default Animal Noise");
            // (Second test)
            // Instead of Zebra.base.LetsBePolymorphic()
            // this.LetsBePolymorphic();
        }

        protected virtual void LetsBePolymorphic()
        {
            Console.WriteLine("Calling Animal.LetsBePolymorphic");
        }

        public void PublicMakeNoise()
        {
            InternalMakeNoise();
        }

        // Lots of operator overloads. Someone might find one useful (and buggy) someday
        public static bool operator == (Animal a, Animal b)
        {
            return a == null && b == null || (a?.Type == b?.Type && a?.animalName == b?.animalName);
        }

        public static bool operator !=(Animal a, Animal b)
        {
            return !(a == b);
        }

        // More protected helper goodies here

        // C++ had the concept of friend classes here - a bit like 'internal'.
    }

    public class Zebra : Animal
    {
        // Burden of meeting the inherited contracts
        public Zebra(string animalName)
            : base(animalName)
        {
        }

        // The famed copy constructor
        public Zebra(Zebra zebraToClone)
            : base(zebraToClone)
        {
        }

        public override string Type => "Zebra!";

        protected override void InternalMakeNoise()
        {
            Console.WriteLine("Make Special Zebra Noises!");
            // Do we need this?
            base.InternalMakeNoise();
            // This calls Animal.LetsBePolymorphic, right?
            // base.LetsBePolymorphic();
            // What if we try from the base class?
        }

        protected override void LetsBePolymorphic()
        {
            Console.WriteLine("Calling Zebra.LetsBePolymorphic");
        }
    }

    public class Giraffe : Animal
    {
        public Giraffe(string animalName)
            : base(animalName)
        {
        }

        public override string Type => "Giraffe!";

        protected override void InternalMakeNoise()
        {
            Console.WriteLine("Make Special Giraffe Noises!");
            base.InternalMakeNoise();
        }
    }

    //    public class Zebraffe : Giraffe, Zebra
    //    {
    //        protected override void InternalMakeNoise()
    //        {
    //              base.InternalMakeNoise() ... eh?
    //              Giraffe.InternalMakeNoise(); or Zebra.InternalMakeNoise();
    //        }
    //    }

    // Lets throw the whole GOF at our hierarchy
    public class AnimalBuilder
    {
        private Type _animalType;
        private string _name = "Default Name";

        public AnimalBuilder WithType(Type animalType)
        {
            _animalType = animalType;
            return this;
        }

        public AnimalBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public Animal Build()
        {
            return _animalType == typeof(Giraffe)
                ? new Giraffe(_name) as Animal
                : new Zebra(_name) as Animal;
        }
    }

    // Before STL, DIY collections were all the rage
    // NB : Aggregator doesn't actually refer to the 1:Many in UML land. It refers to the lifespan of the contained animals
    public class ZooAggregator
    {
        // This would obviously be our own Linked List framework monstrosity
        private IList<Animal> _animals = new List<Animal>();

        public void AddAnimal(Animal animalToAdd)
        {
            _animals.Add(animalToAdd);
        }

        // Look ma! Encapsulation
        public IEnumerable<Animal> FindAnimal(string animalName)
        {
            // OK, we didn't have LINQ nor yield returns. This would have been done in a loop, adding to a materialized collection
            return _animals.Where(a => a.GetName() == animalName);
            // We don't actually care what the caller does with our animals. Not our problem
        }
    }

    public class ZooComposer
    {
        // This would obviously be our own Linked List framework monstrosity (Has A)
        private IList<Animal> _animals = new List<Animal>();
        // In UML land, this would be a dependency (Uses A)
        private AnimalBuilder animalBuilder = new AnimalBuilder();

        public void AddAnimal(Type animalType, string animalName)
        {
            // The difference being is that the ZooComposer OWNS the lifespans of the contained animals
            _animals.Add(animalBuilder
                .WithType(animalType)
                .WithName(animalName)
                .Build());
        }

        // Look ma! Encapsulation
        // If we simply exposed our Animal collection, we would be breaking encapsulation
        // and we would be violating the Law of Demeter (more of a kind of guideline and probably a productivity anti pattern, in reality)
        public IEnumerable<Animal> FindAnimal(string animalName)
        {
            // OK, we didn't have LINQ nor yield returns. This would have been done in a loop, adding to a materialized collection
            return _animals.Where(a => a.GetName() == animalName);
            // These are OUR animals - we may lose some sleep if we pass them out of our control
        }
    }
}

// In short, we have way too much code, half of it doesn't work or serve any purpose
// and none of it is tested (nor testable)

// Java was borne into this mindset, and has gotten stuck there.