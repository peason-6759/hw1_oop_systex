using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace hw1_oop_systex
{
    class T30FileConvert : FileConvert
    {
        private static readonly int[] _data_length_indexes = { 6, 9, 9, 9, 8, 1, 1, 1, 1, 2, 2, 1, 16, 3, 6, 6, 3, 1, 1, 1, 1 };
        private static readonly string[] _column_indexes = {
                                "@pStock", // 0
                                "@pTPrice", // 1
                                "@pCPrice", // 2
                                "@pBPrice", // 3
                                "@pTEDate", // 4
                                "@pTStatus", // 5
                                "@pTMark", // 6
                                "@pCntDType", // 7
                                "@pSClass", // 9
                                "@pCName", // 12
                                "@pEName", // 17
                                "@pMType", // 18
                                "@pSType", // 19
                                "@pTSDate", // 20
                                "@pCLDate", // 21
                                "@pBrkNo", // 22
                                "@pIDate", // 23
                                "@pIRate", // 24
                                "@pCurrency", // 25
                                "@pCountry", // 26
                                "@pShare", // 27
                                "@pWarning", // 28
                                "@pMFlag", // 29
                                "@pWMark", // 30
                                "@pTaxType", // 31
                                "@pPType", // 32
                                "@pDRDate", // 33
                                "@pPDRDate", // 34
                                "@pCDiv", // 35
                                "@pSDiv", // 36
                                "@pTRDate", // 37
                                "@pTRTime", // 38
                                "@pModDate", // 39
                                "@pModTime", // 40
                                "@pModUser" // 41 (NOT THE LENGTH!!)
                            };
        public T30FileConvert() : base(_data_length_indexes, _column_indexes) {}
        public T30FileConvert(MysqlTableCRUD conn_obj) : base(conn_obj, _data_length_indexes, _column_indexes) {}
        public override void CreateNewTable(string? command_text = null)
           
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            if (command_text == null)
            {
                command_text = $"create table {_conn_obj.GetTableName()} (\r\n    " +
                                    "stock varchar(6) collate utf8mb4_unicode_ci primary key,\r\n    " +
                                    "cname varchar(32) collate utf8mb4_unicode_ci,\r\n    " +
                                    "ename varchar(255) collate utf8mb4_unicode_ci,\r\n    " +
                                    "mtype varchar(1) collate utf8mb4_unicode_ci,\r\n       " +
                                    "stype varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "sclass varchar(2) collate utf8mb4_unicode_ci,\r\n    " +
                                    "tsdate varchar(8) collate utf8mb4_unicode_ci,\r\n    " +
                                    "tedate varchar(8) collate utf8mb4_unicode_ci,\r\n    " +
                                    "cldate varchar(10) collate utf8mb4_unicode_ci,\r\n    " +
                                    "cprice decimal(10, 4),\r\n    " +
                                    "tprice decimal(10, 4),\r\n    " +
                                    "bprice decimal(10, 4),\r\n    " +
                                    "tstatus varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "brkno varchar(4) collate utf8mb4_unicode_ci,\r\n    " +
                                    "idate varchar(8) collate utf8mb4_unicode_ci,\r\n    " +
                                    "irate decimal(8, 6),\r\n    " +
                                    "currency varchar(3) collate utf8mb4_unicode_ci,\r\n    " +
                                    "country varchar(3) collate utf8mb4_unicode_ci,\r\n    " +
                                    "share decimal(6, 0),\r\n    " +
                                    "warning char(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "tmark char(1) collate utf8mb4_unicode_ci,\r\n   " +
                                    "mflag char(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "wmark char(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "taxtype varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "ptype varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "drdate varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "pdrdate varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "cdiv decimal(13, 8),\r\n    sdiv decimal(14, 8),\r\n    " +
                                    "cntdtype varchar(1) collate utf8mb4_unicode_ci,\r\n    " +
                                    "trdate varchar(8) collate utf8mb4_unicode_ci,\r\n    " +
                                    "trtime varchar(6) collate utf8mb4_unicode_ci,\r\n    " +
                                    "moddate varchar(8) collate utf8mb4_unicode_ci,\r\n    " +
                                    "modtime varchar(6) collate utf8mb4_unicode_ci,\r\n    " +
                                    "moduser varchar(10) collate utf8mb4_unicode_ci\r\n) " +
                                    "character set utf8mb4 collate utf8mb4_unicode_ci;\r\n)";


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
                cmd.Parameters.AddWithValue(_column_indexes[0], row_items_str[0]);
                cmd.Parameters.AddWithValue(_column_indexes[1], StrToDecimal(row_items_str[1])); //DICIMAL
                cmd.Parameters.AddWithValue(_column_indexes[2], StrToDecimal(row_items_str[2])); //收盤價-開盤競價基準?
                cmd.Parameters.AddWithValue(_column_indexes[3], StrToDecimal(row_items_str[3]));
                cmd.Parameters.AddWithValue(_column_indexes[4], row_items_str[4]); //交易截止日期-上次成交日?
                cmd.Parameters.AddWithValue(_column_indexes[5], row_items_str[5]); //交易方式
                cmd.Parameters.AddWithValue(_column_indexes[6], row_items_str[6]); //處置股票註記
                cmd.Parameters.AddWithValue(_column_indexes[7], row_items_str[7]); //現股當沖別-注意股票註記?
                cmd.Parameters.AddWithValue(_column_indexes[8], row_items_str[9]); //股票分類-產業別代碼?
                cmd.Parameters.AddWithValue(_column_indexes[9], row_items_str[12]);

                for (int i = 10; i < _column_indexes.Length; i++)
                {
                    cmd.Parameters.AddWithValue(_column_indexes[i], null);
                }
                int rowsAffected = cmd.ExecuteNonQuery();
                Debug.WriteLine("Data inserted successfully. Rows affected: " + rowsAffected);
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine("Error " + ex.Number + " has occurred: " + ex.Message);
            }
        }
    }
}
