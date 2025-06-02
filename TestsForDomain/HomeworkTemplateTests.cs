using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;

public class HomeworkTemplateTests
{
    [Fact]
    public void Constructor_ShouldCreateTemplate_WhenValidValues()
    {
        var topic = new LessonTopic("Math Topic");
        var title = new HomeworkTitle("Упражнение");

        var template = new HomeworkTemplate(topic, title);

        template.Topic.Should().Be(topic);
        template.Title.Should().Be(title);
    }

    [Fact]
    public void UpdateTopic_ShouldReplaceTopic()
    {
        var template = new HomeworkTemplate(new LessonTopic("Old"), new HomeworkTitle("Задача"));
        var newTopic = new LessonTopic("New");

        template.UpdateTopic(newTopic);

        template.Topic.Should().Be(newTopic);
    }

    [Fact]
    public void UpdateTitle_ShouldReplaceTitle()
    {
        var template = new HomeworkTemplate(new LessonTopic("T"), new HomeworkTitle("Old"));
        var newTitle = new HomeworkTitle("New");

        template.UpdateTitle(newTitle);

        template.Title.Should().Be(newTitle);
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTopicIsNull()
    {
        Action act = () => new HomeworkTemplate(null!, new HomeworkTitle("Valid"));

        act.Should().Throw<ArgumentNullException>().WithParameterName("Topic");
    }

    [Fact]
    public void Constructor_ShouldThrow_WhenTitleIsNull()
    {
        Action act = () => new HomeworkTemplate(new LessonTopic("Valid"), null!);

        act.Should().Throw<ArgumentNullException>().WithParameterName("title");
    }
}
