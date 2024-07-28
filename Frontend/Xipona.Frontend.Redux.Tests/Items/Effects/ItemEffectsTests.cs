using Moq.Contrib.InOrder;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions;
using ProjectHermes.Xipona.Frontend.Redux.Items.Actions.Search;
using ProjectHermes.Xipona.Frontend.Redux.Items.Effects;
using ProjectHermes.Xipona.Frontend.Redux.Items.States;
using ProjectHermes.Xipona.Frontend.Redux.Shared.States;
using ProjectHermes.Xipona.Frontend.Redux.TestKit.Common;
using ProjectHermes.Xipona.Frontend.TestTools.Exceptions;
using RestEase;

namespace ProjectHermes.Xipona.Frontend.Redux.Tests.Items.Effects;

public class ItemEffectsTests
{
    public class HandleEnterItemSearchPageAction
    {
        private readonly HandleEnterItemSearchPageActionFixture _fixture = new();

        [Fact]
        public async Task HandleEnterItemSearchPageAction_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingLoadingQuantityTypes();
                _fixture.SetupDispatchingLoadingQuantityTypesInPacket();
                _fixture.SetupDispatchingLoadingActiveStores();
            });

            // Act
            await ItemEffects.HandleEnterItemSearchPageAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleEnterItemSearchPageActionFixture : ItemEffectsFixture
        {
            public void SetupDispatchingLoadingQuantityTypes()
            {
                SetupDispatchingAction<LoadQuantityTypesAction>();
            }

            public void SetupDispatchingLoadingQuantityTypesInPacket()
            {
                SetupDispatchingAction<LoadQuantityTypesInPacketAction>();
            }

            public void SetupDispatchingLoadingActiveStores()
            {
                SetupDispatchingAction<LoadActiveStoresAction>();
            }
        }
    }

    public class HandleRetrieveSearchResultCountAction
    {
        private readonly HandleRetrieveSearchResultCountActionFixture _fixture = new();

        [Fact]
        public async Task
            HandleRetrieveSearchResultCountAction_WithSearchInputEmpty_ShouldDispatchDefaultFinishedAndPageChangeAction()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupDispatchingFinishActionWithZeroResults();
                _fixture.SetupDispatchingPageChangeActionWithPageOne();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRetrieveSearchResultCountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleRetrieveSearchResultCountAction_WithSearchInput_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingTotalSearchResultCount();
                _fixture.SetupDispatchingFinishAction();
                _fixture.SetupDispatchingPageChangeActionWithPageOne();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRetrieveSearchResultCountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleRetrieveSearchResultCountAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingTotalSearchResultCountFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRetrieveSearchResultCountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleRetrieveSearchResultCountAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartAction();
                _fixture.SetupGettingTotalSearchResultCountFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleRetrieveSearchResultCountAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleRetrieveSearchResultCountActionFixture : ItemEffectsFixture
        {
            private string? _searchInput;
            private int? _totalSearchResultCount;

            public void SetupSearchInput()
            {
                _searchInput = new DomainTestBuilder<string>().Create();
                State = State with
                {
                    Search = State.Search with
                    {
                        Input = _searchInput
                    }
                };
            }

            public void SetupSearchInputEmpty()
            {
                _searchInput = string.Empty;
                State = State with
                {
                    Search = State.Search with
                    {
                        Input = _searchInput
                    }
                };
            }

            public void SetupGettingTotalSearchResultCount()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                _totalSearchResultCount = new DomainTestBuilder<int>().Create();
                ApiClientMock.SetupGetTotalSearchResultCountAsync(_searchInput, _totalSearchResultCount.Value);
            }

            public void SetupGettingTotalSearchResultCountFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                ApiClientMock.SetupGetTotalSearchResultCountAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupGettingTotalSearchResultCountFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                ApiClientMock.SetupGetTotalSearchResultCountAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingStartAction()
            {
                SetupDispatchingAction<RetrieveSearchResultCountStartedAction>();
            }

            public void SetupDispatchingFinishAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_totalSearchResultCount);
                SetupDispatchingAction(new RetrieveSearchResultCountFinishedAction(_totalSearchResultCount.Value));
            }

            public void SetupDispatchingFinishActionWithZeroResults()
            {
                SetupDispatchingAction(new RetrieveSearchResultCountFinishedAction(0));
            }

            public void SetupDispatchingPageChangeActionWithPageOne()
            {
                SetupDispatchingAction(new SearchPageChangedAction(1));
            }
        }
    }

    public class HandleSearchPageChangedAction
    {
        private readonly HandleSearchPageChangedActionFixture _fixture = new();

        [Fact]
        public async Task HandleSearchPageChangedAction_WithSearchInputEmpty_ShouldDispatchFinishedActionWithEmptyResult()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupTotalResultCountZero();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchPageChangedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchPageChangedAction_WithSearchInput_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupPageAndSize();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchSucceeded();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchPageChangedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchPageChangedAction_WithWithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupPageAndSize();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchPageChangedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleSearchPageChangedAction_WithWithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupPageAndSize();
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleSearchPageChangedAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleSearchPageChangedActionFixture : ItemEffectsFixture
        {
            private string? _searchInput;
            private IReadOnlyCollection<ItemSearchResult>? _searchResult;
            private int? _page;
            private int? _pageSize;

            public void SetupSearchInput()
            {
                _searchInput = new DomainTestBuilder<string>().Create();
                State = State with
                {
                    Search = State.Search with
                    {
                        Input = _searchInput
                    }
                };
            }

            public void SetupTotalResultCountZero()
            {
                State = State with
                {
                    Search = State.Search with
                    {
                        TotalResultCount = 0
                    }
                };
            }

            public void SetupPageAndSize()
            {
                _page = new DomainTestBuilder<int>().Create();
                _pageSize = new DomainTestBuilder<int>().Create();

                State = State with
                {
                    Search = State.Search with
                    {
                        Page = _page.Value,
                        PageSize = _pageSize.Value
                    }
                };
            }

            public void SetupSearchResult()
            {
                _searchResult = new DomainTestBuilder<ItemSearchResult>().CreateMany(2).ToList();
            }

            public void SetupSearchResultEmpty()
            {
                _searchResult = [];
            }

            public void SetupSearchSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);
                TestPropertyNotSetException.ThrowIfNull(_page);
                TestPropertyNotSetException.ThrowIfNull(_pageSize);

                ApiClientMock.SetupSearchItemsAsync(_searchInput, _page.Value, _pageSize.Value, _searchResult);
            }

            public void SetupSearchFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_page);
                TestPropertyNotSetException.ThrowIfNull(_pageSize);

                ApiClientMock.SetupSearchItemsAsyncThrowing(_searchInput, _page.Value, _pageSize.Value,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_page);
                TestPropertyNotSetException.ThrowIfNull(_pageSize);

                ApiClientMock.SetupSearchItemsAsyncThrowing(_searchInput, _page.Value, _pageSize.Value,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchItemsFinishedAction(_searchResult));
            }

            public void SetupDispatchingStartedAction()
            {
                SetupDispatchingAction<SearchItemsStartedAction>();
            }
        }
    }

    public class HandleLoadQuantityTypesAction
    {
        private readonly HandleLoadQuantityTypesActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithQuantityTypesAlreadyLoaded_ShouldNotLoadQuantityTypes()
        {
            // Arrange
            _fixture.SetupQuantityTypesAlreadyLoaded();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithQuantityTypesNotLoaded_ShouldLoadQuantityTypes()
        {
            // Arrange
            _fixture.SetupQuantityTypesNotLoaded();
            _fixture.SetupQuantityTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesSucceeded();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupQuantityTypesNotLoaded();
            _fixture.SetupQuantityTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupQuantityTypesNotLoaded();
            _fixture.SetupQuantityTypes();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadQuantityTypesActionFixture : ItemEffectsFixture
        {
            private IReadOnlyCollection<QuantityType>? _quantityTypes;

            public void SetupQuantityTypesAlreadyLoaded()
            {
                State = State with
                {
                    QuantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList()
                };
            }

            public void SetupQuantityTypesNotLoaded()
            {
                State = State with
                {
                    QuantityTypes = new List<QuantityType>(0)
                };
            }

            public void SetupQuantityTypes()
            {
                _quantityTypes = new DomainTestBuilder<QuantityType>().CreateMany(2).ToList();
            }

            public void SetupLoadingQuantityTypesSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                ApiClientMock.SetupGetAllQuantityTypesAsync(_quantityTypes);
            }

            public void SetupLoadingQuantityTypesFailedWithErrorInApi()
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupLoadingQuantityTypesFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);

                SetupDispatchingAction(new LoadQuantityTypesFinishedAction(_quantityTypes));
            }
        }
    }

    public class HandleLoadQuantityTypesInPacketAction
    {
        private readonly HandleLoadQuantityTypesInPacketActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithQuantityTypesInPacketAlreadyLoaded_ShouldNotLoadQuantityTypesInPacket()
        {
            // Arrange
            _fixture.SetupQuantityTypesInPacketAlreadyLoaded();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithQuantityTypesInPacketNotLoaded_ShouldLoadQuantityTypesInPacket()
        {
            // Arrange
            _fixture.SetupQuantityTypesInPacketNotLoaded();
            _fixture.SetupQuantityTypesInPacket();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketSucceeded();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupQuantityTypesInPacketNotLoaded();
            _fixture.SetupQuantityTypesInPacket();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadQuantityTypesInPacketAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupQuantityTypesInPacketNotLoaded();
            _fixture.SetupQuantityTypesInPacket();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadQuantityTypesInPacketAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadQuantityTypesInPacketActionFixture : ItemEffectsFixture
        {
            private IReadOnlyCollection<QuantityTypeInPacket>? _quantityTypes;

            public void SetupQuantityTypesInPacketAlreadyLoaded()
            {
                State = State with
                {
                    QuantityTypesInPacket = new DomainTestBuilder<QuantityTypeInPacket>().CreateMany(2).ToList()
                };
            }

            public void SetupQuantityTypesInPacketNotLoaded()
            {
                State = State with
                {
                    QuantityTypesInPacket = new List<QuantityTypeInPacket>(0)
                };
            }

            public void SetupQuantityTypesInPacket()
            {
                _quantityTypes = new DomainTestBuilder<QuantityTypeInPacket>().CreateMany(2).ToList();
            }

            public void SetupLoadingQuantityTypesInPacketSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsync(_quantityTypes);
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorInApi()
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);

                SetupDispatchingAction(new LoadQuantityTypesInPacketFinishedAction(_quantityTypes));
            }
        }
    }

    public class HandleLoadActiveStoresAction
    {
        private readonly HandleLoadActiveStoresActionFixture _fixture = new();

        [Fact]
        public async Task HandleLoadActiveStoresAction_WithStoresAlreadyLoaded_ShouldNotLoadActiveStores()
        {
            // Arrange
            _fixture.SetupStoresAlreadyLoaded();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresAction_WithStoresNotLoaded_ShouldLoadActiveStores()
        {
            // Arrange
            _fixture.SetupStoresNotLoaded();
            _fixture.SetupStores();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketSucceeded();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupStoresNotLoaded();
            _fixture.SetupStores();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLoadActiveStoresAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupStoresNotLoaded();
            _fixture.SetupStores();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleLoadActiveStoresAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLoadActiveStoresActionFixture : ItemEffectsFixture
        {
            private IReadOnlyCollection<ItemStore>? _stores;

            public void SetupStoresAlreadyLoaded()
            {
                State = State with
                {
                    Stores = State.Stores with
                    {
                        Stores = new DomainTestBuilder<ItemStore>().CreateMany(2).ToList()
                    }
                };
            }

            public void SetupStoresNotLoaded()
            {
                State = State with
                {
                    Stores = State.Stores with
                    {
                        Stores = new List<ItemStore>(0)
                    }
                };
            }

            public void SetupStores()
            {
                _stores = new DomainTestBuilder<ItemStore>().CreateMany(2).ToList();
            }

            public void SetupLoadingQuantityTypesInPacketSucceeded()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);
                ApiClientMock.SetupGetAllActiveStoresForItemAsync(_stores);
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorInApi()
            {
                ApiClientMock.SetupGetAllActiveStoresForItemAsyncThrowing(new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupGetAllActiveStoresForItemAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create());
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                SetupDispatchingAction(new LoadActiveStoresFinishedAction(new ActiveStores(_stores)));
            }
        }
    }

    private abstract class ItemEffectsFixture : ItemEffectsFixtureBase
    {
        public ItemEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemEffects(ApiClientMock.Object, NavigationManagerMock.Object, ItemStateMock.Object);
        }
    }
}