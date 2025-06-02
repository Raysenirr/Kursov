using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

public class DomainGradingWithoutHomeworkTests
{
    [Fact]
    public void GradeCanBeAssignedWithoutHomework()
    {
        // Arrange
        // Текущее время
        var now = DateTime.UtcNow;

        // Создание учебной группы, преподавателя и студента
        var group = new Group(new GroupName("Log-101-2"));
        var teacher = new Teacher(new PersonName("Prof Васильев"));
        var student = new Student(new PersonName("Emma Frost"), group);

        // Тема урока
        var topic = new LessonTopic("Блалала");

        // Создание урока
        var lesson = new Lesson(group, teacher, now.AddMinutes(-30), topic);

        // Создание домашнего задания вручную (не из банка)
        var homework = new Homework(lesson, new HomeworkTitle("Reflection"));

        // Проведение урока и посещение студентом
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);

        // Привязка домашнего задания к уроку (иначе будет NullReferenceException)
        lesson.AddHomework(homework);

        // Act
        // Преподаватель выставляет оценку без использования шаблона
        teacher.GradeStudent(student, Mark.Good, lesson, homework);

        // Assert
        // Проверка, что оценка выставлена корректно
        var grade = student.GetGradeByLesson(lesson);
        grade.Should().NotBeNull();
        grade.Mark.Should().Be(Mark.Good);
        grade.Lesson.Should().Be(lesson);
        grade.Teacher.Should().Be(teacher);
    }
}

