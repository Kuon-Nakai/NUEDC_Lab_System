using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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