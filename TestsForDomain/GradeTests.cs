using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class GradeTests
{
    [Fact]
    public void Constructor_ShouldCreateGrade_WhenValid()
    {
        // Arrange — создаём все необходимые сущности
        var group = new Group(new GroupName("G-Grade-1"));
        var teacher = new Teacher(new PersonName("MrGrade"));
        var student = new Student(new PersonName("Sewdfw"), group);
        var topic = new LessonTopic("Grading");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow.AddMinutes(-5), topic);

        teacher.TeachLesson(lesson); // урок проводится
        student.AttendLesson(lesson); // студент присутствует

        var now = DateTime.UtcNow;

        // Act — создаём объект Grade (оценка)
        var grade = new Grade(teacher, student, lesson, now, Mark.Good);

        // Assert — проверяем, что поля заданы корректно
        grade.Teacher.Should().Be(teacher);
        grade.Student.Should().Be(student);
        grade.Lesson.Should().Be(lesson);
        grade.Mark.Should().Be(Mark.Good);
        grade.GradedTime.Should().BeCloseTo(now, TimeSpan.FromSeconds(1)); // ≈ точно во времени
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenLessonNotTeached()
    {
        // Arrange — создаём урок, который ещё не проводился
        var group = new Group(new GroupName("G-Grade-1"));
        var teacher = new Teacher(new PersonName("MrGrade"));
        var student = new Student(new PersonName("Sewdfw"), group);
        var topic = new LessonTopic("Grading");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow, topic); // нет teacher.TeachLesson()

        // Act & Assert — попытка оценить вызывает исключение
        Action act = () => new Grade(teacher, student, lesson, DateTime.UtcNow, Mark.Satisfactorily);
        act.Should().Throw<LessonNotStartedException>();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTeacherMismatch()
    {
        // Arrange — преподаватель, указанный в Grade, отличается от назначенного в Lesson
        var group = new Group(new GroupName("G-Y-1"));
        var realTeacher = new Teacher(new PersonName("Real"));
        var wrongTeacher = new Teacher(new PersonName("Wrong"));
        var student = new Student(new PersonName("Scxkjzncjn"), group);
        var topic = new LessonTopic("Mismatch");
        var lesson = new Lesson(group, realTeacher, DateTime.UtcNow.AddMinutes(-10), topic);

        realTeacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        // Act & Assert — посторонний учитель не может ставить оценку
        Action act = () => new Grade(wrongTeacher, student, lesson, DateTime.UtcNow, Mark.Good);
        act.Should().Throw<AnotherTeacherLessonGradedException>();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenStudentDidNotAttend()
    {
        // Arrange — студент НЕ посещал урок
        var group = new Group(new GroupName("G-Grade-1"));
        var teacher = new Teacher(new PersonName("MrGrade"));
        var student = new Student(new PersonName("Sewdfw"), group);
        var topic = new LessonTopic("Grading");
        var lesson = new Lesson(group, teacher, DateTime.UtcNow.AddMinutes(-10), topic);

        teacher.TeachLesson(lesson); // но студент не был на уроке

        // Act & Assert — студенту нельзя выставлять оценку
        Action act = () => new Grade(teacher, student, lesson, DateTime.UtcNow, Mark.Excellent);
        act.Should().Throw<LessonNotVisitedException>();
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenGradedTimeIsBeforeLesson()
    {
        // Arrange — оценка ставится раньше времени начала урока
        var group = new Group(new GroupName("G-Grade-1"));
        var teacher = new Teacher(new PersonName("MrGrade"));
        var student = new Student(new PersonName("Sewdfw"), group);
        var topic = new LessonTopic("Grading");
        var lessonTime = DateTime.UtcNow.AddMinutes(10); // в будущем
        var lesson = new Lesson(group, teacher, lessonTime, topic);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        var early = DateTime.UtcNow; // время раньше начала урока

        // Act & Assert — выставление оценки "заранее" недопустимо
        Action act = () => new Grade(teacher, student, lesson, early, Mark.Poor);
        act.Should().Throw<LessonNotStartedException>();
    }
}

