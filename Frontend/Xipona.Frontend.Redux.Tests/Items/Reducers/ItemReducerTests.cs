using FluentAssertions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.Xipona.Frontend.Redux.Items.Reducers;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Reducers;

public class ItemReducerTests
{
    public class OnItemSearchInputChanged
    {
        private readonly OnItemSearchInputChangedFixture _fixture = new();

        [Fact]
        public void OnItemSearchInputChanged_WithSearchNotLoading_ShouldSetLoading()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemReducer.OnItemSearchInputChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnItemSearchInputChangedFixture : ItemReducerFixture
        {
            public ItemSearchInputChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Search = ExpectedState.Search with
                    {
                        Input = new DomainTestBuilder<string>().Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new ItemSearchInputChangedAction(ExpectedState.Search.Input);
            }
        }
    }

    public class OnSearchItemStarted
    {
        private readonly OnSearchItemStartedFixture _fixture = new();

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
        private readonly OnSearchItemFinishedFixture _fixture = new();

        [Fact]
        public void OnSearchItemFinished_WithSearchNotLoading_ShouldSetNotLoadingAndSortResults()
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
        public void OnSearchItemFinished_WithSearchLoading_ShouldSetNotLoadingAndSortResults()
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
        private readonly OnLoadQuantityTypesFinishedFixture _fixture = new();

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
        private readonly OnLoadQuantityTypesInPacketFinishedFixture _fixture = new();

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
        private readonly OnLoadActiveStoresFinishedFixture _fixture = new();

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

    public class OnSaveStoreFinished
    {
        private readonly OnSaveStoreFinishedFixture _fixture = new();

        [Fact]
        public void OnSaveStoreFinished_WithValidData_ShouldClearStores()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemReducer.OnSaveStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnSaveStoreFinishedFixture : ItemReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new DomainTestBuilder<ItemStore>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Stores = new ActiveStores(new List<ItemStore>(0))
                };
            }
        }
    }

    public class OnDeleteStoreFinished
    {
        private readonly OnDeleteStoreFinishedFixture _fixture = new();

        [Fact]
        public void OnDeleteStoreFinished_WithValidData_ShouldClearStores()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemReducer.OnDeleteStoreFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnDeleteStoreFinishedFixture : ItemReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Stores = ExpectedState.Stores with
                    {
                        Stores = new DomainTestBuilder<ItemStore>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Stores = new ActiveStores(new List<ItemStore>(0))
                };
            }
        }
    }

    private abstract class ItemReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}