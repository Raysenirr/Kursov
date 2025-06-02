using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;

public class DomainIntegrationTests
{
    [Fact]
    public void StudentHomeworkAndGrading_FullFlow_WorksCorrectly()
    {
        // Arrange
        // Создание группы, преподавателя и студента
        var group = new Group(new GroupName("INT-20-1"));
        var teacher = new Teacher(new PersonName("Dr Xavier"));
        var student = new Student(new PersonName("Jean Grey"), group);

        // Создание урока по теме
        var topic = new LessonTopic("Telepathy");
        var lessonDate = DateTime.UtcNow.AddMinutes(-30); // урок был 30 минут назад
        var lesson = new Lesson(group, teacher, lessonDate, topic);

        // Проведение урока преподавателем
        teacher.TeachLesson(lesson);

        // Назначение домашнего задания из шаблона
        var homeworkTitle = new HomeworkTitle("Mind Reading Practice");
        teacher.HomeworkBank.AddTemplate(topic, homeworkTitle);
        var homework = teacher.AssignHomeworkFromBank(lesson);

        // Студент посещает урок и получает задание
        student.AttendLesson(lesson);
        var assigned = student.GetAssignedHomeworks();

        // Проверка, что домашка действительно назначена студенту
        assigned.Should().Contain(homework);

        // Студент сдаёт задание вовремя
        student.SubmitHomework(homework, lessonDate);

        // Act
        // Преподаватель выставляет оценку за сданную работу
        teacher.GradeStudent(student, Mark.Excellent, lesson, homework);

        // Assert
        // Проверка, что оценка выставлена корректно
        var grade = student.GetGradeByLesson(lesson);
        grade.Should().NotBeNull(); // оценка должна существовать
        grade.Mark.Should().Be(Mark.Excellent); // оценка должна быть "отлично"
        grade.Teacher.Should().Be(teacher); // выставлена текущим преподавателем
        grade.Lesson.Id.Should().Be(lesson.Id); // по правильному уроку

        // Проверка информации о сдаче — она должна быть одна и не просрочена
        var submission = teacher.GetSubmittedHomeworks();
        submission.Should().ContainSingle(s =>
            s.Homework.Id == homework.Id &&
            s.Student.Id == student.Id &&
            !s.IsLate // работа сдана вовремя
        );
    }
}
