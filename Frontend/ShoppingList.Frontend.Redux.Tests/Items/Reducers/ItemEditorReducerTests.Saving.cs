using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Reducers;

public partial class ItemEditorReducerTests
{
    public class OnModifyItem
    {
        private readonly OnModifyItemFixture _fixture = new();

        [Fact]
        public void OnModifyItem_WithoutItem_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStoreWithoutItem();
            _fixture.SetupInitialStateEqualExpectedState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithEmptyName_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyName();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithEmptyItemCategory_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyItemCategory();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithoutStoresOrTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutStoresOrTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithNoStoresSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithNoTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithTypeNameEmpty_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithTypeNameEmpty();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithNoStoresSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithDuplicatedStoreSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnModifyItem_WithDuplicatedStoreSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnModifyItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnModifyItemFixture : OnSaveItemFixture
        {
        }
    }

    public class OnUpdateItem
    {
        private readonly OnUpdateItemFixture _fixture = new();

        [Fact]
        public void OnUpdateItem_WithoutItem_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStoreWithoutItem();
            _fixture.SetupInitialStateEqualExpectedState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithEmptyName_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyName();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithEmptyItemCategory_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyItemCategory();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithoutStoresOrTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutStoresOrTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithNoStoresSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithNoTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithTypeNameEmpty_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithTypeNameEmpty();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithNoStoresSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithDuplicatedStoreSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnUpdateItem_WithDuplicatedStoreSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnUpdateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnUpdateItemFixture : OnSaveItemFixture
        {
        }
    }

    public class OnCreateItem
    {
        private readonly OnCreateItemFixture _fixture = new();

        [Fact]
        public void OnCreateItem_WithoutItem_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStoreWithoutItem();
            _fixture.SetupInitialStateEqualExpectedState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithEmptyName_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyName();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithEmptyItemCategory_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyItemCategory();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithoutStoresOrTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutStoresOrTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithNoStoresSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithNoTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithTypeNameEmpty_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithTypeNameEmpty();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithNoStoresSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithDuplicatedStoreSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnCreateItem_WithDuplicatedStoreSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnCreateItem(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCreateItemFixture : OnSaveItemFixture
        {
        }
    }

    public class OnMakeItemPermanent
    {
        private readonly OnMakeItemPermanentFixture _fixture = new();

        [Fact]
        public void OnMakeItemPermanent_WithoutItem_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStoreWithoutItem();
            _fixture.SetupInitialStateEqualExpectedState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithEmptyName_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyName();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithEmptyItemCategory_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithEmptyItemCategory();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithoutStoresOrTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithoutStoresOrTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithNoStoresSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithNoTypesSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoTypesSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithTypeNameEmpty_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithTypeNameEmpty();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithNoStoresSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithNoStoresSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithDuplicatedStoreSelectedInType_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelectedInType();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMakeItemPermanent_WithDuplicatedStoreSelected_ShouldSetValidationError()
        {
            // Arrange
            _fixture.SetupExpectedStateWithDuplicatedStoreSelected();
            _fixture.SetupInitialState();

            // Act
            var result = ItemEditorReducer.OnMakeItemPermanent(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnMakeItemPermanentFixture : OnSaveItemFixture
        {
        }
    }

    private abstract class OnSaveItemFixture : ItemEditorReducerFixture
    {
        public void SetupInitialStateEqualExpectedState()
        {
            InitialState = ExpectedState;
        }

        public void SetupInitialState()
        {
            InitialState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    ValidationResult = new DomainTestBuilder<EditorValidationResult>().Create()
                }
            };
        }

