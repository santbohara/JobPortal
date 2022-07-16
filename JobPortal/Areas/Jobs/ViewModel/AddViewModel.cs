using JobPortal.Areas.Config.Models;

namespace JobPortal.Areas.Jobs.ViewModel
{
    public class AddViewModel
    {
        public List<JobCategory> JobCategory { get; set; }
        public List<JobQualification> JobQualification { get; set; }
        public List<JobType> JobType { get; set; }
        public List<SalaryType> SalaryType { get; set; }
        public List<SalaryRange> SalaryRange { get; set; }
        public List<JobExperience> JobExperience { get; set; }
        public List<JobShift> JobShift { get; set; }
        public List<JobLevel> JobLevel { get; set; }
    }
}
