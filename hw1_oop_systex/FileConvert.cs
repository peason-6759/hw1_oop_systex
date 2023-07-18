using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;
using System.Text;

namespace hw1_oop_systex
{
    internal class FileConvert
    {
        private int[] columnLengths;
        protected string[] columnIndexes;
        protected MysqlTableCRUD? _conn_obj = null;
        public FileConvert(int[] indexes_by_length, string[] column_name)
        {
            this.columnLengths = indexes_by_length;
            this.columnIndexes = column_name;
        }
        public FileConvert(MysqlTableCRUD conn_obj, int[] indexes_by_length, string[] column_name)
        {
            _conn_obj = conn_obj;
            this.columnLengths = indexes_by_length;
            this.columnIndexes = column_name;
        }
        public virtual void FileOnExecuteReplaceRowToTable(string directory_path, string file_name, int nums_byte)
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");

            byte[] byte_row = new byte[nums_byte];
            string[] previous_files = Directory.GetFileSystemEntries(directory_path);
            DateTime newest_file_timestamp = DateTime.MinValue;

            while (true)
            {
                string[] current_files = Directory.GetFileSystemEntries(directory_path);
                string[] addedFiles = current_files.Except(previous_files).ToArray();
                string[] removedFiles = previous_files.Except(current_files).ToArray();


                foreach (string addedFile in addedFiles) //尚未處理檔名可不同且一次放多個更新檔案。
                {
                    DateTime fileTimestamp = File.GetLastWriteTimeUtc(addedFile);
                    if (fileTimestamp > newest_file_timestamp)  //若cmd還沒結束過，一個檔案丟出來再丟進去就不會再讀
                    {
                        if (addedFile != ($"{directory_path}" + "\\" + file_name))
                        {
                            MessageBox.Show($"The file provide as {addedFile} is not expected file, please try again.");
                            continue;
                        }

                        _conn_obj.ConnOpen();
                        TruncateTable();
                        newest_file_timestamp = fileTimestamp;
                        MessageBox.Show($"Newest file {addedFile} added, the database is adding the rows.");
                        FileStream file_stream = File.OpenRead($"{directory_path}" + "\\" + file_name);
                        while ((file_stream.Read(byte_row, 0, byte_row.Length)) > 0)
                        {
                            ConvertRowToItemsByIndex(byte_row, out string[] items_array);
                            InsertDataByRowArray(ref items_array);
                        }
                        _conn_obj.ConnClose();

                        MessageBox.Show($"Newest data rows {addedFile} is completed");

                        Application.SetCompatibleTextRenderingDefault(false);
                        ApplicationConfiguration.Initialize();
                        Application.Run(new FormT30(this));
                    }
                }

                foreach (string removedFile in removedFiles)
                {
                    Console.WriteLine($"File removed: {removedFile}");
                }

                previous_files = current_files;
                Thread.Sleep(100);
            }
        }
        public string[] GetColumnIndexes()
        {
            return columnIndexes;
        }
        public string GetTableName()
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            return _conn_obj.GetTableName();
        }
        public virtual void ConvertRowToItemsByIndex(byte[] row_bytes, out string[] items_array)
        {
            items_array = new string[columnLengths.Length];
            for (int i = 0; i < columnLengths.Length; i++)
            {
                byte[] row_column_byte = row_bytes.Take(columnLengths[i]).ToArray();
                string big5String = Encoding.GetEncoding("Big5").GetString(row_column_byte);
                items_array[i] = big5String;
                row_bytes = row_bytes.Skip(columnLengths[i]).ToArray();
            }
        }
        public virtual decimal StrToDecimal(string str) 
        {
            decimal decimalValue = decimal.Parse(str) / 10000;
            return decimalValue;
        }
        public virtual void DropTable()
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            _conn_obj.ConnDropTable();
        }
        public virtual void TruncateTable()
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            _conn_obj.TruncateTable();
        }
        public virtual void CreateNewTable(string command_text)
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            _conn_obj.ConnCreateNewTable(command_text);
        }
        public virtual void InsertDataByRowArray(ref string[] row_items)
        {
            //CreateProcedure(ref cmd);             
        }
        public virtual MySqlDataReader ReadDataByDataReader()
        {
            this._conn_obj = _conn_obj ?? throw new ArgumentNullException(nameof(_conn_obj), "Optional object cannot be null.");
            return _conn_obj.GetReadDataReader();
        }
    }
}