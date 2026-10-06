using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using LicenseEntity = Catalog.Domain.Entities.License.License;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Persistence.Seeds.Licensing;

public class LicenseSeeder : IDataSeeder
{
    private readonly DataContext _context;

    public LicenseSeeder(DataContext context)
    {
        _context = context;
    }

    public int Order => 3;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Licenses.AnyAsync(cancellationToken))
        {
            return;
        }

        List<SoftwareEntity> software = await _context.Softwares.ToListAsync(cancellationToken);

        List<LicenseEntity> licenses = [];

        foreach (SoftwareEntity item in software)
        {
            for (int i = 0; i < 3; i++)
            {
                licenses.Add(new LicenseEntity(item.Id));
            }
        }

        await _context.Licenses.AddRangeAsync(licenses, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
