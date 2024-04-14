﻿using ProjectHermes.Xipona.Api.Domain.Recipes.Models;

namespace ProjectHermes.Xipona.Api.Domain.Recipes.Services.Creations;

public class PreparationStepCreation
{
    public PreparationStepCreation(PreparationStepInstruction instruction, int sortingIndex)
    {
        Instruction = instruction;
        SortingIndex = sortingIndex;
    }

    public PreparationStepInstruction Instruction { get; }
    public int SortingIndex { get; }
}