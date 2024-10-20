using Moq;
using Moq.EntityFrameworkCore;
using Newtonsoft.Json;
using WorkyOne.Contracts.Enums.Reposistories;
using WorkyOne.Contracts.Repositories;
using WorkyOne.Contracts.Requests.Schedule.Shifts;
using WorkyOne.Domain.Entities.Schedule.Shifts;
using WorkyOne.Repositories.Contextes;
using WorkyOne.Repositories.Repositories.Common;
using WorkyOne.Repositories.Repositories.Schedule.Shifts;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Repositories.Shifts
{
    public class TemplatedShiftsRepositoryTests
    {
        private readonly Mock<ApplicationDbContext> _context = new Mock<ApplicationDbContext>();
        private readonly ApplicationBaseRepository _baseRepo;
        private readonly TemplatedShiftsRepository _repo;

        private static readonly string _templateId = Guid.NewGuid().ToString();

        private static readonly List<TemplatedShiftEntity> _shifts = new List<TemplatedShiftEntity>
        {
            new TemplatedShiftEntity
            {
                Name = "Day",
                ColorCode = "#111111",
                TemplateId = _templateId,
                Beginning = new TimeOnly(8, 50),
                Ending = new TimeOnly(20, 50)
            },
            new TemplatedShiftEntity
            {
                Name = "Night",
                ColorCode = "#222222",
                TemplateId = _templateId,
                Beginning = new TimeOnly(20, 50),
                Ending = new TimeOnly(8, 50)
            },
            new TemplatedShiftEntity
            {
                Name = "Shift3",
                ColorCode = "#333333",
                TemplateId = _templateId,
                Beginning = new TimeOnly(9, 0),
                Ending = new TimeOnly(17, 0)
            }
        };

        public TemplatedShiftsRepositoryTests()
        {
            _context.Setup(c => c.TemplatedShifts).ReturnsDbSet(_shifts);
            _context.Setup(c => c.Set<TemplatedShiftEntity>()).ReturnsDbSet(_shifts);
            _baseRepo = new ApplicationBaseRepository(_context.Object);
            _repo = new TemplatedShiftsRepository(_baseRepo, _context.Object);
        }

        [Fact]
        public async Task Get_ShouldReturnCorrectShiftsList()
        {
            var result = (
                await _repo.GetByTemplateIdAsync(
                    new TemplatedShiftRequest { TemplateId = _templateId }
                )
            ).ToList();

            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Equal("Day", result[0].Name);
            Assert.Equal("Night", result[1].Name);
        }

        [Fact]
        public async Task CreateMany_ShouldReturnCorrectResult()
        {
            var entities = new List<TemplatedShiftEntity>
            {
                _shifts[0],
                new TemplatedShiftEntity { }
            };

            var result = await _repo.CreateManyAsync(entities);

            Assert.NotNull(result);

            Assert.True(result.IsSuccess);

            Assert.Equal(entities[1].Id, result.SucceedIds[0]);
            Assert.Equal(entities[0].Id, result.Errors[0].EntityId);
            Assert.Equal(RepositoryErrorType.EntityAlreadyExists, result.Errors[0].ErrorType);
        }
    }
}
