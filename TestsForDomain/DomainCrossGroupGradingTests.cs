using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Exceptions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class DomainCrossGroupGradingTests
{
    [Fact]
    public void GradingStudentFromAnotherGroup_ShouldThrow()
    {
        // Arrange
        // Создаём две разные учебные группы
        var groupA = new Group(new GroupName("Group-A-6"));
        var groupB = new Group(new GroupName("Group-B-7"));

        // Преподаватель и студент из другой (чужой) группы
        var teacher = new Teacher(new PersonName("Prof Васильев"));
        var foreignStudent = new Student(new PersonName("Other Group Student"), groupB);

        // Добавляем шаблон домашнего задания
        teacher.HomeworkBank.AddTemplate(
            new LessonTopic("Cross Validation"),
            new HomeworkTitle("Cross Validation HW"));

        // Создаём урок для группы A
        var lesson = new Lesson(groupA, teacher, DateTime.UtcNow.AddMinutes(-60), new LessonTopic("Cross Validation"));

        // Назначаем домашнее задание из шаблона
        var homework = teacher.AssignHomeworkFromBank(lesson);

        // Преподаватель проводит урок
        teacher.TeachLesson(lesson);

        // Act
        // Пытаемся выставить оценку студенту из другой группы — ожидаем исключение
        Action act = () => teacher.GradeStudent(foreignStudent, Mark.Good, lesson, homework);

        // Assert
        // Проверяем, что выброшено ожидаемое исключение
        act.Should().Throw<LessonNotVisitedException>();
    }
}

