using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories.Schedule;
using WorkyOne.Domain.Entities.Schedule;
using WorkyOne.Repositories.Contextes;

namespace WorkyOne.Repositories.Repositories.Schedule
{
    /// <summary>
    /// Репозиторий по работе с шаблонами
    /// </summary>
    public sealed class TemplatesRepository : ITemplatesRepository
    {
        private readonly ApplicationDbContext _context;

        public TemplatesRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TemplateEntity template)
        {
            bool templateExists = await _context.Templates.AnyAsync(t => t.Id == template.Id);

            if (!templateExists)
            {
                _context.Templates.Add(template);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(string templateId)
        {
            TemplateEntity? deleted = await _context.Templates.FirstOrDefaultAsync(t =>
                t.Id == templateId
            );

            if (deleted != null)
            {
                _context.Remove(deleted);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<TemplateEntity?> GetAsync(string templateId)
        {
            IQueryable<TemplateEntity> query = _context.Templates.Where(t => t.Id == templateId);

            query.Include(t => t.Shifts);
            query.Include(t => t.Sequences);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<TemplateEntity?> GetByScheduleIdAsync(string scheduleId)
        {
            IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                t.ScheduleId == scheduleId
            );

            query.Include(t => t.Shifts);
            query.Include(t => t.Sequences);

            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(TemplateEntity template)
        {
            TemplateEntity? updatedEntity = await GetAsync(template.Id);

            if (updatedEntity == null)
            {
                return;
            }

            updatedEntity.StartDate = template.StartDate;
        }
    }
}
