using Microsoft.EntityFrameworkCore;
using Models.DTOs.Quiz;
using Models.Entities;
using System;
using System.Collections.Generic;
namespace Data;

public  class AppDbContext : DbContext
{

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Attempt> Attempts { get; set; }
    public DbSet<AttemptAnswer> AttemptAnswers { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<QuizQuestion> QuizQuestions { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<StudentProfile> StudentProfiles { get; set; }
    public DbSet<StudentTopicPerformance> StudentTopicPerformances { get; set; }
    public DbSet<Topic> Topics { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<PasswordResetOtp> PasswordResetOtps { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //modelBuilder.Entity<StudentQuizDto>().HasNoKey();
        modelBuilder.Entity<Attempt>(entity =>
        {

            entity.HasKey(e => e.Id).HasName("PK__Attempts__3214EC07804D7275");

            entity.HasIndex(e => new { e.UserId, e.QuizId, e.AttemptNumber }, "UQ_Attempt_Number").IsUnique();

            entity.Property(e => e.AttemptNumber).HasDefaultValue(1);
            entity.Property(e => e.Passed).HasDefaultValue(false);
            entity.Property(e => e.Percentage)
                .HasDefaultValue(0m)
                .HasPrecision(18, 2);
            entity.Property(e => e.Score)
                .HasDefaultValue(0m)
                .HasPrecision(18, 2);
            entity.Property(e => e.StartedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SubmittedAt).HasColumnType("datetime");

            entity.HasOne(d => d.Quiz).WithMany(p => p.Attempts)
                .HasForeignKey(d => d.QuizId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempts_Quizzes");

            entity.HasOne(d => d.User).WithMany(p => p.Attempts)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Attempts_Users");
        });

        modelBuilder.Entity<AttemptAnswer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AttemptA__3214EC07D7E0B882");

            entity.Property(e => e.EarnedPoints).HasDefaultValue(0);
            entity.Property(e => e.IsCorrect).HasDefaultValue(false);

            entity.HasOne(d => d.Attempt).WithMany(p => p.AttemptAnswers)
                .HasForeignKey(d => d.AttemptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttemptAnswers_Attempts");

            entity.HasOne(d => d.Question).WithMany(p => p.AttemptAnswers)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_AttemptAnswers_Questions");

            entity.HasOne(d => d.SelectedOption).WithMany(p => p.AttemptAnswers)
                .HasForeignKey(d => d.SelectedOptionId)
                .HasConstraintName("FK_AttemptAnswers_Options");
        });

        modelBuilder.Entity<Option>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Options__3214EC07B03E68D7");

            entity.Property(e => e.Content).HasColumnType("text");

            entity.HasOne(d => d.Question).WithMany(p => p.Options)
                .HasForeignKey(d => d.QuestionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Options_Questions");
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Question__3214EC07C8FADC5C");

            entity.Property(e => e.Content).HasColumnType("text");
            entity.Property(e => e.Difficulty)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Easy");
            entity.Property(e => e.Points).HasDefaultValue(1);

            entity.HasOne(d => d.Topic).WithMany(p => p.Questions)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Questions_Topics");
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quizzes__3214EC079C82E858");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.MaxAttempts).HasDefaultValue(3);
            entity.Property(e => e.PassPercentage)
                .HasDefaultValue(60.00m)
                .HasPrecision(18, 2);
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .IsUnicode(false);


        });


        modelBuilder.Entity<QuizQuestion>()
    .HasKey(x => new { x.QuizId, x.QuestionId });

        modelBuilder.Entity<QuizQuestion>()
            .HasOne(x => x.Quiz)
            .WithMany(q => q.QuizQuestions)
            .HasForeignKey(x => x.QuizId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<QuizQuestion>()
            .HasOne(x => x.Question)
            .WithMany(q => q.QuizQuestions)
            .HasForeignKey(x => x.QuestionId);


        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC0763F3E92D");

            entity.HasIndex(e => e.Name, "UQ_Roles_Name").IsUnique();

            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__StudentP__1788CC4C0BCF5AAB");

            entity.Property(e => e.UserId).ValueGeneratedNever();
            entity.Property(e => e.CurrentLevel)
                .HasMaxLength(30)
                .IsUnicode(false)
                .HasDefaultValue("Easy");
            entity.Property(e => e.LastAssessmentDate).HasColumnType("datetime");
            entity.Property(e => e.SkillScore)
                .HasDefaultValue(0m)
                .HasPrecision(18, 2);
            entity.Property(e => e.TotalAttempts).HasDefaultValue(0);

            entity.HasOne(d => d.User).WithOne(p => p.StudentProfile)
                .HasForeignKey<StudentProfile>(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_StudentProfiles_Users");
        });

        modelBuilder.Entity<StudentTopicPerformance>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.TopicId });

            entity.ToTable("StudentTopicPerformance");

            entity.Property(e => e.CorrectAnswers).HasDefaultValue(0);
            entity.Property(e => e.LastUpdated)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.SuccessRate)
                .HasDefaultValue(0m)
                .HasPrecision(18, 2);
            entity.Property(e => e.WrongAnswers).HasDefaultValue(0);

            entity.HasOne(d => d.Topic).WithMany(p => p.StudentTopicPerformances)
                .HasForeignKey(d => d.TopicId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STP_Topics");

            entity.HasOne(d => d.User).WithMany(p => p.StudentTopicPerformances)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_STP_Users");
        });

        modelBuilder.Entity<Topic>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Topics__3214EC07730C46A2");

            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC0764C4AFA0");

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Username)
                .HasMaxLength(60)
                .IsUnicode(false);
            entity.HasIndex(e => e.Email)
                            .IsUnique();

            entity.HasIndex(e => e.Username)
                .IsUnique();
            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Users_Roles");
        });
        modelBuilder.Entity<Question>()
             .HasOne(q => q.CreatedByTeacher)
             .WithMany(u => u.CreatedQuestions)
             .HasForeignKey(q => q.CreatedByTeacherId)
             .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Quiz>()
            .HasOne(q => q.CreatedByTeacher)
            .WithMany(u => u.CreatedQuizzes)
            .HasForeignKey(q => q.CreatedByTeacherId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<User>()
    .HasQueryFilter(u => !u.IsDeleted);

        modelBuilder.Entity<User>()
    .HasIndex(u => u.Username)
    .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.GoogleId)
            .IsUnique()
            .HasFilter("[GoogleId] IS NOT NULL");

        modelBuilder.Entity<RefreshToken>().HasKey(rt => rt.Id);
        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<RefreshToken>()
                .HasIndex(x => x.Token)
                .IsUnique();
        modelBuilder.Entity<PasswordResetOtp>()
            .HasKey(otp => otp.Id);

        modelBuilder.Entity<PasswordResetOtp>()
            .Property(otp => otp.Code)
            .IsRequired()
            .HasMaxLength(6);

        modelBuilder.Entity<PasswordResetOtp>()
            .Property(otp => otp.CreatedAt)
            .IsRequired();

        modelBuilder.Entity<PasswordResetOtp>()
            .Property(otp => otp.ExpiresAt)
            .IsRequired();

        modelBuilder.Entity<PasswordResetOtp>()
            .Property(otp => otp.IsUsed)
            .IsRequired();

        modelBuilder.Entity<PasswordResetOtp>()
            .HasIndex(x => new { x.UserId, x.Code });

        modelBuilder.Entity<PasswordResetOtp>()
            .HasOne(otp => otp.User)
            .WithMany(u => u.PasswordResetOtps)
            .HasForeignKey(otp => otp.UserId)
            .OnDelete(DeleteBehavior.Cascade);


    }
}
