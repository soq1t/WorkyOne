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
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.DailyInfoEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<TimeOnly?>("Beginning")
                        .HasColumnType("time without time zone");

                    b.Property<string>("ColorCode")
                        .IsRequired()
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

                    b.ToTable("DailyInfos");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("TemplateId")
                        .HasColumnType("text");

                    b.Property<string>("UserDataId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId")
                        .IsUnique();

                    b.HasIndex("UserDataId");

                    b.ToTable("Schedules");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ShiftSequenceEntity", b =>
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

                    b.ToTable("ShiftSequences");
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

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.DatedShiftEntity", b =>
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("DatedShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.ExampleShiftEntity", b =>
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

                    b.HasKey("Id");

                    b.ToTable("ExampleShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.PeriodicShiftEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<TimeOnly?>("Beginning")
                        .HasColumnType("time without time zone");

                    b.Property<string>("ColorCode")
                        .HasColumnType("text");

                    b.Property<DateOnly>("EndDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly?>("Ending")
                        .HasColumnType("time without time zone");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ScheduleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateOnly>("StartDate")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.HasIndex("ScheduleId");

                    b.ToTable("PeriodicShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.TemplatedShiftEntity", b =>
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

                    b.Property<string>("TemplateId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("TemplateId");

                    b.ToTable("TemplatedShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Users.UserDataEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserDatas");
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
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", "Template")
                        .WithOne("Schedule")
                        .HasForeignKey("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "TemplateId");

                    b.HasOne("WorkyOne.Domain.Entities.Users.UserDataEntity", "UserData")
                        .WithMany("Schedules")
                        .HasForeignKey("UserDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");

                    b.Navigation("UserData");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ShiftSequenceEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Shifts.TemplatedShiftEntity", "Shift")
                        .WithMany("Sequences")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", "Template")
                        .WithMany("Sequences")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.DatedShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithMany("DatedShifts")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.PeriodicShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", "Schedule")
                        .WithMany("PeriodicShifts")
                        .HasForeignKey("ScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Schedule");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.TemplatedShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", "Template")
                        .WithMany("Shifts")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Template");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.ScheduleEntity", b =>
                {
                    b.Navigation("DatedShifts");

                    b.Navigation("PeriodicShifts");

                    b.Navigation("Timetable");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Common.TemplateEntity", b =>
                {
                    b.Navigation("Schedule")
                        .IsRequired();

                    b.Navigation("Sequences");

                    b.Navigation("Shifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.Shifts.TemplatedShiftEntity", b =>
                {
                    b.Navigation("Sequences");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Users.UserDataEntity", b =>
                {
                    b.Navigation("Schedules");
                });
#pragma warning restore 612, 618
        }
    }
}
