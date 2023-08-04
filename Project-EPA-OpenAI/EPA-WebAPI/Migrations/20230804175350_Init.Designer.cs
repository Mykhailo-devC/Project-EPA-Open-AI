﻿// <auto-generated />
using System;
using Epa.Engine.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace EPA_WebAPI.Migrations
{
    [DbContext(typeof(EpaDbContext))]
    [Migration("20230804175350_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.6.23329.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Epa.Engine.Models.Entity_Models.Word", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Value")
                        .IsUnique();

                    b.ToTable("WordPool");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Value = "Orange"
                        },
                        new
                        {
                            Id = 2,
                            Value = "Apple"
                        },
                        new
                        {
                            Id = 3,
                            Value = "Cherry"
                        },
                        new
                        {
                            Id = 4,
                            Value = "Strawberry"
                        },
                        new
                        {
                            Id = 5,
                            Value = "Peach"
                        });
                });

            modelBuilder.Entity("Epa.Engine.Models.Entity_Models.WordList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValue(new DateTime(2023, 8, 4, 20, 53, 50, 139, DateTimeKind.Local).AddTicks(4690));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("WordLists");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreationDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Name = "Fruits"
                        });
                });

            modelBuilder.Entity("Epa.Engine.Models.Entity_Models.WordListWord", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("WordList_Id")
                        .HasColumnType("int");

                    b.Property<int>("Word_Id")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WordList_Id");

                    b.HasIndex("Word_Id");

                    b.ToTable("WordListWords");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            WordList_Id = 1,
                            Word_Id = 1
                        },
                        new
                        {
                            Id = 2,
                            WordList_Id = 1,
                            Word_Id = 2
                        },
                        new
                        {
                            Id = 3,
                            WordList_Id = 1,
                            Word_Id = 3
                        },
                        new
                        {
                            Id = 4,
                            WordList_Id = 1,
                            Word_Id = 4
                        },
                        new
                        {
                            Id = 5,
                            WordList_Id = 1,
                            Word_Id = 5
                        });
                });

            modelBuilder.Entity("Epa.Engine.Models.Entity_Models.WordListWord", b =>
                {
                    b.HasOne("Epa.Engine.Models.Entity_Models.WordList", "WordList")
                        .WithMany("WordList_Word")
                        .HasForeignKey("WordList_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__WordListWord__WordList_Id_Key");

                    b.HasOne("Epa.Engine.Models.Entity_Models.Word", "Word")
                        .WithMany("WordList_Word")
                        .HasForeignKey("Word_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__WordListWord__Word_Id_Key");

                    b.Navigation("Word");

                    b.Navigation("WordList");
                });

            modelBuilder.Entity("Epa.Engine.Models.Entity_Models.Word", b =>
                {
                    b.Navigation("WordList_Word");
                });

            modelBuilder.Entity("Epa.Engine.Models.Entity_Models.WordList", b =>
                {
                    b.Navigation("WordList_Word");
                });
#pragma warning restore 612, 618
        }
    }
}
