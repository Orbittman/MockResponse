using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using MockResponse;

namespace MockResponse.Migrations
{
    [DbContext(typeof(SqlLiteContext))]
    [Migration("20161010101511_v_1.0.0")]
    partial class v_100
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431");

            modelBuilder.Entity("MockResponse.Response", b =>
                {
                    b.Property<int>("ResponseId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CacheControl");

                    b.Property<string>("Content");

                    b.Property<string>("ContentEncoding");

                    b.Property<string>("ContentType");

                    b.Property<string>("Path");

                    b.Property<string>("Server");

                    b.Property<int>("StatusCode");

                    b.Property<string>("Vary");

                    b.HasKey("ResponseId");

                    b.ToTable("Responses");
                });
        }
    }
}
