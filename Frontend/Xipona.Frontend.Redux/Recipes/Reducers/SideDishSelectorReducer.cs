﻿using Fluxor;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.Actions.Editor.SideDishes;
using ProjectHermes.Xipona.Frontend.Redux.Recipes.States;

namespace ProjectHermes.Xipona.Frontend.Redux.Recipes.Reducers;
public static class SideDishSelectorReducer
{
    [ReducerMethod]
    public static RecipeState OnSideDishInputChanged(RecipeState state, SideDishInputChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                SideDishSelector = state.Editor.SideDishSelector with
                {
                    Input = action.Input
                },
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSideDishDropdownClosed(RecipeState state, SideDishDropdownClosedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                SideDishSelector = state.Editor.SideDishSelector with
                {
                    Input = string.Empty
                },
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSideDishChanged(RecipeState state, SideDishChangedAction action)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    SideDish = action.SideDish
                },
                SideDishSelector = state.Editor.SideDishSelector with
                {
                    SideDishes = [action.SideDish],
                    Input = string.Empty
                }
            }
        };
    }

    [ReducerMethod(typeof(SideDishClearedAction))]
    public static RecipeState OnSideDishCleared(RecipeState state)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    SideDish = null
                },
                SideDishSelector = state.Editor.SideDishSelector with
                {
                    SideDishes = [],
                    Input = string.Empty
                }
            }
        };
    }

    [ReducerMethod(typeof(SideDishRemovedAction))]
    public static RecipeState OnSideDishRemoved(RecipeState state)
    {
        if (state.Editor.Recipe is null)
            return state;

        return state with
        {
            Editor = state.Editor with
            {
                Recipe = state.Editor.Recipe with
                {
                    SideDish = null
                },
                SideDishSelector = state.Editor.SideDishSelector with
                {
                    SideDishes = [],
                    Input = string.Empty
                }
            }
        };
    }

    [ReducerMethod]
    public static RecipeState OnSearchSideDishesFinished(RecipeState state, SearchSideDishesFinishedAction action)
    {
        return state with
        {
            Editor = state.Editor with
            {
                SideDishSelector = state.Editor.SideDishSelector with
                {
                    SideDishes = action.SideDishes.Select(s => new SideDish(s.Id, s.Name)).ToList()
                }
            }
        };
    }
}
