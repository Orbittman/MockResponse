using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MockResponse;

namespace MockResponse.Migrations
{
    [DbContext(typeof(SqlLiteContext))]
    [Migration("20161018202915_MockResponse_1.0.1")]
    partial class MockResponse_101
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

                    b.Property<string>("Value");

                    b.HasKey("Id");

                    b.HasIndex("ResponseId");

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
                        .HasForeignKey("ResponseId");
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
