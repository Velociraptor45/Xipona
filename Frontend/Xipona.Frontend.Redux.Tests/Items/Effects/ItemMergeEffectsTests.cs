using Moq.Contrib.InOrder;
using RestEase;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.Effects;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Items.States.Merges;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.Items.Effects;

public class ItemMergeEffectsTests
{
    public class HandleEnterMergerAction
    {
        private readonly HandleEnterMergerActionFixture _fixture = new();
        
        [Fact]
        public async Task HandleEnterMergerAction_WithNoSelectedItems_ShouldNotChangePage()
        {
            // Arrange
            _fixture.SetupEmptySelectedItems();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleEnterMergerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }
        
        [Fact]
        public async Task HandleEnterMergerAction_WithItemNull_ShouldNotChangePage()
        {
            // Arrange
            _fixture.SetupItemNull();
            var queue = CallQueue.Create(_ => { });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleEnterMergerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }
        
        [Fact]
        public async Task HandleEnterMergerAction_WithSelected_ShouldChangeToExpectedPage()
        {
            // Arrange
            _fixture.SetupItems();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNavigatingToUrl();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleEnterMergerAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleEnterMergerActionFixture : ItemMergeEffectsFixture
        {
            private Guid[]? _ids;

            public void SetupItemNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = null
                    }
                };
            }
            
            public void SetupEmptySelectedItems()
            {
                State = State with
                {
                    Merge = State.Merge with
                    {
                        Selector = State.Merge.Selector with
                        {
                            SelectedItems = []
                        }
                    }
                };
            }

            public void SetupItems()
            {
                _ids = new DomainTestBuilder<Guid>().CreateMany(3).ToArray();
                State = State with
                {
                    Merge = State.Merge with
                    {
                        Selector = State.Merge.Selector with
                        {
                            SelectedItems =
                            [
                                new DomainTestBuilder<MergeItemSearchResult>().Create() with { Id = _ids[0] },
                                new DomainTestBuilder<MergeItemSearchResult>().Create() with { Id = _ids[1] },
                                new DomainTestBuilder<MergeItemSearchResult>().Create() with { Id = _ids[2] },
                            ]
                        }
                    }
                };
            }

