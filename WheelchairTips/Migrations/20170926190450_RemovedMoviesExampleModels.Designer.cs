﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using WheelchairTips.Models;

namespace WheelchairTips.Migrations
{
    [DbContext(typeof(WheelchairTipsContext))]
    [Migration("20170926190450_RemovedMoviesExampleModels")]
    partial class RemovedMoviesExampleModels
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.2")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WheelchairTips.Models.Tip", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Author");

                    b.Property<string>("Content");

                    b.Property<string>("Title");

                    b.HasKey("ID");

                    b.ToTable("Tip");
                });
        }
    }
}