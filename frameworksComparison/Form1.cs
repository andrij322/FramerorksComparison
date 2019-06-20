using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SQLite;
using Dapper;
using System.Collections.Generic;
using Ninject;

namespace frameworksComparison
{
    public partial class Form1 : Form
    {
        private IConnector iconnector;
        private string result = "";
        private string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        IKernel kernel;

        public Form1()
        {
            kernel = new StandardKernel();
            InitializeComponent();
        }


        private string ShowWorkersInfo()
        {
            result = "";
            var workers = iconnector.GetWorkers();
            foreach (var worker in workers)
            {
                var w = iconnector.GetWorkerById(worker.BossId);
                var bossName = w==null ? null : w.Name;
                var specialityName = iconnector.GetSpecialityById(worker.SpecialityId).Name;
                result += $"Worker name: {worker.Name}\nWorker boss: {bossName}\nWorker speciality: {specialityName};\n\n";
            }
            return result;
        }


        private void TryEntity(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            kernel.Rebind<IConnector>().To<EntityConnector>();
            iconnector = kernel.Get<IConnector>();
            var result = ShowWorkersInfo();
            var time = watch.ElapsedMilliseconds;
            result += $"Executing time: {time}";
            MessageBox.Show(result);
        }


        private void TryADO(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            kernel.Rebind<IConnector>().To<ADOConnector>().WithConstructorArgument(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            iconnector = kernel.Get<IConnector>();
            var result = ShowWorkersInfo();
            var time = watch.ElapsedMilliseconds;
            result += $"Executing time: {time}";
            MessageBox.Show(result);
        }


        private void TryDapper(object sender, EventArgs e)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            kernel.Rebind<IConnector>().To<DapperConnector>().WithConstructorArgument(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            iconnector = kernel.Get<IConnector>();
            var result = ShowWorkersInfo();
            var time = watch.ElapsedMilliseconds;
            result += $"Executing time: {time}";
            MessageBox.Show(result);
        }
    }
}
