using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace frameworksComparison
{
    public interface IConnector
    {
        List<Worker> GetWorkers();
        Worker GetWorkerById(int id);
        Speciality GetSpecialityById(int id);
    }
}
