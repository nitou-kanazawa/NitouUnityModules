using System;

namespace nitou.LevelActors.Attributes{

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequireInterfaceAttribute : Attribute{

        public Type InterfaceType { get; private set; }
        
        public RequireInterfaceAttribute(Type type){
            InterfaceType = type;
        }
    }
}