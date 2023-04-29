using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimberValueEvaluationSystem.Services
{
    //数据库操作类
    public class DatabaseHelper
    {
        private static string connectionString = "Data Source=";

        //新建数据库
        public static bool CreateDatabase()
        {
            string filepath = FileHelper.GetFolderPath();
            if (filepath == null)
            {
                Growl.Warning("已取消创建");
                return false;
            }
            else
            {
                //数据库文件路径
                string dbPath = System.IO.Path.Combine(filepath, "Database.db");
                //创建数据库
                SQLiteConnection.CreateFile(dbPath);
                Growl.Info("创建数据库成功");
                return true;
            }
        }

        ////创建数据库表
        //public static void CreateTable()
        //{
        //    //创建数据库表
        //    string sql = "CREATE TABLE IF NOT EXISTS 'timber' ('id' INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL, 'name' TEXT NOT NULL, 'type' TEXT NOT NULL, 'unit' TEXT NOT NULL, 'price' REAL NOT NULL, 'volume' REAL NOT NULL, 'total' REAL NOT NULL, 'date' TEXT NOT NULL, 'remark' TEXT NOT NULL)";
        //    UpdateDatabase(sql);
        //}

        //创建数据库——立地质量表
        public static void CreateMSQMTable()
        {
            SQLiteConnection connection = ConnectDatabase();
            string sql = "CREATE TABLE YourTableName (" +
                "id INTEGER PRIMARY KEY AUTOINCREMENT," +
                "SoilThickness TEXT," +
                "Slope TEXT," +
                "Aspect TEXT," +
                "Gradient TEXT," +
                "SiteQuality TEXT);";
            UpdateDatabase(connection,sql);
        }

        ////从CSV导入数据到数据库表中
        //public static void ImportCSV(string filepath)
        //{
        //    //读取CSV文件
        //    string[] lines = System.IO.File.ReadAllLines(filepath);
        //    //获取表头
        //    string[] headers = lines[0].Split(',');
        //    //获取表内容
        //    string[,] contents = new string[lines.Length - 1, headers.Length];
        //    for (int i = 1; i < lines.Length; i++)
        //    {
        //        string[] line = lines[i].Split(',');
        //        for (int j = 0; j < headers.Length; j++)
        //        {
        //            contents[i - 1, j] = line[j];
        //        }
        //    }
        //    //插入数据库表
        //    for (int i = 0; i < contents.GetLength(0); i++)
        //    {
        //        string sql = "INSERT INTO timber (name, type, unit, price, volume, total, date, remark) VALUES ('" + contents[i, 0] + "', '" + contents[i, 1] + "', '" + contents[i, 2] + "', '" + contents[i, 3] + "', '" + contents[i, 4] + "', '" + contents[i, 5] + "', '" + contents[i, 6] + "', '" + contents[i, 7] + "')";
        //        InsertDatabase(sql);
        //    }
        //    Growl.Info("导入数据成功");
        //}

        //连接数据库
        public static SQLiteConnection ConnectDatabase()
        {
            connectionString += FileHelper.GetFilePath();
            if (connectionString == "Data Source=")
            {
                Growl.Warning("已取消创建表");
                return null;
            }
            else
            {
                //创建数据库连接
                SQLiteConnection conn = new SQLiteConnection(connectionString);
                return conn;
            }
        }

        //查询数据库内容
        public static SQLiteDataReader QueryDatabase(string sql)
        {
            SQLiteConnection conn = ConnectDatabase();
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            SQLiteDataReader reader = cmd.ExecuteReader();
            return reader;
        }

        //更新数据库内容
        public static void UpdateDatabase(SQLiteConnection connection,string sql)
        {
            connection.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql,connection);
            cmd.ExecuteNonQuery();
            connection.Close();
        }

        //插入数据库内容
        public static void InsertDatabase(string sql)
        {
            SQLiteConnection conn = ConnectDatabase();
            conn.Open();
            SQLiteCommand cmd = new SQLiteCommand(sql, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        } 

        ////删除数据库内容
        //public static void DeleteDatabase(string sql)
        //{
        //    SQLiteConnection conn = ConnectDatabase();
        //    conn.Open();
        //    SQLiteCommand cmd = new SQLiteCommand(sql, conn);
        //    cmd.ExecuteNonQuery();
        //    conn.Close();
        //}
    }
}
