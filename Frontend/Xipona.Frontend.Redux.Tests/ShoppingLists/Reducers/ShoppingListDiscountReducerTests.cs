using FluentAssertions;
using Xipona.Frontend.Redux.ShoppingList.Actions.ShoppingListDiscounts;
using Xipona.Frontend.Redux.ShoppingList.Reducers;
using Xipona.Frontend.Redux.ShoppingList.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.ShoppingLists.Reducers;
public class ShoppingListDiscountReducerTests
{
    public class OnOpenDiscountDialog
    {
        private readonly OnOpenDiscountDialogFixture _fixture = new();

        [Fact]
        public void OnOpenDiscountDialog_WithValidData_ShouldSetIsOpenToTrue()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ShoppingListDiscountReducer.OnOpenDiscountDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenDiscountDialogFixture : ShoppingListDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsOpen = false,
                        DiscountValue = new DomainTestBuilder<decimal>().Create(),
                        Type = ShoppingListDiscountType.Price,
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsOpen = true,
                        DiscountValue = 1m,
                        Type = ShoppingListDiscountType.Percentage,
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnCloseDiscountDialog
    {
        private readonly OnCloseDiscountDialogFixture _fixture = new();

        [Fact]
        public void OnCloseDiscountDialog_WithValidData_ShouldSetIsOpenToFalse()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();

            // Act
            var result = ShoppingListDiscountReducer.OnCloseDiscountDialog(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnCloseDiscountDialogFixture : ShoppingListDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsOpen = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsOpen = false
                    }
                };
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
            var result = ShoppingListDiscountReducer.OnSaveDiscountStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveDiscountStartedFixture : ShoppingListDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
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
            var result = ShoppingListDiscountReducer.OnSaveDiscountFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveDiscountFinishedFixture : ShoppingListDiscountReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }

    public class OnDiscountValueChanged
    {
        private readonly OnDiscountValueChangedFixture _fixture = new();

        [Fact]
        public void OnDiscountValueChanged_WithValidData_ShouldSetValue()
        {
            // Arrange
            _fixture.SetupAction();
            _fixture.SetupInitialState();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnDiscountValueChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDiscountValueChangedFixture : ShoppingListDiscountReducerFixture
        {
            public DiscountValueChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        DiscountValue = new DomainTestBuilder<decimal>().Create(),
                    }
                };
            }

