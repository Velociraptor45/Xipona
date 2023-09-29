using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Items.States;
using ProjectHermes.ShoppingList.Frontend.Redux.Shared.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Items.Reducers;

public class ItemReducerTests
{
    public class OnSearchItemStarted
    {
        private readonly OnSearchItemStartedFixture _fixture;

        public OnSearchItemStarted()
        {
            _fixture = new OnSearchItemStartedFixture();
        }

        [Fact]
        public void OnSearchItemStarted_WithSearchNotLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupExpectedSearchLoading();

            // Act
            var result = ItemReducer.OnSearchItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnSearchItemStarted_WithSearchLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupExpectedSearchLoading();

            // Act
            var result = ItemReducer.OnSearchItemStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSearchItemStartedFixture : ItemReducerFixture
        {
            public void SetupInitialSearchNotLoading()
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = false
                    }
                };
            }

            public void SetupInitialSearchLoading()
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = true
                    }
                };
            }

            public void SetupExpectedSearchLoading()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = true
                    }
                };
            }
        }
    }

    public class OnSearchItemFinished
    {
        private readonly OnSearchItemFinishedFixture _fixture;

        public OnSearchItemFinished()
        {
            _fixture = new OnSearchItemFinishedFixture();
        }

        [Fact]
        public void OnSearchItemFinished_WithSearchNotLoading_ShouldSetLoadingAndSortResults()
        {
            // Arrange
            _fixture.SetupExpectedSearchLoading();
            _fixture.SetupInitialSearchNotLoading();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemReducer.OnSearchItemFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        [Fact]
        public void OnSearchItemFinished_WithSearchLoading_ShouldSetLoadingAndSortResults()
        {
            // Arrange
            _fixture.SetupExpectedSearchLoading();
            _fixture.SetupInitialSearchLoading();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemReducer.OnSearchItemFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnSearchItemFinishedFixture : ItemReducerFixture
        {
            public SearchItemsFinishedAction? Action { get; private set; }

            public void SetupInitialSearchNotLoading()
            {
                SetupInitialState(false);
            }

            public void SetupInitialSearchLoading()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSearching)
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = isSearching,
                        TriggeredAtLeastOnce = false,
                        SearchResults = new DomainTestBuilder<ItemSearchResult>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedSearchLoading()
            {
                ExpectedState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        IsLoadingSearchResults = false,
                        TriggeredAtLeastOnce = true,
                        SearchResults = new List<ItemSearchResult>
                        {
                            new DomainTestBuilder<ItemSearchResult>()
                                .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<ItemSearchResult>()
                                .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                                .Create(),
                            new DomainTestBuilder<ItemSearchResult>()
                                .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                                .Create()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchItemsFinishedAction(ExpectedState.Search.SearchResults.Reverse().ToList());
            }
        }
    }

    public class OnLoadQuantityTypesFinished
    {
        private readonly OnLoadQuantityTypesFinishedFixture _fixture;

        public OnLoadQuantityTypesFinished()
        {
            _fixture = new OnLoadQuantityTypesFinishedFixture();
        }

        [Fact]
        public void OnLoadQuantityTypesFinished_WithValidData_ShouldSetQuantityTypes()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemReducer.OnLoadQuantityTypesFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadQuantityTypesFinishedFixture : ItemReducerFixture
        {
            public LoadQuantityTypesFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    QuantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList()
                };
            }

            public void SetupAction()
            {
                Action = new LoadQuantityTypesFinishedAction(ExpectedState.QuantityTypes);
            }
        }
    }

    public class OnLoadQuantityTypesInPacketFinished
    {
        private readonly OnLoadQuantityTypesInPacketFinishedFixture _fixture;

        public OnLoadQuantityTypesInPacketFinished()
        {
            _fixture = new OnLoadQuantityTypesInPacketFinishedFixture();
        }

        [Fact]
        public void OnLoadQuantityTypesInPacketFinished_WithValidData_ShouldSetQuantityTypesInPacket()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemReducer.OnLoadQuantityTypesInPacketFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadQuantityTypesInPacketFinishedFixture : ItemReducerFixture
        {
            public LoadQuantityTypesInPacketFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    QuantityTypesInPacket = new DomainTestBuilder<QuantityTypeInPacket>().CreateMany(2).ToList()
                };
            }

            public void SetupAction()
            {
                Action = new LoadQuantityTypesInPacketFinishedAction(ExpectedState.QuantityTypesInPacket);
            }
        }
    }

    public class OnLoadActiveStoresFinished
    {
        private readonly OnLoadActiveStoresFinishedFixture _fixture;

        public OnLoadActiveStoresFinished()
        {
            _fixture = new OnLoadActiveStoresFinishedFixture();
        }

        [Fact]
        public void OnLoadActiveStoresFinished_WithValidData_ShouldSetStores()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemReducer.OnLoadActiveStoresFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnLoadActiveStoresFinishedFixture : ItemReducerFixture
        {
            public LoadActiveStoresFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Stores = new DomainTestBuilder<ActiveStores>().Create()
                };
            }

            public void SetupAction()
            {
                Action = new LoadActiveStoresFinishedAction(ExpectedState.Stores);
            }
        }
    }

    private abstract class ItemReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}