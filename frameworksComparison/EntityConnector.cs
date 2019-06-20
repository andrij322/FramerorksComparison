using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace frameworksComparison
{
    public class EntityConnector : IConnector
    {
        private WorkerContext _db;


        public List<Worker> GetWorkers()
        {
            using (_db = new WorkerContext())
            {
                return _db.Workers.ToList();
            }
        }


        public Worker GetWorkerById(int id)
        {
            var workers = GetWorkers();
            var worker = workers.Where<Worker>(w=>w.Id==id).FirstOrDefault();
            return worker;
        }


        public Speciality GetSpecialityById(int id)
        {
            using (_db = new WorkerContext())
            {
                var specialities = _db.Specialities;
                var speciality = specialities.Where<Speciality>(s => s.Id == id).FirstOrDefault();
                return speciality;
            }
        }

        public void DoSmthng()
        {
            MessageBox.Show("Do something");
        }
    }
}
