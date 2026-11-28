using Moq.Contrib.InOrder;
using Xipona.Frontend.Redux.Items.Actions;
using Xipona.Frontend.Redux.Items.Actions.Search;
using Xipona.Frontend.Redux.Items.Effects;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Shared.States;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;
using RestEase;
using Xipona.Frontend.Redux.Shared.Actions.Settings;

namespace Xipona.Frontend.Redux.Tests.Items.Effects;

public class ItemEffectsTests
{
    public class HandleEnterItemSearchPageAction
    {
        private readonly HandleEnterItemSearchPageActionFixture _fixture = new();

        [Fact]
        public async Task HandleEnterItemSearchPageAction_ShouldDispatchActionsInCorrectOrder()
        {
            // Arrange
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupDispatchingLoadingQuantityTypes(x0);
                _fixture.SetupDispatchingLoadingQuantityTypesInPacket(x0);
                _fixture.SetupDispatchingLoadingActiveStores(x0);
                _fixture.SetupDispatchingLoadingGeneralSettings(x0);
            });

            // Act
            await ItemEffects.HandleEnterItemSearchPageAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleEnterItemSearchPageActionFixture : ItemEffectsFixture
        {
            public void SetupDispatchingLoadingQuantityTypes(IQueueComponent component)
            {
                SetupDispatchingAction<LoadQuantityTypesAction>(component);
            }

            public void SetupDispatchingLoadingQuantityTypesInPacket(IQueueComponent component)
            {
                SetupDispatchingAction<LoadQuantityTypesInPacketAction>(component);
            }

            public void SetupDispatchingLoadingActiveStores(IQueueComponent component)
            {
                SetupDispatchingAction<LoadActiveStoresAction>(component);
            }

            public void SetupDispatchingLoadingGeneralSettings(IQueueComponent component)
            {
                SetupDispatchingAction<LoadGeneralSettingsAction>(component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInputEmpty();
                _fixture.SetupDispatchingFinishActionWithZeroResults(x0);
                _fixture.SetupDispatchingPageChangeActionWithPageOne(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupGettingTotalSearchResultCount(x0);
                _fixture.SetupDispatchingFinishAction(x0);
                _fixture.SetupDispatchingPageChangeActionWithPageOne(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupGettingTotalSearchResultCountFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupDispatchingStartAction(x0);
                _fixture.SetupGettingTotalSearchResultCountFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupGettingTotalSearchResultCount(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                _totalSearchResultCount = new DomainTestBuilder<int>().Create();
                ApiClientMock.SetupGetTotalSearchResultCountAsync(_searchInput, _totalSearchResultCount.Value, component);
            }

            public void SetupGettingTotalSearchResultCountFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                ApiClientMock.SetupGetTotalSearchResultCountAsyncThrowing(_searchInput,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupGettingTotalSearchResultCountFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                ApiClientMock.SetupGetTotalSearchResultCountAsyncThrowing(_searchInput,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingStartAction(IQueueComponent component)
            {
                SetupDispatchingAction<RetrieveSearchResultCountStartedAction>(component);
            }

            public void SetupDispatchingFinishAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_totalSearchResultCount);
                SetupDispatchingAction(new RetrieveSearchResultCountFinishedAction(_totalSearchResultCount.Value), component);
            }

            public void SetupDispatchingFinishActionWithZeroResults(IQueueComponent component)
            {
                SetupDispatchingAction(new RetrieveSearchResultCountFinishedAction(0), component);
            }

            public void SetupDispatchingPageChangeActionWithPageOne(IQueueComponent component)
            {
                SetupDispatchingAction(new SearchPageChangedAction(1), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupTotalResultCountZero();
                _fixture.SetupSearchResultEmpty();
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupSearchResult();
                _fixture.SetupPageAndSize();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupPageAndSize();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupSearchInput();
                _fixture.SetupPageAndSize();
                _fixture.SetupDispatchingStartedAction(x0);
                _fixture.SetupSearchFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupSearchSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_searchResult);
                TestPropertyNotSetException.ThrowIfNull(_page);
                TestPropertyNotSetException.ThrowIfNull(_pageSize);

                ApiClientMock.SetupSearchItemsAsync(_searchInput, _page.Value, _pageSize.Value, _searchResult, component);
            }

            public void SetupSearchFailedWithErrorInApi(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_page);
                TestPropertyNotSetException.ThrowIfNull(_pageSize);

                ApiClientMock.SetupSearchItemsAsyncThrowing(_searchInput, _page.Value, _pageSize.Value,
                    new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupSearchFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchInput);
                TestPropertyNotSetException.ThrowIfNull(_page);
                TestPropertyNotSetException.ThrowIfNull(_pageSize);

                ApiClientMock.SetupSearchItemsAsyncThrowing(_searchInput, _page.Value, _pageSize.Value,
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResult);

                SetupDispatchingAction(new SearchItemsFinishedAction(_searchResult), component);
            }

            public void SetupDispatchingStartedAction(IQueueComponent component)
            {
                SetupDispatchingAction<SearchItemsStartedAction>(component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupLoadingQuantityTypesSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                ApiClientMock.SetupGetAllQuantityTypesAsync(_quantityTypes, component);
            }

            public void SetupLoadingQuantityTypesFailedWithErrorInApi(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupLoadingQuantityTypesFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);

                SetupDispatchingAction(new LoadQuantityTypesFinishedAction(_quantityTypes), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupLoadingQuantityTypesInPacketSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsync(_quantityTypes, component);
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorInApi(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllQuantityTypesInPacketAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_quantityTypes);

                SetupDispatchingAction(new LoadQuantityTypesInPacketFinishedAction(_quantityTypes), component);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketSucceeded(x0);
                _fixture.SetupDispatchingFinishedAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorInApi(x0);
                _fixture.SetupDispatchingExceptionNotificationAction(x0);
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
            var queue = CallQueue.Create(x0 =>
            {
                _fixture.SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest(x0);
                _fixture.SetupDispatchingErrorNotificationAction(x0);
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

            public void SetupLoadingQuantityTypesInPacketSucceeded(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);
                ApiClientMock.SetupGetAllActiveStoresForItemAsync(_stores, component);
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorInApi(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllActiveStoresForItemAsyncThrowing(new DomainTestBuilder<ApiException>().Create(), component);
            }

            public void SetupLoadingQuantityTypesInPacketFailedWithErrorWhileTransmittingRequest(IQueueComponent component)
            {
                ApiClientMock.SetupGetAllActiveStoresForItemAsyncThrowing(
                    new DomainTestBuilder<HttpRequestException>().Create(), component);
            }

            public void SetupDispatchingFinishedAction(IQueueComponent component)
            {
                TestPropertyNotSetException.ThrowIfNull(_stores);

                SetupDispatchingAction(new LoadActiveStoresFinishedAction(new ActiveStores(_stores)), component);
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