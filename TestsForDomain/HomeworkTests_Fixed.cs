using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class HomeworkTests
{
    [Fact]
    public void SubmitBy_ShouldAddSubmission_WhenStudentAttended()
    {
        var teacher = new Teacher(new PersonName("THW"));
        var group = new Group(new GroupName("G-HW-1"));
        var student = new Student(new PersonName("SHW"), group);
        var topic = new LessonTopic("TopicHW");
        var title = new HomeworkTitle("HWрг");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var homework = new Homework(lesson, title);
        lesson.AddHomework(homework);

        var submissionTime = DateTime.UtcNow;
        homework.SubmitBy(student, submissionTime);

        var submission = homework.Submissions.SingleOrDefault(s => s.StudentId == student.Id);
        submission.Should().NotBeNull();
        submission!.SubmissionDate.Should().Be(submissionTime);
    }

    [Fact]
    public void SubmitBy_ShouldThrow_WhenNotAttended()
    {
        var teacher = new Teacher(new PersonName("THW"));
        var group = new Group(new GroupName("G-HW-1"));
        var student = new Student(new PersonName("SHW"), group);
        var topic = new LessonTopic("TopicHW");
        var title = new HomeworkTitle("HWрг");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        var homework = new Homework(lesson, title);
        lesson.AddHomework(homework);

        Action act = () => homework.SubmitBy(student, DateTime.UtcNow);

        act.Should().Throw<LessonNotVisitedException>();
    }

    [Fact]
    public void SubmitBy_ShouldThrow_WhenAlreadySubmitted()
    {
        var teacher = new Teacher(new PersonName("THW"));
        var group = new Group(new GroupName("G-HW-1"));
        var student = new Student(new PersonName("SHW"), group);
        var topic = new LessonTopic("TopicHW");
        var title = new HomeworkTitle("HWрг");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var homework = new Homework(lesson, title);
        lesson.AddHomework(homework);

        homework.SubmitBy(student, DateTime.UtcNow);

        Action act = () => homework.SubmitBy(student, DateTime.UtcNow);
        act.Should().Throw<HomeworkAlreadySubmittedException>();
    }

    [Fact]
    public void IsLate_ShouldReturnTrue_WhenSubmittedAfterLessonDate()
    {
        var teacher = new Teacher(new PersonName("THW"));
        var group = new Group(new GroupName("G-HW-1"));
        var student = new Student(new PersonName("SHW"), group);
        var topic = new LessonTopic("TopicHW");

        var lessonDate = DateTime.UtcNow.AddMinutes(-2);
        var submissionDate = lessonDate.AddSeconds(2);

        var lesson = new Lesson(group, teacher, lessonDate, topic);
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var homework = new Homework(lesson, new HomeworkTitle("HW-Late"));
        lesson.AddHomework(homework);

        homework.SubmitBy(student, submissionDate);

        homework.IsLate(student).Should().BeTrue();
    }


    [Fact]
    public void IsLate_ShouldReturnFalse_WhenSubmittedOnSameDay()
    {
        var teacher = new Teacher(new PersonName("THW"));
        var group = new Group(new GroupName("G-HW-1"));
        var student = new Student(new PersonName("SHW"), group);
        var topic = new LessonTopic("TopicHW");

        var lessonDate = DateTime.UtcNow.AddMinutes(1);
        var lesson = new Lesson(group, teacher, lessonDate, topic);
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var homework = new Homework(lesson, new HomeworkTitle("HW-OnTime"));
        lesson.AddHomework(homework);

        homework.SubmitBy(student, lessonDate);

        homework.IsLate(student).Should().BeFalse();
    }

}
