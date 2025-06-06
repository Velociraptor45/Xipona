using FluentAssertions;
using Xipona.Frontend.Redux.ShoppingList.Actions.ItemDiscounts;
using Xipona.Frontend.Redux.ShoppingList.Reducers;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;
public class ItemDiscountReducerTests
{
    public class OnOpenDiscountDialog
    {
        private readonly OnOpenDiscountDialogFixture _fixture = new();

        [Fact]
        public void OnOpenDiscountDialog_WithValidData_ShouldOpenDialog()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemDiscountReducer.OnOpenDiscountDialog(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenDiscountDialogFixture : ItemDiscountReducerFixture
        {
            public OpenDiscountDialogAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        Item = new DomainTestBuilder<ShoppingListItem>().Create(),
                        Discount = new DomainTestBuilder<decimal>().Create(),
                        IsOpen = false,
                        IsSaving = true,
                        IsRemoving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                var item = new DomainTestBuilder<ShoppingListItem>().Create();

                ExpectedState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        Item = item,
                        Discount = item.PricePerQuantity,
                        IsOpen = true,
                        IsSaving = false,
                        IsRemoving = false
                    }
                };
            }

            public void SetupAction()
            {
                Action = new OpenDiscountDialogAction(ExpectedState.ItemDiscountDialog.Item!);
            }
        }
    }

    public class OnSaveDiscountStarted
    {
        private readonly OnSaveDiscountStartedFixture _fixture = new();

        [Fact]
        public void OnSaveDiscountStarted_WithValidData_ShouldSetIsSavingToTrue()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemDiscountReducer.OnSaveDiscountStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveDiscountStartedFixture : ItemDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }

    public class OnSaveDiscountFinished
    {
        private readonly OnSaveDiscountFinishedFixture _fixture = new();

        [Fact]
        public void OnSaveDiscountFinished_WithValidData_ShouldSetIsSavingToFalse()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemDiscountReducer.OnSaveDiscountFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveDiscountFinishedFixture : ItemDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnRemoveDiscountStarted
    {
        private readonly OnRemoveDiscountStartedFixture _fixture = new();

        [Fact]
        public void OnRemoveDiscountStarted_WithValidData_ShouldSetIsRemovingToTrue()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemDiscountReducer.OnRemoveDiscountStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveDiscountStartedFixture : ItemDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsRemoving = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsRemoving = true
                    }
                };
            }
        }
    }

    public class OnRemoveDiscountFinished
    {
        private readonly OnRemoveDiscountFinishedFixture _fixture = new();

        [Fact]
        public void OnRemoveDiscountFinished_WithValidData_ShouldSetIsRemovingToFalse()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemDiscountReducer.OnRemoveDiscountFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveDiscountFinishedFixture : ItemDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsRemoving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        IsRemoving = false
                    }
                };
            }
        }
    }

    public class OnCloseDiscountDialog
    {
        private readonly OnCloseDiscountDialogFixture _fixture = new();

        [Fact]
        public void OnCloseDiscountDialog_WithValidData_ShouldCloseDialog()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ItemDiscountReducer.OnCloseDiscountDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseDiscountDialogFixture : ItemDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        Item = new DomainTestBuilder<ShoppingListItem>().Create(),
                        IsOpen = true,
                        IsSaving = true,
                        IsRemoving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        Item = null,
                        IsOpen = false,
                        IsSaving = false,
                        IsRemoving = false
                    }
                };
            }
        }
    }

    public class OnDiscountChanged
    {
        private readonly OnDiscountChangedFixture _fixture = new();

        [Fact]
        public void OnDiscountChanged_WithValidData_ShouldSetDiscount()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemDiscountReducer.OnDiscountChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDiscountChangedFixture : ItemDiscountReducerFixture
        {
            public DiscountChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ItemDiscountDialog = ExpectedState.ItemDiscountDialog with
                    {
                        Discount = new DomainTestBuilder<decimal>().Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new DiscountChangedAction(ExpectedState.ItemDiscountDialog.Discount);
            }
        }
    }

    private abstract class ItemDiscountReducerFixture
    {
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}