            public void SetupNavigatingToUrl()
            {
                TestPropertyNotSetException.ThrowIfNull(_ids);
                var uri = $"/items/merge?itemId={_ids[0]}&itemId={_ids[1]}&itemId={_ids[2]}&itemId={State.Editor.Item!.Id}";
                
                NavigationManagerMock.SetupNavigateTo(uri);
            }
        }
    }

    public class HandleInitializeMergingAction
    {
        private readonly HandleInitializeMergingActionFixture _fixture = new();

        [Fact]
        public async Task HandleInitializeMergingAction_WithDistinctItemIds_ShouldLoadAllItems()
        {
            // Arrange
            _fixture.SetupExpectedItems();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingItems();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleInitializeMergingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeMergingAction_WithDuplicatedItemIds_ShouldLoadAllDistinctItems()
        {
            // Arrange
            _fixture.SetupExpectedItems();
            _fixture.SetupActionWithDuplicatedId();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingItems();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleInitializeMergingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeMergingAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupExpectedItems();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingItemsFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleInitializeMergingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleInitializeMergingAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupExpectedItems();
            _fixture.SetupAction();

            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupLoadingItemsFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleInitializeMergingAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleInitializeMergingActionFixture : ItemMergeEffectsFixture
        {
            private List<EditedItem>? _expectedItems;
            private List<Guid>? _itemIds;
            public InitializeMergingAction? Action { get; private set; }

            public void SetupExpectedItems()
            {
                _expectedItems = new DomainTestBuilder<EditedItem>().CreateMany(3).ToList();
                _itemIds = _expectedItems.Select(i => i.Id).ToList();
            }
            
            public void SetupLoadingItems()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedItems);
                foreach (var expectedItem in _expectedItems)
                {
                    ApiClientMock.SetupGetItemByIdAsync(expectedItem.Id, expectedItem);
                }
            }
            
            public void SetupLoadingItemsFailedWithErrorInApi()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedItems);
                for (var i = 0; i < _expectedItems.Count; i++)
                {
                    var item =  _expectedItems[i];
                    if (i == _expectedItems.Count - 1)
                    {
                        ApiClientMock.SetupGetItemByIdAsyncThrowing(item.Id,
                            new DomainTestBuilder<ApiException>().Create());
                        continue;
                    }
                    ApiClientMock.SetupGetItemByIdAsync(item.Id, item);
                }
            }
            
            public void SetupLoadingItemsFailedWithErrorWhileTransmittingRequest()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedItems);
                for (var i = 0; i < _expectedItems.Count; i++)
                {
                    var item =  _expectedItems[i];
                    if (i == 0)
                    {
                        ApiClientMock.SetupGetItemByIdAsyncThrowing(item.Id,
                            new DomainTestBuilder<HttpRequestException>().Create());
                        continue;
                    }
                    ApiClientMock.SetupGetItemByIdAsync(item.Id, item);
                }
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_expectedItems);
                DispatcherMock.SetupDispatch(new InitializeMergingFinishedAction(_expectedItems));
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemIds);
                Action = new InitializeMergingAction(_itemIds);
            }

            public void SetupActionWithDuplicatedId()
            {
                TestPropertyNotSetException.ThrowIfNull(_itemIds);

                var ids = _itemIds.ToList();
                _itemIds.Add(_itemIds[1]);
                
                Action = new InitializeMergingAction(ids);
            }
        }
    }

    public class HandleMergeItemsAction
    {
        private readonly HandleMergeItemsActionFixture _fixture = new();

        [Fact]
        public async Task HandleMergeItemsAction_WithItemNull_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupNoValidationErrors();
            _fixture.SetupItemNull();
            var queue = CallQueue.Create(_ => {});
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMergeItemsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMergeItemsAction_WithValidationErrors_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupValidationErrors();
            var queue = CallQueue.Create(_ => {});
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMergeItemsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMergeItemsAction_WithMergingSuccessful_ShouldDispatchExpectedActions()
        {
            // Arrange
            _fixture.SetupNoValidationErrors();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMergingItemSuccessfully();
                _fixture.SetupDispatchingFinishedAction();
                _fixture.SetupDispatchingLeaveMergerAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMergeItemsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMergeItemsAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            _fixture.SetupNoValidationErrors();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMergingItemFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMergeItemsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleMergeItemsAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            _fixture.SetupNoValidationErrors();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupMergingItemFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleMergeItemsAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleMergeItemsActionFixture : ItemMergeEffectsFixture
        {
            private Guid? _newItemId;

            public void SetupNoValidationErrors()
            {
                State = State with
                {
                    Merge = State.Merge with
                    {
                        ValidationResult = new()
                    }
                };
            }

            public void SetupValidationErrors()
            {
                State = State with
                {
                    Merge = State.Merge with
                    {
                        ValidationResult = new(
                            new DomainTestBuilder<string>().Create(),
                            new DomainTestBuilder<Dictionary<Guid, string>>().Create())
                    }
                };
            }
            
            public void SetupItemNull()
            {
                State = State with
                {
                    Merge = State.Merge with
                    {
                        Item = null
                    }
                };
            }
            
            public void SetupDispatchingStartedAction()
            {
                DispatcherMock.SetupDispatchAny<MergeItemsStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                DispatcherMock.SetupDispatchAny<MergeItemsFinishedAction>();
            }

            public void SetupDispatchingLeaveMergerAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_newItemId);
                DispatcherMock.SetupDispatch(new LeaveItemMergerAction(_newItemId.Value));
            }

            public void SetupMergingItemSuccessfully()
            {
                _newItemId = Guid.NewGuid();
                ApiClientMock.SetupMergeItemsAsync(State.Merge.Item!, _newItemId.Value);
            }

            public void SetupMergingItemFailedWithErrorInApi()
            {
                ApiClientMock.SetupMergeItemsAsyncThrowing(State.Merge.Item!,
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupMergingItemFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupMergeItemsAsyncThrowing(State.Merge.Item!,
                    new DomainTestBuilder<HttpRequestException>().Create());
            }
        }
    }

    public class HandleLeaveItemMergerAction
    {
        private readonly HandleLeaveItemMergerActionFixture _fixture = new();

        [Fact]
        public async Task HandleLeaveItemMergerAction_WithNewItem_ShouldChangeToNewItemPage()
        {
            // Arrange
            _fixture.SetupNewItemId();
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNavigatingToNewItemPage();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLeaveItemMergerAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLeaveItemMergerAction_WithNoNewItem_NoItemSet_ShouldChangeToBaseItemPage()
        {
            // Arrange
            _fixture.SetupNewItemIdNull();
            _fixture.SetupItemNull();
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNavigatingToBaseItemPage();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLeaveItemMergerAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleLeaveItemMergerAction_WithNoNewItem_ItemSet_ShouldChangeToItemPage()
        {
            // Arrange
            _fixture.SetupNewItemIdNull();
            _fixture.SetupAction();
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupNavigatingToItemPage();
            });
            var sut = _fixture.CreateSut();
            
            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            await sut.HandleLeaveItemMergerAction(_fixture.Action, _fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleLeaveItemMergerActionFixture : ItemMergeEffectsFixture
        {
            private Guid? _newItemId;
            public LeaveItemMergerAction? Action { get; private set; }

            public void SetupNewItemId()
            {
                _newItemId = Guid.NewGuid();
            }
            
            public void SetupNewItemIdNull()
            {
                _newItemId = null;
            }
            
            public void SetupItemNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = null
                    }
                };
            }

            public void SetupAction()
            {
                Action = new LeaveItemMergerAction(_newItemId);
            }


            public void SetupNavigatingToBaseItemPage()
            {
                NavigationManagerMock.SetupNavigateTo("items");
            }

            public void SetupNavigatingToItemPage()
            {
                NavigationManagerMock.SetupNavigateTo($"items/{State.Editor.Item!.Id}");
            }

            public void SetupNavigatingToNewItemPage()
            {
                TestPropertyNotSetException.ThrowIfNull(_newItemId);
                NavigationManagerMock.SetupNavigateTo($"items/{_newItemId.Value}");
            }
        }
    }
    
    public class HandleOpenMergeItemSelectorAction
    {
        private readonly HandleOpenMergeItemSelectorActionFixture _fixture = new();

        [Fact]
        public async Task HandleOpenMergeItemSelectorAction_WithItemNull_ShouldNotDispatchAnything()
        {
            // Arrange
            _fixture.SetupItemNull();
            var queue = CallQueue.Create(_ => {});
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenMergeItemSelectorAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenMergeItemSelectorAction_WithSearchingSuccessful_ShouldDispatchExpectedActions()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchingForItemsSuccessfully();
                _fixture.SetupDispatchingFinishedAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenMergeItemSelectorAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenMergeItemSelectorAction_WithApiException_ShouldDispatchExceptionNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchingForItemsFailedWithErrorInApi();
                _fixture.SetupDispatchingExceptionNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenMergeItemSelectorAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        [Fact]
        public async Task HandleOpenMergeItemSelectorAction_WithHttpRequestException_ShouldDispatchErrorNotification()
        {
            // Arrange
            var queue = CallQueue.Create(_ =>
            {
                _fixture.SetupDispatchingStartedAction();
                _fixture.SetupSearchingForItemsFailedWithErrorWhileTransmittingRequest();
                _fixture.SetupDispatchingErrorNotificationAction();
            });
            var sut = _fixture.CreateSut();

            // Act
            await sut.HandleOpenMergeItemSelectorAction(_fixture.DispatcherMock.Object);

            // Assert
            queue.VerifyOrder();
        }

        private sealed class HandleOpenMergeItemSelectorActionFixture : ItemMergeEffectsFixture
        {
            private List<MergeItemSearchResult>? _searchResults;
            
            public void SetupItemNull()
            {
                State = State with
                {
                    Editor = State.Editor with
                    {
                        Item = null
                    }
                };
            }
            
            public void SetupDispatchingStartedAction()
            {
                DispatcherMock.SetupDispatchAny<SearchItemsForMergeStartedAction>();
            }

            public void SetupDispatchingFinishedAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_searchResults);
                DispatcherMock.SetupDispatch(new SearchItemsForMergeFinishedAction(_searchResults));
            }

            public void SetupSearchingForItemsSuccessfully()
            {
                _searchResults = new DomainTestBuilder<MergeItemSearchResult>().CreateMany(3).ToList();
                ApiClientMock.SetupSearchItemsForMergeAsync(State.Editor.Item!, [State.Editor.Item!.Id], _searchResults);
            }

            public void SetupSearchingForItemsFailedWithErrorInApi()
            {
                ApiClientMock.SetupSearchItemsForMergeAsyncThrowing(State.Editor.Item!, [State.Editor.Item!.Id],
                    new DomainTestBuilder<ApiException>().Create());
            }

            public void SetupSearchingForItemsFailedWithErrorWhileTransmittingRequest()
            {
                ApiClientMock.SetupSearchItemsForMergeAsyncThrowing(State.Editor.Item!, [State.Editor.Item!.Id],
                    new DomainTestBuilder<HttpRequestException>().Create());
            }
        }
    }
    
    private abstract class ItemMergeEffectsFixture : ItemEffectsFixtureBase
    {
        public ItemMergeEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemMergeEffects(ApiClientMock.Object, ItemStateMock.Object, NavigationManagerMock.Object);
        }
    }
}