using DemoDPGJobData.Models;

namespace DemoDPGJobData.ViewModels
{
    public class AdministrationViewModel
    {
        public required JobOp JobOp {get; set;}

        public AdministrationViewModel(){}

        public AdministrationViewModel(JobOp jobOp)
        {
            this.JobOp = jobOp;
        }
    }

}