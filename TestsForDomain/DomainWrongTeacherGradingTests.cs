using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class DomainWrongTeacherGradingTests
{
    [Fact]
    public void GradingByAnotherTeacher_ShouldThrowException()
    {
        // 1. Создание группы, студентов и двух учителей
        var group = new Group(new GroupName("G-7-5"));
        var realTeacher = new Teacher(new PersonName("ProfReal"));
        var fakeTeacher = new Teacher(new PersonName("ProfFaker"));
        var student = new Student(new PersonName("Rogue"), group);

        // 2. Урок назначается реальным учителем
        var topic = new LessonTopic("Mutation Control");
        var lesson = new Lesson(group, realTeacher, DateTime.UtcNow.AddMinutes(-20), topic);

        // 3. Урок проводится реальным учителем
        realTeacher.TeachLesson(lesson);

        // 4. Назначается домашнее задание
        realTeacher.HomeworkBank.AddTemplate(topic, new HomeworkTitle("DNA Practice"));
        var homework = realTeacher.AssignHomeworkFromBank(lesson);

        // 5. Студент посещает урок и сдает домашку
        student.AttendLesson(lesson);
        student.SubmitHomework(homework, DateTime.UtcNow);

        // 6. Попытка фальшивого учителя выставить оценку
        Action act = () => fakeTeacher.GradeStudent(student, Mark.Good, lesson, homework);

        // 7. Проверка, что выброшено правильное исключение
        act.Should().Throw<AnotherTeacherLessonGradedException>();
    }
}
