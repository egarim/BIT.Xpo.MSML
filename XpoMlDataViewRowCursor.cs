using Microsoft.ML;
using Microsoft.ML.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BIT.Xpo.MSML
{
    public class XpoMlDataViewRowCursor : DataViewRowCursor
    {
        private bool _disposed;
        private long _position;
        private readonly IEnumerator _enumerator;
        private readonly Delegate[] _getters;

        public override long Position => _position;
        public override long Batch => 0;
        public override DataViewSchema Schema { get; }

        string TextProperty;
        string BoolProperty;
        public XpoMlDataViewRowCursor(XpoInputObjectDataView parent, bool wantsLabel,
            bool wantsText, string TextProperty, string BoolProperty)

        {
            this.BoolProperty = BoolProperty;
            this.TextProperty = TextProperty;
            Schema = parent.Schema;
            _position = -1;
            _enumerator = parent._data.GetEnumerator();
            _getters = new Delegate[]
            {
                        wantsLabel ?
                            (ValueGetter<bool>)LabelGetterImplementation : null,

                        wantsText ?
                            (ValueGetter<ReadOnlyMemory<char>>)
                            TextGetterImplementation : null

            };
        }

        protected override void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                IDisposable disposable = _enumerator as IDisposable;
                if(disposable!=null)
                {
                    disposable.Dispose();
                }
             
                _position = -1;
            }
            _disposed = true;
            base.Dispose(disposing);
        }

        private void LabelGetterImplementation(ref bool value)
        {
            DevExpress.Xpo.ViewRecord Record = _enumerator.Current as DevExpress.Xpo.ViewRecord;
            value = (bool)Record[this.BoolProperty];
            
        }

        private void TextGetterImplementation(
            ref ReadOnlyMemory<char> value)
        {
            DevExpress.Xpo.ViewRecord Record = _enumerator.Current as DevExpress.Xpo.ViewRecord;
            value = ((string)Record[this.TextProperty]).AsMemory() ;
          
        }

        private void IdGetterImplementation(ref DataViewRowId id)
            => id = new DataViewRowId((ulong)_position, 0);

        public override ValueGetter<TValue> GetGetter<TValue>(
            DataViewSchema.Column column)

        {
            if (!IsColumnActive(column))
                throw new ArgumentOutOfRangeException(nameof(column));
            return (ValueGetter<TValue>)_getters[column.Index];
        }

        public override ValueGetter<DataViewRowId> GetIdGetter()
            => IdGetterImplementation;

        public override bool IsColumnActive(DataViewSchema.Column column)
            => _getters[column.Index] != null;

        public override bool MoveNext()
        {
            if (_disposed)
                return false;
            if (_enumerator.MoveNext())
            {
                _position++;
                return true;
            }
            Dispose();
            return false;
        }
    }
   
}