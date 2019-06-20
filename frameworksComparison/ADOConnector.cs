using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace frameworksComparison
{
    public class ADOConnector : IConnector
    {
        List<Worker> workers;
        private string _connectionString;
        private SQLiteConnection connection;


        public ADOConnector(string connectionString)
        {
            _connectionString = connectionString;
            connection = new SQLiteConnection(_connectionString);
        }


        public List<Worker> GetWorkers()
        {
            connection.Open();
            var query = "select * from Workers";
            var command = new SQLiteCommand(query,connection);

            var reader = command.ExecuteReader();

            workers = new List<Worker>();

            while(reader.Read())
            {
                Worker w = new Worker();
                w.Id = reader.GetInt32(0);
                w.Name = reader.GetString(1);
                w.SpecialityId = reader.GetInt32(2);
                w.BossId = reader.GetInt32(3);
                w.IsBoss = reader.GetBoolean(4);
                workers.Add(w);
            }
            connection.Close();
            return workers;
        }


        public Worker GetWorkerById(int id)
        {
            connection.Open();
            var query = "select * from Workers WHERE Id="+id;
            var command = new SQLiteCommand(query, connection);

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var searchedWorker = new Worker();

                searchedWorker.Id = reader.GetInt32(0);
                searchedWorker.Name = reader.GetString(1);
                searchedWorker.SpecialityId = reader.GetInt32(2);
                searchedWorker.BossId = reader.GetInt32(3);
                searchedWorker.IsBoss = reader.GetBoolean(4);
                connection.Close();
                return searchedWorker;
            }
            connection.Close();
            return null;
        }


        public Speciality GetSpecialityById(int id)
        {
            connection.Open();
            
            var query = "SELECT * FROM Specialities WHERE Id=" + id;
            var command = new SQLiteCommand(query, connection);

            var reader = command.ExecuteReader();
            if (reader.Read())
            {
                Speciality speciality = new Speciality();

                speciality.Id = reader.GetInt32(0);
                speciality.Name = reader.GetString(1);
                connection.Close();
                return speciality;
            }
            connection.Close();
            return null;
        }
    }
}
