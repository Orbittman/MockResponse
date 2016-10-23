using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MockResponse;

namespace MockResponse.Migrations
{
    [DbContext(typeof(SqlLiteContext))]
    partial class SqlLiteContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("MockResponse.Domain", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Domains");
                });

            modelBuilder.Entity("MockResponse.Header", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.Property<int?>("ResponseId");

                    b.Property<int?>("ResponseId1");

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ResponseId");

                    b.HasIndex("ResponseId1");

                    b.ToTable("Headers");
                });

            modelBuilder.Entity("MockResponse.Response", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Content");

                    b.Property<int?>("DomainId");

                    b.Property<string>("Path");

                    b.Property<int>("StatusCode");

                    b.HasKey("Id");

                    b.HasIndex("DomainId");

                    b.ToTable("Responses");
                });

            modelBuilder.Entity("MockResponse.Header", b =>
                {
                    b.HasOne("MockResponse.Response", "Response")
                        .WithMany("Headers")
                        .HasForeignKey("ResponseId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MockResponse.Response")
                        .WithMany()
                        .HasForeignKey("ResponseId1");
                });

            modelBuilder.Entity("MockResponse.Response", b =>
                {
                    b.HasOne("MockResponse.Domain", "Domain")
                        .WithMany("Responses")
                        .HasForeignKey("DomainId");
                });
        }
    }
}
