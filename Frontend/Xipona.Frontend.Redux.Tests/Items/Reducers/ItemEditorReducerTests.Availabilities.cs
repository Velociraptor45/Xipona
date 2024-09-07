using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Editor.Availabilities;
using ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Items.States;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Reducers;

public partial class ItemEditorReducerTests
{
    public class OnStoreAddedToItem
    {
        private readonly OnStoreAddedToItemFixture _fixture = new();

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
                                    4.67m)
                            }
                        },
                        ValidationResult = InitialState.Editor.ValidationResult with
                        {
                            NoStores = "Add at least one store",
                            StoreOrTypes = "Add at least one store or type"
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
                                    4.67m),
                                new(
                                    _stores.First().Id,
                                    _stores.First().DefaultSectionId,
                                    1m),
                            }
                        },
                        ValidationResult = InitialState.Editor.ValidationResult with
                        {
                            NoStores = null,
                            StoreOrTypes = null
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
                                    4.67m),
                                new(
                                    _stores.First().Id,
                                    _stores.First().DefaultSectionId,
                                    13.98m),
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
        private readonly OnStoreOfItemChangedFixture _fixture = new();

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
        public void OnStoreOfItemChanged_WithDuplicatedStore_ShouldChangeStoreAndSetValidationError()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupExpectedStateWithDuplicatedStore();
            _fixture.SetupInitialStateWithDuplicatedStore();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemChanged_WithSameStore_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupStore();
            _fixture.SetupExpectedStateWithRandomDefaultSection();
            _fixture.SetupInitialStateEqualExpectedState();
            _fixture.SetupActionWithSameStore();

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
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedStores = "Duplicated stores are not allowed"
                        }
                    }
                };
            }

            public void SetupInitialStateWithDuplicatedStore()
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
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedStores = null
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

            public void SetupExpectedStateWithDuplicatedStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = _store.Id,
                    DefaultSectionId = _store.DefaultSectionId
                };
                availabilities[1] = availabilities[1] with
                {
                    StoreId = _store.Id,
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            Availabilities = availabilities
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedStores = "Duplicated stores are not allowed"
                        }
                    }
                };
            }

            private void SetupExpectedState(Guid defaultSectionId)
            {
                TestPropertyNotSetException.ThrowIfNull(_store);

                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[0] = availabilities.First() with
                {
                    StoreId = _store.Id,
                    DefaultSectionId = defaultSectionId
                };

                ExpectedState = ExpectedState with
                {
                    Editor = ExpectedState.Editor with
                    {
                        Item = ExpectedState.Editor.Item with
                        {
                            Availabilities = availabilities
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedStores = null
                        }
                    }
                };
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                Action = new StoreOfItemChangedAction(InitialState.Editor.Item!.Availabilities.First(), _store.Id);
            }

            public void SetupActionWithSameStore()
            {
                TestPropertyNotSetException.ThrowIfNull(_store);
                var availability = InitialState.Editor.Item!.Availabilities.First();
                Action = new StoreOfItemChangedAction(availability, availability.StoreId);
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

    public class OnDefaultSectionOfItemChanged
    {
        private readonly OnDefaultSectionOfItemChangedFixture _fixture = new();

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

    public class OnPriceOfItemChanged
    {
        private readonly OnPriceOfItemChangedFixture _fixture = new();

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
                    PricePerQuantity = new DomainTestBuilder<decimal>().Create()
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

    public class OnStoreOfItemRemoved
    {
        private readonly OnStoreOfItemRemovedFixture _fixture = new();

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
        public void OnStoreOfItemRemoved_WithDuplication_DuplicationRemoved_ShouldRemoveValidationError()
        {
            // Arrange
            _fixture.SetupInitialStateWithRemovedStoreDuplicated();
            _fixture.SetupExpectedStateWithRemovedStoreDuplicated();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemEditorReducer.OnStoreOfItemRemoved(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStoreOfItemRemoved_WithDuplication_DuplicationNotRemoved_ShouldNotRemoveValidationError()
        {
            // Arrange
            _fixture.SetupInitialStateWithNotRemovedStoreDuplicated();
            _fixture.SetupExpectedStateWithNotRemovedStoreDuplicated();
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
                var availabilities = ExpectedState.Editor.Item!.Availabilities.ToList();
                availabilities[1] = availabilities[1] with
                {
                    StoreId = availabilities[duplicatedStoreIndex].StoreId
                };

                InitialState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item! with
                        {
                            Availabilities = availabilities
                        },
                        ValidationResult = InitialState.Editor.ValidationResult with
                        {
                            DuplicatedStores = "Duplicated stores are not allowed"
                        }
                    }
                };
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
                        },
                        ValidationResult = ExpectedState.Editor.ValidationResult with
                        {
                            DuplicatedStores = null
                        }
                    }
                };
            }

            public void SetupExpectedStateWithNotRemovedStoreDuplicated()
            {
                SetupExpectedStateWithDuplication(false);
            }

            public void SetupExpectedStateWithRemovedStoreDuplicated()
            {
                SetupExpectedStateWithDuplication(true);
            }

            private void SetupExpectedStateWithDuplication(bool duplicationRemoved)
            {
                var availabilities = InitialState.Editor.Item!.Availabilities.ToList();
                availabilities.RemoveAt(0);

                ExpectedState = InitialState with
                {
                    Editor = InitialState.Editor with
                    {
                        Item = InitialState.Editor.Item with
                        {
                            Availabilities = availabilities
                        },
                        ValidationResult = InitialState.Editor.ValidationResult with
                        {
                            DuplicatedStores = duplicationRemoved ? null : "Duplicated stores are not allowed"
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
}