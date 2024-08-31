﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MoneyHeist.DataAccess;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MoneyHeist.DataAccess.Migrations
{
    [DbContext(typeof(RepoContext))]
    partial class RepoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MoneyHeist.Data.Gender", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("ID")
                        .HasName("pk_genders");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_genders_name");

                    b.ToTable("genders", (string)null);
                });

            modelBuilder.Entity("MoneyHeist.Data.Member", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<int>("GenderID")
                        .HasColumnType("integer")
                        .HasColumnName("gender_id");

                    b.Property<int?>("MainSkillID")
                        .HasColumnType("integer")
                        .HasColumnName("main_skill_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("StatusID")
                        .HasColumnType("integer")
                        .HasColumnName("status_id");

                    b.HasKey("ID")
                        .HasName("pk_member");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_member_email");

                    b.HasIndex("GenderID")
                        .HasDatabaseName("ix_member_gender_id");

                    b.HasIndex("MainSkillID")
                        .HasDatabaseName("ix_member_main_skill_id");

                    b.HasIndex("StatusID")
                        .HasDatabaseName("ix_member_status_id");

                    b.ToTable("member", (string)null);
                });

            modelBuilder.Entity("MoneyHeist.Data.MemberStatus", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("ID")
                        .HasName("pk_member_statuses");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_member_statuses_name");

                    b.ToTable("member_statuses", (string)null);
                });

            modelBuilder.Entity("MoneyHeist.Data.MemberToSkill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("Level")
                        .HasColumnType("integer")
                        .HasColumnName("level");

                    b.Property<int>("MemberID")
                        .HasColumnType("integer")
                        .HasColumnName("member_id");

                    b.Property<int>("SkillID")
                        .HasColumnType("integer")
                        .HasColumnName("skill_id");

                    b.HasKey("ID")
                        .HasName("pk_member_to_skills");

                    b.HasIndex("SkillID")
                        .HasDatabaseName("ix_member_to_skills_skill_id");

                    b.HasIndex("MemberID", "SkillID")
                        .IsUnique()
                        .HasDatabaseName("ix_member_to_skills_member_id_skill_id");

                    b.ToTable("member_to_skills", (string)null);
                });

            modelBuilder.Entity("MoneyHeist.Data.Skill", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("ID")
                        .HasName("pk_skills");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_skills_name");

                    b.ToTable("skills", (string)null);
                });

            modelBuilder.Entity("MoneyHeist.Data.Member", b =>
                {
                    b.HasOne("MoneyHeist.Data.Gender", "Gender")
                        .WithMany()
                        .HasForeignKey("GenderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_member_genders_gender_id");

                    b.HasOne("MoneyHeist.Data.Skill", "MainSkill")
                        .WithMany()
                        .HasForeignKey("MainSkillID")
                        .HasConstraintName("fk_member_skills_main_skill_id");

                    b.HasOne("MoneyHeist.Data.MemberStatus", "Status")
                        .WithMany()
                        .HasForeignKey("StatusID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_member_member_statuses_status_id");

                    b.Navigation("Gender");

                    b.Navigation("MainSkill");

                    b.Navigation("Status");
                });

            modelBuilder.Entity("MoneyHeist.Data.MemberToSkill", b =>
                {
                    b.HasOne("MoneyHeist.Data.Member", "Member")
                        .WithMany()
                        .HasForeignKey("MemberID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_member_to_skills_member_member_id");

                    b.HasOne("MoneyHeist.Data.Skill", "Skill")
                        .WithMany()
                        .HasForeignKey("SkillID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_member_to_skills_skills_skill_id");

                    b.Navigation("Member");

                    b.Navigation("Skill");
                });
#pragma warning restore 612, 618
        }
    }
}
