﻿// <auto-generated />
using HealthChecker.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace HealthChecker.Data.Migrations
{
    [DbContext(typeof(HealthCheckerContext))]
    [Migration("20180304125855_AddDefaulyValueForDateTime")]
    partial class AddDefaulyValueForDateTime
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("HealthChecker.Models.Application", b =>
                {
                    b.Property<long>("ApplicationId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<int>("Interval");

                    b.Property<string>("Name");

                    b.Property<string>("Url");

                    b.HasKey("ApplicationId");

                    b.ToTable("Applications");
                });

            modelBuilder.Entity("HealthChecker.Models.HealthCheckerResult", b =>
                {
                    b.Property<long>("ResultId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Application");

                    b.Property<DateTime>("DateTimeCreated")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasDefaultValueSql("GETUTCDATE()");

                    b.Property<string>("ErrorMessage");

                    b.Property<double>("ExecutedIn");

                    b.Property<bool>("HasError");

                    b.Property<string>("StackTrace");

                    b.Property<string>("TestMethod");

                    b.HasKey("ResultId");

                    b.ToTable("Results");
                });
#pragma warning restore 612, 618
        }
    }
}
