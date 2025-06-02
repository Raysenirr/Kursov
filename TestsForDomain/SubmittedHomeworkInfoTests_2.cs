using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

public class SubmittedHomeworkInfoTests
{
    [Fact]
    public void Constructor_ShouldInitializeFields_WhenValid()
    {
        var group = new Group(new GroupName("G-Info-2"));
        var teacher = new Teacher(new PersonName("MrHomework"));
        var student = new Student(new PersonName("StudentH"), group);
        var topic = new LessonTopic("TopicH");
        var title = new HomeworkTitle("HWTitle");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        var homework = new Homework(lesson, title);

        var submitted = new SubmittedHomeworkInfo(homework, student, DateTime.UtcNow);

        submitted.Homework.Should().Be(homework);
        submitted.Student.Should().Be(student);
        submitted.SubmittedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void IsLate_ShouldReturnTrue_WhenSubmittedAfterLessonTime()
    {
        var group = new Group(new GroupName("G-Info-2"));
        var teacher = new Teacher(new PersonName("MrHomework"));
        var student = new Student(new PersonName("StudentH"), group);
        var topic = new LessonTopic("TopicH");
        var lessonTime = DateTime.UtcNow;
        var lesson = new Lesson(group, teacher, lessonTime, topic);
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        var homework = new Homework(lesson, new HomeworkTitle("HWLate"));

        var submissionDate = lessonTime.AddSeconds(5);
        var submitted = new SubmittedHomeworkInfo(homework, student, submissionDate);


        submitted.IsLate.Should().BeTrue();
    }

    [Fact]
    public void IsLate_ShouldReturnFalse_WhenSubmittedAtSameTime()
    {
        var group = new Group(new GroupName("G-Info-2"));
        var teacher = new Teacher(new PersonName("MrHomework"));
        var student = new Student(new PersonName("StudentH"), group);
        var topic = new LessonTopic("TopicH");
        var now = DateTime.UtcNow;
        var lesson = new Lesson(group, teacher, now, topic);
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        var homework = new Homework(lesson, new HomeworkTitle("HWOnTime"));

        var submitted = new SubmittedHomeworkInfo(homework, student, now);

        submitted.IsLate.Should().BeFalse();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenHomeworkIsNull()
    {
        var group = new Group(new GroupName("G-Err-2"));
        var student = new Student(new PersonName("ErrS"), group);

        Action act = () => new SubmittedHomeworkInfo(null!, student, DateTime.UtcNow);

        act.Should().Throw<ArgumentNullException>().WithParameterName("Homework");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenStudentIsNull()
    {
        var group = new Group(new GroupName("G-Info-2"));
        var teacher = new Teacher(new PersonName("MrHomework"));
        var topic = new LessonTopic("TopicH");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        teacher.TeachLesson(lesson);
        var homework = new Homework(lesson, new HomeworkTitle("ErrHW"));

        Action act = () => new SubmittedHomeworkInfo(homework, null!, DateTime.UtcNow);

        act.Should().Throw<ArgumentNullException>().WithParameterName("student");
    }
}
