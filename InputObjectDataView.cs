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
    public class InputObjectDataView : IDataView
    {
        public readonly IEnumerable _data;
        public readonly DevExpress.Xpo.XPDataView _data2;
        public DataViewSchema Schema { get; }
        public bool CanShuffle => false;

        public InputObjectDataView(IEnumerable<InputObject> data)
        {
            _data = data;

            var builder = new DataViewSchema.Builder();
            builder.AddColumn("Label", BooleanDataViewType.Instance);
            builder.AddColumn("Text", TextDataViewType.Instance);
            Schema = builder.ToSchema();
        }
        string TextProperty;
        string BoolProperty;
        public InputObjectDataView(DevExpress.Xpo.XPView data,string TextProperty,string BoolProperty)
        {
            _data = data;
            var Coloumns = data.Properties;
            var builder = new DataViewSchema.Builder();
            this.TextProperty = TextProperty;
            this.BoolProperty = BoolProperty;

            builder.AddColumn("Label", BooleanDataViewType.Instance);
            builder.AddColumn("Text", TextDataViewType.Instance);

            //foreach (ViewProperty viewProperty in data.Properties)
            //{
               
            //    //builder.AddColumn("Label", BooleanDataViewType.Instance);
            //    //builder.AddColumn("Text", TextDataViewType.Instance);

            //    builder.AddColumn(viewProperty.Name, TextDataViewType.Instance);
            //}

           
            Schema = builder.ToSchema();
        }
        public long? GetRowCount()
        {
            return (_data as XPDataView)?.Count;
            //return null;
        }

        public DataViewRowCursor GetRowCursor(
            IEnumerable<DataViewSchema.Column> columnsNeeded,
            Random rand = null)
        {
            return new Cursor(this, columnsNeeded.Any(c => c.Index == 0),
                           columnsNeeded.Any(c => c.Index == 1),this.TextProperty,this.BoolProperty);
        }

        public DataViewRowCursor[] GetRowCursorSet(
            IEnumerable<DataViewSchema.Column> columnsNeeded, int n,
            Random rand = null)
        {
            return new[] { GetRowCursor(columnsNeeded, rand) };
        }

        /// <summary>
        /// Having this be a private sealed nested class follows the typical
        /// pattern: in most <see cref="IDataView"/> implementations, the cursor
        /// instance is almost always that. The only "common" exceptions to this
        /// tendency are those implementations that are such thin wrappings of
        /// existing <see cref="IDataView"/> without even bothering to change
        /// the schema.
        ///
        /// On the subject of schema, note that there is an expectation that
        /// the <see cref="Schema"/> object is reference equal to the
        /// <see cref="IDataView.Schema"/> object that created this cursor, as
        /// we see here.
        ///
        /// Note that <see cref="Batch"/> returns <c>0</c>. As described in the
        /// documentation of that property, that is meant to facilitate the
        /// reconciliation of the partitioning of the data in the case where
        /// multiple cursors are returned from
        /// <see cref="GetRowCursorSet(
        /// IEnumerable{DataViewSchema.Column}, int, Random)"/>, 
        /// but since only one is ever returned from the implementation, this
        /// behavior is appropriate.
        ///
        /// Similarly, since it is impossible to have a shuffled cursor or a
        /// cursor set, it is sufficient for the <see cref="GetIdGetter"/>
        /// implementation to return a simple ID based on the position. If,
        /// however, this had been something built on, hypothetically, an
        /// <see cref="IList{T}"/> or some other such structure, and shuffling
        /// and partitioning was available, an ID based on the index of whatever
        /// item was being returned would be appropriate.
        ///
        /// Note the usage of the <see langword="ref"/> parameters on the
        /// <see cref="ValueGetter{TValue}"/> implementations. This is most
        /// valuable in the case of buffer sharing for <see cref="VBuffer{T}"/>,
        /// but we still of course have to deal with it here.
        ///
        /// Note also that we spend a considerable amount of effort to not make
        /// the <see cref="GetGetter{TValue}(DataViewSchema.Column)"/> and
        /// <see cref="IsColumnActive(DataViewSchema.Column)"/> methods
        /// correctly reflect what was asked for from the
        /// <see cref="GetRowCursor(
        /// IEnumerable{DataViewSchema.Column}, Random)"/> method that was used
        /// to create this method. In this particular case, the point is
        /// somewhat moot: this mechanism exists to enable lazy evaluation,
        /// but since this cursor is implemented to wrap an
        /// <see cref="IEnumerator{T}"/> which has no concept of lazy
        /// evaluation, there is no real practical benefit to doing this.
        /// However, it is best of course to illustrate the general principle
        /// for the sake of the example.
        ///
        /// Even in this simple form, we see the reason why
        /// <see cref="GetGetter{TValue}(DataViewSchema.Column)"/> is
        /// beneficial: the <see cref="ValueGetter{TValue}"/> implementations
        /// themselves are simple to the point where their operation is dwarfed
        /// by the simple acts of casting and validation checking one sees in
        /// <see cref="GetGetter{TValue}(DataViewSchema.Column)"/>. In this way
        /// we only pay the cost of validation and casting once, not every time
        /// we get a value.
        /// </summary>

    }
}