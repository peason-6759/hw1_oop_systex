using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1_oop_systex
{
    internal class MFP085FileConvert:FileConvert
    {
        private static readonly int[] _data_length_indexes = { 8, 6, 1, 4, 9, 10, 3, 1, 3, 4, 3, 1, 8, 8, 8, 8, 8, 8, 8, 8, 8, 8, 10, 8, 2 };
        private static readonly string[] _column_indexes = {
                            "@caseid","@stock","@begindate","@enddate","@biddate", "@stkdate","@cflag", //contain data
                           "@trdate", "@trtime", "@moddate", "@modtime", "@moduser"};     //depreciate
        public MFP085FileConvert():base(_data_length_indexes, _column_indexes){}
        public MFP085FileConvert(MysqlTableCRUD conn_obj) : base(conn_obj, _data_length_indexes, _column_indexes){}
        public override void FileOnExecuteReplaceRowToTable(string directory_path, string file_name, int nums_byte)
        {
            //the multithreading could implemented.
            base.FileOnExecuteReplaceRowToTable(directory_path, file_name, nums_byte);

        }
        public override void CreateNewTable(string? command_text = null)

        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            if (command_text == null)
            {
                command_text = "create table MFP085(\r\n\t" +
                    "caseid char(8) collate utf8mb4_bin primary key comment '案件編號',\r\n    " +      //cfm01
                    "stock char(6) collate utf8mb4_bin comment '股票代號',\r\n    " +                   //cfm02
                    "begindate char(8) collate utf8mb4_bin comment '投標開始日期',\r\n    " +           //cfm13
                    "enddate char(8) collate utf8mb4_bin comment '投標截止日期',\r\n\t" +               //cfm14
                    "biddate char(8) collate utf8mb4_bin comment '開標日期',\r\n    " +                 //cfm17
                    "stkdate char(8) collate utf8mb4_bin comment '撥券日期',\r\n    " +                 //cfm21
                    "cflag char(1) collate utf8mb4_bin comment '取消註記',\r\n    " +                   //cfm22
                    "trdate char(8) collate utf8mb4_bin comment '轉檔日期',\r\n    " +                  //自己加datetime
                    "trtime char(6) collate utf8mb4_bin comment '轉檔時間',\r\n    " +                  //自己加datetime
                    "moddate char(8) collate utf8mb4_bin comment '異動日期',\r\n    " +                 //讀檔案metadate
                    "modtime char(6) collate utf8mb4_bin comment '異動時間',\r\n    " +                 //讀檔案metadate
                    "moduser char(10) collate utf8mb4_bin comment '異動人員'\r\n    );";                //讀mysql帳號名稱??
            }

            base.CreateNewTable(command_text);
        }
        public override void InsertDataByRowArray(ref string[] row_items_str)
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            try
            {
                //待處理MysqlTableCRUD、此functiona
                MySqlCommand cmd = _conn_obj.GetInitiateCmd();
                cmd.CommandText = _conn_obj.GetProcedureName();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue(_column_indexes[0], row_items_str[0]);  //caseid
                cmd.Parameters.AddWithValue(_column_indexes[1], row_items_str[1]);  //stock
                cmd.Parameters.AddWithValue(_column_indexes[2], row_items_str[12]); //begindate
                cmd.Parameters.AddWithValue(_column_indexes[3], row_items_str[13]); //enddate
                cmd.Parameters.AddWithValue(_column_indexes[4], row_items_str[16]); //biddate
                cmd.Parameters.AddWithValue(_column_indexes[5], row_items_str[20]); //stkdate
                cmd.Parameters.AddWithValue(_column_indexes[6], row_items_str[11]); //cflag
                cmd.Parameters.AddWithValue(_column_indexes[7], DateTime.Now.ToString("yyyyMMdd"));   //trdate
                cmd.Parameters.AddWithValue(_column_indexes[8], DateTime.Now.ToString("hhmmss"));   //trtime
                cmd.Parameters.AddWithValue(_column_indexes[9], File.GetLastWriteTime(@"C:\Users\23005241PEARSON\Desktop\\MFP085Convert\\MFP085.txt").ToString("yyyyMMdd"));   //moddate
                cmd.Parameters.AddWithValue(_column_indexes[10], File.GetLastWriteTime(@"C:\Users\23005241PEARSON\Desktop\\MFP085Convert\\MFP085.txt").ToString("hhmmss"));    //modtime
                cmd.Parameters.AddWithValue(_column_indexes[11], _conn_obj.GetUid()); //moduser

                int rowsAffected = cmd.ExecuteNonQuery();
                Console.WriteLine("Data inserted successfully. Rows affected: " + rowsAffected);
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
            }
        }
    }
}
