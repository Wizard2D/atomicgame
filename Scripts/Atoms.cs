using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomGame.Scripts
{
    public struct AtomSpec
    {
        public string Name;
        public string Symbol;
        public int AtomicNumber;
        public float AtomicMass;
        public bool Radioactive;
        public float HalfLife; // delta seconds

        public AtomSpec(string name, string symbol, int atomicNumber, float atomicMass, bool radioactive, float halfLife)
        {
            Name = name;
            Symbol = symbol;
            AtomicNumber = atomicNumber;
            AtomicMass = atomicMass;
            Radioactive = radioactive;
            HalfLife = halfLife;
        }
    }

    internal static class Atoms
    {
        public static readonly AtomSpec Hydrogen = new AtomSpec("Hydrogen", "H", 1, 1.01f, false, -1);
        public static readonly AtomSpec Helium = new AtomSpec("Helium", "He", 2, 4.00f, false, -1);
        public static readonly AtomSpec Lithium = new AtomSpec("Lithium", "Li", 3, 6.94f, false, -1);
        public static readonly AtomSpec Beryllium = new AtomSpec("Beryllium", "Be", 4, 9.01f, false, -1);
        public static readonly AtomSpec Boron = new AtomSpec("Boron", "B", 5, 10.81f, false, -1);
        public static readonly AtomSpec Carbon = new AtomSpec("Carbon", "C", 6, 12.01f, false, -1);
        public static readonly AtomSpec Nitrogen = new AtomSpec("Nitrogen", "N", 7, 14.01f, false, -1);
        public static readonly AtomSpec Oxygen = new AtomSpec("Oxygen", "O", 8, 16.00f, false, -1);
        public static readonly AtomSpec Fluorine = new AtomSpec("Fluorine", "F", 9, 19.00f, false, -1);
        public static readonly AtomSpec Neon = new AtomSpec("Neon", "Ne", 10, 20.18f, false, -1);

        public static readonly AtomSpec Deuterium = new AtomSpec("Deuterium", "D", 1, 2.02f, false, -1); // Heavy Hydrogen
        public static readonly AtomSpec Tritium = new AtomSpec("Tritium", "T", 1, 3.02f, true, 15); // Radioactive (4500 Earth years -> 4500/1000 game seconds)
        public static readonly AtomSpec Uranium235 = new AtomSpec("Uranium-235", "U", 92, 235.04f, true, 703.8f); // Radioactive (703.8 million years -> 703800000/1000 game seconds)
        public static readonly AtomSpec Uranium238 = new AtomSpec("Uranium-238", "U", 92, 238.05f, true, 447); // Radioactive (4.47 billion years -> 4470000000/1000 game seconds)
        public static readonly AtomSpec Plutonium239 = new AtomSpec("Plutonium-239", "Pu", 94, 239.05f, true, 24.1f); // Radioactive (24100 years -> 24100/1000 game seconds)

        private static readonly AtomSpec[] allAtoms = {
            Hydrogen, Helium, Lithium, Beryllium, Boron,
            Carbon, Nitrogen, Oxygen, Fluorine, Neon,
            Deuterium, Tritium, Uranium235, Uranium238, Plutonium239
        };

        // Method to find an atom by its atomic number
        public static AtomSpec? FindAtomByAtomicNumber(int atomicNumber)
        {
            foreach (var atom in allAtoms)
            {
                if (atom.AtomicNumber == atomicNumber)
                {
                    return atom;
                }
            }
            return null; // Return null if not found
        }

        // Method to get all atoms
        public static AtomSpec[] GetAllAtoms()
        {
            return allAtoms;
        }
    }
}
