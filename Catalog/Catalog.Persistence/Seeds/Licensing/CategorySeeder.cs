using System;
using System.Collections.Generic;
using System.Text;
using Catalog.Domain.Entities.Categories;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Persistence.Seeds.Licensing;

public class CategorySeeder : IDataSeeder
{
    private readonly DataContext _context;

    public CategorySeeder(DataContext context)
    {
        _context = context;
    }

    public int Order => 1;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        if (await _context.Categories.AnyAsync(cancellationToken))
        {
            return;
        }

        List<Category> categories =
        [
            new Category("Sistemas operativos"),
            new Category("Ofimática"),
            new Category("Desarrollo"),
            new Category("Diseño"),
            new Category("Seguridad"),
        ];

        await _context.Categories.AddRangeAsync(categories, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }
}
