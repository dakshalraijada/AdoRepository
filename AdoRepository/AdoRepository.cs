using AdoRepository.Attributes;
using AdoRepository.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace AdoRepository
{
    public class AdoRepository
    {
        public SqlConnection _connection;

        public AdoRepository(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        protected static void AddParameters<X>(X value, SqlParameterCollection collection) where X : class
        {
            foreach (var prop in typeof(X).GetProperties().Where(p => !p.IsMarkedWith<NotAParamAttribute>() && !p.IsMarkedWith<IsOutputParamAttribute>()))
            {
                var propValue = prop.GetValue(value);
                if (prop.PropertyType == typeof(int))
                {
                    collection.AddInt(prop.Name, (int)propValue);
                }
                else if (prop.PropertyType == typeof(int?))
                {
                    collection.AddNullableInt(prop.Name, (int?)propValue);
                }
                else if (prop.PropertyType == typeof(short))
                {
                    collection.AddSmallInt(prop.Name, (short)propValue);
                }
                else if (prop.PropertyType == typeof(short?))
                {
                    collection.AddNullableSmallInt(prop.Name, (short?)propValue);
                }
                else if (prop.PropertyType == typeof(DataTable))
                {
                    collection.AddTable(prop.Name, (DataTable)propValue);
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    collection.AddNullableDateTime(prop.Name, (DateTime?)propValue);
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    collection.AddNullableDateTime(prop.Name, (DateTime)propValue);
                }
                else if (prop.PropertyType == typeof(TimeSpan?))
                {
                    collection.AddNullableTime(prop.Name, (TimeSpan?)propValue);
                }
                else if (prop.PropertyType == typeof(TimeSpan))
                {
                    collection.AddNullableTime(prop.Name, (TimeSpan)propValue);
                }
                else if (prop.PropertyType == typeof(decimal?))
                {
                    collection.AddNullableMoney(prop.Name, (decimal?)propValue);
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    collection.AddBit(prop.Name, (bool)propValue);
                }
                else if (prop.PropertyType == typeof(bool?))
                {
                    if (propValue != null)
                    {
                        collection.AddBit(prop.Name, (bool)propValue);
                    }
                    else if (prop.IsMarkedWith<DefaultIfNullAttribute>())
                    {
                        bool defaultIfNull = (bool)prop.GetAttributes<DefaultIfNullAttribute>().First().DefaultValue;
                        collection.AddBitWithDefault(prop.Name, (bool?)propValue, defaultIfNull);
                    }
                    else
                    {
                        collection.AddWithValue(prop.Name, DBNull.Value);
                    }
                }
                else if (prop.PropertyType == typeof(string))
                {
                    if (prop.IsMarkedWith<MaxLengthAttribute>())
                    {
                        int maxLength = prop.GetAttributes<MaxLengthAttribute>().First().Length;
                        collection.AddNVarChar(prop.Name, (string)propValue, maxLength);
                    }
                    else
                    {
                        collection.AddNVarCharMax(prop.Name, (string)propValue);
                    }
                }
                else
                {
                    if (propValue != null)
                    {
                        collection.AddWithValue(prop.Name, propValue);
                    }
                }
            }
        }

        protected static X PopulateSingleResult<X>(SqlDataReader reader) where X : new()
        {
            var returnObject = new X();
            foreach (var prop in typeof(X).GetProperties().Where(p => !p.IsMarkedWith<NotAParamAttribute>() && !p.IsMarkedWith<IsOutputParamAttribute>()))
            {
                dynamic value = null;
                bool canSetValue = true;

                var exists = Enumerable.Range(0, reader.FieldCount).Any(i =>
                        string.Equals(
                                reader.GetName(i),
                                prop.Name,
                                StringComparison.OrdinalIgnoreCase
                          ));
                if (!exists) continue;

                if (prop.PropertyType == typeof(int))
                {
                    value = reader.ReadIntValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(int?))
                {
                    value = reader.ReadNullableIntValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(DateTime?))
                {
                    value = reader.ReadNullableDateTimeValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(DateTime))
                {
                    value = reader.ReadDateTimeValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(TimeSpan?))
                {
                    value = reader.ReadNullableTimeSpanValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(TimeSpan))
                {
                    value = reader.ReadTimeSpanValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(decimal?))
                {
                    value = reader.ReadNullableDecimalValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(decimal))
                {
                    value = reader.ReadDecimalValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(bool))
                {
                    value = reader.ReadBooleanValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(bool?))
                {
                    value = reader.ReadNullableBoolValue(prop.Name);
                }
                else if (prop.PropertyType == typeof(string))
                {
                    value = reader.ReadStringValue(prop.Name);
                }
                else
                {
                    canSetValue = false;
                }

                if (canSetValue)
                {
                    prop.SetValue(returnObject, value);
                }
            }

            return returnObject;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Security", "CA2100:Review SQL queries for security vulnerabilities", Justification = "<Pending>")]
        protected SqlCommand CreateSpCommandWithParams<X>(X args, string spName) where X : class
        {
            using (SqlCommand command = new SqlCommand(spName)
            {
                CommandType = CommandType.StoredProcedure
            })
            {
                AddParameters(args, command.Parameters);

                command.Connection = _connection;
                return command;
            }
        }

        /// <summary>
        /// Does while loop reader.read for ExecuteReaderAsync
        /// </summary>
        /// <typeparam name="X">Args</typeparam>
        /// <typeparam name="Y">Return type</typeparam>
        /// <param name="args">args</param>
        /// <param name="spName">Name of the SP</param>
        /// <returns>List<Y></returns>

        protected async Task<List<Y>> CallSpWithListReturnTypeAsync<X, Y>(X args, string spName) where X : class where Y : class, new()
        {
            var command = CreateSpCommandWithParams(args, spName);
            _connection.Open();
            try
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    var returnObject = new List<Y>();
                    while (reader.Read())
                    {
                        returnObject.Add(PopulateSingleResult<Y>(reader));
                    }
                    return returnObject;
                }
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        protected async Task<Y> CallSpReturnFirstRowAsync<X, Y>(X args, string spName) where X : class where Y : class, new()
        {
            var command = CreateSpCommandWithParams(args, spName);
            _connection.Open();
            try
            {
                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (reader.Read())
                    {
                        return PopulateSingleResult<Y>(reader);
                    }
                }
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
            return null;
        }

        protected async Task<Y> CallSpReturnScalarAsync<X, Y>(X args, string spName) where X : class where Y : struct
        {
            var command = CreateSpCommandWithParams(args, spName);
            _connection.Open();
            try
            {
                return (Y)await command.ExecuteScalarAsync();
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        protected async Task<bool> CallSpNonQueryWithOutputBoolParamAsync<X>(X args, string spName) where X : class
        {
            var command = CreateSpCommandWithParams(args, spName);
            var outputProp = typeof(X).GetProperties().First(o => o.IsMarkedWith<IsOutputParamAttribute>());
            SqlParameter outputParam = new SqlParameter("@" + outputProp.Name, 0)
            {
                SqlDbType = SqlDbType.Bit,
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);
            _connection.Open();
            try
            {
                await command.ExecuteNonQueryAsync();
                return bool.Parse(outputParam.Value.ToString());
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        protected async Task<int> CallSpNonQueryWithOutputIntParamAsync<X>(X args, string spName) where X : class
        {
            var command = CreateSpCommandWithParams(args, spName);
            var outputProp = typeof(X).GetProperties().First(o => o.IsMarkedWith<IsOutputParamAttribute>());
            SqlParameter outputParam = new SqlParameter("@" + outputProp.Name, 0)
            {
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };
            command.Parameters.Add(outputParam);
            _connection.Open();
            try
            {
                await command.ExecuteNonQueryAsync();
                return int.Parse(outputParam.Value.ToString());
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }

        protected async Task CallSpNonQueryAsync<X>(X args, string spName) where X : class
        {
            var command = CreateSpCommandWithParams(args, spName);
            _connection.Open();
            try
            {
                await command.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw ex;
            }
            finally
            {
                _connection.Close();
            }
        }
    }
}
