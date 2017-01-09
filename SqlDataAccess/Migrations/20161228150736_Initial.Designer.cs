using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MMU.BoerseDownloader.SqlDataAccess;

namespace MMU.BoerseDownloader.SqlDataAccess.Migrations
{
    [DbContext(typeof(BoerseDownloaderDbContext))]
    [Migration("20161228150736_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MMU.BoerseDownloader.Model.BoerseUser", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("LoginName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("BoerseUser","Core");
                });

            modelBuilder.Entity("MMU.BoerseDownloader.Model.DownloadContext", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BoerseLinkProvider");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("ThreadUrl")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("DownloadContext","Core");
                });

            modelBuilder.Entity("MMU.BoerseDownloader.Model.DownloadEntryConfiguration", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("DownloadLinkIdentifier");

                    b.Property<bool>("IsLinkVisited");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("DownloadEntryConfiguration","Core");
                });
        }
    }
}
