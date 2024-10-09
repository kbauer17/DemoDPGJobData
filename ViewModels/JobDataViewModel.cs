using DemoDPGJobData.Models;

namespace DemoDPGJobData.ViewModels
{
    public class JobDataViewModel
    {
        public required DemoJobData DemoJobData {get; set;}

        public required JobOp JobOp {get; set;}

        public JobDataViewModel(){}

        public JobDataViewModel(DemoJobData demoJobData, JobOp jobOp)
        {
            this.DemoJobData = demoJobData;
            this.JobOp = jobOp;
        }
    }
}