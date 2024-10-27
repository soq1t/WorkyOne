using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using WorkyOne.Contracts.DTOs.Common;
using WorkyOne.DependencyRegister;
using WorkyOne.Domain.Entities.Schedule.Common;
using WorkyOne.Domain.Entities.Users;
using WorkyOne.Infrastructure.Mappers.AutoMapperProfiles.Common;
using Xunit;

namespace WorkyOne.Tests.UnitTests.Other
{
    public class AutoMapperTests
    {
        private readonly IMapper _mapper;

        public AutoMapperTests()
        {
            var services = new ServiceCollection();
            services.AddAutoMapper(typeof(UserInfoProfile).Assembly);

            var serviceProvider = services.BuildServiceProvider();

            _mapper = serviceProvider.GetRequiredService<IMapper>();
        }

        [Fact]
        public void AutoMapper_ShouldMapUserInfoCorrectly()
        {
            var user = new UserEntity
            {
                UserName = "Username",
                FirstName = "Name",
                Email = "Email"
            };

            var userData = new UserDataEntity
            {
                UserId = user.Id,
                Schedules = new List<ScheduleEntity>
                {
                    new ScheduleEntity() { Name = "Schedule1" },
                    new ScheduleEntity() { Name = "Schedule2" }
                }
            };

            var dto = _mapper.Map<UserInfoDto>(user);
            _mapper.Map(userData, dto);

            Assert.Equal("Username", dto.UserName);
            Assert.Equal("Name", dto.FirstName);
            Assert.Equal("Email", dto.Email);
            Assert.Equal(user.Id, dto.Id);
            Assert.Equal(userData.Id, dto.UserDataId);
            Assert.Equal(userData.Schedules[0].Id, dto.Schedules[0].Id);
            Assert.Equal(userData.Schedules[0].Name, dto.Schedules[0].Name);
            Assert.Equal(userData.Schedules[1].Id, dto.Schedules[1].Id);
            Assert.Equal(userData.Schedules[1].Name, dto.Schedules[1].Name);
        }
    }
}
