using ITTV.WPF.Abstractions.Base.Command;

namespace ITTV.WPF.MVVM.DTOs
{
    public class TimeTableQuestionDto
    {
        public TimeTableQuestionDto()
        { }

        public TimeTableQuestionDto(string title,
            CommandBase navigateCommand)
        {
            Title = title;
            NavigateCommand = navigateCommand;
        }
        public string Title { get; set; }
        public CommandBase NavigateCommand { get; set; }
    }
}