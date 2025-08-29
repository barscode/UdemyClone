using Microsoft.EntityFrameworkCore;
using UdemyClone.Api.Domain;

namespace UdemyClone.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    // Yeni özellikler
    public DbSet<Kupon> Kuponlar => Set<Kupon>();
    public DbSet<Mesaj> Mesajlar => Set<Mesaj>();
    public DbSet<Yorum> Yorumlar => Set<Yorum>();
    public DbSet<DersTamamlama> DersTamamlamalar => Set<DersTamamlama>();
    public DbSet<KuponKurs> KuponKurslar => Set<KuponKurs>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map Kategori entity to Kategoriler table with Turkish column names
        modelBuilder.Entity<Category>().ToTable("Kategoriler");

        // Kullanici -> Kullanicilar
        modelBuilder.Entity<User>()
            .ToTable("Kullanicilar")
            .HasIndex(u => u.Email)
            .IsUnique();

        // Kurs configuration
        modelBuilder.Entity<Course>()
            .ToTable("Kurslar")
            .HasOne(c => c.Egitmen) // was Instructor
            .WithMany(u => u.VerdigiKurslar) // was CoursesTaught
            .HasForeignKey(c => c.EgitmenId) // was InstructorId
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Course>()
            .HasOne(c => c.Kategori) // was Category
            .WithMany()
            .HasForeignKey(c => c.KategoriId); // was CategoryId

        // decimal precision
        modelBuilder.Entity<Course>().Property(c => c.Fiyat) // was Price
            .HasColumnType("decimal(18,2)");

        // Ders configuration
        modelBuilder.Entity<Lesson>()
            .ToTable("Dersler")
            .HasOne(l => l.Kurs) // was Course
            .WithMany(c => c.Dersler)
            .HasForeignKey(l => l.KursId); // was CourseId

        // KursKayit (Enrollment) configuration
        modelBuilder.Entity<Enrollment>()
            .ToTable("KursKayitlari")
            .HasIndex(e => new { e.UserId, e.CourseId })
            .IsUnique();

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.User)
            .WithMany(u => u.KursKayitlari) // was Enrollments
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Kayitlar) // was Enrollments
            .HasForeignKey(e => e.CourseId);

        // Kupon configuration
        modelBuilder.Entity<Kupon>()
            .ToTable("Kuponlar")
            .HasIndex(k => k.Code)
            .IsUnique();

        modelBuilder.Entity<Kupon>()
            .Property(k => k.DiscountPercent)
            .HasColumnType("decimal(5,2)");

        modelBuilder.Entity<Kupon>()
            .HasOne(k => k.Instructor)
            .WithMany() // removed Coupons navigation from Kullanici
            .HasForeignKey(k => k.InstructorId)
            .OnDelete(DeleteBehavior.Cascade);

        // KuponKurs configuration
        modelBuilder.Entity<KuponKurs>()
            .ToTable("KuponKurs")
            .HasKey(x => new { x.KuponId, x.CourseId });

        modelBuilder.Entity<KuponKurs>()
            .HasOne(x => x.Kupon)
            .WithMany(k => k.KuponKurslar)
            .HasForeignKey(x => x.KuponId);

        modelBuilder.Entity<KuponKurs>()
            .HasOne(x => x.Course)
            .WithMany() // no navigation property in Kurs for join
            .HasForeignKey(x => x.CourseId);

        // Mesaj configuration
        modelBuilder.Entity<Mesaj>()
            .ToTable("Mesajlar")
            .HasOne(m => m.Sender)
            .WithMany(u => u.GonderilenMesajlar) // was SentMessages
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Mesaj>()
            .HasOne(m => m.Recipient)
            .WithMany(u => u.AlinanMesajlar) // was ReceivedMessages
            .HasForeignKey(m => m.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);

        // Yorum configuration
        modelBuilder.Entity<Yorum>()
            .ToTable("Yorumlar")
            .HasOne(y => y.Author)
            .WithMany(u => u.Yorumlar) // was Comments
            .HasForeignKey(y => y.AuthorId);

        // DersTamamlama configuration
        modelBuilder.Entity<DersTamamlama>()
            .ToTable("DersTamamlamalar")
            .HasIndex(d => new { d.UserId, d.LessonId })
            .IsUnique();
    }
}

