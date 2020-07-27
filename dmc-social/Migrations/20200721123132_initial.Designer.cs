﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using DmcSocial.Repositories;

namespace DmcSocial.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200721123132_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DmcSocial.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<bool>("CanComment")
                        .HasColumnType("boolean");

                    b.Property<int>("CommentCount")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateRemoved")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedById")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastModifiedDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string[]>("PostAccessUsers")
                        .HasColumnType("text[]");

                    b.Property<int>("PostRestrictionType")
                        .HasColumnType("integer");

                    b.Property<string>("RemovedBy")
                        .HasColumnType("text");

                    b.Property<string>("RemovedById")
                        .HasColumnType("text");

                    b.Property<string>("Subject")
                        .HasColumnType("text");

                    b.Property<int>("ViewCount")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("DmcSocial.Models.PostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateRemoved")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedById")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int?>("ParentPostCommentId")
                        .HasColumnType("integer");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.Property<string>("RemovedBy")
                        .HasColumnType("text");

                    b.Property<string>("RemovedById")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ParentPostCommentId");

                    b.HasIndex("PostId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("DmcSocial.Models.PostTag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateRemoved")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedById")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PostId")
                        .HasColumnType("integer");

                    b.Property<string>("RemovedBy")
                        .HasColumnType("text");

                    b.Property<string>("RemovedById")
                        .HasColumnType("text");

                    b.Property<string>("TagId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("TagId");

                    b.ToTable("PostTags");
                });

            modelBuilder.Entity("DmcSocial.Models.Tag", b =>
                {
                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text");

                    b.Property<string>("CreatedById")
                        .HasColumnType("text");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("DateRemoved")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Id")
                        .HasColumnType("integer");

                    b.Property<bool>("IsSystemTag")
                        .HasColumnType("boolean");

                    b.Property<string>("LastModifiedBy")
                        .HasColumnType("text");

                    b.Property<string>("LastModifiedById")
                        .HasColumnType("text");

                    b.Property<DateTime?>("LastModifiedTime")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("NormalizeValue")
                        .HasColumnType("text");

                    b.Property<int>("PostCount")
                        .HasColumnType("integer");

                    b.Property<string>("RemovedBy")
                        .HasColumnType("text");

                    b.Property<string>("RemovedById")
                        .HasColumnType("text");

                    b.HasKey("Value");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("DmcSocial.Models.PostComment", b =>
                {
                    b.HasOne("DmcSocial.Models.PostComment", "ParentPostComment")
                        .WithMany("ChildrenPostComments")
                        .HasForeignKey("ParentPostCommentId");

                    b.HasOne("DmcSocial.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("DmcSocial.Models.PostTag", b =>
                {
                    b.HasOne("DmcSocial.Models.Post", "Post")
                        .WithMany("PostTags")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DmcSocial.Models.Tag", "Tag")
                        .WithMany()
                        .HasForeignKey("TagId");
                });
#pragma warning restore 612, 618
        }
    }
}
