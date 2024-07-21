using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Items.States;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Reducers;

public partial class ItemEditorReducerTests
{
    public class OnPriceOfItemTypeChanged
    {
        private readonly OnPriceOfItemTypeChangedFixture _fixture = new();

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
                    PricePerQuantity = new DomainTestBuilder<decimal>().Create()
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

    public class OnStoreAddedToItemType
    {
        private readonly OnStoreAddedToItemTypeFixture _fixture = new();

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
                                            4.67m)
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
                                            4.67m)
                                    }
                                }
                            }
                        },
                        ValidationResult = InitialState.Editor.ValidationResult with
                        {
                            NoTypeStores = new Dictionary<Guid, string>
                            {
                                { InitialState.Editor.Item.ItemTypes.First().Key, "Add at least one store" },
                                { _typeKey!.Value, "Add at least one store" }
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
                                            4.67m),
                                        new(
                                            _stores.First().Id,
                                            _stores.First().DefaultSectionId,
                                            1m),
                                    }
                                }
                            }
                        },
                        ValidationResult = InitialState.Editor.ValidationResult with
                        {
                            NoTypeStores = new Dictionary<Guid, string>
                            {
                                { InitialState.Editor.Item.ItemTypes.First().Key, "Add at least one store" },
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
                                            4.67m),
                                        new(
                                            _stores.First().Id,
                                            _stores.First().DefaultSectionId,
                                            13.98m),
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

    public class OnStoreOfItemTypeRemoved
    {
        private readonly OnStoreOfItemTypeRemovedFixture _fixture = new();

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
        public void OnStoreOfItemTypeRemoved_WithDuplicatedStores_DuplicationRemoved_ShouldRemoveValidationError()
        {
            // Arrange
            _fixture.SetupInitialStateWithRemovedStoreDuplicated();
            _fixture.SetupExpectedStateWithRemovedStoreDuplicated();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeRemoved_WithDuplicatedStores_DuplicationNotRemoved_ShouldNotRemoveValidationError()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotRemovedStoreDuplicated();
            _fixture.SetupExpectedStateWithNotRemovedStoreDuplicated();
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

            public void SetupInitialStateWithNotRemovedStoreDuplicated()
            {
                SetupInitialStateWithDuplication(2);
            }

            public void SetupInitialStateWithRemovedStoreDuplicated()
            {
                SetupInitialStateWithDuplication(0);
            }

            private void SetupInitialStateWithDuplication(int duplicatedStoreIndex)
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities[1] = availabilities[1] with
                {
                    StoreId = availabilities[duplicatedStoreIndex].StoreId
                };

                itemTypes[0] = itemTypes.First() with { Availabilities = availabilities };

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedTypeStores = new Dictionary<Guid, string>
                            {
                                { ExpectedState.Editor.Item!.ItemTypes.First().Key, "Duplicated stores are not allowed" },
                                { ExpectedState.Editor.Item!.ItemTypes.Last().Key, "Duplicated stores are not allowed" }
                            }
                        }
                    }
                };
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

            public void SetupExpectedStateWithNotRemovedStoreDuplicated()
            {
                SetupExpectedStateWithDuplication(false, 2);
            }

            public void SetupExpectedStateWithRemovedStoreDuplicated()
            {
                SetupExpectedStateWithDuplication(true, 0);
            }

            private void SetupExpectedStateWithDuplication(bool duplicationRemoved, int duplicatedStoreIndex)
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities[1] = availabilities[1] with
                {
                    StoreId = availabilities[duplicatedStoreIndex].StoreId
                };
                availabilities.RemoveAt(0);

                itemTypes[0] = itemTypes.First() with { Availabilities = availabilities };

                var duplicatedTypeStoresErrors = new Dictionary<Guid, string>
                {
                    { itemTypes.Last().Key, "Duplicated stores are not allowed" }
                };
                if (!duplicationRemoved)
                    duplicatedTypeStoresErrors.Add(itemTypes.First().Key, "Duplicated stores are not allowed");

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedTypeStores = duplicatedTypeStoresErrors
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
        private readonly OnItemTypeNameChangedFixture _fixture = new();

        [Fact]
        public void OnItemTypeNameChanged_WithValidData_ShouldChangeName()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeNameChanged_WithEmptyName_ShouldChangeNameAndFillValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNameEmpty();
            _fixture.SetupInitialStateWithNameEmpty();
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
                SetupInitialState(true);
            }

            public void SetupInitialStateWithNameEmpty()
            {
                SetupInitialState(false);
            }

            private void SetupInitialState(bool nameCurrentlyInvalid)
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();
                itemTypes[0] = itemTypes.First() with { Name = new DomainTestBuilder<string>().Create() };

                var typeNames = new Dictionary<Guid, string>
                {
                    { ExpectedState.Editor.Item!.ItemTypes.Last().Key, "Name must be set" }
                };

                if (nameCurrentlyInvalid)
                    typeNames[itemTypes.First().Key] = "Name must be set";

                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            TypeNames = typeNames
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            TypeNames = new Dictionary<Guid, string>
                            {
                                { ExpectedState.Editor.Item!.ItemTypes.Last().Key, "Name must be set" }
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithNameEmpty()
            {
                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();
                itemTypes[0] = itemTypes.First() with { Name = string.Empty };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            TypeNames = new Dictionary<Guid, string>
                            {
                                { ExpectedState.Editor.Item!.ItemTypes.First().Key, "Name must not be empty" },
                                { ExpectedState.Editor.Item!.ItemTypes.Last().Key, "Name must be set" },
                            }
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
        private readonly OnItemTypeAddedFixture _fixture = new();

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
            result.Editor.Item!.ItemTypes.First().Key.Should().NotBeEmpty();
        }

        private sealed class OnItemTypeAddedFixture : ItemEditorReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            NoTypes = "No types selected",
                            StoreOrTypes = "Select a store or a type"
                        }
                    }
                };
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
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            NoTypes = null,
                            StoreOrTypes = null
                        }
                    }
                };
            }
        }
    }

    public class OnItemTypeRemoved
    {
        private readonly OnItemTypeRemovedFixture _fixture = new();

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

    public class OnStoreOfItemTypeChanged
    {
        private readonly OnStoreOfItemTypeChangedFixture _fixture = new();

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
        public void OnStoreOfItemTypeChanged_WithDuplicatedStore_ShouldChangeStore()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupExpectedStateWithDuplicatedStore();
            _fixture.SetupInitialStateWithDuplicatedStore();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemTypeChanged_WithSameStore_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupExpectedStateWithRandomDefaultSection();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithSameStore();

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
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedTypeStores = new Dictionary<Guid, string>
                            {
                                { itemTypes.First().Key, "Duplicated stores are not allowed" },
                                { itemTypes.Last().Key, "Duplicated stores are not allowed" }
                            }
                        }
                    }
                };
            }

            public void SetupInitialStateWithDuplicatedStore()
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
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedTypeStores = new Dictionary<Guid, string>
                            {
                                { itemTypes.Last().Key, "Duplicated stores are not allowed" }
                            }
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                SetupExpectedState(_store.DefaultSectionId);
            }

            public void SetupExpectedStateWithRandomDefaultSection()
            {
                SetupExpectedState(Guid.NewGuid());
            }

            private void SetupExpectedState(Guid defaultSectionId)
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = _store.Id,
                    DefaultSectionId = defaultSectionId
                };

                itemTypes[0] = itemTypes.First() with { Availabilities = availabilities };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedTypeStores = new Dictionary<Guid, string>
                            {
                                { itemTypes.Last().Key, "Duplicated stores are not allowed" }
                            }
                        }
                    }
                };
            }

            public void SetupExpectedStateWithDuplicatedStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemTypes = ExpectedState.Editor.Item!.ItemTypes.ToList();

                var availabilities = itemTypes.First().Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = _store.Id,
                    DefaultSectionId = _store.DefaultSectionId
                };
                availabilities[1] = availabilities[1] with
                {
                    StoreId = _store.Id,
                };

                itemTypes[0] = itemTypes.First() with { Availabilities = availabilities };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            ItemTypes = itemTypes
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedTypeStores = new Dictionary<Guid, string>
                            {
                                { itemTypes.First().Key, "Duplicated stores are not allowed" },
                                { itemTypes.Last().Key, "Duplicated stores are not allowed" }
                            }
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

            public void SetupActionWithSameStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var itemType = InitialState.Editor.Item!.ItemTypes.First();
                var availability = itemType.Availabilities.First();
                Action = new StoreOfItemTypeChangedAction(itemType, availability, availability.StoreId);
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

    public class OnDefaultSectionOfItemTypeChanged
    {
        private readonly OnDefaultSectionOfItemTypeChangedFixture _fixture = new();

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
}