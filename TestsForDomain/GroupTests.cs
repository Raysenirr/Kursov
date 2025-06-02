using System;
using Xunit;
using FluentAssertions;
using Education.Domain.Entities;
using Education.Domain.ValueObjects;
using Education.Domain.Exceptions;
using Education.Domain.ValueObjects.Education.Domain.ValueObjects;


public class GroupTests
{
    [Fact]
    public void Constructor_ShouldSetName()
    {
        var groupName = new GroupName("Group-A-B");
        var group = new Group(groupName);

        group.Name.Should().Be(groupName);
    }

    [Fact]
    public void AddStudent_ShouldSucceed_WhenStudentIsNew()
    {
        var group = new Group(new GroupName("Group-1-1"));
        var student = new Student(new PersonName("Alice"), group);

        group.Students.Should().Contain(student);
    }

}
