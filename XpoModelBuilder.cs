using BIT.MSML;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Text;

namespace BIT.Xpo.MSML
{
    public class XpoModelBuilder: ModelBuildersBase
    {
        IDataLayer dataLayer;
        public XpoModelBuilder(IDataLayer dataLayer)
        {
            //IDataStore DataStore = new DevExpress.Xpo.DB.InMemoryDataStore();
            //XpoDefault.DataLayer = new SimpleDataLayer(DataStore)
        }
        protected virtual UnitOfWork GetUnitOfWorks()
        {
            return new UnitOfWork(this.dataLayer);
        }
    }
}
