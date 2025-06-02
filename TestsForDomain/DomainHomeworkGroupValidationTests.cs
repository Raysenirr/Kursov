using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class DomainHomeworkGroupValidationTests
{
    [Fact]
    public void SubmittingHomeworkFromAnotherGroup_ShouldThrow()
    {
        // Arrange
        // Создание двух разных групп
        var groupA = new Group(new GroupName("A-7-7"));
        var groupB = new Group(new GroupName("B-7-7"));

        // Преподаватель и студент из другой (неправильной) группы
        var teacher = new Teacher(new PersonName("Prof Васильев"));
        var student = new Student(new PersonName("Wrong Group Student"), groupB); // студент не из той группы

        // Создание и проведение урока для groupA
        var lesson = new Lesson(groupA, teacher, DateTime.UtcNow.AddMinutes(-45), new LessonTopic("БЛалала"));
        teacher.TeachLesson(lesson);

        // Назначение домашнего задания к этому уроку
        var homework = new Homework(lesson, new HomeworkTitle("Subgroups"));
        lesson.AddHomework(homework); // важно добавить, чтобы не было NullReference

        // Act
        // Попытка студента из другой группы сдать задание — должна вызвать исключение
        Action act = () => student.SubmitHomework(homework, DateTime.UtcNow);

        // Assert
        // Ожидаем, что будет выброшено исключение о принадлежности к другой группе
        act.Should().Throw<AnotherGroupLessonException>();
    }
}
