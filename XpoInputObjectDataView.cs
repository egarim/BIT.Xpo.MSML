using BIT.Xpo.MSML;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML;
using Microsoft.ML.Data;
using System.Collections;
using DevExpress.Xpo;

namespace BIT.Xpo.MSML
{
    /// <summary>
    /// This is an implementation of <see cref="IDataView"/> that wraps an
    /// <see cref="IEnumerable{T}"/> of the above <see cref="InputObject"/>.
    /// Note that normally under these circumstances, the first recommendation
    /// would be to use a convenience like 
    /// <see cref="DataOperationsCatalog
    /// .LoadFromEnumerable{TRow}(IEnumerable{TRow}, SchemaDefinition)"/>
    /// or something like that, rather than implementing <see cref="IDataView"/>
    /// outright. However, sometimes when code generation is impossible on some
    /// situations, like Unity or other similar platforms, implementing
    /// something even closely resembling this may become necessary.
    ///
    /// This implementation of <see cref="IDataView"/>, being didactic, is much
    /// simpler than practically anything one would find in the ML.NET codebase.
    /// In this case we have a completely fixed schema (the two fields of
    /// <see cref="InputObject"/>), with fixed types.
    ///
    /// For <see cref="Schema"/>, note that we keep a very simple schema based
    /// off the members of the object. You may in fact note that it is possible
    /// in this specific case, this implementation of <see cref="IDatView"/>
    /// could share the same <see cref="DataViewSchema"/> object across all
    /// instances of this object, but since this is almost never the case, I do
    /// not take advantage of that.
    ///
    /// We have chosen to wrap an <see cref="IEnumerable{T}"/>, so in fact only
    /// a very simple implementation is possible. Specifically: we cannot
    /// meaningfully shuffle (so <see cref="CanShuffle"/> is
    /// <see langword="false"/>, and even if a <see cref="Random"/>
    /// parameter were passed to
    /// <see cref="GetRowCursor(IEnumerable{DataViewSchema.Column}, Random)"/>,
    /// we could not make use of it), we do not know the count of the item right
    /// away without counting (so, it is most correct for
    /// <see cref="GetRowCount"/> to return <see langword="null"/>, even after
    /// we might hypothetically know after the first pass, given the
    /// immutability principle of <see cref="IDatView"/>), and the
    /// <see cref="GetRowCursorSet(
    /// IEnumerable{DataViewSchema.Column}, int, Random)"/> method returns a
    /// single item.
    ///
    /// The <see cref="DataViewRowCursor"/> derived class has more documentation
    /// specific to its behavior.
    ///
    /// Note that this implementation, as well as the nested
    /// <see cref="DataViewRowCursor"/> derived class, does almost no validation
    /// of parameters or guard against misuse than we would like from, say,
    /// implementations of the same classes within the ML.NET codebase.
    /// </summary>
    public class XpoInputObjectDataView : IDataView
    {
        public readonly IEnumerable _data;
        public readonly DevExpress.Xpo.XPDataView _data2;
        public DataViewSchema Schema { get; }
        public bool CanShuffle => false;

      
        string TextProperty;
        string BoolProperty;
        public XpoInputObjectDataView(DevExpress.Xpo.XPView data,string TextProperty,string BoolProperty)
        {
            _data = data;
           
            var builder = new DataViewSchema.Builder();
            this.TextProperty = TextProperty;
            this.BoolProperty = BoolProperty;

            builder.AddColumn(BoolProperty, BooleanDataViewType.Instance);
            builder.AddColumn(TextProperty, TextDataViewType.Instance);


           
            Schema = builder.ToSchema();
        }
        public long? GetRowCount()
        {
            return (_data as XPDataView)?.Count;
          
        }

        public DataViewRowCursor GetRowCursor(
            IEnumerable<DataViewSchema.Column> columnsNeeded,
            Random rand = null)
        {
            return new XpoMlDataViewRowCursor(this, columnsNeeded.Any(c => c.Index == 0),
                           columnsNeeded.Any(c => c.Index == 1),this.TextProperty,this.BoolProperty);
        }

        public DataViewRowCursor[] GetRowCursorSet(
            IEnumerable<DataViewSchema.Column> columnsNeeded, int n,
            Random rand = null)
        {
            return new[] { GetRowCursor(columnsNeeded, rand) };
        }

       

    }
}