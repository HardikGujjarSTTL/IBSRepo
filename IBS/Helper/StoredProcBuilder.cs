using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Collections.Concurrent;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace IBS.Helper
{
    internal class StoredProcBuilder : IStoredProcBuilder
    {
        private const string RetParamName = "_retParam";
        private readonly DbCommand _cmd;

        public StoredProcBuilder(DbContext ctx, string name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            DbCommand cmd = ctx.Database.GetDbConnection().CreateCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = name;
            cmd.Transaction = ctx.Database.CurrentTransaction?.GetDbTransaction();
            cmd.CommandTimeout = ctx.Database.GetCommandTimeout().GetValueOrDefault(cmd.CommandTimeout);

            _cmd = cmd;
        }

        public IStoredProcBuilder AddParam<T>(string name, T val)
        {
            AddParamInner(name, val, ParameterDirection.Input);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam)
        {
            outParam = AddOutputParamInner(name, default(T), ParameterDirection.Output);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0)
        {
            outParam = AddOutputParamInner(name, val, ParameterDirection.InputOutput, size, precision, scale);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0)
        {
            outParam = AddOutputParamInner(name, default(T), ParameterDirection.Output, size, precision, scale);
            return this;
        }

        public IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam)
        {
            outParam = AddOutputParamInner(name, val, ParameterDirection.InputOutput);
            return this;
        }

        public IStoredProcBuilder AddParam(DbParameter parameter)
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter));
            }

            _cmd.Parameters.Add(parameter);
            return this;
        }

        public IStoredProcBuilder ReturnValue<T>(out IOutParam<T> retParam)
        {
            retParam = AddOutputParamInner(RetParamName, default(T), ParameterDirection.ReturnValue);
            return this;
        }

        public IStoredProcBuilder SetTimeout(int timeout)
        {
            _cmd.CommandTimeout = timeout;
            return this;
        }

        public void Exec(Action<DbDataReader> action)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            bool ownsConnection = false;
            try
            {
                ownsConnection = OpenConnection();
                using (DbDataReader r = _cmd.ExecuteReader())
                {
                    action(r);
                }
            }
            finally
            {
                if (ownsConnection)
                    CloseConnection();
                Dispose();
            }
        }

        public Task ExecAsync(Func<DbDataReader, Task> action)
        {
            return ExecAsync(action, CancellationToken.None);
        }

        public async Task ExecAsync(Func<DbDataReader, Task> action, CancellationToken cancellationToken)
        {
            if (action is null)
                throw new ArgumentNullException(nameof(action));

            bool ownsConnection = false;
            try
            {
                ownsConnection = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                using (DbDataReader r = await _cmd.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
                {
                    try
                    {
                        await action(r).ConfigureAwait(false);
                    }
                    catch (Exception)
                    {
                        // In case the action bombs out, cancel the command and rethrow to propagate the actual action
                        // exception. If we don't cancel the command, we will be stuck on disposing of the reader until
                        // the sproc completes, even though the action has already thrown an exception. This is also the
                        // case when the cancellation token is cancelled after the action exception but before the sproc
                        // completes: we will still be stuck on disposing of the reader until the sproc completes. This
                        // is caused by the fact that DbDataReader.Dispose does not react to cancellations and simply
                        // waits for the sproc to complete. // The only way to cancel the execution when the reader has
                        // been engaged and the action has thrown, is to cancel the command.
                        _cmd.Cancel();
                        throw;
                    }
                }
            }
            finally
            {
                if (ownsConnection)
                    CloseConnection();
                Dispose();
            }
        }

        public int ExecNonQuery()
        {
            bool ownsConnection = false;
            try
            {
                ownsConnection = OpenConnection();
                return _cmd.ExecuteNonQuery();
            }
            finally
            {
                if (ownsConnection)
                    CloseConnection();
                Dispose();
            }
        }

        public Task<int> ExecNonQueryAsync()
        {
            return ExecNonQueryAsync(CancellationToken.None);
        }

        public async Task<int> ExecNonQueryAsync(CancellationToken cancellationToken)
        {
            bool ownsConnection = false;
            try
            {
                ownsConnection = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                return await _cmd.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }
            finally
            {
                if (ownsConnection)
                    CloseConnection();
                Dispose();
            }
        }

        public void ExecScalar<T>(out T val)
        {
            bool ownsConnection = false;
            try
            {
                ownsConnection = OpenConnection();
                object scalar = _cmd.ExecuteScalar();
                val = DefaultIfDBNull<T>(scalar);
            }
            finally
            {
                if (ownsConnection)
                    CloseConnection();
                Dispose();
            }
        }

        public Task ExecScalarAsync<T>(Action<T> action)
        {
            return ExecScalarAsync(action, CancellationToken.None);
        }

        public async Task ExecScalarAsync<T>(Action<T> action, CancellationToken cancellationToken)
        {
            bool ownsConnection = false;
            try
            {
                ownsConnection = await OpenConnectionAsync(cancellationToken).ConfigureAwait(false);
                object scalar = await _cmd.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                T val = DefaultIfDBNull<T>(scalar);
                action(val);
            }
            finally
            {
                if (ownsConnection)
                    CloseConnection();
                Dispose();
            }
        }

        public void Dispose()
        {
            _cmd.Dispose();
        }

        private OutputParam<T> AddOutputParamInner<T>(string name, T val, ParameterDirection direction, int size = 0, byte precision = 0, byte scale = 0)
        {
            DbParameter param = AddParamInner(name, val, direction, size, precision, scale);
            return new OutputParam<T>(param);
        }

        private DbParameter AddParamInner<T>(string name, T val, ParameterDirection direction, int size = 0, byte precision = 0, byte scale = 0)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));

            DbParameter param = _cmd.CreateParameter();
            param.ParameterName = name;
            param.Value = (object) val ?? DBNull.Value;
            param.Direction = direction;
            param.DbType = DbTypeConverter.ConvertToDbType<T>();
            param.Size = size;
            param.Precision = precision;
            param.Scale = scale;

            _cmd.Parameters.Add(param);
            return param;
        }

        private bool OpenConnection()
        {
            if (_cmd.Connection.State == ConnectionState.Closed)
            {
                _cmd.Connection.Open();

                return true;
            }

            return false;
        }

        private async Task<bool> OpenConnectionAsync(CancellationToken cancellationToken)
        {
            if (_cmd.Connection.State == ConnectionState.Closed)
            {
                await _cmd.Connection.OpenAsync(cancellationToken).ConfigureAwait(false);

                return true;
            }

            return false;
        }

        private void CloseConnection()
        {
            _cmd.Connection.Close();
        }

        private static T DefaultIfDBNull<T>(object o)
        {
            return o == DBNull.Value ? default(T) : (T) o;
        }
    }

    public interface IStoredProcBuilder : IDisposable
    {
        /// <summary>
        /// Add input parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        IStoredProcBuilder AddParam<T>(string name, T val);

        /// <summary>
        /// Add input/output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam);

        /// <summary>
        /// Add input/output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="val">Value of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        /// <param name="size">Number of decimal places to which <see cref="IOutParam{T}.Value"/> is resolved.</param>
        /// <param name="precision">Number of digits used to represent the <see cref="IOutParam{T}.Value"/> property.</param>
        /// <param name="scale">Maximum size, in bytes, of the data within the column.</param>
        IStoredProcBuilder AddParam<T>(string name, T val, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0);

        /// <summary>
        /// Add output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam);

        /// <summary>
        /// Add output parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="name">Name of the parameter.</param>
        /// <param name="outParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        /// <param name="size">Number of decimal places to which <see cref="IOutParam{T}.Value"/> is resolved.</param>
        /// <param name="precision">Number of digits used to represent the <see cref="IOutParam{T}.Value"/> property.</param>
        /// <param name="scale">Maximum size, in bytes, of the data within the column.</param>
        IStoredProcBuilder AddParam<T>(string name, out IOutParam<T> outParam, int size = 0, byte precision = 0, byte scale = 0);

        /// <summary>
        /// Add pre configured DB query execution parameter directly command.
        /// </summary>
        /// <param name="parameter">DB query execution parameter <see cref="DbParameter"/>.</param>
        IStoredProcBuilder AddParam(DbParameter parameter);

        /// <summary>
        /// Add return value parameter.
        /// </summary>
        /// <typeparam name="T">Type of the parameter. Can be nullable.</typeparam>
        /// <param name="retParam">Created parameter. Value will be populated after calling <see cref="Exec(Action{DbDataReader})"/>.</param>
        IStoredProcBuilder ReturnValue<T>(out IOutParam<T> retParam);

        /// <summary>
        /// Set the wait time before terminating the attempt to execute the stored procedure and generating an error.
        /// </summary>
        /// <param name="timeout"></param>
        /// <returns></returns>
        IStoredProcBuilder SetTimeout(int timeout);

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <param name="action">Actions to do with the result sets.</param>
        void Exec(Action<DbDataReader> action);

        /// <summary>
        /// Execute the stored procedure
        /// </summary>
        /// <param name="action">Actions to do with the result sets.</param>
        Task ExecAsync(Func<DbDataReader, Task> action);

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <param name="action">Actions to do with the result sets.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="TaskCanceledException">When <paramref name="cancellationToken"/> was cancelled.</exception>
        Task ExecAsync(Func<DbDataReader, Task> action, CancellationToken cancellationToken);

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        int ExecNonQuery();

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecNonQueryAsync();

        /// <summary>
        /// Execute the stored procedure.
        /// </summary>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="TaskCanceledException">When <paramref name="cancellationToken"/> was cancelled.</exception>
        /// <returns>The number of rows affected.</returns>
        Task<int> ExecNonQueryAsync(CancellationToken cancellationToken);

        /// <summary>
        /// Execute the stored procedure and return the first column of the first row.
        /// </summary>
        /// <typeparam name="T">Type of the scalar value.</typeparam>
        /// <param name="val">The first column of the first row in the result set.</param>
        void ExecScalar<T>(out T val);

        /// <summary>
        /// Execute the stored procedure and return the first column of the first row.
        /// </summary>
        /// <typeparam name="T">Type of the scalar value.</typeparam>
        /// <param name="action">Action applied to the first column of the first row in the result set.</param>
        Task ExecScalarAsync<T>(Action<T> action);

        /// <summary>
        /// Execute the stored procedure and return the first column of the first row.
        /// </summary>
        /// <typeparam name="T">Type of the scalar value.</typeparam>.
        /// <param name="action">Action applied to the first column of the first row in the result set.</param>
        /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
        /// <exception cref="TaskCanceledException">When <paramref name="cancellationToken"/> was cancelled.</exception>
        Task ExecScalarAsync<T>(Action<T> action, CancellationToken cancellationToken);
    }

    public static class DbContextExtension
    {
        /// <summary>
        /// Load a stored procedure
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="name">Procedure's name</param>
        /// <returns></returns>
        public static IStoredProcBuilder LoadStoredProc(this DbContext ctx, string name)
        {
            return new StoredProcBuilder(ctx, name);
        }
    }

    public static class DbDataReaderExtension
    {
        private const string NoElementError = "Sequence contains no element";
        private const string MoreThanOneElementError = "Sequence contains more than one element";

        /// <summary>
        /// Map data source to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this DbDataReader reader) where T : class, new()
        {
            var res = new List<T>();
            var mapper = new Mapper<T>(reader);
            mapper.Map(row => res.Add(row));
            return res;
        }

        /// <summary>
        /// Map data source to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<List<T>> ToListAsync<T>(this DbDataReader reader) where T : class, new()
        {
            return ToListAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Map data source to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static async Task<List<T>> ToListAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : class, new()
        {
            var res = new List<T>();
            var mapper = new Mapper<T>(reader);
            await mapper.MapAsync(row => res.Add(row), cancellationToken).ConfigureAwait(false);
            return res;
        }


        /// <summary>
        /// Map the first column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> Column<T>(this DbDataReader reader) where T : IComparable
        {
            return Column<T>(reader, 0);
        }

        /// <summary>
        /// Map the specified column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName">Name of the column to read. Use first column if null</param>
        /// <returns></returns>
        public static List<T> Column<T>(this DbDataReader reader, string columnName) where T : IComparable
        {
            int ordinal = columnName is null ? 0 : reader.GetOrdinal(columnName);
            return Column<T>(reader, ordinal);
        }

        /// <summary>
        /// Map the specified column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="ordinal">Zero-based column ordinal</param>
        /// <returns></returns>
        public static List<T> Column<T>(this DbDataReader reader, int ordinal) where T : IComparable
        {
            var res = new List<T>();
            while (reader.Read())
            {
                T value = reader.IsDBNull(ordinal) ? default(T) : (T)reader.GetValue(ordinal);
                res.Add(value);
            }
            return res;
        }


        /// <summary>
        /// Map the first column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<List<T>> ColumnAsync<T>(this DbDataReader reader) where T : IComparable
        {
            return ColumnAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Map the first column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static Task<List<T>> ColumnAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : IComparable
        {
            return ColumnAsync<T>(reader, 0, cancellationToken);
        }

        /// <summary>
        /// Map the specified column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName">Name of the column to read. Use first column if null</param>
        /// <returns></returns>
        public static Task<List<T>> ColumnAsync<T>(this DbDataReader reader, string columnName) where T : IComparable
        {
            return ColumnAsync<T>(reader, columnName, CancellationToken.None);
        }

        /// <summary>
        /// Map the specified column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="columnName">Name of the column to read. Use first column if null</param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static Task<List<T>> ColumnAsync<T>(this DbDataReader reader, string columnName, CancellationToken cancellationToken) where T : IComparable
        {
            int ordinal = columnName is null ? 0 : reader.GetOrdinal(columnName);
            return ColumnAsync<T>(reader, ordinal, cancellationToken);
        }

        /// <summary>
        /// Map the specified column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="ordinal">Zero-based column ordinal</param>
        /// <returns></returns>
        public static Task<List<T>> ColumnAsync<T>(this DbDataReader reader, int ordinal) where T : IComparable
        {
            return ColumnAsync<T>(reader, ordinal, CancellationToken.None);
        }

        /// <summary>
        /// Map the specified column to a list
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="ordinal">Zero-based column ordinal</param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static async Task<List<T>> ColumnAsync<T>(this DbDataReader reader, int ordinal, CancellationToken cancellationToken) where T : IComparable
        {
            var res = new List<T>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                T value = await reader.IsDBNullAsync(ordinal, cancellationToken).ConfigureAwait(false) ? default(T) : (T)reader.GetValue(ordinal);
                res.Add(value);
            }
            return res;
        }

        /// <summary>
        /// Create a dictionary. Keys must be unique
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="reader"></param>
        /// <param name="keyProjection">Projection to get the key</param>
        /// <returns></returns>
        public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this DbDataReader reader, Func<TValue, TKey> keyProjection) where TKey : IComparable where TValue : class, new()
        {
            var res = new Dictionary<TKey, TValue>();
            var mapper = new Mapper<TValue>(reader);
            mapper.Map(val =>
            {
                TKey key = keyProjection(val);
                res[key] = val;
            });
            return res;
        }

        /// <summary>
        /// Create a dictionary. Keys must be unique
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="keyProjection">Projection to get the key</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(this DbDataReader reader, Func<TValue, TKey> keyProjection) where TKey : IComparable where TValue : class, new()
        {
            return ToDictionaryAsync<TKey, TValue>(reader, keyProjection, CancellationToken.None);
        }
        /// <summary>
        /// Create a dictionary. Keys must be unique
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="keyProjection">Projection to get the key</param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static async Task<Dictionary<TKey, TValue>> ToDictionaryAsync<TKey, TValue>(this DbDataReader reader, Func<TValue, TKey> keyProjection, CancellationToken cancellationToken) where TKey : IComparable where TValue : class, new()
        {
            var res = new Dictionary<TKey, TValue>();
            var mapper = new Mapper<TValue>(reader);
            await mapper.MapAsync(val =>
            {
                TKey key = keyProjection(val);
                res[key] = val;
            }, cancellationToken).ConfigureAwait(false);

            return res;
        }

        /// <summary>
        /// Create a dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="reader"></param>
        /// <param name="keyProjection">Projection to get the key</param>
        /// <returns></returns>
        public static Dictionary<TKey, List<TValue>> ToLookup<TKey, TValue>(this DbDataReader reader, Func<TValue, TKey> keyProjection) where TKey : IComparable where TValue : class, new()
        {
            var res = new Dictionary<TKey, List<TValue>>();
            var mapper = new Mapper<TValue>(reader);
            mapper.Map(val =>
            {
                TKey key = keyProjection(val);

                if (res.ContainsKey(key))
                {
                    res[key].Add(val);
                }
                else
                {
                    res[key] = new List<TValue> { val };
                }
            });
            return res;
        }

        /// <summary>
        /// Create a dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="reader"></param>
        /// <param name="keyProjection">Projection to get the key</param>
        /// <returns></returns>
        public static Task<Dictionary<TKey, List<TValue>>> ToLookupAsync<TKey, TValue>(this DbDataReader reader, Func<TValue, TKey> keyProjection) where TKey : IComparable where TValue : class, new()
        {
            return ToLookupAsync(reader, keyProjection, CancellationToken.None);
        }

        /// <summary>
        /// Create a dictionary
        /// </summary>
        /// <typeparam name="TKey">Type of the keys</typeparam>
        /// <typeparam name="TValue">Type of the values</typeparam>
        /// <param name="reader"></param>
        /// <param name="keyProjection">Projection to get the key</param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static async Task<Dictionary<TKey, List<TValue>>> ToLookupAsync<TKey, TValue>(this DbDataReader reader, Func<TValue, TKey> keyProjection, CancellationToken cancellationToken) where TKey : IComparable where TValue : class, new()
        {
            var res = new Dictionary<TKey, List<TValue>>();
            var mapper = new Mapper<TValue>(reader);
            await mapper.MapAsync(val =>
            {
                TKey key = keyProjection(val);

                if (res.ContainsKey(key))
                {
                    res[key].Add(val);
                }
                else
                {
                    res[key] = new List<TValue>() { val };
                }
            }, cancellationToken).ConfigureAwait(false);

            return res;
        }

        /// <summary>
        /// Create a set with the first column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static HashSet<T> ToSet<T>(this DbDataReader reader) where T : IComparable
        {
            var res = new HashSet<T>();
            while (reader.Read())
            {
                T val = (T)reader.GetValue(0);
                res.Add(val);
            }
            return res;
        }

        /// <summary>
        /// Create a set with the first column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<HashSet<T>> ToSetAsync<T>(this DbDataReader reader) where T : IComparable
        {
            return ToSetAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Create a set with the first column
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static async Task<HashSet<T>> ToSetAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : IComparable
        {
            var res = new HashSet<T>();
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                T val = (T)reader.GetValue(0);
                res.Add(val);
            }
            return res;
        }

        /// <summary>
        /// Read first row
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T First<T>(this DbDataReader reader) where T : class, new()
        {
            return First<T>(reader, false, false);
        }

        /// <summary>
        /// Read first row or return default value if the data source is empty
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T FirstOrDefault<T>(this DbDataReader reader) where T : class, new()
        {
            return First<T>(reader, true, false);
        }

        /// <summary>
        /// Read first row or throw an exception if data source contains more than one row
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T Single<T>(this DbDataReader reader) where T : class, new()
        {
            return First<T>(reader, false, true);
        }

        /// <summary>
        /// Read first row or return default value if the data source is empty
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static T SingleOrDefault<T>(this DbDataReader reader) where T : class, new()
        {
            return First<T>(reader, true, true);
        }

        /// <summary>
        /// Read first row
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<T> FirstAsync<T>(this DbDataReader reader) where T : class, new()
        {
            return FirstAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Read first row
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static Task<T> FirstAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : class, new()
        {
            return FirstAsync<T>(reader, false, false, cancellationToken);
        }

        /// <summary>
        /// Read first row or return default value if the data source is empty
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<T> FirstOrDefaultAsync<T>(this DbDataReader reader) where T : class, new()
        {
            return FirstOrDefaultAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Read first row or return default value if the data source is empty
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static Task<T> FirstOrDefaultAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : class, new()
        {
            return FirstAsync<T>(reader, true, false, cancellationToken);
        }

        /// <summary>
        /// Read first row or throw an exception if data source contains more than one row
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<T> SingleAsync<T>(this DbDataReader reader) where T : class, new()
        {
            return SingleAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Read first row or throw an exception if data source contains more than one row
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static Task<T> SingleAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : class, new()
        {
            return FirstAsync<T>(reader, false, true, cancellationToken);
        }

        /// <summary>
        /// Read first row or return default value if the data source is empty
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static Task<T> SingleOrDefaultAsync<T>(this DbDataReader reader) where T : class, new()
        {
            return SingleOrDefaultAsync<T>(reader, CancellationToken.None);
        }

        /// <summary>
        /// Read first row or return default value if the data source is empty
        /// </summary>
        /// <typeparam name="T">Model</typeparam>
        /// <param name="reader"></param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        /// <returns></returns>
        public static Task<T> SingleOrDefaultAsync<T>(this DbDataReader reader, CancellationToken cancellationToken) where T : class, new()
        {
            return FirstAsync<T>(reader, true, true, cancellationToken);
        }

        private static T First<T>(DbDataReader reader, bool orDefault, bool throwIfNotSingle) where T : class, new()
        {
            if (reader.Read())
            {
                var mapper = new Mapper<T>(reader);
                T row = mapper.MapNextRow();

                if (throwIfNotSingle && reader.Read())
                    throw new InvalidOperationException(MoreThanOneElementError);

                return row;
            }

            if (orDefault)
                return default;

            throw new InvalidOperationException(NoElementError);
        }

        private static async Task<T> FirstAsync<T>(DbDataReader reader, bool orDefault, bool throwIfNotSingle, CancellationToken cancellationToken) where T : class, new()
        {
            if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                var mapper = new Mapper<T>(reader);
                T row = await mapper.MapNextRowAsync(cancellationToken).ConfigureAwait(false);

                if (throwIfNotSingle && await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    throw new InvalidOperationException(MoreThanOneElementError);

                // Siphon out all the remaining records to allow for long running sprocs to be cancelled. If we leave
                // out of here without looping over the remaining records, a long running sproc will run to its end with
                // no chance to be cancelled. This is caused by the fact that DbDataReader.Dispose does not react to
                // cancellations and simply waits for the sproc to complete.
                while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                    continue;

                return row;
            }

            if (orDefault)
                return default;

            throw new InvalidOperationException(NoElementError);
        }
    }

    internal static class DbTypeConverter
    {
        private static readonly Dictionary<Type, DbType> _typeMap = new Dictionary<Type, DbType>
        {
            [typeof(byte)] = DbType.Byte,
            [typeof(sbyte)] = DbType.SByte,
            [typeof(short)] = DbType.Int16,
            [typeof(ushort)] = DbType.UInt16,
            [typeof(int)] = DbType.Int32,
            [typeof(uint)] = DbType.UInt32,
            [typeof(long)] = DbType.Int64,
            [typeof(ulong)] = DbType.UInt64,
            [typeof(float)] = DbType.Single,
            [typeof(double)] = DbType.Double,
            [typeof(decimal)] = DbType.Decimal,
            [typeof(bool)] = DbType.Boolean,
            [typeof(string)] = DbType.String,
            [typeof(char)] = DbType.StringFixedLength,
            [typeof(Guid)] = DbType.Guid,
            [typeof(DateTime)] = DbType.DateTime,
            [typeof(DateTimeOffset)] = DbType.DateTimeOffset,
            [typeof(byte[])] = DbType.Binary,
            [typeof(byte?)] = DbType.Byte,
            [typeof(sbyte?)] = DbType.SByte,
            [typeof(short?)] = DbType.Int16,
            [typeof(ushort?)] = DbType.UInt16,
            [typeof(int?)] = DbType.Int32,
            [typeof(uint?)] = DbType.UInt32,
            [typeof(long?)] = DbType.Int64,
            [typeof(ulong?)] = DbType.UInt64,
            [typeof(float?)] = DbType.Single,
            [typeof(double?)] = DbType.Double,
            [typeof(decimal?)] = DbType.Decimal,
            [typeof(bool?)] = DbType.Boolean,
            [typeof(char?)] = DbType.StringFixedLength,
            [typeof(Guid?)] = DbType.Guid,
            [typeof(DateTime?)] = DbType.DateTime,
            [typeof(DateTimeOffset?)] = DbType.DateTimeOffset
        };

        public static DbType ConvertToDbType<T>()
        {
            Type t = typeof(T);
            if (_typeMap.TryGetValue(t, out DbType dbType))
                return dbType;

            throw new NotSupportedException("Type not supported : " + t.Name);
        }
    }

    public interface IOutParam<T>
    {
        T Value { get; }
    }

    internal class Mapper<T> where T : class, new()
    {
        /// <summary>
        /// Contains different columns set information mapped to type <typeparamref name="T"/>.
        /// </summary>
        private static readonly ConcurrentDictionary<int, Prop[]> PropertiesCache = new ConcurrentDictionary<int, Prop[]>();

        private readonly DbDataReader _reader;
        private readonly Prop[] _properties;

        public Mapper(DbDataReader reader)
        {
            _reader = reader;
            _properties = MapColumnsToProperties();
        }

        /// <summary>
        /// Map <see cref="DbDataReader"/> to a T and apply an action on it for each row
        /// </summary>
        /// <param name="action">Action to apply to each row</param>
        public void Map(Action<T> action)
        {
            while (_reader.Read())
            {
                T row = MapNextRow();
                action(row);
            }
        }

        /// <summary>
        /// Map <see cref="DbDataReader"/> to a T and apply an action on it for each row
        /// </summary>
        /// <param name="action">Action to apply to each row</param>
        public Task MapAsync(Action<T> action)
        {
            return MapAsync(action, CancellationToken.None);
        }

        /// <summary>
        /// Map <see cref="DbDataReader"/> to a T and apply an action on it for each row
        /// </summary>
        /// <param name="action">Action to apply to each row</param>
        /// <param name="cancellationToken">The cancellation instruction, which propagates a notification that operations should be canceled</param>
        public async Task MapAsync(Action<T> action, CancellationToken cancellationToken)
        {
            while (await _reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                T row = await MapNextRowAsync(cancellationToken).ConfigureAwait(false);
                action(row);
            }
        }

        public T MapNextRow()
        {
            T row = new T();
            for (int i = 0; i < _properties.Length; ++i)
            {
                object value = _reader.IsDBNull(_properties[i].ColumnOrdinal) ? null : _reader.GetValue(_properties[i].ColumnOrdinal);
                _properties[i].Setter(row, value);
            }
            return row;
        }

        public Task<T> MapNextRowAsync()
        {
            return MapNextRowAsync(CancellationToken.None);
        }

        public async Task<T> MapNextRowAsync(CancellationToken cancellationToken)
        {
            T row = new T();
            for (int i = 0; i < _properties.Length; ++i)
            {
                object value = await _reader.IsDBNullAsync(_properties[i].ColumnOrdinal, cancellationToken).ConfigureAwait(false)
                    ? null
                    : _reader.GetValue(_properties[i].ColumnOrdinal);
                _properties[i].Setter(row, value);
            }
            return row;
        }

        internal static int ComputePropertyKey(IEnumerable<string> columns)
        {
            unchecked
            {
                int hashCode = 17;
                foreach (string column in columns)
                {
                    hashCode = (hashCode * 31) + column.GetHashCode();
                }
                return hashCode;
            }
        }

        private Prop[] MapColumnsToProperties()
        {
            Type modelType = typeof(T);

            string[] columns = new string[_reader.FieldCount];
            for (int i = 0; i < _reader.FieldCount; ++i)
                columns[i] = _reader.GetName(i);

            int propKey = ComputePropertyKey(columns);
            if (PropertiesCache.TryGetValue(propKey, out Prop[] s))
            {
                return s;
            }

            var properties = new List<Prop>(columns.Length);
            for (int i = 0; i < columns.Length; i++)
            {
                string name = columns[i].Replace("_", "");
                PropertyInfo prop = modelType.GetProperty(name, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (prop == null)
                    continue;

                ParameterExpression instance = Expression.Parameter(typeof(object), "instance");
                ParameterExpression value = Expression.Parameter(typeof(object), "value");

                // "x as T" is faster than "(T) x" if x is a reference type
                UnaryExpression instanceCast = prop.DeclaringType.IsValueType
                    ? Expression.Convert(instance, prop.DeclaringType)
                    : Expression.TypeAs(instance, prop.DeclaringType);

                UnaryExpression valueCast = prop.PropertyType.IsValueType
                    ? Expression.Convert(value, prop.PropertyType)
                    : Expression.TypeAs(value, prop.PropertyType);

                MethodCallExpression setterCall = Expression.Call(instanceCast, prop.GetSetMethod(), valueCast);
                var setter = (Action<object, object>)Expression.Lambda(setterCall, instance, value).Compile();

                properties.Add(new Prop
                {
                    ColumnOrdinal = i,
                    Setter = setter,
                });
            }
            Prop[] propertiesArray = properties.ToArray();
            PropertiesCache[propKey] = propertiesArray;
            return propertiesArray;
        }
    }

    internal class OutputParam<T> : IOutParam<T>
    {
        public OutputParam(DbParameter param)
        {
            _dbParam = param;
        }

        public T Value
        {
            get
            {
                if (_dbParam.Value is DBNull)
                {
                    if (default(T) == null)
                    {
                        return default;
                    }
                    else
                    {
                        throw new InvalidOperationException($"{_dbParam.ParameterName} is null and can't be assigned to a non-nullable type");
                    }
                }

                var nullableUnderlyingType = Nullable.GetUnderlyingType(typeof(T));
                if (nullableUnderlyingType != null)
                {
                    return (T)Convert.ChangeType(_dbParam.Value, nullableUnderlyingType);
                }

                return (T)Convert.ChangeType(_dbParam.Value, typeof(T));
            }
        }

        public override string ToString() => _dbParam.Value.ToString();

        private readonly DbParameter _dbParam;
    }

    struct Prop
    {
        public int ColumnOrdinal { get; set; }
        public Action<object, object> Setter { get; set; }
    }
}
