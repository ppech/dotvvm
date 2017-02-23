using System;
using DotVVM.Framework.Compilation.ControlTree;

namespace DotVVM.Framework.Binding
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = true)]
    public abstract class DataContextChangeAttribute : Attribute
    {
        public abstract int Order { get; }

        public abstract ITypeDescriptor GetChildDataContextType(ITypeDescriptor dataContext,
            IDataContextStack controlContextStack, IAbstractControl control, IPropertyDescriptor property = null);
    }
}