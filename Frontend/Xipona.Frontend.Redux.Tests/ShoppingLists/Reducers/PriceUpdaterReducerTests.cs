using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Actions.PriceUpdater;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States;
using ProjectHermes.Xipona.Frontend.Redux.ShoppingList.States.Comparer;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;

public class PriceUpdaterReducerTests
{
    public class OnPriceOnPriceUpdaterChanged
    {
        private readonly OnPriceOnPriceUpdaterChangedFixture _fixture;

        public OnPriceOnPriceUpdaterChanged()
        {
            _fixture = new OnPriceOnPriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnPriceOnPriceUpdaterChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnPriceOnPriceUpdaterChanged(_fixture.InitialState, _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnPriceOnPriceUpdaterChangedFixture : PriceUpdaterReducerFixture
        {
            public PriceOnPriceUpdaterChangedAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new PriceOnPriceUpdaterChangedAction(ExpectedState.PriceUpdate.Price);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Price = new DomainTestBuilder<decimal>().Create()
                    }
                };
            }
        }
    }

    public class OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged
    {
        private readonly OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture _fixture;

        public OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged()
        {
            _fixture = new OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture();
        }

        [Fact]
        public void OnUpdatePriceForAllTypesOnPriceUpdaterChangedChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnUpdatePriceForAllTypesOnPriceUpdaterChanged(_fixture.InitialState, _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnUpdatePriceForAllTypesOnPriceUpdaterChangedChangedFixture : PriceUpdaterReducerFixture
        {
            public UpdatePriceForAllTypesOnPriceUpdaterChangedAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new UpdatePriceForAllTypesOnPriceUpdaterChangedAction(ExpectedState.PriceUpdate.UpdatePriceForAllTypes);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        UpdatePriceForAllTypes = !ExpectedState.PriceUpdate.UpdatePriceForAllTypes
                    }
                };
            }
        }
    }

    public class OnOpenPriceUpdaterChanged
    {
        private readonly OnOpenPriceUpdaterChangedFixture _fixture;

        public OnOpenPriceUpdaterChanged()
        {
            _fixture = new OnOpenPriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnOpenPriceUpdaterChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnOpenPriceUpdater(_fixture.InitialState, _fixture.ChangeAction);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenPriceUpdaterChangedFixture : PriceUpdaterReducerFixture
        {
            public OpenPriceUpdaterAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new OpenPriceUpdaterAction(ExpectedState.PriceUpdate.Item!);
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Item = null,
                        Price = new DomainTestBuilder<decimal>().Create(),
                        UpdatePriceForAllTypes = false,
                        IsOpen = false,
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Price = ExpectedState.PriceUpdate.Item!.PricePerQuantity,
                        UpdatePriceForAllTypes = true,
                        IsOpen = true,
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnClosePriceUpdaterChanged
    {
        private readonly OnClosePriceUpdaterChangedFixture _fixture;

        public OnClosePriceUpdaterChanged()
        {
            _fixture = new OnClosePriceUpdaterChangedFixture();
        }

        [Fact]
        public void OnClosePriceUpdaterChanged_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.ChangeAction);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnClosePriceUpdater(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnClosePriceUpdaterChangedFixture : PriceUpdaterReducerFixture
        {
            public ClosePriceUpdaterAction? ChangeAction { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupAction()
            {
                ChangeAction = new ClosePriceUpdaterAction();
            }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Item = new DomainTestBuilder<ShoppingListItem>().Create(),
                        IsOpen = true,
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        Item = null,
                        IsOpen = false,
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnSavePriceUpdateFinished
    {
        private readonly OnSavePriceUpdateFinishedFixture _fixture;

        public OnSavePriceUpdateFinished()
        {
            _fixture = new OnSavePriceUpdateFinishedFixture();
        }

        [Fact]
        public void OnSavePriceUpdateFinished_WithUpdateForAllTypes_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupItemIdAndPrice();
            _fixture.SetupExpectedStateForAllTypes();
            _fixture.SetupActionForAllTypes();
            _fixture.SetupInitialStateForAllTypes();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnSavePriceUpdateFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSavePriceUpdateFinished_WithUpdateForOneType_ShouldUpdateState()
        {
            // Arrange
            _fixture.SetupItemIdAndPrice();
            _fixture.SetupTypeId();
            _fixture.SetupExpectedStateForOneType();
            _fixture.SetupActionForOneType();
            _fixture.SetupInitialStateForOneType();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            TestPropertyNotSetException.ThrowIfNull(_fixture.InitialState);

            // Act
            var result = PriceUpdaterReducer.OnSavePriceUpdateFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSavePriceUpdateFinishedFixture : PriceUpdaterReducerFixture
        {
            private Guid? _itemId;
            private Guid? _itemTypeId;
            private decimal? _price;

            public SavePriceUpdateFinishedAction? Action { get; private set; }
            public ShoppingListState? InitialState { get; private set; }

            public void SetupItemIdAndPrice()
            {
                _itemId = Guid.NewGuid();
                _price = new DomainTestBuilder<decimal>().Create();
            }

            public void SetupTypeId()
            {
                _itemTypeId = Guid.NewGuid();
            }

            public void SetupActionForAllTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                TestPropertyNotSetException.ThrowIfNull(_price);

                Action = new SavePriceUpdateFinishedAction(_itemId.Value, null, _price.Value);
            }

            public void SetupActionForOneType()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeId);
                TestPropertyNotSetException.ThrowIfNull(_price);

                Action = new SavePriceUpdateFinishedAction(_itemId.Value, _itemTypeId.Value, _price.Value);
            }

            public void SetupInitialStateForAllTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);

                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                for (int i = 0; i < sections.Count; i++)
                {
                    var section = sections[i];
                    var items = section.Items.ToList();
                    for (int ii = 0; ii < items.Count; ii++)
                    {
                        var item = items[ii];
                        items[ii] = item with
                        {
                            PricePerQuantity =
                                item.Id.ActualId == _itemId.Value
                                    ? new DomainTestBuilder<decimal>().Create()
                                    : item.PricePerQuantity
                        };
                    }

                    sections[i] = section with
                    {
                        Items = items
                    };
                }

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    },
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupInitialStateForOneType()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeId);

                var sections = ExpectedState.ShoppingList!.Sections.ToList();
                for (int i = 0; i < sections.Count; i++)
                {
                    var section = sections[i];
                    var items = section.Items.ToList();
                    for (int ii = 0; ii < items.Count; ii++)
                    {
                        var item = items[ii];
                        items[ii] = item with
                        {
                            PricePerQuantity =
                                item.Id.ActualId == _itemId.Value && item.TypeId == _itemTypeId.Value
                                    ? new DomainTestBuilder<decimal>().Create()
                                    : item.PricePerQuantity
                        };
                    }

                    sections[i] = section with
                    {
                        Items = items
                    };
                }

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Sections = new SortedSet<ShoppingListSection>(sections, new SortingIndexComparer())
                    },
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedStateForAllTypes()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                TestPropertyNotSetException.ThrowIfNull(_price);

                var section1 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items = new List<ShoppingListItem>
                    {
                        new DomainTestBuilder<ShoppingListItem>().Create(),
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId.Value),
                            PricePerQuantity = _price.Value
                        }
                    },
                    SortingIndex = 0
                };
                var section2 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items = new List<ShoppingListItem>
                    {
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId.Value),
                            TypeId = null,
                            PricePerQuantity = _price.Value
                        },
                        new DomainTestBuilder<ShoppingListItem>().Create()
                    },
                    SortingIndex = 1
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList! with
                    {
                        Sections = new SortedSet<ShoppingListSection>(new SortingIndexComparer()) { section1, section2 }
                    },
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedStateForOneType()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemId);
                TestPropertyNotSetException.ThrowIfNull(_itemTypeId);
                TestPropertyNotSetException.ThrowIfNull(_price);

                var section1 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items = new List<ShoppingListItem>
                    {
                        new DomainTestBuilder<ShoppingListItem>().Create(),
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId.Value),
                            TypeId = _itemTypeId.Value,
                            PricePerQuantity = _price.Value
                        },
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId.Value),
                            TypeId = null,
                            PricePerQuantity = _price.Value
                        }
                    },
                    SortingIndex = 0
                };
                var section2 = new DomainTestBuilder<ShoppingListSection>().Create() with
                {
                    Items = new List<ShoppingListItem>
                    {
                        new DomainTestBuilder<ShoppingListItem>().Create() with
                        {
                            Id = ShoppingListItemId.FromActualId(_itemId.Value),
                            PricePerQuantity = _price.Value
                        },
                        new DomainTestBuilder<ShoppingListItem>().Create()
                    },
                    SortingIndex = 1
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList! with
                    {
                        Sections = new SortedSet<ShoppingListSection>(new SortingIndexComparer()) { section1, section2 }
                    },
                    PriceUpdate = ExpectedState.PriceUpdate with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    private abstract class PriceUpdaterReducerFixture
    {
        public ShoppingListState ExpectedState { get; protected set; } =
            new DomainTestBuilder<ShoppingListState>().Create();
    }
}