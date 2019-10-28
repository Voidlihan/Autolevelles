using System;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace AutoLevelLesson
{
    /*
     * 1.Отправить схемы таблиц в БД
     * 2.Отправить данные из методов Fill... в БД
     * 3.Вручную добавить в таблицы БД по 1 значению
     * 4.Скачать изменения в локальный DataSet
     * 5.Используем фабрику провайдеров      
    */
    class Program
    {
        private static DataTable usertable;
        private static DataTable peopletable;
        static void Main(string[] args)
        {
            var dataset = new DataSet("Shop_db");
            CreateUserTable();
            CreatePeopleTable();
            dataset.Tables.AddRange(new DataTable[] { usertable, peopletable });
            Fillpeople();
            Fillusers();
            SqlDataAdapter dataAdapter = new SqlDataAdapter();
            var connection = new SqlConnection("Server=A-305-03;Database=Shop_db;Trusted_Connection=True;");
            var selectCommand = new SqlCommand("select * from UserTable", connection);
            dataAdapter.SelectCommand = selectCommand;
            var selectCommand1 = new SqlCommand("select * from PeopleTable", connection);
            dataAdapter.SelectCommand = selectCommand1;
            dataset.Relations.Add("UserPeople", dataset.Tables["UserTable"].Columns["id"],
                dataset.Tables["PeopleTable"].Columns["peopleId"]);
            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
            dataAdapter.Fill(dataset); //получает данные из БД
            dataset.AcceptChanges();
            dataAdapter.Update(dataset); //обновляет БД исходя из локального DataSet
        }

        private static void Fillusers()
        {
            usertable.Rows.Add(1, 1);
            usertable.Rows.Add(2, 2);
        }

        private static void Fillpeople()
        {
            //var idRow = peopletable.NewRow();
            
            peopletable.Rows.Add(1, "Петрович");
            peopletable.Rows.Add(2, "Василич");
        }
        private static void CreatePeopleTable()
        {
            var peopletable = new DataTable("peoples");
            peopletable.Columns.Add(new DataColumn
            {
                ColumnName = "Fullname",
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
                AllowDBNull = false,
                Unique = false,
                DataType = typeof(string)
            });
            peopletable.PrimaryKey = new DataColumn[] { peopletable.Columns["id"] };
            peopletable.Columns.Add(new DataColumn
            {
                ColumnName = "peopleid",
                AllowDBNull = false,
                Unique = true,
                DataType = typeof(int)
            });
        }
        private static void CreateUserTable()
        {
            var usertable = new DataTable("users");
            usertable.Columns.Add(new DataColumn
            {
                ColumnName = "id",
                AutoIncrement = true,
                AutoIncrementSeed = 1,
                AutoIncrementStep = 1,
                AllowDBNull = false,
                Unique = true,
                DataType = typeof(int)
            });
            usertable.PrimaryKey = new DataColumn[] { usertable.Columns["id"] };
            usertable.Columns.Add(new DataColumn
            {
                ColumnName = "peopleId",
                AllowDBNull = false,
                Unique = true,
                DataType = typeof(int)
            });
        }
    }
}
