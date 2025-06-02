using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;

public class HomeworkBankTests
{
    [Fact]
    public void AddTemplate_ShouldAddTemplate_WhenTopicIsUnique()
    {
        var bank = new HomeworkBank();
        var topic = new LessonTopic("Mathda");
        var title = new HomeworkTitle("Сложение и вычитание");

        bank.AddTemplate(topic, title);

        bank.Templates.Should().ContainSingle(t => t.Topic == topic && t.Title == title);
    }

    [Fact]
    public void AddTemplate_ShouldThrow_WhenTopicIsDuplicate()
    {
        var bank = new HomeworkBank();
        var topic = new LessonTopic("Math");
        var title = new HomeworkTitle("Простое уравнение");

        bank.AddTemplate(topic, title);

        Action act = () => bank.AddTemplate(topic, new HomeworkTitle("Повторная тема"));

        act.Should().Throw<DuplicateHomeworkTemplateException>();
    }

    [Fact]
    public void GetTemplateByTopic_ShouldReturnTemplate_WhenExists()
    {
        var bank = new HomeworkBank();
        var topic = new LessonTopic("Physics");
        var title = new HomeworkTitle("Законы Ньютона");

        bank.AddTemplate(topic, title);

        var found = bank.FindTemplate(topic);

        found.Should().NotBeNull();
        found!.Title.Should().Be(title);
    }

    [Fact]
    public void GetTemplateByTopic_ShouldReturnNull_WhenNotExists()
    {
        var bank = new HomeworkBank();
        var topic = new LessonTopic("Biology");

        var result = bank.FindTemplate(topic);

        result.Should().BeNull();
    }
}
