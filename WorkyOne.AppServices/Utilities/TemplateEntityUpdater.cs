using WorkyOne.AppServices.DTOs;
using WorkyOne.AppServices.Interfaces.Utilities;
using WorkyOne.Domain.Entities.Schedule;

namespace WorkyOne.AppServices.Utilities
{
    /// <summary>
    /// Инструмент по обновлению сущности шаблона из DTO шаблона
    /// </summary>
    public class TemplateEntityUpdater : IEntityFromDtoUpdater<TemplateEntity, TemplateDto>
    {
        public TemplateEntityUpdater() { }

        public void Update(TemplateEntity entity, TemplateDto dto)
        {
            entity.Name = dto.Name;
            entity.StartDate = dto.StartDate;
            entity.IsMirrored = dto.IsMirrored;

            UpdateRepititions(entity, dto.Repetitions);
        }

        private void UpdateRepititions(TemplateEntity entity, List<RepititionDto> repititions)
        {
            entity.Repititions.Clear();

            string id = Guid.NewGuid().ToString();

            foreach (RepititionDto repititionDto in repititions.OrderBy(r => r.Position))
            {
                RepititionEntity repitition = new RepititionEntity
                {
                    Id = id,
                    Position = repititionDto.Position,
                    RepetitionAmount = repititionDto.RepetitionAmount,
                    ShiftId = repititionDto.ShiftId,
                    Template = entity,
                    TemplateId = entity.Id
                };

                entity.Repititions.Add(repitition);
            }
        }
    }
}
