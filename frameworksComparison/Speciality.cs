using System.ComponentModel.DataAnnotations;

namespace frameworksComparison
{
    public class Speciality
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
