using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1_oop_systex
{
    internal class MysqlTableCRUD
    {
        public static MySqlConnection conn = new MySqlConnection();
        private MySqlCommand cmd = new MySqlCommand();
        private static readonly string server = "127.0.0.1";
        private static readonly string database = "peason_systex";
        private static readonly string Uid = "root";
        private static readonly string password = "123456";

        private readonly string table_name;
        private string procedure_name;

        public MysqlTableCRUD(string table_name, string procedure_name)
        {
            this.table_name = table_name;
            this.procedure_name = procedure_name;
        }

        protected internal MySqlConnection DataSource()
        {
            conn = new MySqlConnection($"Persist Security Info=False;database={database};server={server};Connect Timeout=30;user id={Uid}; pwd={password}");
            cmd.Connection = conn;
            return conn;
        }

        protected internal void ConnOpen()
        {
            DataSource();
            if (conn.State != ConnectionState.Open)
                conn.Open();
        }

        protected internal void ConnClose()
        {
            DataSource();
            conn.Close();
        }
        protected internal MySqlConnection GetConn()
        {
            return conn;
        }
        protected internal MySqlCommand GetInitiateCmd()
        {
            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = GetConn();
            return cmd;
        }
        protected internal string GetTableName()
        {
            return table_name;
        }
        protected internal string GetProcedureName() 
        {
            return procedure_name;
        }
        protected internal string GetUid()
        {
            return Uid;
        }
        //TBD
        protected internal void ConnInsertDataByRow() 
        {

        }
        protected internal void ConnDropTable() 
        {
            try 
            {
                ConnLoadProcedure(procedure_name);
                cmd.CommandText = $"drop table if exists {table_name}; ";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
            }

        }
        protected internal void TruncateTable()
        {
            try
            {
                ConnLoadProcedure(procedure_name);
                cmd.CommandText = $"Truncate table {table_name};";
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
            }

        }
        protected internal void ConnCreateNewTable(string sql_create_command) 
        {
            try
            {
                ConnLoadProcedure(procedure_name);
                cmd.CommandText = sql_create_command;
                cmd.ExecuteNonQuery();
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
            }
        }
        protected internal void ConnInsertRow() //未解決
        {
            ConnLoadProcedure(procedure_name);
            cmd.CommandType = CommandType.StoredProcedure;

        }
        protected internal MySqlDataReader GetReadDataReader()
        {
            MySqlCommand cmd = GetInitiateCmd();
            cmd.CommandText = procedure_name;
            cmd.CommandText = $"SELECT * FROM {table_name}";
            return cmd.ExecuteReader();
        }

        protected internal void ConnCreateProcedure() { }
        protected internal void ConnLoadProcedure(string command_procdure_name) 
        {
            procedure_name = command_procdure_name;
            cmd.CommandText = procedure_name;
        } // To alter the procedure name or  exception if not connect into procuder

    }
}
