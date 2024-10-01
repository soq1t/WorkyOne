using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WorkyOne.AppServices.Interfaces.Repositories;
using WorkyOne.AppServices.Interfaces.Repositories.Requests;
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
        private readonly IShiftRepository _shiftRepository;
        private readonly IRepititionsRepository _repititionsRepository;
        private readonly ISingleDayShiftRepository _singleDayShiftRepository;

        public TemplatesRepository(
            ApplicationDbContext context,
            IShiftRepository shiftRepository,
            IRepititionsRepository repititionsRepository,
            ISingleDayShiftRepository singleDayShiftRepository
        )
        {
            _context = context;
            _shiftRepository = shiftRepository;
            _repititionsRepository = repititionsRepository;
            _singleDayShiftRepository = singleDayShiftRepository;
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

        public async Task<TemplateEntity?> GetAsync(TemplateRequest request)
        {
            IQueryable<TemplateEntity> query = _context.Templates.Where(x =>
                x.Id == request.TemplateId
            );

            if (request.IncludeFullData)
            {
                AddFullDataToQuery(query);
            }

            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<TemplateEntity>> GetUserTemplatesAsync(TemplateRequest request)
        {
            IQueryable<TemplateEntity> query = _context.Templates.Where(t =>
                t.UserDataId == request.UserDataId
            );

            if (request.IncludeFullData)
            {
                AddFullDataToQuery(query);
            }

            return await query.ToListAsync();
        }

        private void AddFullDataToQuery(IQueryable<TemplateEntity> query)
        {
            query = query.Include(t => t.Shifts);
            query = query.Include(t => t.SingleDayShifts).ThenInclude(s => s.Shift);
            query = query.Include(t => t.Repititions).ThenInclude(r => r.Shift);
        }

        public async Task UpdateAsync(TemplateEntity template)
        {
            TemplateEntity? updatedTemplate = await _context
                .Templates.Include(t => t.Repititions)
                .Include(t => t.Shifts)
                .Include(t => t.SingleDayShifts)
                .FirstOrDefaultAsync(x => x.Id == template.Id);

            if (updatedTemplate != null)
            {
                await UpdateTemplate(updatedTemplate, template);
            }
        }

        #region Методы обновления шаблона
        private async Task UpdateTemplate(TemplateEntity template, TemplateEntity newValues)
        {
            template.Name = newValues.Name;
            template.StartDate = newValues.StartDate;
            template.IsMirrored = newValues.IsMirrored;

            await UpdateShifts(template, newValues.Shifts);
            await UpdateRepititions(template, newValues.Repititions);
            await UpdateSingleDayShifts(template, newValues.SingleDayShifts);
        }

        #region Методы обновления смен
        private async Task UpdateShifts(TemplateEntity template, List<ShiftEntity> shifts)
        {
            if (!shifts.Any())
            {
                template.Shifts.Clear();

                List<string> deletedShiftsIds = template
                    .Shifts.Where(s => !s.IsPredefined)
                    .Select(s => s.Id)
                    .ToList();

                await _shiftRepository.DeleteAsync(deletedShiftsIds);
            }
            else
            {
                await RemoveShifts(template, shifts);
                await AddUpdateShifts(template, shifts);
            }
        }

        private async Task RemoveShifts(TemplateEntity template, List<ShiftEntity> shifts)
        {
            List<ShiftEntity> removedShifts = template
                .Shifts.Where(t => !shifts.Any(n => n.Id == t.Id))
                .ToList();

            foreach (ShiftEntity shift in removedShifts)
            {
                template.Shifts.Remove(shift);
            }

            await _shiftRepository.DeleteAsync(
                removedShifts.Where(s => !s.IsPredefined).Select(s => s.Id).ToList()
            );
        }

        private async Task AddUpdateShifts(TemplateEntity template, List<ShiftEntity> shifts)
        {
            List<ShiftEntity> updatedShifts = new List<ShiftEntity>();
            List<ShiftEntity> newShifts = new List<ShiftEntity>();

            foreach (ShiftEntity shift in shifts)
            {
                if (template.Shifts.Any(s => s.Id == shift.Id))
                {
                    updatedShifts.Add(shift);
                }
                else
                {
                    newShifts.Add(shift);
                }
            }

            if (updatedShifts.Any())
            {
                await _shiftRepository.UpdateAsync(updatedShifts);
            }

            if (newShifts.Any())
            {
                await _shiftRepository.AddAsync(newShifts);
            }

            template.Shifts = await _shiftRepository.GetByTemplateIdAsync(template.Id);
        }
        #endregion

        private async Task UpdateRepititions(
            TemplateEntity template,
            List<RepititionEntity> repititions
        )
        {
            if (!repititions.Any())
            {
                await _repititionsRepository.DeleteAsync(template.Id);
            }
            else
            {
                await _repititionsRepository.UpdateAsync(template.Id, repititions);
            }

            template.Repititions = await _repititionsRepository.GetAsync(template.Id);
        }

        #region Методы обновления одиночных смен
        private async Task UpdateSingleDayShifts(
            TemplateEntity template,
            List<SingleDayShiftEntity> shifts
        )
        {
            if (!shifts.Any())
            {
                template.SingleDayShifts.Clear();

                await _singleDayShiftRepository.DeleteAsync(
                    template.SingleDayShifts.Select(s => s.Id).ToList()
                );
            }
            else
            {
                await RemoveSingleDayShifts(template, shifts);
                await AddUpdateSingleDayShifts(template, shifts);
            }
        }

        private async Task RemoveSingleDayShifts(
            TemplateEntity template,
            List<SingleDayShiftEntity> shifts
        )
        {
            List<SingleDayShiftEntity> removedShifts = template
                .SingleDayShifts.Where(s => !shifts.Any(n => n.Id == s.Id))
                .ToList();

            foreach (SingleDayShiftEntity shift in removedShifts)
            {
                template.SingleDayShifts.Remove(shift);
            }

            if (removedShifts.Any())
            {
                await _singleDayShiftRepository.DeleteAsync(
                    removedShifts.Select(s => s.Id).ToList()
                );
            }
        }

        private async Task AddUpdateSingleDayShifts(
            TemplateEntity template,
            List<SingleDayShiftEntity> shifts
        )
        {
            var updatedShifts = new List<SingleDayShiftEntity>();
            var newShifts = new List<SingleDayShiftEntity>();

            foreach (SingleDayShiftEntity shift in shifts)
            {
                if (template.Shifts.Any(s => s.Id == shift.Id))
                {
                    updatedShifts.Add(shift);
                }
                else
                {
                    newShifts.Add(shift);
                }
            }

            if (updatedShifts.Any())
            {
                await _singleDayShiftRepository.UpdateAsync(updatedShifts);
            }

            if (newShifts.Any())
            {
                await _singleDayShiftRepository.AddAsync(newShifts);
            }

            template.SingleDayShifts = await _singleDayShiftRepository.GetByTemplateIdAsync(
                template.Id
            );
        }
        #endregion
        #endregion
    }
}
