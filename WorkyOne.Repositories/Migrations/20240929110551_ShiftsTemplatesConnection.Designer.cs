﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WorkyOne.Repositories.Contextes;

#nullable disable

namespace WorkyOne.Repositories.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240929110551_ShiftsTemplatesConnection")]
    partial class ShiftsTemplatesConnection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ShiftEntityTemplateEntity", b =>
                {
                    b.Property<string>("ShiftsId")
                        .HasColumnType("text");

                    b.Property<string>("TemplatesId")
                        .HasColumnType("text");

                    b.HasKey("ShiftsId", "TemplatesId");

                    b.HasIndex("TemplatesId");

                    b.ToTable("ShiftEntityTemplateEntity");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.RepititionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("Position")
                        .HasColumnType("integer");

                    b.Property<int>("RepetitionAmount")
                        .HasColumnType("integer");

                    b.Property<string>("ShiftId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TemplateEntityId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.HasIndex("TemplateEntityId");

                    b.ToTable("ShiftRepititions");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.ShiftEntity", b =>
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
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.HasKey("Id");

                    b.ToTable("Shifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.SingleDayShiftEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<DateOnly>("ShiftDate")
                        .HasColumnType("date");

                    b.Property<string>("ShiftId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TemplateId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ShiftId");

                    b.HasIndex("TemplateId");

                    b.ToTable("SingleDayShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.TemplateEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("UserDataId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserDataId");

                    b.ToTable("Templates");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.UserDataEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("UserDataEntity");
                });

            modelBuilder.Entity("ShiftEntityTemplateEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.ShiftEntity", null)
                        .WithMany()
                        .HasForeignKey("ShiftsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Schedule.TemplateEntity", null)
                        .WithMany()
                        .HasForeignKey("TemplatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.RepititionEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.ShiftEntity", "Shift")
                        .WithMany("Repetitions")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Schedule.TemplateEntity", null)
                        .WithMany("Repititions")
                        .HasForeignKey("TemplateEntityId");

                    b.Navigation("Shift");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.SingleDayShiftEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.Schedule.ShiftEntity", "Shift")
                        .WithMany("SingleDayShifts")
                        .HasForeignKey("ShiftId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WorkyOne.Domain.Entities.Schedule.TemplateEntity", "Template")
                        .WithMany("SingleDayShifts")
                        .HasForeignKey("TemplateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shift");

                    b.Navigation("Template");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.TemplateEntity", b =>
                {
                    b.HasOne("WorkyOne.Domain.Entities.UserDataEntity", "UserData")
                        .WithMany("Templates")
                        .HasForeignKey("UserDataId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserData");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.ShiftEntity", b =>
                {
                    b.Navigation("Repetitions");

                    b.Navigation("SingleDayShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.Schedule.TemplateEntity", b =>
                {
                    b.Navigation("Repititions");

                    b.Navigation("SingleDayShifts");
                });

            modelBuilder.Entity("WorkyOne.Domain.Entities.UserDataEntity", b =>
                {
                    b.Navigation("Templates");
                });
#pragma warning restore 612, 618
        }
    }
}
