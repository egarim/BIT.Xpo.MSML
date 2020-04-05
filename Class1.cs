using BIT.MSML;
using Microsoft.ML;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BIT.Xpo.MSML
{
    public class MyClass<T>: CursorProxyBase
    {
        
        public MyClass(string TextProperty, string BoolProperty)
        {
            
        }
        public override long? GetRowCount()
        {
            return base.GetRowCount();
        }
    }


}
