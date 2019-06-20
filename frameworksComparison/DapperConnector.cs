using System.Collections.Generic;
using System.Linq;
using Dapper;
using System.Data.SQLite;

namespace frameworksComparison
{
    public class DapperConnector : IConnector
    {
        private string _connectionString;
        private SQLiteConnection connection;


        public DapperConnector(string connectionString)
        {
            _connectionString = connectionString;
            connection = new SQLiteConnection(connectionString);
        }


        public List<Worker> GetWorkers()
        {

            connection.Open();
            var workers = connection.Query<Worker>("SELECT * FROM Workers").ToList();
            connection.Close();
            return workers;
        }


        public Worker GetWorkerById(int id)
        {
            connection.Open();
            var worker = connection.Query<Worker>("SELECT * FROM Workers WHERE Id=" + id).ToList().FirstOrDefault();
            connection.Close();
            return worker;
        }


        public Speciality GetSpecialityById(int id)
        {
            connection.Open();
            var speciality = connection.Query<Speciality>("SELECT * FROM Specialities WHERE Id=" + id).ToList().FirstOrDefault();
            connection.Close();
            return speciality;
        }
    }
}
