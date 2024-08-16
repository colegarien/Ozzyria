using Grynt.Model.Packages;
using Ozzyria.Model.CodeGen.Packages;
using System.IO;

namespace Ozzyria.Content
{
    public class Packages
    {
        protected static Packages _instance;

        public TypePackage TypePackage;
        public ComponentPackage ComponentPackage;
        public PrefabPackage PrefabPackage;

        public static Packages GetInstance()
        {
            if (_instance == null)
            {
                _instance = new Packages {
                    TypePackage = TypePackage.Load(Path.Combine(Loader.Root(), "Definitions", "types.json")),
                    ComponentPackage = ComponentPackage.Load(Path.Combine(Loader.Root(), "Definitions", "components.json")),
                    PrefabPackage = PrefabPackage.Load(Path.Combine(Loader.Root(), "Definitions", "prefabs.json")),
                };
            }

            return _instance;
        }
    }
}
