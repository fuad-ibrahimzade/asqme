﻿// <auto-generated />
using System;
using AsqMe.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace AsqMe.Migrations
{
    [DbContext(typeof(AsqMeDbContext))]
    partial class AsqMeDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("AsqMe.Data.Models.Answer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ApplicationUserId")
                        .HasColumnName("applicationuserid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("createddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<decimal>("MoneyReceived")
                        .HasColumnName("moneyreceived")
                        .HasColumnType("numeric");

                    b.Property<decimal>("MoneySpent")
                        .HasColumnName("moneyspent")
                        .HasColumnType("numeric");

                    b.Property<int>("QuestionId")
                        .HasColumnName("questionid")
                        .HasColumnType("integer");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("updateddate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id")
                        .HasName("pk_answer");

                    b.HasIndex("ApplicationUserId")
                        .HasName("ix_answer_applicationuserid");

                    b.HasIndex("QuestionId")
                        .HasName("ix_answer_questionid");

                    b.ToTable("answer");
                });

            modelBuilder.Entity("AsqMe.Data.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnName("accessfailedcount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrencystamp")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("createddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Email")
                        .HasColumnName("email")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnName("emailconfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FacebookName")
                        .HasColumnName("facebookname")
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnName("lockoutenabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnName("lockoutend")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnName("normalizedemail")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnName("normalizedusername")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnName("passwordhash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnName("phonenumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnName("phonenumberconfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("ProfilePicture")
                        .HasColumnName("profilepicture")
                        .HasColumnType("text");

                    b.Property<string>("ProfilePictureWeb")
                        .HasColumnName("profilepictureweb")
                        .HasColumnType("text");

                    b.Property<string>("SecurityStamp")
                        .HasColumnName("securitystamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnName("twofactorenabled")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("updateddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UserName")
                        .HasColumnName("username")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_users");

                    b.HasIndex("NormalizedEmail")
                        .HasName("emailindex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("usernameindex");

                    b.ToTable("aspnetusers");
                });

            modelBuilder.Entity("AsqMe.Data.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("createddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("ImageOrIcon")
                        .HasColumnName("imageoricon")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("updateddate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id")
                        .HasName("pk_category");

                    b.ToTable("category");
                });

            modelBuilder.Entity("AsqMe.Data.Models.Earning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("Amount")
                        .HasColumnName("amount")
                        .HasColumnType("numeric");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnName("applicationuserid")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("createddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("EarningsType")
                        .IsRequired()
                        .HasColumnName("earningstype")
                        .HasColumnType("text");

                    b.Property<string>("SenderId")
                        .HasColumnName("senderid")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("updateddate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id")
                        .HasName("pk_earning");

                    b.HasIndex("ApplicationUserId")
                        .HasName("ix_earning_applicationuserid");

                    b.ToTable("earning");
                });

            modelBuilder.Entity("AsqMe.Data.Models.Question", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ApplicationUserId")
                        .HasColumnName("applicationuserid")
                        .HasColumnType("text");

                    b.Property<int>("CategoryId")
                        .HasColumnName("categoryid")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("createddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Description")
                        .HasColumnName("description")
                        .HasColumnType("text");

                    b.Property<decimal>("MoneyReceived")
                        .HasColumnName("moneyreceived")
                        .HasColumnType("numeric");

                    b.Property<decimal>("MoneySpent")
                        .HasColumnName("moneyspent")
                        .HasColumnType("numeric");

                    b.Property<int>("SolverAnswerId")
                        .HasColumnName("solveranswerid")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .HasColumnName("title")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("updateddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("Views")
                        .HasColumnName("views")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("pk_question");

                    b.HasIndex("ApplicationUserId")
                        .HasName("ix_question_applicationuserid");

                    b.HasIndex("CategoryId")
                        .HasName("ix_question_categoryid");

                    b.ToTable("question");
                });

            modelBuilder.Entity("AsqMe.Data.Models.QuestionTag", b =>
                {
                    b.Property<int>("QuestionId")
                        .HasColumnName("questionid")
                        .HasColumnType("integer");

                    b.Property<int>("TagId")
                        .HasColumnName("tagid")
                        .HasColumnType("integer");

                    b.HasKey("QuestionId", "TagId")
                        .HasName("pk_questiontag");

                    b.HasIndex("TagId")
                        .HasName("ix_questiontag_tagid");

                    b.ToTable("questiontag");
                });

            modelBuilder.Entity("AsqMe.Data.Models.Tag", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnName("createddate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdatedDate")
                        .HasColumnName("updateddate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id")
                        .HasName("pk_tag");

                    b.ToTable("tag");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnName("id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnName("concurrencystamp")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnName("normalizedname")
                        .HasColumnType("character varying(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id")
                        .HasName("pk_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("rolenameindex");

                    b.ToTable("aspnetroles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnName("claimtype")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claimvalue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnName("roleid")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_roleclaims");

                    b.HasIndex("RoleId")
                        .HasName("ix_roleclaims_roleid");

                    b.ToTable("aspnetroleclaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnName("claimtype")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnName("claimvalue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("userid")
                        .HasColumnType("text");

                    b.HasKey("Id")
                        .HasName("pk_userclaims");

                    b.HasIndex("UserId")
                        .HasName("ix_userclaims_userid");

                    b.ToTable("aspnetuserclaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnName("loginprovider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnName("providerkey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnName("providerdisplayname")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnName("userid")
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_userlogins");

                    b.HasIndex("UserId")
                        .HasName("ix_userlogins_userid");

                    b.ToTable("aspnetuserlogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("userid")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnName("roleid")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_userroles");

                    b.HasIndex("RoleId")
                        .HasName("ix_userroles_roleid");

                    b.ToTable("aspnetuserroles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnName("userid")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnName("loginprovider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnName("value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_usertokens");

                    b.ToTable("aspnetusertokens");
                });

            modelBuilder.Entity("AsqMe.Data.Models.Answer", b =>
                {
                    b.HasOne("AsqMe.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .HasConstraintName("fk_answer_users_applicationuserid");

                    b.HasOne("AsqMe.Data.Models.Question", "Question")
                        .WithMany()
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("fk_answer_question_questionid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AsqMe.Data.Models.Earning", b =>
                {
                    b.HasOne("AsqMe.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .HasConstraintName("fk_earning_users_applicationuserid")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("AsqMe.Data.Models.Question", b =>
                {
                    b.HasOne("AsqMe.Data.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId")
                        .HasConstraintName("fk_question_users_applicationuserid")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("AsqMe.Data.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .HasConstraintName("fk_question_category_categoryid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AsqMe.Data.Models.QuestionTag", b =>
                {
                    b.HasOne("AsqMe.Data.Models.Question", "Question")
                        .WithMany("QuestionTags")
                        .HasForeignKey("QuestionId")
                        .HasConstraintName("fk_questiontag_question_questionid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AsqMe.Data.Models.Tag", "Tag")
                        .WithMany("QuestionTags")
                        .HasForeignKey("TagId")
                        .HasConstraintName("fk_questiontag_tag_tagid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_roleclaims_aspnetroles_identityroleid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("AsqMe.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_userclaims_aspnetusers_applicationuserid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("AsqMe.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_userlogins_aspnetusers_applicationuserid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .HasConstraintName("fk_userroles_aspnetroles_identityroleid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AsqMe.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_userroles_aspnetusers_applicationuserid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("AsqMe.Data.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .HasConstraintName("fk_usertokens_aspnetusers_applicationuserid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
