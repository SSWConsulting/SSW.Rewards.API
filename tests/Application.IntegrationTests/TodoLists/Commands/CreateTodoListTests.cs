﻿// Replace with remaining queries and commands

//using SSW.Rewards.Application.Common.Exceptions;
//using SSW.Rewards.Application.TodoLists.Commands.CreateTodoList;
//using SSW.Rewards.Domain.Entities;
//using FluentAssertions;
//using NUnit.Framework;

//namespace SSW.Rewards.Application.IntegrationTests.TodoLists.Commands;

//using static Testing;

//public class CreateTodoListTests : BaseTestFixture
//{
//    [Test]
//    public async Task ShouldRequireMinimumFields()
//    {
//        var command = new CreateTodoListCommand();
//        await FluentActions.Invoking(() => SendAsync(command)).Should().ThrowAsync<ValidationException>();
//    }

//    [Test]
//    public async Task ShouldRequireUniqueTitle()
//    {
//        await SendAsync(new CreateTodoListCommand
//        {
//            Title = "Shopping"
//        });

//        var command = new CreateTodoListCommand
//        {
//            Title = "Shopping"
//        };

//        await FluentActions.Invoking(() =>
//            SendAsync(command)).Should().ThrowAsync<ValidationException>();
//    }

//    [Test]
//    public async Task ShouldCreateTodoList()
//    {
//        var userId = await RunAsDefaultUserAsync();

//        var command = new CreateTodoListCommand
//        {
//            Title = "Tasks"
//        };

//        var id = await SendAsync(command);

//        var list = await FindAsync<TodoList>(id);

//        list.Should().NotBeNull();
//        list!.Title.Should().Be(command.Title);
//        list.CreatedBy.Should().Be(userId);
//        list.Created.Should().BeCloseTo(DateTime.Now, TimeSpan.FromMilliseconds(10000));
//    }
//}