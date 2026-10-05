using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;
using SoftwareEntity = Catalog.Domain.Entities.Software.Software;

namespace Catalog.Persistence.Seeds.Licensing;

public class SoftwareSeeder : IDataSeeder
{
    private readonly DataContext _context;

    public SoftwareSeeder(DataContext context)
    {
        _context = context;
    }

    public int Order => 2;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Softwares.AnyAsync(cancellationToken))
        {
            return;
        }

        List<Category> categories = await _context.Categories.ToListAsync(cancellationToken);

        Guid CategoryIdOf(string name) => categories.First(c => c.Nombre.Valor == name).Id;

        List<SoftwareEntity> software =
        [
            new SoftwareEntity(CategoryIdOf("Sistemas operativos"), "Windows 11 Pro"),
            new SoftwareEntity(CategoryIdOf("Ofimática"), "Microsoft Office 365"),
            new SoftwareEntity(CategoryIdOf("Desarrollo"), "Visual Studio Enterprise"),
            new SoftwareEntity(CategoryIdOf("Diseño"), "Adobe Photoshop"),
            new SoftwareEntity(CategoryIdOf("Seguridad"), "Kaspersky Total Security"),
        ];

        await _context.Softwares.AddRangeAsync(software, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
