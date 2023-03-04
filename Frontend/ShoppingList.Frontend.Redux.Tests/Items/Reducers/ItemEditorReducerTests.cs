using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.ItemCategories.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Manufacturers.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Items.States;
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

    public class OnLoadItemForEditingStarted
    {
        private readonly OnLoadItemForEditingStartedFixture _fixture;

        public OnLoadItemForEditingStarted()
        {
            _fixture = new OnLoadItemForEditingStartedFixture();
        }

        [Fact]
        public void OnLoadItemForEditingStarted_ShouldSetIsLoadingEditedItem()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnLoadItemForEditingStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadItemForEditingStartedFixture : ItemEditorReducerFixture
        {
            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedItem = true
                    }
                };
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedItem = false
                    }
                };
            }
        }
    }

    public class OnLoadItemForEditingFinished
    {
        private readonly OnLoadItemForEditingFinishedFixture _fixture;

        public OnLoadItemForEditingFinished()
        {
            _fixture = new OnLoadItemForEditingFinishedFixture();
        }

        [Fact]
        public void OnLoadItemForEditingFinished_ShouldItemAndIsLoadingEditedItem()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnLoadItemForEditingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadItemForEditingFinishedFixture : ItemEditorReducerFixture
        {
            public LoadItemForEditingFinishedAction? Action { get; private set; }

            public void SetupAction()
            {
                Action = new DomainTestBuilder<LoadItemForEditingFinishedAction>().Create() with
                {
                    Item = ExpectedState.Editor.Item!
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsLoadingEditedItem = false
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
                        IsLoadingEditedItem = true
                    }
                };
            }
        }
    }

    public class OnStoreAddedToItem
    {
        private readonly OnStoreAddedToItemFixture _fixture;

        public OnStoreAddedToItem()
        {
            _fixture = new OnStoreAddedToItemFixture();
        }

        [Fact]
        public void OnStoreAddedToItem_WithOneStoreAvailable_ShouldAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupOneStoreAvailable();

            // Act
            var result = ItemEditorReducer.OnStoreAddedToItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreAddedToItem_WithNoStoreAvailable_ShouldNotAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupNoStoreAvailable();

            // Act
            var result = ItemEditorReducer.OnStoreAddedToItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreAddedToItemFixture : ItemEditorReducerFixture
        {
            private List<ItemStore>? _stores;

            public void SetupStores()
            {
                _stores = new List<ItemStore>
                {
                    ItemStoreMother.GetValid(),
                    ItemStoreMother.GetValid()
                };
                ExpectedState = ExpectedState with
                {
                    Stores = new ActiveStores(_stores)
                };
                InitialState = InitialState with
                {
                    Stores = new ActiveStores(_stores)
                };
            }

            public void SetupOneStoreAvailable()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                InitialState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            Availabilities = new List<EditedItemAvailability>
                            {
                                new EditedItemAvailability(
                                    _stores.Last().Id,
                                    _stores.Last().DefaultSectionId,
                                    4.67f)
                            }
                        }
                    }
                };

                ExpectedState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            Availabilities = new List<EditedItemAvailability>
                            {
                                new EditedItemAvailability(
                                    _stores.Last().Id,
                                    _stores.Last().DefaultSectionId,
                                    4.67f),
                                new EditedItemAvailability(
                                    _stores.First().Id,
                                    _stores.First().DefaultSectionId,
                                    1f),
                            }
                        }
                    }
                };
            }

            public void SetupNoStoreAvailable()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                InitialState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            Availabilities = new List<EditedItemAvailability>
                            {
                                new EditedItemAvailability(
                                    _stores.Last().Id,
                                    _stores.Last().DefaultSectionId,
                                    4.67f),
                                new EditedItemAvailability(
                                    _stores.First().Id,
                                    _stores.First().DefaultSectionId,
                                    13.98f),
                            }
                        }
                    }
                };

                ExpectedState = InitialState;
            }
        }
    }

    public class OnStoreAddedToItemType
    {
        private readonly OnStoreAddedToItemTypeFixture _fixture;

        public OnStoreAddedToItemType()
        {
            _fixture = new OnStoreAddedToItemTypeFixture();
        }

        [Fact]
        public void OnStoreAddedToItemType_WithInvalidTypeId_ShouldNotAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupItemTypes();
            _fixture.SetupActionWithInvalidTypeId();
            _fixture.SetupOneStoreAvailableForInvalidTypeId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreAddedToItemType(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreAddedToItemType_WithOneStoreAvailable_ShouldAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupItemTypes();
            _fixture.SetupAction();
            _fixture.SetupOneStoreAvailable();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreAddedToItemType(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreAddedToItemType_WithNoStoreAvailable_ShouldNotAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupItemTypes();
            _fixture.SetupAction();
            _fixture.SetupNoStoreAvailable();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreAddedToItemType(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreAddedToItemTypeFixture : ItemEditorReducerFixture
        {
            private List<ItemStore>? _stores;
            private Guid? _typeId;

            public StoreAddedToItemTypeAction? Action { get; private set; }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_typeId);
                Action = new StoreAddedToItemTypeAction(_typeId.Value);
            }

            public void SetupActionWithInvalidTypeId()
            {
                Action = new StoreAddedToItemTypeAction(Guid.NewGuid());
            }

            public void SetupItemTypes()
            {
                var types = new List<EditedItemType>()
                {
                    new DomainTestBuilder<EditedItemType>().Create() with
                    {
                        Availabilities = new List<EditedItemAvailability>()
                    },
                    new DomainTestBuilder<EditedItemType>().Create() with
                    {
                        Availabilities = new List<EditedItemAvailability>()
                    }
                };
                _typeId = types.Last().Id;
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemTypes = types
                        }
                    }
                };
                InitialState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            ItemTypes = types
                        }
                    }
                };
            }

            public void SetupStores()
            {
                _stores = new List<ItemStore>
                {
                    ItemStoreMother.GetValid(),
                    ItemStoreMother.GetValid()
                };
                ExpectedState = ExpectedState with
                {
                    Stores = new ActiveStores(_stores)
                };
                InitialState = InitialState with
                {
                    Stores = new ActiveStores(_stores)
                };
            }

            public void SetupOneStoreAvailableForInvalidTypeId()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                InitialState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            ItemTypes = new List<EditedItemType>
                            {
                                InitialState.Editor.Item.ItemTypes.First(),
                                InitialState.Editor.Item.ItemTypes.Last() with
                                {
                                    Availabilities = new List<EditedItemAvailability>
                                    {
                                        new EditedItemAvailability(
                                            _stores.Last().Id,
                                            _stores.Last().DefaultSectionId,
                                            4.67f)
                                    }
                                }
                            }
                        }
                    }
                };

                ExpectedState = InitialState;
            }

            public void SetupOneStoreAvailable()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                InitialState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            ItemTypes = new List<EditedItemType>
                            {
                                InitialState.Editor.Item.ItemTypes.First(),
                                InitialState.Editor.Item.ItemTypes.Last() with
                                {
                                    Availabilities = new List<EditedItemAvailability>
                                    {
                                        new EditedItemAvailability(
                                            _stores.Last().Id,
                                            _stores.Last().DefaultSectionId,
                                            4.67f)
                                    }
                                }
                            }
                        }
                    }
                };

                ExpectedState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            ItemTypes = new List<EditedItemType>
                            {
                                InitialState.Editor.Item.ItemTypes.First(),
                                InitialState.Editor.Item.ItemTypes.Last() with
                                {
                                    Availabilities = new List<EditedItemAvailability>
                                    {
                                        new EditedItemAvailability(
                                            _stores.Last().Id,
                                            _stores.Last().DefaultSectionId,
                                            4.67f),
                                        new EditedItemAvailability(
                                            _stores.First().Id,
                                            _stores.First().DefaultSectionId,
                                            1f),
                                    }
                                }
                            }
                        }
                    }
                };
            }

            public void SetupNoStoreAvailable()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item! with
                        {
                            ItemTypes = new List<EditedItemType>
                            {
                                ExpectedState.Editor.Item.ItemTypes.First(),
                                ExpectedState.Editor.Item.ItemTypes.Last() with
                                {
                                    Availabilities = new List<EditedItemAvailability>
                                    {
                                        new EditedItemAvailability(
                                            _stores.Last().Id,
                                            _stores.Last().DefaultSectionId,
                                            4.67f),
                                        new EditedItemAvailability(
                                            _stores.First().Id,
                                            _stores.First().DefaultSectionId,
                                            13.98f),
                                    }
                                }
                            }
                        }
                    }
                };

                ExpectedState = InitialState;
            }
        }
    }

    public class OnStoreOfItemChanged
    {
        private readonly OnStoreOfItemChangedFixture _fixture;

        public OnStoreOfItemChanged()
        {
            _fixture = new OnStoreOfItemChangedFixture();
        }

        [Fact]
        public void OnStoreOfItemChanged_WithValidData_ShouldChangeStore()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemChanged_WithInvalidAvailability_ShouldNotChangeStore()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemChanged_WithInvalidStoreId_ShouldNotChangeStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidStoreId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreOfItemChangedFixture : ItemEditorReducerFixture
        {
            private ItemStore? _store;
            public StoreOfItemChangedAction? Action { get; private set; }

            public void SetupStore()
            {
                ExpectedState = ExpectedState with
                {
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new List<ItemStore>
                        {
                            ItemStoreMother.GetValid(),
                            ItemStoreMother.GetValid(),
                            ItemStoreMother.GetValid()
                        }
                    }
                };

                _store = ExpectedState.Stores.Stores.First();
            }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = Guid.NewGuid(),
                    DefaultSectionId = Guid.NewGuid()
                };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            Availabilities = availabilities
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = _store.Id,
                    DefaultSectionId = _store.DefaultSectionId
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            Availabilities = availabilities
                        }
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                Action = new StoreOfItemChangedAction(InitialState.Editor.Item!.Availabilities.First(), _store.Id);
            }

            public void SetupActionWithInvalidStoreId()
            {
                Action = new StoreOfItemChangedAction(InitialState.Editor.Item!.Availabilities.First(), Guid.NewGuid());
            }

            public void SetupActionWithInvalidAvailability()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                Action = new StoreOfItemChangedAction(new DomainTestBuilder<EditedItemAvailability>().Create(), _store.Id);
            }
        }
    }

    public class OnStoreOfItemTypeChanged
    {
        private readonly OnStoreOfItemTypeChangedFixture _fixture;

        public OnStoreOfItemTypeChanged()
        {
            _fixture = new OnStoreOfItemTypeChangedFixture();
        }

        [Fact]
        public void OnStoreOfItemTypeChanged_WithValidData_ShouldChangeStore()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeChanged_WithInvalidItemType_ShouldNotChangeStore()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidItemType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeChanged_WithInvalidAvailability_ShouldNotChangeStore()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeChanged_WithInvalidStoreId_ShouldNotChangeStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidStoreId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreOfItemTypeChangedFixture : ItemEditorReducerFixture
        {
            private ItemStore? _store;
            public StoreOfItemTypeChangedAction? Action { get; private set; }

            public void SetupStore()
            {
                ExpectedState = ExpectedState with
                {
                    Stores = new ActiveStores(new List<ItemStore>
                    {
                        ItemStoreMother.GetValid(),
                        ItemStoreMother.GetValid(),
                        ItemStoreMother.GetValid()
                    })
                };

                _store = ExpectedState.Stores.Stores.First();
            }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = Guid.NewGuid(),
                    DefaultSectionId = Guid.NewGuid()
                };

                itemTypes[0] = itemTypes.First() with { Availabilities = availabilities };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = _store.Id,
                    DefaultSectionId = _store.DefaultSectionId
                };

                itemTypes[0] = itemTypes.First() with { Availabilities = availabilities };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        }
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                var availability = itemType.Availabilities.First();
                Action = new StoreOfItemTypeChangedAction(itemType, availability, _store.Id);
            }

            public void SetupActionWithInvalidStoreId()
            {
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                var availability = itemType.Availabilities.First();
                Action = new StoreOfItemTypeChangedAction(itemType, availability, Guid.NewGuid());
            }

            public void SetupActionWithInvalidItemType()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemType = new DomainTestBuilder<EditedItemType>().Create();
                Action = new StoreOfItemTypeChangedAction(
                    itemType,
                    InitialState.Editor.Item!.ItemTypes.First().Availabilities.First(),
                    _store.Id);
            }

            public void SetupActionWithInvalidAvailability()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                Action = new StoreOfItemTypeChangedAction(itemType,
                    new DomainTestBuilder<EditedItemAvailability>().Create(), _store.Id);
            }
        }
    }

    private abstract class ItemEditorReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}