using System;
using System.Linq;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

public class TeacherTests
{
    [Fact]
    public void TeachLesson_ShouldMarkLessonAsTeached_AndAddToList()
    {
        var teacher = new Teacher(new PersonName("IvanTeach"));
        var group = new Group(new GroupName("Math-1-1"));
        var topic = new LessonTopic("Topic");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow.AddHours(1), topic);

        teacher.TeachLesson(lesson);

        lesson.State.Should().Be(LessonStatus.Teached);
        teacher.TeachedLessons.Should().Contain(lesson);
    }

    [Fact]
    public void GradeStudent_ShouldAddGrade_WhenConditionsAreMet()
    {
        var teacher = new Teacher(new PersonName("Anna"));
        var group = new Group(new GroupName("Phys-1-1"));
        var student = new Student(new PersonName("Max"), group);
        var topic = new LessonTopic("Topi аллввщы");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        var homework = new Homework(lesson, new HomeworkTitle("HWsds"));
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        teacher.GradeStudent(student, Mark.Excellent, lesson, homework);

        teacher.AssignedGrades.Should().ContainSingle(g => g.Student == student && g.Lesson == lesson);
        student.RecievedGrades.Should().ContainSingle();
    }

    //[Fact]
    //public void ScheduleLesson_ShouldAddLesson_IfNew()
    //{
    //    var teacher = new Teacher(new PersonName("Greg"));
    //    var group = new Group(new GroupName("Hist-1-1"));
    //    var topic = new LessonTopic("Hist");
    //    var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

    //    teacher.SchedulledLessons.Should().Contain(lesson);
    //}

    [Fact]
    public void AssignHomeworkFromBank_ShouldCreateAndAddHomework()
    {
        var teacher = new Teacher(new PersonName("Olga"));
        var group = new Group(new GroupName("Bio-1-1"));
        var topic = new LessonTopic("Biology");
        var title = new HomeworkTitle("Describe cell structure");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow.AddDays(1), topic);

        teacher.HomeworkBank.AddTemplate(topic, title);
        var homework = teacher.AssignHomeworkFromBank(lesson);

        lesson.AssignedHomeworks.Should().Contain(homework);
        homework.Title.Value.Should().Be(title.Value);
    }

    [Fact]
    public void GetTeachedGroups_ShouldReturnDistinctGroups()
    {
        var teacher = new Teacher(new PersonName("Bio"));
        var group = new Group(new GroupName("Bio-1-1"));
        var topic = new LessonTopic("BioA");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        teacher.TeachLesson(lesson);
        var groups = teacher.GetTeachedGroups();

        groups.Should().ContainSingle().And.Contain(group);
    }

    [Fact]
    public void GetTeachedStudents_ShouldReturnAllUniqueStudents()
    {
        var teacher = new Teacher(new PersonName("Geo"));
        var group = new Group(new GroupName("Geo-1-1"));
        var topic = new LessonTopic("Geo");
        var student = new Student(new PersonName("Geo"), group);
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        var students = teacher.GetTeachedStudents();

        students.Should().ContainSingle().And.Contain(student);
    }

    [Fact]
    public void GetSubmittedHomeworks_ShouldReturnSubmittedAssignments()
    {
        var teacher = new Teacher(new PersonName("ITTeach"));
        var group = new Group(new GroupName("IT-12-1"));
        var topic = new LessonTopic("ProgrammingIntro");
        var title = new HomeworkTitle("Print Hello World");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic);
        var student = new Student(new PersonName("StudCoder"), group);

        teacher.TeachLesson(lesson);
        teacher.HomeworkBank.AddTemplate(topic, title);
        var homework = teacher.AssignHomeworkFromBank(lesson);
        student.AttendLesson(lesson);
        student.SubmitHomework(homework, DateTime.UtcNow);

        var submitted = teacher.GetSubmittedHomeworks();

        submitted.Should().ContainSingle(info => info.Student == student && info.Homework == homework);
    }
}
