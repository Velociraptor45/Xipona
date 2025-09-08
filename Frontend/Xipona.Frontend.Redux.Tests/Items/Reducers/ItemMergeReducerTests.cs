using FluentAssertions;
using System.Text.RegularExpressions;
using Xipona.Frontend.Redux.Items.Actions.Merges;
using Xipona.Frontend.Redux.Items.Reducers;
using Xipona.Frontend.Redux.Items.States;
using Xipona.Frontend.Redux.Items.States.Merges;
using Xipona.Frontend.Redux.TestKit.Common;
using Xipona.Frontend.Redux.TestKit.Common.Extensions;
using Xipona.Frontend.TestTools.Exceptions;

namespace Xipona.Frontend.Redux.Tests.Items.Reducers;
public class ItemMergeReducerTests
{
    public class OnInitializeMergingFinished
    {
        private readonly OnInitializeMergingFinishedFixture _fixture = new();

        [Fact]
        public void OnInitializeMergingFinished_WithItemsHavingDifferentNameStart_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedStateWithDifferentNameStart();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnInitializeMergingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => Regex.IsMatch(info.Path, @"^Merge\.Item\.Types\[\d\]\.Key$")));
        }

        [Fact]
        public void OnInitializeMergingFinished_WithItemsSharingNameStart_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedStateWithSharedNameStart();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnInitializeMergingFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState,
                opt => opt.Excluding(info => Regex.IsMatch(info.Path, @"^Merge\.Item\.Types\[\d\]\.Key$")));
        }

        private class OnInitializeMergingFinishedFixture : ItemMergeReducerFixture
        {
            public InitializeMergingFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedStateWithDifferentNameStart()
            {
                var names = new DomainTestBuilder<string>().CreateMany(3).ToList();
                EditedItem[] items =
                [
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"A {names[0]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"B {names[1]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"C {names[2]}" }
                ];

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = string.Empty,
                            Types =
                            [
                                new MergedItemType(Guid.NewGuid(), items[0], items[0].Name),
                                new MergedItemType(Guid.NewGuid(), items[1], items[1].Name),
                                new MergedItemType(Guid.NewGuid(), items[2], items[2].Name)
                            ]
                        },
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedStateWithSharedNameStart()
            {
                var start = new DomainTestBuilder<string>().Create();
                var trimmedNames = new DomainTestBuilder<string>().CreateMany(3).ToList();
                EditedItem[] items =
                [
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"{start} {trimmedNames[0]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"{start} {trimmedNames[1]}" },
                    new DomainTestBuilder<EditedItem>().Create() with { Name = $"{start} {trimmedNames[2]}" }
                ];

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = start,
                            Types =
                            [
                                new MergedItemType(Guid.NewGuid(), items[0], trimmedNames[0]),
                                new MergedItemType(Guid.NewGuid(), items[1], trimmedNames[1]),
                                new MergedItemType(Guid.NewGuid(), items[2], trimmedNames[2])
                            ]
                        },
                        IsSaving = false
                    }
                };
            }

            public void SetupAction()
            {
                Action = new InitializeMergingFinishedAction(
                    ExpectedState.Merge.Item!.Types.Select(t => t.OriginalItem).ToList());
            }

        }
    }

    public class OnItemTypeNameChanged
    {
        private readonly OnItemTypeNameChangedFixture _fixture = new();

        [Fact]
        public void OnItemTypeNameChanged_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SelectType();
            _fixture.SetupInitialStateWithExistingValidationError();
            _fixture.SetupExpectedResult();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeNameChanged_WithNameErrorExisting_ShouldClearNameError()
        {
            // Arrange
            _fixture.SelectType();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedResult();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeNameChanged_WithEmptyName_ShouldSetNameError()
        {
            // Arrange
            _fixture.SelectType();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedResultWithNameValidationError();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemTypeNameChanged_WithInvalidKey_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SelectType();
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupActionWithInvalidKey();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemTypeNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnItemTypeNameChangedFixture : ItemMergeReducerFixture
        {
            public ItemTypeNameChangedAction? Action { get; private set; }
            private MergedItemType? _type;
            private int? _typeIndex;

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupInitialStateWithExistingValidationError()
            {
                TestPropertyNotSetException.ThrowIfNull(_typeIndex);
                var types = ExpectedState.Merge.Item!.Types.ToList();
                var type = types[_typeIndex.Value];
                var typeNameValidations = ExpectedState.Merge.ValidationResult.TypeNames.ToDictionary();
                typeNameValidations[type.Key] = "Name must not be empty";

                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        ValidationResult = ExpectedState.Merge.ValidationResult with
                        {
                            TypeNames = typeNameValidations
                        }
                    }
                };
            }

            public void SetupExpectedResult()
            {
                var types = GetRandomType(new DomainTestBuilder<string>().Create());

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Types = types
                        }
                    }
                };
            }

            public void SetupExpectedResultWithNameValidationError()
            {
                var types = GetRandomType(string.Empty);

                var typeNameValidations = ExpectedState.Merge.ValidationResult.TypeNames.ToDictionary();
                typeNameValidations[_type!.Key] = "Name must not be empty";

                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with { Types = types },
                        ValidationResult = ExpectedState.Merge.ValidationResult with
                        {
                            TypeNames = typeNameValidations
                        }
                    }
                };
            }

            private List<MergedItemType> GetRandomType(string name)
            {
                TestPropertyNotSetException.ThrowIfNull(_typeIndex);
                var types = ExpectedState.Merge.Item!.Types.ToList();
                types[_typeIndex.Value] = types[_typeIndex.Value] with
                {
                    Name = name
                };
                _type = types[_typeIndex.Value];
                return types;
            }

            public void SelectType()
            {
                var types = ExpectedState.Merge.Item!.Types.ToList();
                (_, _typeIndex) = types.Random();
            }

            public void SetupAction()
            {
                TestPropertyNotSetException.ThrowIfNull(_type);
                Action = new ItemTypeNameChangedAction(_type.Key, _type.Name);
            }

            public void SetupActionWithInvalidKey()
            {
                Action = new ItemTypeNameChangedAction(Guid.NewGuid(), "New Name");
            }
        }
    }

    public class OnItemNameChanged
    {
        private readonly OnItemNameChangedFixture _fixture = new();

        [Fact]
        public void OnItemNameChanged_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedResult();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnItemNameChanged_WithNameEmpty_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialStateEqualsExpectedState();
            _fixture.SetupExpectedResultWithNameValidatorError();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);

            // Act
            var result = ItemMergeReducer.OnItemNameChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private class OnItemNameChangedFixture : ItemMergeReducerFixture
        {
            public ItemNameChangedAction? Action { get; private set; }

            public void SetupInitialStateEqualsExpectedState()
            {
                InitialState = ExpectedState;
            }

            public void SetupExpectedResult()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = new DomainTestBuilder<string>().Create()
                        },
                        ValidationResult = ExpectedState.Merge.ValidationResult with
                        {
                            Name = null
                        }
                    }
                };
            }

            public void SetupExpectedResultWithNameValidatorError()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = string.Empty
                        },
                        ValidationResult = ExpectedState.Merge.ValidationResult with
                        {
                            Name = "Name must not be empty"
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new ItemNameChangedAction(ExpectedState.Merge.Item!.Name);
            }
        }
    }

    public class OnOpenMergeItemSelector
    {
        private readonly OnOpenMergeItemSelectorFixture _fixture = new();

        [Fact]
        public void OnOpenMergeItemSelector_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnOpenMergeItemSelector(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        private sealed class OnOpenMergeItemSelectorFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsOpen = false,
                            IsSearching = true
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            SearchResults = [],
                            SelectedItems = [],
                            IsOpen = true,
                            IsSearching = false
                        }
                    }
                };
            }
        }
    }

    public class OnCloseMergeItemSelector
    {
        private readonly OnCloseMergeItemSelectorFixture _fixture = new();

        [Fact]
        public void OnCloseMergeItemSelector_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemMergeReducer.OnCloseMergeItemSelector(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        
        private sealed class OnCloseMergeItemSelectorFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsOpen = true
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsOpen = false
                        }
                    }
                };
            }
        }
    }

    public class OnSearchItemsForMergeStarted
    {
        private readonly OnSearchItemsForMergeStartedFixture _fixture = new();

        [Fact]
        public void OnSearchItemsForMergeStarted_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();

            // Act
            var result = ItemMergeReducer.OnSearchItemsForMergeStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnSearchItemsForMergeStartedFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsSearching = false
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsSearching = true
                        }
                    }
                };
            }
        }
    }
    
    public class OnSearchItemsForMergeFinished
    {
        private readonly OnSearchItemsForMergeFinishedFixture _fixture = new();

        [Fact]
        public void OnSearchItemsForMergeFinished_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            
            // Act
            var result = ItemMergeReducer.OnSearchItemsForMergeFinished(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnSearchItemsForMergeFinishedFixture : ItemMergeReducerFixture
        {
            public SearchItemsForMergeFinishedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsSearching = true,
                            SearchResults = new DomainTestBuilder<MergeItemSearchResult>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsSearching = false
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SearchItemsForMergeFinishedAction(ExpectedState.Merge.Selector.SearchResults);
            }
        }
    }
    
    public class OnSelectedMergeItemsChanged
    {
        private readonly OnSelectedMergeItemsChangedFixture _fixture = new();

        [Fact]
        public void OnSelectedMergeItemsChanged_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupAction();

            TestPropertyNotSetException.ThrowIfNull(_fixture.Action);
            
            // Act
            var result = ItemMergeReducer.OnSelectedMergeItemsChanged(_fixture.InitialState, _fixture.Action);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnSelectedMergeItemsChangedFixture : ItemMergeReducerFixture
        {
            public SelectedMergeItemsChangedAction? Action { get; private set; }

            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            SelectedItems = new DomainTestBuilder<MergeItemSearchResult>().CreateMany(2).ToList()
                        }
                    }
                };
            }

            public void SetupAction()
            {
                Action = new SelectedMergeItemsChangedAction(ExpectedState.Merge.Selector.SelectedItems);
            }
        }
    }
    
    public class OnInitializeMerging
    {
        private readonly OnInitializeMergingFixture _fixture = new();

        [Fact]
        public void OnInitializeMerging_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnInitializeMerging(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnInitializeMergingFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            SearchResults = new DomainTestBuilder<MergeItemSearchResult>().CreateMany(2).ToList()
                        },
                        ValidationResult = new ItemMergeValidationResult(
                            new DomainTestBuilder<string>().Create(),
                            new DomainTestBuilder<IReadOnlyDictionary<Guid, string>>().Create())
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            SelectedItems = [],
                            SearchResults = [],
                        },
                        ValidationResult = new()
                    }
                };
            }
        }
    }
    
    public class OnEnterMergerAction
    {
        private readonly OnEnterMergerActionFixture _fixture = new();

        [Fact]
        public void OnEnterMergerAction_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnEnterMergerAction(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnEnterMergerActionFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsOpen = true,
                            IsSearching = true
                        },
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Selector = ExpectedState.Merge.Selector with
                        {
                            IsOpen = false,
                            IsSearching = false
                        }
                    }
                };
            }
        }
    }
    
    public class OnMergeItemsStarted
    {
        private readonly OnMergeItemsStartedFixture _fixture = new();

        [Fact]
        public void OnMergeItemsStarted_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnMergeItemsStarted(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnMergeItemsStartedFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        IsSaving = false
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        IsSaving = true
                    }
                };
            }
        }
    }
    
    public class OnMergeItemsFinished
    {
        private readonly OnMergeItemsFinishedFixture _fixture = new();

        [Fact]
        public void OnMergeItemsFinished_WithValidData_ShouldReturnExpectedResult()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnMergeItemsFinished(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnMergeItemsFinishedFixture : ItemMergeReducerFixture
        {
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        IsSaving = true
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        IsSaving = false
                    }
                };
            }
        }
    }
    
    public class OnMergeItemsAction
    {
        private readonly OnMergeItemsActionFixture _fixture = new();

        [Fact]
        public void OnMergeItemsAction_WithItemNull_ShouldNotChangeAnything()
        {
            // Arrange
            _fixture.SetupExpectedStateWithItemNull();
            _fixture.SetupInitialStateEqualExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnMergeItemsAction(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMergeItemsAction_WithNoErrors_ShouldClearErrors()
        {
            // Arrange
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnMergeItemsAction(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMergeItemsAction_WithNameErrors_ShouldSetNameErrors()
        {
            // Arrange
            _fixture.SetupNameError();
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnMergeItemsAction(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }

        [Fact]
        public void OnMergeItemsAction_WithTypeNameErrors_ShouldSetTypeNameErrors()
        {
            // Arrange
            _fixture.SetupTypeNameError();
            _fixture.SetupInitialState();
            _fixture.SetupExpectedState();
            
            // Act
            var result = ItemMergeReducer.OnMergeItemsAction(_fixture.InitialState);

            // Assert
            result.Should().BeEquivalentTo(_fixture.ExpectedState);
        }
        
        private sealed class OnMergeItemsActionFixture : ItemMergeReducerFixture
        {
            private string? _nameError = null;
            private readonly Dictionary<Guid, string> _typeNameErrors = new();

            public void SetupInitialStateEqualExpectedState()
            {
                InitialState = ExpectedState;
            }
            
            public void SetupInitialState()
            {
                InitialState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        ValidationResult = new ItemMergeValidationResult(
                            new DomainTestBuilder<string>().Create(),
                            new DomainTestBuilder<IReadOnlyDictionary<Guid, string>>().Create())
                    }
                };
            }

            public void SetupNameError()
            {
                _nameError = "Name must not be empty";
                
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item! with
                        {
                            Name = string.Empty 
                        }
                    }
                };
            }

            public void SetupTypeNameError()
            {
                var types = ExpectedState.Merge.Item!.Types.ToList();
                for (var i = 0; i < types.Count; i++)
                {
                    types[i] = types[i] with
                    {
                        Name = string.Empty
                    };
                }
                
                foreach (var type in types)
                {
                    _typeNameErrors[type.Key] = "Name must not be empty";
                }
                
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = ExpectedState.Merge.Item with
                        {
                            Types = types 
                        }
                    }
                };
            }

            public void SetupExpectedStateWithItemNull()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        Item = null
                    }
                };
            }

            public void SetupExpectedState()
            {
                ExpectedState = ExpectedState with
                {
                    Merge = ExpectedState.Merge with
                    {
                        ValidationResult = new ItemMergeValidationResult(
                            _nameError,
                            _typeNameErrors)
                    }
                };
            }
        }
    }
    
    private abstract class ItemMergeReducerFixture
    {
        public ItemState ExpectedState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
        public ItemState InitialState { get; protected set; } = new DomainTestBuilder<ItemState>().Create();
    }
}
