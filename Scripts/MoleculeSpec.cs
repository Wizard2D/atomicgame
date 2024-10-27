using AtomGame.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AtomGame
{
    public struct MoleculeSpec
    {
        public string Name;
        public List<AtomSpec> Content;
        public string Formula;
        public bool IsOxidator;
        public float Charge;
        public float Stability;
    }
}
