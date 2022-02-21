﻿// <auto-generated />
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220220221612_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DataAccess.Repository.Flights", b =>
                {
                    b.Property<int>("IdFlight")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Destination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("IdTransport")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Origin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("IdFlight");

                    b.HasIndex("IdTransport");

                    b.ToTable("Flights");
                });

            modelBuilder.Entity("DataAccess.Repository.JourneyFlights", b =>
                {
                    b.Property<int>("IdFlight")
                        .HasColumnType("int");

                    b.Property<int>("IdJourney")
                        .HasColumnType("int");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.HasKey("IdFlight", "IdJourney");

                    b.HasIndex("IdJourney");

                    b.ToTable("JourneyFlights");
                });

            modelBuilder.Entity("DataAccess.Repository.Journeys", b =>
                {
                    b.Property<int>("IdJourney")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Destination")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Index")
                        .HasColumnType("int");

                    b.Property<string>("Origin")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("IdJourney");

                    b.ToTable("Journeys");
                });

            modelBuilder.Entity("DataAccess.Repository.Transports", b =>
                {
                    b.Property<int>("IdTransport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("FlightCarrier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FlightNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdTransport");

                    b.ToTable("Transports");
                });

            modelBuilder.Entity("DataAccess.Repository.Flights", b =>
                {
                    b.HasOne("DataAccess.Repository.Transports", "Transport")
                        .WithMany("Flights")
                        .HasForeignKey("IdTransport")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Transport");
                });

            modelBuilder.Entity("DataAccess.Repository.JourneyFlights", b =>
                {
                    b.HasOne("DataAccess.Repository.Flights", "Flights")
                        .WithMany("JourneyFlights")
                        .HasForeignKey("IdFlight")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DataAccess.Repository.Journeys", "Journey")
                        .WithMany("JourneyFlights")
                        .HasForeignKey("IdJourney")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Flights");

                    b.Navigation("Journey");
                });

            modelBuilder.Entity("DataAccess.Repository.Flights", b =>
                {
                    b.Navigation("JourneyFlights");
                });

            modelBuilder.Entity("DataAccess.Repository.Journeys", b =>
                {
                    b.Navigation("JourneyFlights");
                });

            modelBuilder.Entity("DataAccess.Repository.Transports", b =>
                {
                    b.Navigation("Flights");
                });
#pragma warning restore 612, 618
        }
    }
}