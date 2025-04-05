namespace HiringManager.Models.ViewModel
{
    public class ApplicationFormViewModel
    {
        public ApplicationDetails ApplicationDetails { get; set; } = new ApplicationDetails();
        public PersonalDetails PersonalDetails { get; set; } = new PersonalDetails();
        public EducationDetails EducationDetails { get; set; } = new EducationDetails();
        public ExperienceDetails ExperienceDetails { get; set; } = new ExperienceDetails();
        public int CurrentStage { get; set; } = 1;
    }
}
