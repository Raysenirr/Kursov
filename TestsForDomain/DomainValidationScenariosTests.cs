using Education.Domain.Entities;
using Education.Domain.Enums;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;
using Education.Domain.ValueObjects;
using FluentAssertions;

public class DomainValidationScenariosTests
{
    [Fact]
    public void DuplicateGrade_ShouldThrow()
    {
        // Arrange — подготовка сущностей: урок, студент, преподаватель
        var now = DateTime.UtcNow;
        var group = new Group(new GroupName("Grade-D-3"));
        var teacher = new Teacher(new PersonName("Prof Васильев"));
        var student = new Student(new PersonName("Logan"), group);
        var topic = new LessonTopic("Steel Skin");
        var lesson = new Lesson(group, teacher, now, topic);
        var homework = new Homework(lesson, new HomeworkTitle("Durability"));

        lesson.AddHomework(homework);
        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        student.SubmitHomework(homework, now);

        // Первая корректная попытка оценки
        teacher.GradeStudent(student, Mark.Good, lesson, homework);

        // Act  Assert — повторная попытка оценивания должна вызвать исключение
        Action act = () => teacher.GradeStudent(student, Mark.Excellent, lesson, homework);
        act.Should().Throw<DoubleGradeStudentLesson>();
    }

    [Fact]
    public void GradingUnstartedLesson_ShouldThrow()
    {
        // Arrange — урок запланирован на будущее
        var now = DateTime.UtcNow;
        var group = new Group(new GroupName("Un-6-8"));
        var teacher = new Teacher(new PersonName("Prof NotYet"));
        var student = new Student(new PersonName("Xапв"), group);
        var topic = new LessonTopic("Focus");
        var lesson = new Lesson(group, teacher, now.AddMinutes(30), topic); // будущий урок
        var homework = new Homework(lesson, new HomeworkTitle("Mind HW"));
        lesson.AddHomework(homework);

        // Act  Assert — оценивание недоступно до проведения урока
        Action act = () => teacher.GradeStudent(student, Mark.Good, lesson, homework);
        act.Should().Throw<LessonNotStartedException>();
    }

    [Fact]
    public void SubmittingBeforeLesson_ShouldThrow()
    {
        // Arrange — попытка сдачи до урока
        var lessonTime = DateTime.UtcNow.AddMinutes(30);
        var earlySubmit = DateTime.UtcNow;

        var group = new Group(new GroupName("E-2-4"));
        var teacher = new Teacher(new PersonName("Prof Timewarp"));
        var student = new Student(new PersonName("Blink"), group);
        var topic = new LessonTopic("Portals");

        var lesson = new Lesson(group, teacher, lessonTime, topic);
        var homework = new Homework(lesson, new HomeworkTitle("Jumps"));
        lesson.AddHomework(homework);

        // Act  Assert — студент не может сдать до посещения урока
        Action act = () => student.SubmitHomework(homework, earlySubmit);
        act.Should().Throw<LessonNotVisitedException>();
    }

    [Fact]
    public void SubmitWithoutGrading_ShouldBePossible()
    {
        // Arrange — нормальный сценарий: сдача без выставления оценки
        var now = DateTime.UtcNow;
        var group = new Group(new GroupName("No-4-2"));
        var teacher = new Teacher(new PersonName("Prof Soft"));
        var student = new Student(new PersonName("Silva"), group);
        var topic = new LessonTopic("Silence");
        var lesson = new Lesson(group, teacher, now, topic);
        var homework = new Homework(lesson, new HomeworkTitle("Quiet HW"));
        lesson.AddHomework(homework);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        student.SubmitHomework(homework, now);

        // Assert — сдача произошла вовремя, просрочки нет
        homework.IsLate(student).Should().BeFalse();
    }

    [Fact]
    public void ResubmittingSameHomework_ShouldThrow()
    {
        // Arrange — вторая попытка сдачи одной и той же домашки
        var now = DateTime.UtcNow;
        var group = new Group(new GroupName("NoR-4-5"));
        var teacher = new Teacher(new PersonName("Prof Once"));
        var student = new Student(new PersonName("Remy"), group);
        var topic = new LessonTopic("Cards");
        var lesson = new Lesson(group, teacher, now, topic);
        var homework = new Homework(lesson, new HomeworkTitle("Throw HW"));
        lesson.AddHomework(homework);

        teacher.TeachLesson(lesson);
        student.AttendLesson(lesson);
        student.SubmitHomework(homework, now);

        // Act  Assert — повторная сдача вызывает исключение
        Action act = () => student.SubmitHomework(homework, now.AddMinutes(5));
        act.Should().Throw<HomeworkAlreadySubmittedException>();
    }
}
