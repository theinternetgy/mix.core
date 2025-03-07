﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Models.Account;

namespace Mix.Cms.Lib.Migrations.MixCmsAccount
{
    [DbContext(typeof(MixCmsAccountContext))]
    [Migration("20190804034827_init_mysql")]
    partial class init_mysql
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetRoleClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetRoles", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<string>("Name")
                        .HasMaxLength(250);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("([NormalizedName] IS NOT NULL)");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserClaims", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserLogins", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ApplicationUserId");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserRoles", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.Property<string>("ApplicationUserId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserTokens", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUsers", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("Avatar");

                    b.Property<string>("ConcurrencyStamp");

                    b.Property<int>("CountryId");

                    b.Property<string>("Culture");

                    b.Property<DateTime?>("Dob")
                        .HasColumnName("DOB");

                    b.Property<string>("Email")
                        .HasMaxLength(250);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FirstName");

                    b.Property<string>("Gender");

                    b.Property<bool>("IsActived");

                    b.Property<DateTime>("JoinDate");

                    b.Property<DateTime>("LastModified");

                    b.Property<string>("LastName");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("ModifiedBy");

                    b.Property<string>("NickName");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(250);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(250);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("RegisterType");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(250);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("([NormalizedUserName] IS NOT NULL)");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.Clients", b =>
                {
                    b.Property<string>("Id");

                    b.Property<bool>("Active");

                    b.Property<string>("AllowedOrigin")
                        .HasMaxLength(100);

                    b.Property<int>("ApplicationType");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<int>("RefreshTokenLifeTime");

                    b.Property<string>("Secret")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.RefreshTokens", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ClientId");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<DateTime>("ExpiresUtc");

                    b.Property<DateTime>("IssuedUtc");

                    b.Property<string>("Username");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetRoleClaims", b =>
                {
                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetRoles", "Role")
                        .WithMany("AspNetRoleClaims")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserClaims", b =>
                {
                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "ApplicationUser")
                        .WithMany("AspNetUserClaimsApplicationUser")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "User")
                        .WithMany("AspNetUserClaimsUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserLogins", b =>
                {
                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "ApplicationUser")
                        .WithMany("AspNetUserLoginsApplicationUser")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "User")
                        .WithMany("AspNetUserLoginsUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserRoles", b =>
                {
                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "ApplicationUser")
                        .WithMany("AspNetUserRolesApplicationUser")
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetRoles", "Role")
                        .WithMany("AspNetUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "User")
                        .WithMany("AspNetUserRolesUser")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Mix.Cms.Lib.Models.Account.AspNetUserTokens", b =>
                {
                    b.HasOne("Mix.Cms.Lib.Models.Account.AspNetUsers", "User")
                        .WithMany("AspNetUserTokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
