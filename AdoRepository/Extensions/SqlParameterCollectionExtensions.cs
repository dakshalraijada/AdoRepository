using System;
using System.Data;
using System.Data.SqlClient;

namespace AdoRepository.Extensions
{
    public static class SqlParameterCollectionExtensions
    {
        public static SqlParameter AddNullableStringValue(this SqlParameterCollection collection, string name, string value)
        {
            return collection.AddWithValue(name, string.IsNullOrEmpty(value) ? DBNull.Value : (object)value);
        }

        public static SqlParameter AddNullableUniqueidentifier(this SqlParameterCollection collection, string name, Guid? value)
        {
            object dbValue = DBNull.Value;
            if (value != null)
                dbValue = value;
            return collection.Add(new SqlParameter("@" + name, SqlDbType.UniqueIdentifier, -1, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, dbValue));
        }

        public static SqlParameter AddNVarChar(this SqlParameterCollection collection, string name, string value, int size)
        {
            /*Helper.AddSqlFtsMarkup(args.SearchText)*/
            return collection.Add(new SqlParameter("@" + name, SqlDbType.NVarChar, size, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value.ToEmptyWhenNull()));
        }

        public static SqlParameter AddUniqueIdentifier(this SqlParameterCollection collection, string name, string value)
        {
            /*Helper.AddSqlFtsMarkup(args.SearchText)*/
            return collection.Add(new SqlParameter("@" + name, SqlDbType.UniqueIdentifier, 0, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, new Guid(value)));
        }

        public static SqlParameter AddNVarCharMax(this SqlParameterCollection collection, string name, string value)
        {
            /*Helper.AddSqlFtsMarkup(args.SearchText)*/
            return collection.Add(new SqlParameter("@" + name, SqlDbType.NVarChar, -1, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value.ToEmptyWhenNull()));
        }

        public static SqlParameter AddNullableDateTime(this SqlParameterCollection collection, string name, DateTime? value)
        {
            SqlParameter dateParameter = new SqlParameter("@" + name, value);
            dateParameter.IsNullable = true;
            if (dateParameter.Value == null)
                dateParameter.Value = DBNull.Value;
            dateParameter.Direction = ParameterDirection.Input;
            dateParameter.SqlDbType = SqlDbType.DateTime;
            return collection.Add(dateParameter);
        }

        public static SqlParameter AddNullableTime(this SqlParameterCollection collection, string name, TimeSpan? value)
        {
            SqlParameter timeParameter = new SqlParameter("@" + name, value);
            timeParameter.IsNullable = true;
            if (timeParameter.Value == null)
                timeParameter.Value = DBNull.Value;
            timeParameter.Direction = ParameterDirection.Input;
            timeParameter.SqlDbType = SqlDbType.Time;
            return collection.Add(timeParameter);
        }

        public static SqlParameter AddSmallInt(this SqlParameterCollection collection, string name, int value)
        {
            return collection.Add(new SqlParameter("@" + name, SqlDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value));
        }

        public static SqlParameter AddNullableSmallInt(this SqlParameterCollection collection, string name, short? value)
        {
            object dbValue = DBNull.Value;
            if (value.HasValue)
                dbValue = value.Value;
            return collection.Add(new SqlParameter("@" + name, SqlDbType.SmallInt, 2, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, dbValue));
        }

        public static SqlParameter AddInt(this SqlParameterCollection collection, string name, int value)
        {
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value));
        }

        public static SqlParameter AddIntFromString(this SqlParameterCollection collection, string name, string value)
        {
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Int, 4, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value));
        }

        public static SqlParameter AddBigInt(this SqlParameterCollection collection, string name, long value)
        {
            return collection.Add(new SqlParameter("@" + name, SqlDbType.BigInt, 8, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value));
        }

        public static SqlParameter AddNullableMoney(this SqlParameterCollection collection, string name, decimal? value)
        {
            object dbValue = DBNull.Value;
            if (value.HasValue)
                dbValue = value.Value;
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Money, 8, ParameterDirection.Input, true, 0, 0, name, DataRowVersion.Current, dbValue));
        }

        public static SqlParameter AddNullableDecimal(this SqlParameterCollection collection, string name, decimal? value, byte precision, byte scale)
        {
            object dbValue = DBNull.Value;
            if (value.HasValue)
                dbValue = value.Value;
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Decimal, precision, ParameterDirection.Input, true, precision, scale, name, DataRowVersion.Current, dbValue));
        }

        public static SqlParameter AddNullableInt(this SqlParameterCollection collection, string name, int? value)
        {
            object dbValue = DBNull.Value;
            if (value.HasValue)
                dbValue = value.Value;
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Int, 4, ParameterDirection.Input, true, 0, 0, name, DataRowVersion.Current, dbValue));
        }

        public static SqlParameter AddNullableBit(this SqlParameterCollection collection, string name, bool? value)
        {
            object dbValue = DBNull.Value;
            if (value.HasValue)
                dbValue = value.Value;
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Bit, 1, ParameterDirection.Input, true, 0, 0, name, DataRowVersion.Current, dbValue));
        }

        public static SqlParameter AddBit(this SqlParameterCollection collection, string name, bool value)
        {
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value));
        }

        public static SqlParameter AddBitWithDefault(this SqlParameterCollection collection, string name, bool? value, bool defaultIfNull)
        {
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Bit, 1, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value.HasValue ? value.Value : defaultIfNull));
        }

        public static SqlParameter AddTable(this SqlParameterCollection collection, string name, DataTable value)
        {
            //command.Parameters.Add(new SqlParameter("@TagFilter", SqlDbType.Structured, -1, ParameterDirection.Input, false, 0, 0, "TagFilter", DataRowVersion.Current, tblTags));
            return collection.Add(new SqlParameter("@" + name, SqlDbType.Structured, -1, ParameterDirection.Input, false, 0, 0, name, DataRowVersion.Current, value));
        }
    }
}