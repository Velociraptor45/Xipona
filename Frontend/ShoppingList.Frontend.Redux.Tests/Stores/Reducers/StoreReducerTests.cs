using FluentAssertions;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Actions;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.Reducers;
using ProjectHermes.ShoppingList.Frontend.Redux.Stores.States;
using ProjectHermes.ShoppingList.Frontend.Redux.TestKit.Common;
using ProjectHermes.ShoppingList.Frontend.TestTools.Exceptions;

namespace ProjectHermes.ShoppingList.Frontend.Redux.Tests.Stores.Reducers;

public class StoreReducerTests
{
    public class OnLoadStoresOverviewFinished
    {
        private readonly OnLoadStoresOverviewFinishedFixture _fixture;

        public OnLoadStoresOverviewFinished()
        {
            _fixture = new OnLoadStoresOverviewFinishedFixture();
        }

        [Fact]
        public void OnLoadStoresOverviewFinished_WithValidData_ShouldSetSearchResults()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = StoreReducer.OnLoadStoresOverviewFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState, opt => opt.WithStrictOrdering());
        }

        private sealed class OnLoadStoresOverviewFinishedFixture : StoreReducerFixture
        {
            public LoadStoresOverviewFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    SearchResults = new DomainTestBuilder<StoreSearchResult>().CreateMany(2).ToList()
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchResults = new List<StoreSearchResult>
                    {
                        new DomainTestBuilder<StoreSearchResult>()
                            .FillPropertyWith(r => r.Name, $"A{new DomainTestBuilder<string>().Create()}")
                            .Create(),
                        new DomainTestBuilder<StoreSearchResult>()
                            .FillPropertyWith(r => r.Name, $"B{new DomainTestBuilder<string>().Create()}")
                            .Create(),
                        new DomainTestBuilder<StoreSearchResult>()
                            .FillPropertyWith(r => r.Name, $"Z{new DomainTestBuilder<string>().Create()}")
                            .Create()
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LoadStoresOverviewFinishedAction(ExpectedState.SearchResults.Reverse().ToList());
            }
        }
    }

    public class OnStorePageInitialized
    {
        private readonly OnStorePageInitializedFixture _fixture;

        public OnStorePageInitialized()
        {
            _fixture = new OnStorePageInitializedFixture();
        }

        [Fact]
        public void OnStorePageInitialized_WithNotSaving_ShouldClearStateAndSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialNotSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreReducer.OnStorePageInitialized(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnStorePageInitialized_WithSaving_ShouldClearStateAndSetNotSaving()
        {
            // Arrange
            _fixture.SetupInitialSaving();
            _fixture.SetupExpectedState();

            // Act
            var result = StoreReducer.OnStorePageInitialized(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnStorePageInitializedFixture : StoreReducerFixture
        {
            public void SetupInitialNotSaving()
            {
                SetupInitialState(false);
            }

            public void SetupInitialSaving()
            {
                SetupInitialState(true);
            }

            private void SetupInitialState(bool isSaving)
            {
                InitialState = ExpectedState with
                {
                    SearchResults = new DomainTestBuilder<StoreSearchResult>().CreateMany(2).ToList(),
                    Editor = ExpectedState.Editor with
                    {
                        Store = new DomainTestBuilder<EditedStore>().Create(),
                        IsSaving = isSaving
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    SearchResults = new List<StoreSearchResult>(),
                    Editor = ExpectedState.Editor with
                    {
                        Store = null,
                        IsSaving = false
                    }
                };
            }
        }
    }

    private abstract class StoreReducerFixture
    {
        public StoreState ExpectedState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
        public StoreState InitialState { get; protected set; } = new DomainTestBuilder<StoreState>().Create();
    }
}