using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hw1_oop_systex
{
    partial class FormT30 : Form
    {
        private DataGridView dataGridView;
        private readonly FileConvert _convert_obj;  //eq const in c++
        private readonly MysqlTableCRUD _connect_obj;
        public FormT30(FileConvert convert_obj)
        {
            InitializeComponent();
            _convert_obj = convert_obj;
            _connect_obj = new MysqlTableCRUD(_convert_obj.GetTableName(), "Insert_T30_Row");
        }

        private void FormT30_Load(object sender, EventArgs e)
        {
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill;  //自動排寬度?
            Controls.Add(dataGridView);

            foreach (string columnName in _convert_obj.GetColumnIndexes())
            {
                dataGridView.Columns.Add(columnName, columnName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //read sql table 
            //TBD: use mysqlAddRows function
            _connect_obj.ConnOpen();
            MySqlDataReader reader = _convert_obj.ReadDataByDataReader();
            while (reader.Read())
            {
                DataGridViewRow row = new DataGridViewRow();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    DataGridViewTextBoxCell cell = new DataGridViewTextBoxCell();
                    cell.Value = reader.GetValue(i);
                    row.Cells.Add(cell);
                }
                dataGridView.Rows.Add(row);
            }
            _connect_obj.ConnClose();

        }
    }
}
