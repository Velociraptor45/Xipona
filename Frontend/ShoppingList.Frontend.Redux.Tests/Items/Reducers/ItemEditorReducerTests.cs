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
                                new(
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
                                new(
                                    _stores.Last().Id,
                                    _stores.Last().DefaultSectionId,
                                    4.67f),
                                new(
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
                                new(
                                    _stores.Last().Id,
                                    _stores.Last().DefaultSectionId,
                                    4.67f),
                                new(
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
        public void OnStoreAddedToItemType_WithInvalidTypeKey_ShouldNotAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupItemTypes();
            _fixture.SetupActionWithInvalidTypeKey();
            _fixture.SetupOneStoreAvailableForInvalidTypeId();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreAddedToItemType(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreAddedToItemType_WithOneStoreAvailable_WithTwoTypesWithSameId_ShouldAddStore()
        {
            // Arrange
            _fixture.SetupStores();
            _fixture.SetupItemTypesWithInitialId();
            _fixture.SetupAction();
            _fixture.SetupOneStoreAvailable();

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
            private Guid? _typeKey;

            public StoreAddedToItemTypeAction? Action { get; private set; }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_typeKey);
                Action = new StoreAddedToItemTypeAction(_typeKey.Value);
            }

            public void SetupActionWithInvalidTypeKey()
            {
                Action = new StoreAddedToItemTypeAction(Guid.NewGuid());
            }

            public void SetupItemTypes()
            {
                SetupItemTypes(false);
            }

            public void SetupItemTypesWithInitialId()
            {
                SetupItemTypes(true);
            }

            private void SetupItemTypes(bool withInitialId)
            {
                var types = new List<EditedItemType>()
                {
                    new DomainTestBuilder<EditedItemType>().Create() with
                    {
                        Id = withInitialId ? Guid.Empty : Guid.NewGuid(),
                        Availabilities = new List<EditedItemAvailability>()
                    },
                    new DomainTestBuilder<EditedItemType>().Create() with
                    {
                        Id = withInitialId ? Guid.Empty : Guid.NewGuid(),
                        Availabilities = new List<EditedItemAvailability>()
                    }
                };
                _typeKey = types.Last().Key;
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
                                        new(
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
                                        new(
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
                                        new(
                                            _stores.Last().Id,
                                            _stores.Last().DefaultSectionId,
                                            4.67f),
                                        new(
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
                                        new(
                                            _stores.Last().Id,
                                            _stores.Last().DefaultSectionId,
                                            4.67f),
                                        new(
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

    public class OnDefaultSectionOfItemChanged
    {
        private readonly OnDefaultSectionOfItemChangedFixture _fixture;

        public OnDefaultSectionOfItemChanged()
        {
            _fixture = new OnDefaultSectionOfItemChangedFixture();
        }

        [Fact]
        public void OnDefaultSectionOfItemChanged_WithValidData_ShouldChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnDefaultSectionOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDefaultSectionOfItemChanged_WithInvalidAvailability_ShouldNotChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnDefaultSectionOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDefaultSectionOfItemChangedFixture : ItemEditorReducerFixture
        {
            public DefaultSectionOfItemChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
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

            public void SetupAction()
            {
                Action = new DefaultSectionOfItemChangedAction(InitialState.Editor.Item!.Availabilities.First(),
                    ExpectedState.Editor.Item!.Availabilities.First().DefaultSectionId);
            }

            public void SetupActionWithInvalidAvailability()
            {
                Action = new DefaultSectionOfItemChangedAction(new DomainTestBuilder<EditedItemAvailability>().Create(),
                    ExpectedState.Editor.Item!.Availabilities.First().DefaultSectionId);
            }
        }
    }

    public class OnDefaultSectionOfItemTypeChanged
    {
        private readonly OnDefaultSectionOfItemTypeChangedFixture _fixture;

        public OnDefaultSectionOfItemTypeChanged()
        {
            _fixture = new OnDefaultSectionOfItemTypeChangedFixture();
        }

        [Fact]
        public void OnDefaultSectionOfItemTypeChanged_WithValidData_ShouldChangeDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnDefaultSectionOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDefaultSectionOfItemTypeChanged_WithInvalidItemType_ShouldNotDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidItemType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnDefaultSectionOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDefaultSectionOfItemTypeChanged_WithInvalidAvailability_ShouldNotDefaultSection()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnDefaultSectionOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDefaultSectionOfItemTypeChangedFixture : ItemEditorReducerFixture
        {
            public DefaultSectionOfItemTypeChangedAction? Action { get; private set; }

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

            public void SetupAction()
            {
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                var availability = itemType.Availabilities.First();
                Action = new DefaultSectionOfItemTypeChangedAction(itemType, availability,
                    ExpectedState.Editor.Item!.ItemTypes.First().Availabilities.First().DefaultSectionId);
            }

            public void SetupActionWithInvalidItemType()
            {
                var itemType = new DomainTestBuilder<EditedItemType>().Create();
                Action = new DefaultSectionOfItemTypeChangedAction(
                    itemType,
                    InitialState.Editor.Item!.ItemTypes.First().Availabilities.First(),
                    ExpectedState.Editor.Item!.ItemTypes.First().Availabilities.First().DefaultSectionId);
            }

            public void SetupActionWithInvalidAvailability()
            {
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                Action = new DefaultSectionOfItemTypeChangedAction(itemType,
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    ExpectedState.Editor.Item!.ItemTypes.First().Availabilities.First().DefaultSectionId);
            }
        }
    }

    public class OnPriceOfItemChanged
    {
        private readonly OnPriceOfItemChangedFixture _fixture;

        public OnPriceOfItemChanged()
        {
            _fixture = new OnPriceOfItemChangedFixture();
        }

        [Fact]
        public void OnPriceOfItemChanged_WithValidData_ShouldChangePrice()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnPriceOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnPriceOfItemChanged_WithInvalidAvailability_ShouldNotChangePrice()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnPriceOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnPriceOfItemChangedFixture : ItemEditorReducerFixture
        {
            public PriceOfItemChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    PricePerQuantity = new DomainTestBuilder<float>().Create()
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

            public void SetupAction()
            {
                Action = new PriceOfItemChangedAction(InitialState.Editor.Item!.Availabilities.First(),
                    ExpectedState.Editor.Item!.Availabilities.First().PricePerQuantity);
            }

            public void SetupActionWithInvalidAvailability()
            {
                Action = new PriceOfItemChangedAction(new DomainTestBuilder<EditedItemAvailability>().Create(),
                    ExpectedState.Editor.Item!.Availabilities.First().PricePerQuantity);
            }
        }
    }

    public class OnPriceOfItemTypeChanged
    {
        private readonly OnPriceOfItemTypeChangedFixture _fixture;

        public OnPriceOfItemTypeChanged()
        {
            _fixture = new OnPriceOfItemTypeChangedFixture();
        }

        [Fact]
        public void OnPriceOfItemTypeChanged_WithValidData_ShouldChangePrice()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnPriceOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnPriceOfItemTypeChanged_WithInvalidItemType_ShouldNotChangePrice()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidItemType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnPriceOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnPriceOfItemTypeChanged_WithInvalidAvailability_ShouldNotChangePrice()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnPriceOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnPriceOfItemTypeChangedFixture : ItemEditorReducerFixture
        {
            public PriceOfItemTypeChangedAction? Action { get; private set; }

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
                    PricePerQuantity = new DomainTestBuilder<float>().Create()
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

            public void SetupAction()
            {
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                var availability = itemType.Availabilities.First();
                Action = new PriceOfItemTypeChangedAction(itemType, availability,
                    ExpectedState.Editor.Item!.ItemTypes.First().Availabilities.First().PricePerQuantity);
            }

            public void SetupActionWithInvalidItemType()
            {
                var itemType = new DomainTestBuilder<EditedItemType>().Create();
                Action = new PriceOfItemTypeChangedAction(
                    itemType,
                    InitialState.Editor.Item!.ItemTypes.First().Availabilities.First(),
                    ExpectedState.Editor.Item!.ItemTypes.First().Availabilities.First().PricePerQuantity);
            }

            public void SetupActionWithInvalidAvailability()
            {
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                Action = new PriceOfItemTypeChangedAction(itemType,
                    new DomainTestBuilder<EditedItemAvailability>().Create(),
                    ExpectedState.Editor.Item!.ItemTypes.First().Availabilities.First().PricePerQuantity);
            }
        }
    }

    public class OnStoreOfItemRemoved
    {
        private readonly OnStoreOfItemRemovedFixture _fixture;

        public OnStoreOfItemRemoved()
        {
            _fixture = new OnStoreOfItemRemovedFixture();
        }

        [Fact]
        public void OnStoreOfItemRemoved_WithValidData_ShouldRemoveStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemRemoved_WithInvalidAvailability_ShouldNotRemoveStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreOfItemRemovedFixture : ItemEditorReducerFixture
        {
            public StoreOfItemRemovedAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedState()
            {
                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities.RemoveAt(0);

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
                Action = new StoreOfItemRemovedAction(InitialState.Editor.Item!.Availabilities.First());
            }

            public void SetupActionWithInvalidAvailability()
            {
                Action = new StoreOfItemRemovedAction(new DomainTestBuilder<EditedItemAvailability>().Create());
            }
        }
    }

    public class OnStoreOfItemTypeRemoved
    {
        private readonly OnStoreOfItemTypeRemovedFixture _fixture;

        public OnStoreOfItemTypeRemoved()
        {
            _fixture = new OnStoreOfItemTypeRemovedFixture();
        }

        [Fact]
        public void OnStoreOfItemTypeRemoved_WithValidData_ShouldRemoveStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeRemoved_WithInvalidItemType_ShouldNotRemoveStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidItemType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeRemoved_WithInvalidAvailability_ShouldNotRemoveStore()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidAvailability();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStoreOfItemTypeRemovedFixture : ItemEditorReducerFixture
        {
            public StoreOfItemTypeRemovedAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedState()
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities.RemoveAt(0);

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
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                var availability = itemType.Availabilities.First();
                Action = new StoreOfItemTypeRemovedAction(itemType, availability);
            }

            public void SetupActionWithInvalidItemType()
            {
                var itemType = new DomainTestBuilder<EditedItemType>().Create();
                Action = new StoreOfItemTypeRemovedAction(
                    itemType,
                    InitialState.Editor.Item!.ItemTypes.First().Availabilities.First());
            }

            public void SetupActionWithInvalidAvailability()
            {
                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                Action = new StoreOfItemTypeRemovedAction(itemType,
                    new DomainTestBuilder<EditedItemAvailability>().Create());
            }
        }
    }

    public class OnItemTypeNameChanged
    {
        private readonly OnItemTypeNameChangedFixture _fixture;

        public OnItemTypeNameChanged()
        {
            _fixture = new OnItemTypeNameChangedFixture();
        }

        [Fact]
        public void OnItemTypeNameChanged_WithValidData_ShouldChangeName()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeNameChanged_WithInvalidItemType_ShouldNotChangeName()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidItemType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemTypeNameChangedFixture : ItemEditorReducerFixture
        {
            public ItemTypeNameChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();
                itemTypes[0] = itemTypes.First() with { Name = new DomainTestBuilder<string>().Create() };

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

            public void SetupAction()
            {
                var itemTypeInitial = InitialState.Editor.Item!.ItemTypes.First();
                var itemTypeExpected = ExpectedState.Editor.Item!.ItemTypes.First();
                Action = new ItemTypeNameChangedAction(itemTypeInitial, itemTypeExpected.Name);
            }

            public void SetupActionWithInvalidItemType()
            {
                var itemType = new DomainTestBuilder<EditedItemType>().Create();
                Action = new ItemTypeNameChangedAction(
                    itemType,
                    ExpectedState.Editor.Item!.ItemTypes.First().Name);
            }
        }
    }

    public class OnItemTypeAdded
    {
        private readonly OnItemTypeAddedFixture _fixture;

        public OnItemTypeAdded()
        {
            _fixture = new OnItemTypeAddedFixture();
        }

        [Fact]
        public void OnItemTypeAdded_WithValidData_ShouldAddItemType()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnItemTypeAdded(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => info.Path == "Editor.Item.ItemTypes[0].Key"));
        }

        private sealed class OnItemTypeAddedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedState()
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();
                itemTypes.Insert(0, new(Guid.Empty, Guid.NewGuid(), string.Empty, new List<EditedItemAvailability>()));

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
        }
    }

    public class OnItemTypeRemoved
    {
        private readonly OnItemTypeRemovedFixture _fixture;

        public OnItemTypeRemoved()
        {
            _fixture = new OnItemTypeRemovedFixture();
        }

        [Fact]
        public void OnItemTypeRemoved_WithValidData_ShouldRemoveType()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemTypeRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeRemoved_WithInvalidItemType_ShouldNotRemoveType()
        {
            // Arrange
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithInvalidItemType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemTypeRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemTypeRemovedFixture : ItemEditorReducerFixture
        {
            public ItemTypeRemovedAction? Action { get; private set; }

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialState()
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();
                itemTypes.Insert(0, new DomainTestBuilder<EditedItemType>().Create());

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

            public void SetupAction()
            {
                var itemTypeInitial = InitialState.Editor.Item!.ItemTypes.First();
                Action = new ItemTypeRemovedAction(itemTypeInitial);
            }

            public void SetupActionWithInvalidItemType()
            {
                var itemType = new DomainTestBuilder<EditedItemType>().Create();
                Action = new ItemTypeRemovedAction(itemType);
            }
        }
    }

    public class OnEnterItemSearchPage
    {
        private readonly OnEnterItemSearchPageFixture _fixture;

        public OnEnterItemSearchPage()
        {
            _fixture = new OnEnterItemSearchPageFixture();
        }

        [Fact]
        public void OnEnterItemSearchPage_WithValidData_ShouldRemoveItem()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnEnterItemSearchPage(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnEnterItemSearchPage_WithNoItemInStore_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateWithoutItem();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnEnterItemSearchPage(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnEnterItemSearchPageFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateWithoutItem()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = null
                    }
                };
            }
        }
    }

    public class OnCreateItemStarted
    {
        private readonly OnCreateItemStartedFixture _fixture;

        public OnCreateItemStarted()
        {
            _fixture = new OnCreateItemStartedFixture();
        }

        [Fact]
        public void OnCreateItemStarted_WithValidData_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnCreateItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItemStarted_WithAlreadySaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithAlreadySaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnCreateItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCreateItemStartedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }

            public void SetupInitialStateWithAlreadySaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }
        }
    }

    public class OnCreateItemFinished
    {
        private readonly OnCreateItemFinishedFixture _fixture;

        public OnCreateItemFinished()
        {
            _fixture = new OnCreateItemFinishedFixture();
        }

        [Fact]
        public void OnCreateItemFinished_WithValidData_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnCreateItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItemFinished_WithNotSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnCreateItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCreateItemFinishedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }

            public void SetupInitialStateWithNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }
        }
    }

    public class OnUpdateItemStarted
    {
        private readonly OnUpdateItemStartedFixture _fixture;

        public OnUpdateItemStarted()
        {
            _fixture = new OnUpdateItemStartedFixture();
        }

        [Fact]
        public void OnUpdateItemStarted_WithNotSaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnUpdateItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItemStarted_WithSaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithAlreadySaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnUpdateItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnUpdateItemStartedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsUpdating = false
                    }
                };
            }

            public void SetupInitialStateWithAlreadySaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsUpdating = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsUpdating = true
                    }
                };
            }
        }
    }

    public class OnUpdateItemFinished
    {
        private readonly OnUpdateItemFinishedFixture _fixture;

        public OnUpdateItemFinished()
        {
            _fixture = new OnUpdateItemFinishedFixture();
        }

        [Fact]
        public void OnUpdateItemFinished_WithSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnUpdateItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItemFinished_WithNotSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnUpdateItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnUpdateItemFinishedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsUpdating = true
                    }
                };
            }

            public void SetupInitialStateWithNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsUpdating = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsUpdating = false
                    }
                };
            }
        }
    }

    public class OnModifyItemStarted
    {
        private readonly OnModifyItemStartedFixture _fixture;

        public OnModifyItemStarted()
        {
            _fixture = new OnModifyItemStartedFixture();
        }

        [Fact]
        public void OnModifyItemStarted_WithNotSaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnModifyItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItemStarted_WithSaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithAlreadySaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnModifyItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnModifyItemStartedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }

            public void SetupInitialStateWithAlreadySaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }
        }
    }

    public class OnModifyItemFinished
    {
        private readonly OnModifyItemFinishedFixture _fixture;

        public OnModifyItemFinished()
        {
            _fixture = new OnModifyItemFinishedFixture();
        }

        [Fact]
        public void OnModifyItemFinished_WithSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnModifyItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItemFinished_WithNotSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnModifyItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnModifyItemFinishedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }

            public void SetupInitialStateWithNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }
        }
    }

    public class OnMakeItemPermanentStarted
    {
        private readonly OnMakeItemPermanentStartedFixture _fixture;

        public OnMakeItemPermanentStarted()
        {
            _fixture = new OnMakeItemPermanentStartedFixture();
        }

        [Fact]
        public void OnMakeItemPermanentStarted_WithNotSaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanentStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanentStarted_WithSaving_ShouldEnableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithAlreadySaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanentStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnMakeItemPermanentStartedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }

            public void SetupInitialStateWithAlreadySaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }
        }
    }

    public class OnMakeItemPermanentFinished
    {
        private readonly OnMakeItemPermanentFinishedFixture _fixture;

        public OnMakeItemPermanentFinished()
        {
            _fixture = new OnMakeItemPermanentFinishedFixture();
        }

        [Fact]
        public void OnMakeItemPermanentFinished_WithSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanentFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanentFinished_WithNotSaving_ShouldDisableSaving()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanentFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnMakeItemPermanentFinishedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = true
                    }
                };
            }

            public void SetupInitialStateWithNotSaving()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsModifying = false
                    }
                };
            }
        }
    }

    public class OnDeleteItemStarted
    {
        private readonly OnDeleteItemStartedFixture _fixture;

        public OnDeleteItemStarted()
        {
            _fixture = new OnDeleteItemStartedFixture();
        }

        [Fact]
        public void OnDeleteItemStarted_WithNotDeleting_ShouldEnableDeleting()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnDeleteItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteItemStarted_WithDeleting_ShouldEnableDeleting()
        {
            // Arrange
            _fixture.SetupInitialStateWithAlreadyDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnDeleteItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteItemStartedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = false
                    }
                };
            }

            public void SetupInitialStateWithAlreadyDeleting()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = true
                    }
                };
            }
        }
    }

    public class OnDeleteItemFinished
    {
        private readonly OnDeleteItemFinishedFixture _fixture;

        public OnDeleteItemFinished()
        {
            _fixture = new OnDeleteItemFinishedFixture();
        }

        [Fact]
        public void OnDeleteItemFinished_WithDeleting_ShouldDisableDeleting()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnDeleteItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnDeleteItemFinished_WithNotDeleting_ShouldDisableDeleting()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotDeleting();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnDeleteItemFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteItemFinishedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = true
                    }
                };
            }

            public void SetupInitialStateWithNotDeleting()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleting = false
                    }
                };
            }
        }
    }

    public class OnOpenDeleteItemDialog
    {
        private readonly OnOpenDeleteItemDialogFixture _fixture;

        public OnOpenDeleteItemDialog()
        {
            _fixture = new OnOpenDeleteItemDialogFixture();
        }

        [Fact]
        public void OnOpenDeleteItemDialog_WithDialogClosed_ShouldOpenDialog()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnOpenDeleteItemDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnOpenDeleteItemDialog_WithDialogOpen_ShouldOpenDialog()
        {
            // Arrange
            _fixture.SetupInitialStateWithOpenDialog();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnOpenDeleteItemDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenDeleteItemDialogFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = false
                    }
                };
            }

            public void SetupInitialStateWithOpenDialog()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = true
                    }
                };
            }
        }
    }

    public class OnCloseDeleteItemDialog
    {
        private readonly OnCloseDeleteItemDialogFixture _fixture;

        public OnCloseDeleteItemDialog()
        {
            _fixture = new OnCloseDeleteItemDialogFixture();
        }

        [Fact]
        public void OnCloseDeleteItemDialog_WithDialogOpen_ShouldCloseDialog()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnCloseDeleteItemDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCloseDeleteItemDialog_WithDialogClosed_ShouldCloseDialog()
        {
            // Arrange
            _fixture.SetupInitialStateWithClosedDialog();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemEditorReducer.OnCloseDeleteItemDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseDeleteItemDialogFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = true
                    }
                };
            }

            public void SetupInitialStateWithClosedDialog()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        IsDeleteDialogOpen = false
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