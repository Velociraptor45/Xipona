﻿using ProjectHermes.Xipona.Api.Domain.Stores.Services.Creations;

namespace ProjectHermes.Xipona.Api.Domain.Stores.Models.Factories;

public interface ISectionFactory
{
    ISection Create(SectionId id, SectionName name, int sortingIndex, bool isDefaultSection, bool isDeleted);

    ISection CreateNew(SectionName name, int sortingIndex, bool isDefaultSection);

    ISection CreateNew(SectionCreation creation);
}