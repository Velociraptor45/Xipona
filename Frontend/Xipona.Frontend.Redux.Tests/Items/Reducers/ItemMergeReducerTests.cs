using FluentAssertions;
using System.Text.RegularExpressions;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.Reducers;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Items.States.Merges;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Common.Extensions;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.Items.Reducers;
public class ItemMergeReducerTests
{
    public class OnInitializeMergingFinished
    {
        private readonly OnInitializeMergingFinishedFixture _fixture = new();

        [Fact]
        public void OnInitializeMergingFinished_WithItemsHavingDifferentNameStart_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedStateWithDifferentNameStart();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnInitializeMergingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => Regex.IsMatch(info.Path, @"^Merge\.Item\.Types\[\d\]\.Key$")));
        }

        [Fact]
        public void OnInitializeMergingFinished_WithItemsSharingNameStart_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedStateWithSharedNameStart();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnInitializeMergingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => Regex.IsMatch(info.Path, @"^Merge\.Item\.Types\[\d\]\.Key$")));
        }

        private class OnInitializeMergingFinishedFixture : ItemMergeReducerFixture
        {
            public InitializeMergingFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedStateWithDifferentNameStart()
            {
                var names = new DomainTestBuilder<string>().CreateMany(3).ToList();
                EditedItem[] items =
                [
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"A {names[0]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"B {names[1]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"C {names[2]}" }
                ];

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = string.Empty,
                            Types =
                            [
                                new MergedItemType(Guid.NewGuid(), items[0], items[0].Name),
                                new MergedItemType(Guid.NewGuid(), items[1], items[1].Name),
                                new MergedItemType(Guid.NewGuid(), items[2], items[2].Name)
                            ]
                        },
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedStateWithSharedNameStart()
            {
                var start = new DomainTestBuilder<string>().Create();
                var trimmedNames = new DomainTestBuilder<string>().CreateMany(3).ToList();
                EditedItem[] items =
                [
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"{start} {trimmedNames[0]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"{start} {trimmedNames[1]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"{start} {trimmedNames[2]}" }
                ];

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = start,
                            Types =
                            [
                                new MergedItemType(Guid.NewGuid(), items[0], trimmedNames[0]),
                                new MergedItemType(Guid.NewGuid(), items[1], trimmedNames[1]),
                                new MergedItemType(Guid.NewGuid(), items[2], trimmedNames[2])
                            ]
                        },
                        IsSaving = false
                    }
                };
            }

            public void SetupAction()
            {
                Action = new InitializeMergingFinishedAction(
                    ExpectedState.Merge.Item!.Types.Select(t => t.OriginalItem).ToList());
            }

        }
    }

    public class OnItemTypeNameChanged
    {
        private readonly OnItemTypeNameChangedFixture _fixture = new();

        [Fact]
        public void OnItemTypeNameChanged_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedResult();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeNameChanged_WithInvalidKey_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionWithInvalidKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnItemTypeNameChangedFixture : ItemMergeReducerFixture
        {
            public ItemTypeNameChangedAction? Action { get; private set; }
            private MergedItemType? _type;

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedResult()
            {
                var types = ExpectedState.Merge.Item!.Types.ToList();
                var (_, typeIndex) = types.Random();
                types[typeIndex] = types[typeIndex] with
                {
                    Name = new DomainTestBuilder<string>().Create()
                };
                _type = types[typeIndex];

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Types = types
                        }
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_type);
                Action = new ItemTypeNameChangedAction(_type.Key, _type.Name);
            }

            public void SetupActionWithInvalidKey()
            {
                Action = new ItemTypeNameChangedAction(Guid.NewGuid(), "New Name");
            }
        }
    }

    public class OnItemNameChanged
    {
        private readonly OnItemNameChangedFixture _fixture = new();

        [Fact]
        public void OnItemNameChanged_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedResult();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnItemNameChangedFixture : ItemMergeReducerFixture
        {
            public ItemNameChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedResult()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new ItemNameChangedAction(ExpectedState.Merge.Item!.Name);
            }
        }
    }

    private abstract class ItemMergeReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}
