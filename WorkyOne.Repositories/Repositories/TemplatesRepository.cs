using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories
{
    /// <summary>
    /// Репозиторий шаблонов расписаний
    /// </summary>
    public class TemplatesRepository : ITemplatesRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TemplateEntity template)
        {
            bool templateExists = await _context.Templates.AnyAsync(x => x.Id == template.Id);

            if (!templateExists)
            {
                _context.Templates.Add(template);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string id)
        {
            var deletedTemplate = await _context.Templates.FirstOrDefaultAsync(x => x.Id == id);

            if (deletedTemplate != null)
            {
                _context.Remove(deletedTemplate);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TemplateEntity?> GetAsync(string id)
        {
            TemplateEntity? template = await _context.Templates.FirstOrDefaultAsync(x =>
                x.Id == id
            );
            return template;
        }

        public async Task UpdateAsync(TemplateEntity template)
        {
            TemplateEntity? updatedTemplate = await _context.Templates.FirstOrDefaultAsync(x =>
                x.Id == template.Id
            );

            if (updatedTemplate != null)
            {
                updatedTemplate.Update(template);
                _context.Update(updatedTemplate);
                await _context.SaveChangesAsync();
            }
        }
    }
}
