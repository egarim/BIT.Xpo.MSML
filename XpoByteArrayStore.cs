using BIT.MSML;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIT.Xpo.MSML
{
    public class XpoByteArrayStore : XPCustomObject,IModelData
    {
        public XpoByteArrayStore(Session session) : base(session)
        { }

        Guid _oid;
        DateTime _date;
        string _name;
        string _log;

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Log
        {
            get => _log;
            set => SetPropertyValue(nameof(Log), ref _log, value);
        }

        [Size(SizeAttribute.DefaultStringMappingFieldSize)]
        public string Name
        {
            get => _name;
            set => SetPropertyValue(nameof(Name), ref _name, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetPropertyValue(nameof(Date), ref _date, value);
        }
        
        [Key(false)]
        public Guid Oid
        {
            get => _oid;
            set => SetPropertyValue(nameof(Oid), ref _oid, value);
        }
        public object Key { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
       


       
        
    }
}
