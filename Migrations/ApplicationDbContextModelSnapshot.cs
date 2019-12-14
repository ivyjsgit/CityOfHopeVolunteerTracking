// <auto-generated />
using System;
using CoHO.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CoHO.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1");

            modelBuilder.Entity("CoHO.Models.Disability", b =>
                {
                    b.Property<int>("DisabilityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("DisabilityID");

                    b.ToTable("Disability");
                });

            modelBuilder.Entity("CoHO.Models.DisabilitySelection", b =>
                {
                    b.Property<int>("DisabilitySelectionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DisabilityID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("DisabilitySelectionID");

                    b.HasIndex("DisabilityID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("DisabilitySelection");
                });

            modelBuilder.Entity("CoHO.Models.EducationLevel", b =>
                {
                    b.Property<int>("EducationLevelID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("EducationLevelID");

                    b.ToTable("EducationLevel");

                    b.HasData(
                        new
                        {
                            EducationLevelID = 1,
                            Description = "GED",
                            InActive = false
                        },
                        new
                        {
                            EducationLevelID = 2,
                            Description = "High School Degree",
                            InActive = false
                        });
                });

            modelBuilder.Entity("CoHO.Models.Initiative", b =>
                {
                    b.Property<int>("InitiativeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("InitiativeID");

                    b.ToTable("Initiative");

                    b.HasData(
                        new
                        {
                            InitiativeID = 1,
                            Description = "Academy",
                            InActive = false
                        },
                        new
                        {
                            InitiativeID = 2,
                            Description = "Community Garden",
                            InActive = false
                        },
                        new
                        {
                            InitiativeID = 3,
                            Description = "Office",
                            InActive = false
                        },
                        new
                        {
                            InitiativeID = 4,
                            Description = "Housing",
                            InActive = false
                        });
                });

            modelBuilder.Entity("CoHO.Models.Race", b =>
                {
                    b.Property<int>("RaceID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("RaceID");

                    b.ToTable("Race");

                    b.HasData(
                        new
                        {
                            RaceID = 1,
                            Description = "American Indian",
                            InActive = false
                        },
                        new
                        {
                            RaceID = 2,
                            Description = "Asian",
                            InActive = false
                        },
                        new
                        {
                            RaceID = 3,
                            Description = "White",
                            InActive = false
                        },
                        new
                        {
                            RaceID = 4,
                            Description = "Back or African American",
                            InActive = false
                        });
                });

            modelBuilder.Entity("CoHO.Models.Skill", b =>
                {
                    b.Property<int>("SkillID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("SkillID");

                    b.ToTable("Skill");
                });

            modelBuilder.Entity("CoHO.Models.SkillSelection", b =>
                {
                    b.Property<int>("SkillSelectionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SKillID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VolunteerID")
                        .HasColumnType("INTEGER");

                    b.HasKey("SkillSelectionID");

                    b.HasIndex("SKillID");

                    b.HasIndex("VolunteerID");

                    b.ToTable("SkillSelection");
                });

            modelBuilder.Entity("CoHO.Models.ValueOfHour", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EffectiveDate")
                        .HasColumnType("TEXT");

                    b.Property<float>("Value")
                        .HasColumnType("REAL");

                    b.HasKey("ID");

                    b.ToTable("ValueOfHour");
                });

            modelBuilder.Entity("CoHO.Models.Volunteer", b =>
                {
                    b.Property<int>("VolunteerID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Admin")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("Birthday")
                        .HasColumnType("TEXT");

                    b.Property<string>("Cell")
                        .HasColumnType("TEXT");

                    b.Property<bool>("ClockedIn")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("CommunityService")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("EducationLevelID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("First")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Home")
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Last")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("RaceID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasMaxLength(50);

                    b.Property<bool>("Veteran")
                        .HasColumnType("INTEGER");

                    b.Property<int>("VolunteerTypeID")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("WorkersComp")
                        .HasColumnType("INTEGER");

                    b.HasKey("VolunteerID");

                    b.HasIndex("EducationLevelID");

                    b.HasIndex("RaceID");

                    b.HasIndex("VolunteerTypeID");

                    b.ToTable("Volunteer");

                    b.HasData(
                        new
                        {
                            VolunteerID = 1,
                            Admin = true,
                            ClockedIn = false,
                            CommunityService = false,
                            Email = "admin",
                            First = "Zadmin",
                            InActive = true,
                            Last = "Zadmin",
                            UserName = "admin",
                            Veteran = false,
                            VolunteerTypeID = 2,
                            WorkersComp = false
                        });
                });

            modelBuilder.Entity("CoHO.Models.VolunteerActivity", b =>
                {
                    b.Property<int>("VolunteerActivityID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ClockedIn")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("InitiativeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("TEXT");

                    b.Property<int>("VolunteerId")
                        .HasColumnType("INTEGER");

                    b.HasKey("VolunteerActivityID");

                    b.HasIndex("InitiativeId");

                    b.HasIndex("VolunteerId");

                    b.ToTable("VolunteerActivity");
                });

            modelBuilder.Entity("CoHO.Models.VolunteerType", b =>
                {
                    b.Property<int>("VolunteerTypeID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("InActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("VolunteerTypeID");

                    b.ToTable("VolunteerType");

                    b.HasData(
                        new
                        {
                            VolunteerTypeID = 1,
                            Description = "Volunteer",
                            InActive = false
                        },
                        new
                        {
                            VolunteerTypeID = 2,
                            Description = "Staff",
                            InActive = false
                        },
                        new
                        {
                            VolunteerTypeID = 3,
                            Description = "Board",
                            InActive = false
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasColumnType("TEXT")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("TEXT")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("CoHO.Models.DisabilitySelection", b =>
                {
                    b.HasOne("CoHO.Models.Disability", "Disability")
                        .WithMany("DisabilitySelection")
                        .HasForeignKey("DisabilityID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoHO.Models.Volunteer", "Volunteer")
                        .WithMany("DisabilitySelection")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoHO.Models.SkillSelection", b =>
                {
                    b.HasOne("CoHO.Models.Skill", "Skill")
                        .WithMany("SkillSelection")
                        .HasForeignKey("SKillID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoHO.Models.Volunteer", "Volunteer")
                        .WithMany("SkillSelection")
                        .HasForeignKey("VolunteerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoHO.Models.Volunteer", b =>
                {
                    b.HasOne("CoHO.Models.EducationLevel", "EducationLevel")
                        .WithMany("Volunteer")
                        .HasForeignKey("EducationLevelID");

                    b.HasOne("CoHO.Models.Race", "Race")
                        .WithMany("Volunteer")
                        .HasForeignKey("RaceID");

                    b.HasOne("CoHO.Models.VolunteerType", "VolunteerType")
                        .WithMany("Volunteer")
                        .HasForeignKey("VolunteerTypeID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CoHO.Models.VolunteerActivity", b =>
                {
                    b.HasOne("CoHO.Models.Initiative", "Initiative")
                        .WithMany("VolunteerActivity")
                        .HasForeignKey("InitiativeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CoHO.Models.Volunteer", "Volunteer")
                        .WithMany("VolunteerActivities")
                        .HasForeignKey("VolunteerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
