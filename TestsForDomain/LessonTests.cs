using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class LessonTests
{
    [Fact]
    public void AddHomework_ShouldAdd_WhenLessonMatches()
    {
        var teacher = new Teacher(new PersonName("TLesson"));
        var group = new Group(new GroupName("G-7-1"));
        var topic = new LessonTopic("T");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow.AddHours(1), topic);
        var homework = new Homework(lesson, new HomeworkTitle("Task A"));

        lesson.AddHomework(homework);

        lesson.AssignedHomeworks.Should().Contain(homework);
    }

    [Fact]
    public void AddHomework_ShouldThrow_WhenLessonDoesNotMatch()
    {
        var teacher = new Teacher(new PersonName("Tкуку"));
        var group = new Group(new GroupName("Gкуку-7-1"));
        var topic = new LessonTopic("Tукук");
        var lesson1 = new Lesson(group, teacher, DateTime.UtcNow, topic);
        var lesson2 = new Lesson(group, teacher, DateTime.UtcNow.AddDays(1), topic);
        var homework = new Homework(lesson2, new HomeworkTitle("Mismatch"));

        Action act = () => lesson1.AddHomework(homework);

        act.Should().Throw<HomeworkLessonMismatchException>();
    }

    [Fact]
    public void Reschedule_ShouldChangeTime_WhenValid()
    {
        var teacher = new Teacher(new PersonName("TResched"));
        var group = new Group(new GroupName("GRes-7-1"));
        var topic = new LessonTopic("аваука");
        var oldTime = DateTime.UtcNow.AddDays(2);
        var newTime = oldTime.AddDays(1);
        var lesson = new Lesson(group, teacher, oldTime, topic);

        lesson.Reschedule(newTime);

        lesson.ClassTime.Should().Be(newTime);
    }

    [Fact]
    public void Reschedule_ShouldThrow_WhenAlreadyTeached()
    {
        var teacher = new Teacher(new PersonName("TававR"));
        var group = new Group(new GroupName("GR-7-1"));
        var topic = new LessonTopic("Tепа");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        lesson.Teach();

        Action act = () => lesson.Reschedule(DateTime.UtcNow.AddDays(3));

        act.Should().Throw<LessonAlreadyTeachedException>();
    }

    [Fact]
    public void Cancel_ShouldChangeStateToCancelled()
    {
        var teacher = new Teacher(new PersonName("TCancel"));
        var group = new Group(new GroupName("G-7-1"));
        var topic = new LessonTopic("Tывы");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        lesson.Cancel();

        lesson.State.Should().Be(LessonStatus.Canselled);
    }

    [Fact]
    public void Teach_ShouldChangeStateToTeached()
    {
        var teacher = new Teacher(new PersonName("TTeach"));
        var group = new Group(new GroupName("Gавв-7-1"));
        var topic = new LessonTopic("Tава");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        lesson.Teach();

        lesson.State.Should().Be(LessonStatus.Teached);
    }

    [Fact]
    public void GetHomeworkById_ShouldReturnCorrectHomework()
    {
        var teacher = new Teacher(new PersonName("TTopic"));
        var group = new Group(new GroupName("GTopic-7-1"));
        var topic = new LessonTopic("TopicX");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        var homework = new Homework(lesson, new HomeworkTitle("TopicX"));
        lesson.AddHomework(homework);

        var found = lesson.GetHomeworkById(homework.Id); 

        found.Should().Be(homework);
    }

    [Fact]
    public void Cancel_ShouldThrow_WhenAlreadyTeached()
    {
        var teacher = new Teacher(new PersonName("TEdge"));
        var group = new Group(new GroupName("Gввы-7-1"));
        var topic = new LessonTopic("TEdge");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        lesson.Teach();

        Action act = () => lesson.Cancel();

        act.Should().Throw<LessonAlreadyTeachedException>();
    }
}
