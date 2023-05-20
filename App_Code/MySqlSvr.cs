using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;

/// <summary>
/// Summary description for MySqlSvr
/// </summary>
public class MySqlSvr
{
    /// <summary>
    /// 对象的SQL连接
    /// </summary>
    public MySqlConnection cn;
    /// <summary>
    /// Reader的数据处理委托 所有对reader的操作都会在函数内部完成 只需要实现读取数据
    /// </summary>
    /// <param name="rd">Reader对象</param>
    public delegate void ReaderDataHandler(MySqlDataReader rd);
    public struct QueryReaderTask
    {
        public string sql;
        public ReaderDataHandler ReaderDataHandler;
        public Action NoDataHandler;
    }
    /// <summary>
    /// 返回对应sql语句的command对象
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <returns></returns>
    public MySqlCommand Cmd(string sql)
    {
        return new MySqlCommand(sql, cn);
    }
    /// <summary>
    /// 查询单个数据 或多个数据的第一个
    /// </summary>
    /// <param name="sql">SQL select语句</param>
    /// <returns></returns>
    public object QuerySingle(string sql)
    {
        cn.Open();
        var r = new MySqlCommand(sql, cn).ExecuteScalar();
        cn.Close();
        return r;
    }
    /// <summary>
    /// 直接获取查询语句的reader对象 需要手动关闭连接 不建议使用
    /// </summary>
    /// <param name="sql"></param>
    /// <returns></returns>
    [Obsolete("GetReader() is deprecated, use QueryReader() instead.", false)]
    public MySqlDataReader GetReader(string sql)
    {
        cn.Open();
        return new MySqlCommand(sql, cn).ExecuteReader();
    }
    /// <summary>
    /// 创建Reader对象 并对每行结果执行操作
    /// </summary>
    /// <param name="sql">SQL查询语句</param>
    /// <param name="rowHandler">委托函数 参数: 位于一行结果的Reader对象 每行执行一次</param>
    public void QueryReader(string sql, ReaderDataHandler rowHandler)
    {
        cn.Open();
        var rd = new MySqlCommand(sql, cn).ExecuteReader();
        if (rd.HasRows)
        {
            while (rd.Read())
            {
                rowHandler(rd);
            }
        }
        rd.Close();
        cn.Close();
    }
    /// <summary>
    /// 创建Reader对象 并对每行结果执行操作
    /// </summary>
    /// <param name="sql">SQL查询语句</param>
    /// <param name="rowHandler">委托函数 参数: 位于一行结果的Reader对象 每行执行一次</param>
    /// <param name="noDataHandler">委托函数 没有查询到数据时调用</param>
    public void QueryReader(string sql, ReaderDataHandler rowHandler, Action noDataHandler)
    {
        cn.Open();
        var rd = new MySqlCommand(sql, cn).ExecuteReader();
        if (rd.HasRows)
        {
            while (rd.Read())
            {
                rowHandler(rd);
            }
        }
        else
        {
            noDataHandler();
        }
        rd.Close();
        cn.Close();
    }
    /// <summary>
    /// 利用异步访问实现多个查询同时进行 数据量大时效率较高
    /// 没试过 可以用一下试试
    /// </summary>
    /// <param name="tasks">任务列表 把所有结构体全都列在参数里就行</param>
    public async void QueryReader_Parallel(params QueryReaderTask[] tasks)
    {
        cn.Open();
        var trds = new Dictionary<Task<DbDataReader>, QueryReaderTask>();
        var rds = new Dictionary<DbDataReader, Task<bool>>();
        var rhs = new Dictionary<DbDataReader, ReaderDataHandler>();

        foreach (var task in tasks)
        {
            trds.Add(new MySqlCommand(task.sql, cn).ExecuteReaderAsync(), task);
        }
        //verify queries
        foreach (var trd in trds.Keys)
        {
            var rd = await trd;
            if (!rd.HasRows)
            {
                trds[trd].NoDataHandler();
                rd.Close();
                continue;
            }
            rds.Add(rd, rd.ReadAsync());
            rhs.Add(rd, trds[trd].ReaderDataHandler);
        }
        //read
        var rd2clear = new List<DbDataReader>();
        while(rds.Count > 0)
        {
            foreach (var rd in rds.Keys)
            {
                if (!await rds[rd])
                {
                    rd2clear.Add(rd);
                    continue;
                }
                rhs[rd]((MySqlDataReader)rd);
                rds[rd] = rd.ReadAsync();
            }
            foreach(var rd in rd2clear)
            {
                rds.Remove(rd);
                rhs.Remove(rd);
                rd.Close();
            }
            rd2clear.Clear();
        }
        cn.Close();
    }
    /// <summary>
    /// 执行非查询操作
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="errorHandler">错误处理函数 可为null</param>
    /// <returns>影响行数 如果执行失败返回null</returns>
    public int? Execute(string sql, Action<Exception> errorHandler)
    {
        try
        {
            cn.Open();
            var r = new MySqlCommand(sql, cn).ExecuteNonQuery();
            cn.Close();
            return r;
        }
        catch(Exception ex)
        {
            errorHandler?.Invoke(ex);
            return null;
        }
    }
    /// <summary>
    /// 执行非查询操作
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <param name="transaction">事务</param>
    /// <returns>影响行数</returns>
    public int Execute(string sql, MySqlTransaction transaction)
    {
        cn.Open();
        var r = new MySqlCommand(sql, cn, transaction).ExecuteNonQuery();
        cn.Close();
        return r;
    }
    /// <summary>
    /// 查询并以DataSet形式返回所有数据
    /// </summary>
    /// <param name="sql">SQL语句</param>
    /// <returns>查询结果</returns>
    public DataSet QueryDataset(string sql)
    {
        var ds = new DataSet();
        new MySqlDataAdapter(sql, cn).Fill(ds);
        cn.Close();
        return ds;
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="conn">SQL连接</param>
    public MySqlSvr(MySqlConnection conn)
    {
        cn = conn;
    }
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="str">连接字符串</param>
    public MySqlSvr(string str)
    {
        cn = new MySqlConnection(str);
    }
}