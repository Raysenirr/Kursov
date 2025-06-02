using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class StudentTests
{
    [Fact]
    public void AttendLesson_ShouldSucceed_WhenValid()
    {
        var group = new Group(new GroupName("Test-1-1"));
        var teacher = new Teacher(new PersonName("MrSmith"));
        var student = new Student(new PersonName("Alice"), group);
        var topic = new LessonTopic("LessonA");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        student.AttendedLessons.Should().Contain(lesson);
    }

    [Fact]
    public void SubmitHomework_ShouldSucceed_WhenValid()
    {
        // Arrange
        var group = new Group(new GroupName("Test-1-1"));
        var teacher = new Teacher(new PersonName("MrSmith"));
        var student = new Student(new PersonName("Alice"), group);
        var topic = new LessonTopic("LessonA");
        var title = new HomeworkTitle("Submit HW");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var homework = new Homework(lesson, title);
        lesson.AddHomework(homework);

        var submissionTime = DateTime.UtcNow;

        // Act
        student.SubmitHomework(homework, submissionTime);

        // Assert
        homework.Submissions.Should().ContainSingle(s =>
            s.StudentId == student.Id &&
            s.SubmissionDate == submissionTime);
    }

    [Fact]
    public void GetGradeByLesson_ShouldReturnGrade_WhenExists()
    {
        var group = new Group(new GroupName("Test-1-1"));
        var teacher = new Teacher(new PersonName("MrSmith"));
        var student = new Student(new PersonName("Alice"), group);
        var topic = new LessonTopic("LessonA");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        var homework = new Homework(lesson, new HomeworkTitle("HWsds"));
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        teacher.GradeStudent(student, Mark.Excellent, lesson, homework);

        var grade = student.GetGradeByLesson(lesson);
        grade.Mark.Should().Be(Mark.Excellent);
    }

    [Fact]
    public void GetAssignedHomeworks_ShouldReturnHomeworks_ForAttendedLessons()
    {
        var group = new Group(new GroupName("Test-1-1"));
        var teacher = new Teacher(new PersonName("MrSmith"));
        var student = new Student(new PersonName("Alice"), group);
        var topic = new LessonTopic("LessonA");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        var homework = new Homework(lesson, new HomeworkTitle("HWsds"));

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        lesson.AddHomework(homework);

        var result = student.GetAssignedHomeworks();

        result.Should().Contain(homework);
    }
}
