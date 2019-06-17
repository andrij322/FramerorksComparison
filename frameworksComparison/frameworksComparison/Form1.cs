using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SQLite;
using Dapper;

namespace frameworksComparison
{
    public partial class Form1 : Form
    {
        private string result = "";
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        public Form1()
        {
            
            InitializeComponent();
        }

        private void TryEntity(object sender, EventArgs e)
        {
            result = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            using (var db = new WorkerContext())
            {
                var workers = db.Workers;
                var specialities = db.Specialities;
                foreach (var worker in workers)
                {
                    var BossName = (from w in workers where worker.BossId == w.Id select w.Name).FirstOrDefault();
                    var SpecialityName = (from s in specialities where worker.SpecialityId == s.Id select s.Name).First();
                    result+=$"Worker name: {worker.Name}\nWorker boss: {BossName}\nWorker speciality: {SpecialityName};\n\n";
                }
            }
            watch.Stop();
            var time = watch.ElapsedMilliseconds;
            result += $"\nExecuting time: {time}";
            MessageBox.Show(result);
        }

        private async void TryADO(object sender, EventArgs e)
        {
            result = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Worker worker = new Worker();
            Speciality speciality = new Speciality();
            using (SQLiteConnection liteConnection = new SQLiteConnection(connectionString))
            {
                await liteConnection.OpenAsync();
                using (SQLiteCommand command = new SQLiteCommand(@"select w.Id,w.Name,SpecialityId,BossId,IsBoss,s.Name,cw.Name
                                                                from Workers as w
                                                                left join (select Id,Name from Workers) as cw
                                                                on w.BossId=cw.Id
                                                                inner join Specialities as s
                                                                on w.SpecialityId=s.Id", liteConnection))
                {
                    var reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        worker.Id = reader.GetInt32(0);
                        worker.Name = reader.GetString(1);
                        worker.SpecialityId = reader.GetInt32(2);
                        worker.BossId = reader.GetInt32(3);
                        worker.IsBoss = reader.GetInt32(4) != 0;
                        speciality.Id = worker.SpecialityId;
                        speciality.Name = reader.GetString(5);
                        var BossName = reader.GetValue(6);
                        result += $"Worker name: {worker.Name}\nBoss Name: {BossName} \nWorker speciality: {speciality.Name};\n\n";
                    }
                }
            }
            var time = watch.ElapsedMilliseconds;
            result += $"\nExecuting time: {time}";
            MessageBox.Show(result);
        }
        private async void TryDapper(object sender, EventArgs e)
        {
            result = "";
            var watch = System.Diagnostics.Stopwatch.StartNew();
            using (IDbConnection cnn = new SQLiteConnection(connectionString))
            {
                cnn.Open();
                var Workers = await cnn.QueryAsync<Worker>("select * from Workers");
                foreach (var worker in Workers)
                {

                    var SpecialityName = await cnn.QueryAsync<string>($"select Name from Specialities where Id={worker.SpecialityId}");
                    var BossName = await cnn.QueryAsync<string>($"select Name from Workers where Id = {worker.BossId}");
                    result += $"Worker name: {worker.Name}\nWorker boss: {BossName.FirstOrDefault()}\nWorker speciality: {SpecialityName.First()};\n\n";
                }
            }
            var time = watch.ElapsedMilliseconds;
            result += $"Executing time: {time}";
            MessageBox.Show(result);
        }
    }
}
