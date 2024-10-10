﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using WorkyOne.Domain.Abstractions;
using WorkyOne.Domain.Attributes;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Domain.Interfaces.Common;

namespace WorkyOne.Domain.Entities.Schedule.Common
{
    /// <summary>
    /// Сущность, описывающая определённую последовательность смен
    /// </summary>
    public sealed class TemplateEntity : EntityBase, IUpdatable<TemplateEntity>
    {
        /// <summary>
        /// ID расписания
        /// </summary>
        [Required]
        [ForeignKey(nameof(Schedule))]
        public string ScheduleId { get; set; }

        /// <summary>
        /// Расписание, к которому относится данный шаблон
        /// </summary>
        [Required]
        public ScheduleEntity Schedule { get; set; }

        /// <summary>
        /// Список рабочих смен, используемых в шаблоне
        /// </summary>
        [Required]
        [Renewable]
        public List<TemplatedShiftEntity> Shifts { get; set; } = new List<TemplatedShiftEntity>();

        /// <summary>
        /// Последовательность смен в шаблоне
        /// </summary>
        [Required]
        [Renewable]
        public List<ShiftSequenceEntity> Sequences { get; set; } = new List<ShiftSequenceEntity>();

        /// <summary>
        /// Дата, с которой начинается отсчёт повторений шаблона
        /// </summary>
        [Required]
        public DateOnly StartDate { get; set; }

        public void UpdateFields(TemplateEntity entity)
        {
            ScheduleId = entity.ScheduleId;
            StartDate = entity.StartDate;
        }
    }
}