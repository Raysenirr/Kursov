using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

public class DomainLateHomeworkTestsa
{
    [Fact]
    public void LateSubmission_ShouldBeDetected()
    {
        // Arrange
        var lessonTime = DateTime.UtcNow;
        var submissionTime = lessonTime.AddMinutes(1);

        var group = new Group(new GroupName("Phys-10-12"));
        var teacher = new Teacher(new PersonName("Professor X"));
        var student = new Student(new PersonName("Scott Summers"), group);

        var lesson = new Lesson(group, teacher, lessonTime, new LessonTopic("Optics"));

        // Act
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var homework = new Homework(lesson, new HomeworkTitle("Reflection"));
        lesson.AddHomework(homework);

        homework.SubmitBy(student, submissionTime);
        teacher.GradeStudent(student, Mark.Excellent, lesson, homework);

        // Assert
        var grade = student.GetGradeByLesson(lesson);
        grade.Mark.Should().Be(Mark.Satisfactorily);
    }
}
