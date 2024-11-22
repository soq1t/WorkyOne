﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WorkyOne.Repositories.Contextes;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<TimeOnly?>("Beginning")
                        .HasColumnType("time without time zone");

                    b.Property<string>("ColorCode")
                        .HasColumnType("text");

                    b.Property<TimeOnly?>("Ending")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Shifts", (string)null);

                    b.HasDiscriminator<int>("Type");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.DailyInfoEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<TimeOnly?>("Beginning")
                        .HasColumnType("time without time zone");

                    b.Property<string>("ColorCode")
                        .HasColumnType("text");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<TimeOnly?>("Ending")
                        .HasColumnType("time without time zone");

                    b.Property<bool>("IsBusyDay")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<TimeSpan?>("ShiftProlongation")
                        .HasColumnType("interval");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("DailyInfos", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("UserDataId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserDataId");

                    b.ToTable("Schedules", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId")
                        .IsUnique();

                    b.ToTable("Templates", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Special.DatedShiftEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShiftId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("ShiftId");

                    b.ToTable("DatedShifts", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Special.PeriodicShiftEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ShiftId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.HasIndex("ShiftId");

                    b.ToTable("PeriodicShifts", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Special.TemplatedShiftEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<string>("ShiftId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TemplateId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.HasIndex("TemplateId");

                    b.ToTable("TemplatedShifts", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Users.UserDataEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("FavoriteScheduleId")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("FavoriteScheduleId");

                    b.ToTable("UserDatas", (string)null);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Basic.PersonalShiftEntity", b =>
                {
                    b.HasBaseType("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasIndex("ScheduleId");

                    b.HasDiscriminator().HasValue(1);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Basic.SharedShiftEntity", b =>
                {
                    b.HasBaseType("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity");

                    b.HasDiscriminator().HasValue(0);
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.DailyInfoEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithMany("Timetable")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Users.UserDataEntity", "UserData")
                        .WithMany("Schedules")
                        .HasForeignKey("UserDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserData");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithOne("Template")
                        .HasForeignKey("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", "ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Special.DatedShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithMany("DatedShifts")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity", "Shift")
                        .WithMany("DatedShifts")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Special.PeriodicShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithMany("PeriodicShifts")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity", "Shift")
                        .WithMany("PeriodicShifts")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Special.TemplatedShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity", "Shift")
                        .WithMany("TemplatedShifts")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", "Template")
                        .WithMany("Shifts")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Users.UserDataEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "FavoriteSchedule")
                        .WithMany()
                        .HasForeignKey("FavoriteScheduleId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("FavoriteSchedule");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.Basic.PersonalShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithMany("PersonalShifts")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Abstractions.Shifts.ShiftEntity", b =>
                {
                    b.Navigation("DatedShifts");

                    b.Navigation("PeriodicShifts");

                    b.Navigation("TemplatedShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", b =>
                {
                    b.Navigation("DatedShifts");

                    b.Navigation("PeriodicShifts");

                    b.Navigation("PersonalShifts");

                    b.Navigation("Template");

                    b.Navigation("Timetable");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", b =>
                {
                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Users.UserDataEntity", b =>
                {
                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
