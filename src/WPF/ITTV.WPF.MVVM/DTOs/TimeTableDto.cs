using ITTV.WPF.Abstractions.Enums;

namespace ITTV.WPF.MVVM.DTOs
{
    public class TimeTableDto
    {
        public TimeTableDto()
        { }

        public TimeTableDto(DegreeEnum? degree,
            int? courseNumber, 
            string groupType,
            string groupName)
        {
            Degree = degree;
            CourseNumber = courseNumber;
            GroupType = groupType;
            GroupName = groupName;
        }
        public DegreeEnum? Degree { get; private set; }
        public int? CourseNumber { get; private set; }
        public string GroupType { get; private set; }
        public string GroupName { get; private set; }

        public void SetDegree(DegreeEnum degree)
        {
            Degree = degree;
        }
        public void SetCourseNumber(int courseNumber)
        {
            CourseNumber = courseNumber;
        }
        public void SetGroupType(string groupType)
        {
            GroupType = groupType;
        }
        public void SetGroupName(string groupName)
        {
            GroupName = groupName;
        }

        public void Merge(TimeTableDto timeTableDto)
        {
            Degree = timeTableDto.Degree;
            CourseNumber = timeTableDto.CourseNumber;
            GroupType = timeTableDto.GroupType;
            GroupName = timeTableDto.GroupName;
        }
    }
}