using System;

namespace HnSF
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class StateVariableAttribute : Attribute
    {
        public string menuName;
        
        public StateVariableAttribute(string menuName)
        {
            this.menuName = menuName;
        }
    }
}