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
    public MySqlConnection cn;
    public delegate void ReaderDataHandler(MySqlDataReader rd);
    public struct QueryReaderTask
    {
        public string sql;
        public ReaderDataHandler ReaderDataHandler;
        public Action NoDataHandler;
    }
    public MySqlCommand cmd(string sql)
    {
        return new MySqlCommand(sql, cn);
    }
    public object QuerySingle(string sql)
    {
        cn.Open();
        var r = new MySqlCommand(sql, cn).ExecuteScalar();
        cn.Close();
        return r;
    }
    public MySqlDataReader GetReader(string sql)
    {
        cn.Open();
        return new MySqlCommand(sql, cn).ExecuteReader();
    }
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
    public int Execute(string sql)
    {
        cn.Open();
        var r = new MySqlCommand(sql, cn).ExecuteNonQuery();
        cn.Close();
        return r;
    }
    public int Execute(string sql, MySqlTransaction transaction)
    {
        cn.Open();
        var r = new MySqlCommand(sql, cn, transaction).ExecuteNonQuery();
        cn.Close();
        return r;
    }
    public DataSet QueryDataset(string sql)
    {
        var ds = new DataSet();
        new MySqlDataAdapter(sql, cn).Fill(ds);
        cn.Close();
        return ds;
    }
    public MySqlSvr(MySqlConnection conn)
    {
        cn = conn;
    }
    public MySqlSvr(string str)
    {
        cn = new MySqlConnection(str);
    }
}