            public void SetupAction()
            {
                Action = new DiscountValueChangedAction(ExpectedState.ShoppingListDiscountDialog.DiscountValue);
            }
        }
    }

    public class OnDiscountTypeChanged
    {
        private readonly OnDiscountTypeChangedFixture _fixture = new();

        [Theory]
        [InlineData(ShoppingListDiscountType.Percentage, ShoppingListDiscountType.Price)]
        [InlineData(ShoppingListDiscountType.Price, ShoppingListDiscountType.Percentage)]
        public void OnDiscountTypeChanged_WithValidData_ShouldSetType(ShoppingListDiscountType current,
            ShoppingListDiscountType expected)
        {
            // Arrange
            _fixture.SetupExpectedState(expected);
            _fixture.SetupInitialState(current);
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnDiscountTypeChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDiscountTypeChangedFixture : ShoppingListDiscountReducerFixture
        {
            public DiscountTypeChangedAction? Action { get; private set; }

            public void SetupInitialState(ShoppingListDiscountType type)
            {
                InitialState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        Type = type
                    }
                };
            }

            public void SetupExpectedState(ShoppingListDiscountType type)
            {
                ExpectedState = ExpectedState with
                {
                    ShoppingListDiscountDialog = ExpectedState.ShoppingListDiscountDialog with
                    {
                        Type = type
                    }
                };
            }

            public void SetupAction()
            {
                Action = new DiscountTypeChangedAction(ExpectedState.ShoppingListDiscountDialog.Type);
            }
        }
    }

    public class OnRemoveDiscountStarted
    {
        private readonly OnRemoveDiscountStartedFixture _fixture = new();

        [Fact]
        public void OnRemoveDiscountStarted_WithValidData_ShouldSetIsDeletingToTrue()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountStarted(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveDiscountStarted_WithDiscountNotExisting_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStateEqualInitialState();
            _fixture.SetupActionWithNotExistingDiscount();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountStarted(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveDiscountStarted_WithShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateWithoutShoppingList();
            _fixture.SetupExpectedStateEqualInitialState();
            _fixture.SetupActionWithNotExistingDiscount();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountStarted(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveDiscountStartedFixture : ShoppingListDiscountReducerFixture
        {
            private Guid? _discountId;
            private int _discountIdx = -1;
            public RemoveDiscountStartedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                var discounts = ExpectedState.ShoppingList!.Discounts.ToList();

                discounts[_discountIdx] = discounts[_discountIdx] with
                {
                    IsDeleting = false
                };

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Discounts = discounts
                    }
                };
            }

            public void SetupInitialStateWithoutShoppingList()
            {
                InitialState = ExpectedState with
                {
                    ShoppingList = null
                };
            }

            public void SetupExpectedState()
            {
                var discounts = ExpectedState.ShoppingList!.Discounts.ToList();
                _discountIdx = new Random().Next(0, discounts.Count - 1);

                _discountId = discounts[_discountIdx].Id;
                discounts[_discountIdx] = discounts[_discountIdx] with
                {
                    IsDeleting = true
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Discounts = discounts
                    }
                };
            }

            public void SetupExpectedStateEqualInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_discountId);
                Action = new RemoveDiscountStartedAction(_discountId.Value);
            }

            public void SetupActionWithNotExistingDiscount()
            {
                Action = new RemoveDiscountStartedAction(Guid.NewGuid());
            }
        }
    }

    public class OnRemoveDiscountFinished
    {
        private readonly OnRemoveDiscountFinishedFixture _fixture = new();

        [Fact]
        public void OnRemoveDiscountFinished_WithValidData_ShouldSetRemoveDiscount()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveDiscountFinished_WithDiscountNotExisting_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStateEqualInitialState();
            _fixture.SetupActionWithNotExistingDiscount();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveDiscountFinished_WithShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateWithoutShoppingList();
            _fixture.SetupExpectedStateEqualInitialState();
            _fixture.SetupActionWithNotExistingDiscount();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveDiscountFinishedFixture : ShoppingListDiscountReducerFixture
        {
            private Guid? _discountId;
            public RemoveDiscountFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                var discountToDelete = new DomainTestBuilder<ShoppingListDiscount>().Create();
                _discountId = discountToDelete.Id;

                var discounts = ExpectedState.ShoppingList!.Discounts.ToList();
                var idx = new Random().Next(0, discounts.Count - 1);

                discounts.Insert(idx, discountToDelete);

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Discounts = discounts
                    }
                };
            }

            public void SetupInitialStateWithoutShoppingList()
            {
                InitialState = ExpectedState with
                {
                    ShoppingList = null
                };
            }

            public void SetupExpectedStateEqualInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_discountId);
                Action = new RemoveDiscountFinishedAction(_discountId.Value);
            }

            public void SetupActionWithNotExistingDiscount()
            {
                Action = new RemoveDiscountFinishedAction(Guid.NewGuid());
            }
        }
    }

    public class OnRemoveDiscountFailed
    {
        private readonly OnRemoveDiscountFailedFixture _fixture = new();

        [Fact]
        public void OnRemoveDiscountFailed_WithValidData_ShouldSetIsDeletingToFalse()
        {
            // Arrange
            _fixture.SetupExpectedState();
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountFailed(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveDiscountFailed_WithDiscountNotExisting_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupExpectedStateEqualInitialState();
            _fixture.SetupActionWithNotExistingDiscount();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountFailed(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnRemoveDiscountFailed_WithShoppingListNull_ShouldDoNothing()
        {
            // Arrange
            _fixture.SetupInitialStateWithoutShoppingList();
            _fixture.SetupExpectedStateEqualInitialState();
            _fixture.SetupActionWithNotExistingDiscount();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ShoppingListDiscountReducer.OnRemoveDiscountFailed(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnRemoveDiscountFailedFixture : ShoppingListDiscountReducerFixture
        {
            private Guid? _discountId;
            private int _discountIdx = -1;
            public RemoveDiscountFailedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                var discounts = ExpectedState.ShoppingList!.Discounts.ToList();

                discounts[_discountIdx] = discounts[_discountIdx] with
                {
                    IsDeleting = true
                };

                InitialState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Discounts = discounts
                    }
                };
            }

            public void SetupInitialStateWithoutShoppingList()
            {
                InitialState = ExpectedState with
                {
                    ShoppingList = null
                };
            }

            public void SetupExpectedState()
            {
                var discounts = ExpectedState.ShoppingList!.Discounts.ToList();
                _discountIdx = new Random().Next(0, discounts.Count - 1);

                _discountId = discounts[_discountIdx].Id;
                discounts[_discountIdx] = discounts[_discountIdx] with
                {
                    IsDeleting = false
                };

                ExpectedState = ExpectedState with
                {
                    ShoppingList = ExpectedState.ShoppingList with
                    {
                        Discounts = discounts
                    }
                };
            }

            public void SetupExpectedStateEqualInitialState()
            {
                ExpectedState = InitialState;
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_discountId);
                Action = new RemoveDiscountFailedAction(_discountId.Value);
            }

            public void SetupActionWithNotExistingDiscount()
            {
                Action = new RemoveDiscountFailedAction(Guid.NewGuid());
            }
        }
    }

    private abstract class ShoppingListDiscountReducerFixture
    {
        public ShoppingListState InitialState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
        public ShoppingListState ExpectedState { get; protected set; } = new DomainTestBuilder<ShoppingListState>().Create();
    }
}
