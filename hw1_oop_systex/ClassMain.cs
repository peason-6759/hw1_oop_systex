using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Bcpg.OpenPgp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hw1_oop_systex
{
    internal class ClassMain
    {
        [STAThread]
        public static void Main()
        {

            //已知問題:兩thread同時進db導致 => System.NotSupportedException: ' The ReadAsync method cannot be called when another read operation is pending.'
            Thread mfp085_thread = new Thread(() =>
            {
                string mfp085_directory_path = @"C:\Users\23005241PEARSON\Desktop\\MFP085Convert";
                string mfp085_file_name = "MFP085.txt";
                int mfp085_nums_row = 155;

                MysqlTableCRUD mfp085_mysql_CRUD = new MysqlTableCRUD("MFP085", "Insert_MFP085_Row");
                MFP085FileConvert mfp085_obj = new MFP085FileConvert(mfp085_mysql_CRUD);
                mfp085_obj.FileOnExecuteReplaceRowToTable(mfp085_directory_path, mfp085_file_name, mfp085_nums_row);

            });

            Thread t30_thread = new Thread(() =>
            {
                string t30_directory_path = @"C:\Users\23005241PEARSON\Desktop\T30ConvertData";
                string t30_file_name = "T30.TXT";
                int t30_nums_row = 100;

                MysqlTableCRUD t30_mysql_CRUD = new MysqlTableCRUD("Stock_Volatility", "Insert_T30_Row");
                T30FileConvert t30_obj = new T30FileConvert(t30_mysql_CRUD);
                t30_obj.FileOnExecuteReplaceRowToTable(t30_directory_path, t30_file_name, t30_nums_row);
            });

            mfp085_thread.Start();
            t30_thread.Start();

            mfp085_thread.Join(); //除非例外發生，否則皆不會join
            t30_thread.Join();

        }

    }
}