        public void SetupExpectedStoreWithoutItem()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = null
                }
            };
        }

        public void SetupExpectedStateWithEmptyName()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        Name = string.Empty
                    },
                    ValidationResult = new EditorValidationResult { Name = "Name must not be empty" }
                }
            };
        }

        public void SetupExpectedStateWithEmptyItemCategory()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        ItemCategoryId = null
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        ItemCategory = "Item category must not be empty"
                    }
                }
            };
        }

        public void SetupExpectedStateWithoutStoresOrTypesSelected()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        Availabilities = new List<EditedItemAvailability>(0),
                        ItemTypes = new List<EditedItemType>(0),
                        ItemMode = ItemMode.NotDefined
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        StoreOrTypes = "Select either stores or types"
                    }
                }
            };
        }

        public void SetupExpectedStateWithNoStoresSelected()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        Availabilities = new List<EditedItemAvailability>(0),
                        ItemMode = ItemMode.WithoutTypes
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        NoStores = "Add at least one store"
                    }
                }
            };
        }

        public void SetupExpectedStateWithNoTypesSelected()
        {
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        ItemTypes = new List<EditedItemType>(0),
                        ItemMode = ItemMode.WithTypes
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        NoTypes = "Add at least one type"
                    }
                }
            };
        }

        public void SetupExpectedStateWithTypeNameEmpty()
        {
            var key = Guid.NewGuid();
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        ItemTypes = new List<EditedItemType>()
                            {
                                new DomainTestBuilder<EditedItemType>().Create(),
                                new DomainTestBuilder<EditedItemType>().Create() with
                                {
                                    Key = key,
                                    Name = string.Empty
                                }
                            },
                        ItemMode = ItemMode.WithTypes
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        TypeNames = new Dictionary<Guid, string>
                            {
                                { key, "Name must not be empty" }
                            }
                    }
                }
            };
        }

        public void SetupExpectedStateWithNoStoresSelectedInType()
        {
            var key = Guid.NewGuid();
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        ItemTypes = new List<EditedItemType>()
                            {
                                new DomainTestBuilder<EditedItemType>().Create(),
                                new DomainTestBuilder<EditedItemType>().Create() with
                                {
                                    Key = key,
                                    Availabilities = new List<EditedItemAvailability>(0)
                                }
                            },
                        ItemMode = ItemMode.WithTypes
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        NoTypeStores = new Dictionary<Guid, string>
                            {
                                { key, "Add at least one store" }
                            }
                    }
                }
            };
        }

        public void SetupExpectedStateWithDuplicatedStoreSelectedInType()
        {
            var key = Guid.NewGuid();
            var storeId = Guid.NewGuid();
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        ItemTypes = new List<EditedItemType>()
                            {
                                new DomainTestBuilder<EditedItemType>().Create(),
                                new DomainTestBuilder<EditedItemType>().Create() with
                                {
                                    Key = key,
                                    Availabilities = new List<EditedItemAvailability>()
                                    {
                                        new DomainTestBuilder<EditedItemAvailability>().Create() with
                                        {
                                            StoreId = storeId
                                        },
                                        new DomainTestBuilder<EditedItemAvailability>().Create() with
                                        {
                                            StoreId = storeId
                                        }
                                    }
                                }
                            },
                        ItemMode = ItemMode.WithTypes
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        DuplicatedTypeStores = new Dictionary<Guid, string>
                            {
                                { key, "There are duplicated stores" }
                            }
                    }
                }
            };
        }

        public void SetupExpectedStateWithDuplicatedStoreSelected()
        {
            var storeId = Guid.NewGuid();
            ExpectedState = ExpectedState with
            {
                Editor = ExpectedState.Editor with
                {
                    Item = ExpectedState.Editor.Item! with
                    {
                        Availabilities = new List<EditedItemAvailability>()
                            {
                                new DomainTestBuilder<EditedItemAvailability>().Create() with
                                {
                                    StoreId = storeId
                                },
                                new DomainTestBuilder<EditedItemAvailability>().Create() with
                                {
                                    StoreId = storeId
                                }
                            },
                        ItemMode = ItemMode.WithTypes
                    },
                    ValidationResult = new EditorValidationResult
                    {
                        DuplicatedStores = "There are duplicated stores"
                    }
                }
            };
        }
    }

    public class OnCreateItemStarted
    {
        private readonly OnCreateItemStartedFixture _fixture = new();

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
        private readonly OnCreateItemFinishedFixture _fixture = new();

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
        private readonly OnUpdateItemStartedFixture _fixture = new();

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
        private readonly OnUpdateItemFinishedFixture _fixture = new();

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
        private readonly OnModifyItemStartedFixture _fixture = new();

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
        private readonly OnModifyItemFinishedFixture _fixture = new();

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
        private readonly OnMakeItemPermanentStartedFixture _fixture = new();

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
        private readonly OnMakeItemPermanentFinishedFixture _fixture = new();

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
        private readonly OnDeleteItemStartedFixture _fixture = new();

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
        private readonly OnDeleteItemFinishedFixture _fixture = new();

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
        private readonly OnOpenDeleteItemDialogFixture _fixture = new();

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
        private readonly OnCloseDeleteItemDialogFixture _fixture = new();

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
}