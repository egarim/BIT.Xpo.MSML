using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using Microsoft.ML;
using System;

namespace BIT.Xpo.MSML
{
    public static class DataOperationsExtensions
    {
        public static IDataView LoadFromXpoObject(this DataOperationsCatalog Instance,Session session,Type ObjectType,string Properties,CriteriaOperator Criteria,string TextPropertyName,string LabelPropertyName)
        {
            DevExpress.Xpo.XPView View = new DevExpress.Xpo.XPView(session, ObjectType, Properties, Criteria);
            return new XpoInputObjectDataView(View, TextPropertyName, LabelPropertyName);
        }
    }
}
