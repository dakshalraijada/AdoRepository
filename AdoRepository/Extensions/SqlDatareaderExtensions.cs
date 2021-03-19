using System;
using System.Data.SqlClient;
using System.Linq;

namespace AdoRepository.Extensions
{
    public static class SqlDatareaderExtensions
    {
        public static int ReadIntValue(this SqlDataReader reader, string field)
        {
            int intValue = 0;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    int.TryParse(rawValue.ToString(), out intValue);
                }
            }

            return intValue;
        }

        public static long ReadBigIntValue(this SqlDataReader reader, string field)
        {
            long longValue = 0;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    long.TryParse(rawValue.ToString(), out longValue);
                }
            }

            return longValue;
        }

        public static string ReadIntValueToString(this SqlDataReader reader, string field)
        {
            int intValue = 0;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    int.TryParse(rawValue.ToString(), out intValue);
                }
            }

            if (intValue == 0) return string.Empty;
            return intValue.ToString();
        }

        public static int ReadByteValue(this SqlDataReader reader, string field)
        {
            byte value = 0;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    byte.TryParse(rawValue.ToString(), out value);
                }
            }

            return value;
        }

        public static decimal ReadDecimalValue(this SqlDataReader reader, string field)
        {
            decimal decValue = 0;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    decimal.TryParse(rawValue.ToString(), out decValue);
                }
            }

            return decValue;
        }

        public static bool ReadBooleanValue(this SqlDataReader reader, string field)
        {
            bool booValue = false;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    bool.TryParse(rawValue.ToString(), out booValue);
                }
            }

            return booValue;
        }

        public static TimeSpan? ReadNullableTimeSpanValue(this SqlDataReader reader, string field)
        {
            TimeSpan value = TimeSpan.Zero;
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (TimeSpan.TryParse(rawValue.ToString(), out value))
                        return value;
                }
            }

            return null;
        }

        public static TimeSpan ReadTimeSpanValue(this SqlDataReader reader, string field)
        {
            TimeSpan value = TimeSpan.Zero;
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (TimeSpan.TryParse(rawValue.ToString(), out value))
                        return value;
                }
            }

            return value;
        }

        public static DateTime ReadDateTimeValue(this SqlDataReader reader, string field)
        {
            DateTime value = DateTime.MinValue;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (DateTime.TryParse(rawValue.ToString(), out value))
                        return value;
                }
            }

            return DateTime.MinValue;
        }

        public static DateTime? ReadNullableDateTimeValue(this SqlDataReader reader, string field)
        {
            DateTime value = DateTime.MinValue;

            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (DateTime.TryParse(rawValue.ToString(), out value))
                        return value;
                }
            }

            return null;
        }

        public static int? ReadNullableIntValue(this SqlDataReader reader, string field)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (int.TryParse(rawValue.ToString(), out int value))
                    {
                        return value;
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        public static decimal? ReadNullableDecimalValue(this SqlDataReader reader, string field)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (decimal.TryParse(rawValue.ToString(), out decimal value))
                    {
                        return value;
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        public static bool? ReadNullableBoolValue(this SqlDataReader reader, string field)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (bool.TryParse(rawValue.ToString(), out bool value))
                    {
                        return value;
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        public static Guid? ReadNullableGuidValue(this SqlDataReader reader, string field)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    if (Guid.TryParse(rawValue.ToString(), out Guid value))
                    {
                        return value;
                    }

                    return null;
                }

                return null;
            }

            return null;
        }

        public static string ReadStringValue(this SqlDataReader reader, string field)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    return rawValue.ToString();
                }
            }

            return string.Empty;
        }

        public static string ReadNullableStringValue(this SqlDataReader reader, string field)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];

                if (rawValue != DBNull.Value && rawValue != null)
                {
                    return rawValue.ToString();
                }
            }

            return null;
        }

        public static string ReadNullableTimeValueAsString(this SqlDataReader reader, string field, bool includeSeconds)
        {
            if (reader.GetSchemaTable().Select($"ColumnName = '{field}'").Any())
            {
                var rawValue = reader[field];
                if (rawValue != DBNull.Value && rawValue != null)
                {
                    return includeSeconds ? rawValue.ToString().Substring(0, 8) : rawValue.ToString().Substring(0, 5);
                }
            }
            return string.Empty;
        }
    }
}