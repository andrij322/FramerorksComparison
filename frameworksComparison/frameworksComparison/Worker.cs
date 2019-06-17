using System.ComponentModel.DataAnnotations;

namespace frameworksComparison
{
    public class Worker
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
        public int SpecialityId { get; set; }
        public int BossId { get; set; }
        public bool IsBoss { get; set; }
    }
}
