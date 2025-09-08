using Microsoft.AspNetCore.Components;
using Moq.Contrib.InOrder;
using Moq.Contrib.InOrder.Extensions;
using Moq.Protected;
using Xipona.Frontend.Redux.Items.Effects;
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

    private abstract class ItemMergeEffectsFixture : ItemEffectsFixtureBase
    {
        public ItemMergeEffects CreateSut()
        {
            SetupStateReturningState();
            return new ItemMergeEffects(ApiClientMock.Object, ItemStateMock.Object, NavigationManagerMock.Object);
        }
    }
}