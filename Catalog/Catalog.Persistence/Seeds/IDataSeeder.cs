using System;
using System.Collections.Generic;
using System.Text;

namespace Catalog.Persistence.Seeds;

public interface IDataSeeder
{
    int Order { get; }

    Task SeedAsync(CancellationToken cancellationToken = default);
}
