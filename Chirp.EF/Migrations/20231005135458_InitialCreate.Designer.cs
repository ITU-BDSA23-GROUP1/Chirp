﻿// <auto-generated />
using System;
using Chirp.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Chirp.EF.Migrations
{
    [DbContext(typeof(CheepContext))]
    [Migration("20231005135458_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("Chirp.EF.Author", b =>
                {
                    b.Property<int>("authorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("authorID");

                    b.ToTable("authors");
                });

            modelBuilder.Entity("Chirp.EF.Cheep", b =>
                {
                    b.Property<int>("cheepID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("authorID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("text")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("timeStamp")
                        .HasColumnType("TEXT");

                    b.HasKey("cheepID");

                    b.HasIndex("authorID");

                    b.ToTable("cheeps");
                });

            modelBuilder.Entity("Chirp.EF.Cheep", b =>
                {
                    b.HasOne("Chirp.EF.Author", "author")
                        .WithMany("cheeps")
                        .HasForeignKey("authorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("author");
                });

            modelBuilder.Entity("Chirp.EF.Author", b =>
                {
                    b.Navigation("cheeps");
                });
#pragma warning restore 612, 618
        }
    }
}