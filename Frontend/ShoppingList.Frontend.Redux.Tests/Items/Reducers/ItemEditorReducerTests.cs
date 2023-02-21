using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Reducers;

public class ItemEditorReducerTests
{
    public class OnItemNameChanged
    {
        private readonly OnItemNameChangedFixture _fixture;

        public OnItemNameChanged()
        {
            _fixture = new OnItemNameChangedFixture();
        }

        [Fact]
        public void OnItemNameChanged_WithValidName_ShouldSetName()
        {
            // Arrange
            _fixture.SetupActionWithName();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemNameChanged_WithNameNull_ShouldSetName()
        {
            // Arrange
            _fixture.SetupActionWithoutName();
            _fixture.SetupInitialState();
            _fixture.SetupExpectedResultWithEmptyName();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemNameChangedFixture : ItemEditorReducerFixture
        {
            public ItemNameChangedAction? Action { get; private set; }

            public void SetupActionWithName()
            {
                Action = new DomainTestBuilder<ItemNameChangedAction>().Create() with
                {
                    Name = ExpectedState.Editor.Item!.Name
                };
            }

            public void SetupActionWithoutName()
            {
                Action = new DomainTestBuilder<ItemNameChangedAction>().Create() with
                {
                    Name = null
                };
            }

            public void SetupExpectedResultWithEmptyName()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            Name = string.Empty
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }
        }
    }

    public class OnQuantityTypeInPacketChanged
    {
        private readonly OnQuantityTypeInPacketChangedFixture _fixture;

        public OnQuantityTypeInPacketChanged()
        {
            _fixture = new OnQuantityTypeInPacketChangedFixture();
        }

        [Fact]
        public void OnQuantityTypeInPacketChanged_ShouldSetQuantityTypeInPacket()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnQuantityTypeInPacketChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnQuantityTypeInPacketChangedFixture : ItemEditorReducerFixture
        {
            public QuantityTypeInPacketChangedAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<QuantityTypeInPacketChangedAction>().Create() with
                {
                    QuantityTypeInPacket = ExpectedState.Editor.Item!.QuantityInPacketType!
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            QuantityInPacketType = new DomainTestBuilder<QuantityTypeInPacket>().Create()
                        }
                    }
                };
            }
        }
    }

    public class OnQuantityTypeChanged
    {
        private readonly OnQuantityTypeChangedFixture _fixture;

        public OnQuantityTypeChanged()
        {
            _fixture = new OnQuantityTypeChangedFixture();
        }

        [Fact]
        public void OnQuantityTypeChanged_WithUnitType_ShouldSetQuantityType()
        {
            // Arrange
            _fixture.SetupExpectedResultWithUnitType();
            _fixture.SetupAction();
            _fixture.SetupInitialStateForUnitType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnQuantityTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnQuantityTypeChanged_WithWeightType_ShouldSetQuantityType()
        {
            // Arrange
            _fixture.SetupExpectedResultWithWeightType();
            _fixture.SetupAction();
            _fixture.SetupInitialStateForWeightType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnQuantityTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnQuantityTypeChangedFixture : ItemEditorReducerFixture
        {
            public QuantityTypeChangedAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<QuantityTypeChangedAction>().Create() with
                {
                    QuantityType = ExpectedState.Editor.Item!.QuantityType
                };
            }

            public void SetupExpectedResultWithWeightType()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                            {
                                Id = 1
                            },
                            QuantityInPacket = null,
                            QuantityInPacketType = null
                        }
                    }
                };
            }

            public void SetupExpectedResultWithUnitType()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                            {
                                Id = 0
                            },
                            QuantityInPacket = 1,
                            QuantityInPacketType = ExpectedState.QuantityTypesInPacket.First()
                        }
                    }
                };
            }

            public void SetupInitialStateForWeightType()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            QuantityType = new DomainTestBuilder<QuantityType>().Create()
                        }
                    }
                };
            }

            public void SetupInitialStateForUnitType()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            QuantityType = new DomainTestBuilder<QuantityType>().Create() with
                            {
                                Id = 0
                            },
                            QuantityInPacketType = null
                        }
                    }
                };
            }
        }
    }

    public class OnItemQuantityInPacketChanged
    {
        private readonly OnItemQuantityInPacketChangedFixture _fixture;

        public OnItemQuantityInPacketChanged()
        {
            _fixture = new OnItemQuantityInPacketChangedFixture();
        }

        [Fact]
        public void OnItemQuantityInPacketChanged_ShouldSetQuantityInPacket()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemQuantityInPacketChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemQuantityInPacketChangedFixture : ItemEditorReducerFixture
        {
            public ItemQuantityInPacketChangedAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<ItemQuantityInPacketChangedAction>().Create() with
                {
                    QuantityInPacket = ExpectedState.Editor.Item!.QuantityInPacket!.Value
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            QuantityInPacket = new DomainTestBuilder<float>().Create()
                        }
                    }
                };
            }
        }
    }

    public class OnItemCommentChanged
    {
        private readonly OnItemCommentChangedFixture _fixture;

        public OnItemCommentChanged()
        {
            _fixture = new OnItemCommentChangedFixture();
        }

        [Fact]
        public void OnItemCommentChanged_ShouldSetComment()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemCommentChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemCommentChangedFixture : ItemEditorReducerFixture
        {
            public ItemCommentChangedAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<ItemCommentChangedAction>().Create() with
                {
                    Comment = ExpectedState.Editor.Item!.Comment
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            Comment = new DomainTestBuilder<string>().Create()
                        }
                    }
                };
            }
        }
    }

    public class OnSetNewItem
    {
        private readonly OnSetNewItemFixture _fixture;

        public OnSetNewItem()
        {
            _fixture = new OnSetNewItemFixture();
        }

        [Fact]
        public void OnSetNewItem_ShouldSetItemAndSelectors()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnSetNewItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSetNewItemFixture : ItemEditorReducerFixture
        {
            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = new EditedItem(
                            Guid.Empty,
                            string.Empty,
                            false,
                            string.Empty,
                            false,
                            ExpectedState.QuantityTypes.First(),
                            1,
                            ExpectedState.QuantityTypesInPacket.First(),
                            null,
                            null,
                            new List<EditedItemAvailability>(),
                            new List<EditedItemType>(),
                            ItemMode.NotDefined),
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new List<ItemCategorySearchResult>(0)
                        },
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new List<ManufacturerSearchResult>(0)
                        }
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = new DomainTestBuilder<EditedItem>().Create(),
                        ItemCategorySelector = ExpectedState.Editor.ItemCategorySelector with
                        {
                            ItemCategories = new DomainTestBuilder<ItemCategorySearchResult>().CreateMany(2).ToList()
                        },
                        ManufacturerSelector = ExpectedState.Editor.ManufacturerSelector with
                        {
                            Manufacturers = new DomainTestBuilder<ManufacturerSearchResult>().CreateMany(3).ToList()
                        }
                    }
                };
            }
        }
    }

    private abstract class ItemEditorReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}