﻿// <auto-generated />
using System;
using HackChallenge.DAL.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace HackChallenge.DAL.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20210127222714_AddedNewEntities")]
    partial class AddedNewEntities
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("HackChallenge.DAL.Entities.Directory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("DirectoryId")
                        .HasColumnType("int");

                    b.Property<int?>("LinuxSystemId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.HasIndex("LinuxSystemId");

                    b.ToTable("Directories");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.File", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int?>("DirectoryId")
                        .HasColumnType("int");

                    b.Property<string>("Text")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("DirectoryId");

                    b.ToTable("Files");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.LinuxSystem", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("LinuxSystems");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<int>("CountOfCorrectUserName")
                        .HasColumnType("int");

                    b.Property<int>("CountOfIncorrectLoginData")
                        .HasColumnType("int");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("HaveLinuxPermission")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("isAuthorized")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.Directory", b =>
                {
                    b.HasOne("HackChallenge.DAL.Entities.Directory", null)
                        .WithMany("Directories")
                        .HasForeignKey("DirectoryId");

                    b.HasOne("HackChallenge.DAL.Entities.LinuxSystem", "LinuxSystem")
                        .WithMany("Directories")
                        .HasForeignKey("LinuxSystemId");

                    b.Navigation("LinuxSystem");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.File", b =>
                {
                    b.HasOne("HackChallenge.DAL.Entities.Directory", "Directory")
                        .WithMany("Files")
                        .HasForeignKey("DirectoryId");

                    b.Navigation("Directory");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.LinuxSystem", b =>
                {
                    b.HasOne("HackChallenge.DAL.Entities.User", "User")
                        .WithOne("LinuxSystem")
                        .HasForeignKey("HackChallenge.DAL.Entities.LinuxSystem", "Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.Directory", b =>
                {
                    b.Navigation("Directories");

                    b.Navigation("Files");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.LinuxSystem", b =>
                {
                    b.Navigation("Directories");
                });

            modelBuilder.Entity("HackChallenge.DAL.Entities.User", b =>
                {
                    b.Navigation("LinuxSystem");
                });
#pragma warning restore 612, 618
        }
    }
}