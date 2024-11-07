namespace IAutor.Api.Data.Context;

public class IAutorDb(DbContextOptions<IAutorDb> o, IConfiguration config) : DbContext(o)
{
    public DbSet<Email> Emails => Set<Email>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Owner> Owners => Set<Owner>();
    public DbSet<Param> Params => Set<Param>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<User> Users => Set<User>();
    public DbSet<UserLog> UserLogs => Set<UserLog>();
    public DbSet<Book> Books => Set<Book>();
    public DbSet<UserBookLog> UserBookLogs => Set<UserBookLog>();
    public DbSet<Plan> Plans => Set<Plan>();
    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Question> Questions => Set<Question>();
    public DbSet<Theme> Themes => Set<Theme>();

    public DbSet<PlanChapter> PlansChapters => Set<PlanChapter>();

    public DbSet<PlanChapterQuestion> PlansChapterQuestion => Set<PlanChapterQuestion>();

    protected override void OnConfiguring(DbContextOptionsBuilder o)
    {
        o.UseNpgsql(config.GetConnectionString("IAutorDb"));
       // o.UseSqlite("Data Source=IAutor.db;Cache=Shared");
    }

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(u => u.FirstName).HasColumnType("varchar(50)").HasColumnName("first_name");
            entity.Property(u => u.LastName).HasColumnType("varchar(50)").HasColumnName("last_name");
            entity.Property(u => u.Email).IsRequired().HasColumnType("varchar(100)").HasColumnName("email");
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(o => o.Cpf).HasColumnType("varchar(50)").HasColumnName("cpf");
            entity.Property(u => u.SignInWith).IsRequired().HasColumnType("varchar(10)").HasColumnName("sign_in_with");
            entity.Property(o => o.Type).HasColumnType("smallint").HasColumnName("type");
            entity.Property(u => u.BirthDate).HasColumnType("date").HasColumnName("birth_date");
            entity.Property(u => u.PasswordHash).HasColumnName("password_hash");
            entity.Property(u => u.ActivationCode).HasColumnType("varchar(50)").HasColumnName("activation_code");
            entity.Property(u => u.ActivationAt).HasColumnType("timestamp").HasColumnName("activation_at");
            entity.Property(u => u.ResetPassword).HasColumnType("boolean").HasColumnName("reset_password");
            entity.Property(u => u.ResetPasswordCode).HasColumnType("varchar(50)").HasColumnName("reset_password_code");
            entity.Property(u => u.ResetPasswordAt).HasColumnType("timestamp").HasColumnName("reset_password_at");
        });

        b.Entity<Email>(entity =>
        {
            entity.ToTable("emails");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(e => e.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(e => e.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(e => e.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");

            entity.Property(e => e.UserId).HasColumnType("bigint").HasColumnName("user_id");
            entity.Property(u => u.BookId).HasColumnType("bigint").HasColumnName("Book_id");
            entity.Property(e => e.EmailType).HasColumnType("smallint").HasColumnName("email_type");
            entity.Property(e => e.ScheduleDate).HasColumnType("timestamp").HasColumnName("schedule_date");
            entity.Property(e => e.DateSent).HasColumnType("timestamp").HasColumnName("date_sent");
            entity.Property(e => e.SendAttempts).HasColumnType("smallint").HasColumnName("send_attempts");

            entity.HasOne(e => e.User).WithMany(e => e.Emails).HasForeignKey(e => e.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(o => o.Book).WithMany(o => o.Emails).HasForeignKey(o => o.BookId).OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Owner>(entity =>
        {
            entity.ToTable("owners");
            entity.Property(o => o.Id).HasColumnName("id");
            entity.HasKey(o => o.Id);
            entity.Property(o => o.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(o => o.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(o => o.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(o => o.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(o => o.FirstName).HasColumnType("varchar(50)").HasColumnName("first_name");
            entity.Property(o => o.LastName).HasColumnType("varchar(50)").HasColumnName("last_name");
            entity.Property(o => o.UserId).HasColumnType("bigint").HasColumnName("user_id");
            entity.Property(o => o.PersonType).HasColumnType("varchar(50)").HasColumnName("person_type");
            entity.Property(o => o.Cpf).HasColumnType("varchar(50)").HasColumnName("cpf");
            entity.Property(o => o.Cnpj).HasColumnType("varchar(50)").HasColumnName("cnpj");
            entity.Property(o => o.CnpjRespName).HasColumnType("varchar(100)").HasColumnName("cnpj_rep_name");
            entity.Property(o => o.CnpjRespCpf).HasColumnType("varchar(50)").HasColumnName("cnpj_resp_cpf");
            entity.Property(o => o.Address).HasColumnType("varchar(100)").HasColumnName("address");
            entity.Property(o => o.Cep).HasColumnType("varchar(50)").HasColumnName("cep");
            entity.Property(o => o.City).HasColumnType("varchar(50)").HasColumnName("city");
            entity.Property(o => o.District).HasColumnType("varchar(50)").HasColumnName("district");
            entity.Property(o => o.State).HasColumnType("varchar(50)").HasColumnName("state");
            entity.Property(o => o.Telephone).HasColumnType("varchar(50)").HasColumnName("telephone");
            entity.Property(o => o.Bank).HasColumnType("varchar(50)").HasColumnName("bank");
            entity.Property(o => o.BankAg).HasColumnType("varchar(50)").HasColumnName("bank_ag");
            entity.Property(o => o.BankAccountNumber).HasColumnType("varchar(50)").HasColumnName("bank_account_number");
            entity.Property(o => o.BankAccountType).HasColumnType("varchar(50)").HasColumnName("bank_account_type");
            entity.Property(o => o.IuguAccountId).HasColumnType("varchar(50)").HasColumnName("iugu_account_id");
            entity.Property(o => o.IuguAccountVerified).HasColumnType("boolean").HasColumnName("iugu_account_verified");
            entity.Property(o => o.IuguUserToken).HasColumnType("varchar(1000)").HasColumnName("user_token");
            entity.Property(o => o.IuguLiveApiToken).HasColumnType("varchar(1000)").HasColumnName("live_api_token");
            entity.Property(o => o.IuguTestApiToken).HasColumnType("varchar(1000)").HasColumnName("test_api_token");

            entity.HasOne(o => o.User).WithOne(o => o.Owner).HasForeignKey<Owner>(o => o.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Book>(entity =>
        {
            entity.ToTable("books");
            entity.Property(v => v.Id).HasColumnName("id");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(v => v.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(v => v.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(v => v.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(v => v.Title).IsRequired().HasColumnType("varchar(100)").HasColumnName("title");
            entity.Property(v => v.Description).HasColumnType("varchar(100)").HasColumnName("description");
            entity.Property(v => v.Price).HasColumnType("decimal(10,2)").HasColumnName("price");
            entity.Property(v => v.PublicId).IsRequired().HasColumnType("varchar(1000)").HasColumnName("cloudinary_public_id");
            entity.Property(v => v.ThumbImgUrl).HasColumnType("varchar(1000)").HasColumnName("thumb_img_url");
            entity.Property(v => v.SaleExpirationDate).HasColumnType("timestamp").HasColumnName("sale_expiration_date");
            entity.Property(v => v.PromotionPrice).HasColumnType("decimal(10,2)").HasColumnName("promotion_price");
            entity.Property(v => v.PromotionExpirationDate).HasColumnType("timestamp").HasColumnName("promotion_expiration_date");
            entity.Property(v => v.DownloadExpirationDate).HasColumnType("timestamp").HasColumnName("download_expiration_date");
        });

        b.Entity<Order>(entity =>
        {
            entity.ToTable("orders");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(v => v.Id);
            entity.Property(u => u.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");

            entity.Property(u => u.UserId).HasColumnType("bigint").HasColumnName("user_id");
            entity.Property(u => u.BookId).HasColumnType("bigint").HasColumnName("Book_id");

            entity.HasOne(v => v.User).WithMany(u => u.Orders).HasForeignKey(v => v.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(v => v.Book).WithMany(v => v.Orders).HasForeignKey(v => v.BookId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Payment>(entity =>
        {
            entity.ToTable("payments");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");

            entity.Property(u => u.OrderId).HasColumnType("bigint").HasColumnName("order_id");
            entity.Property(p => p.PricePaid).IsRequired().HasColumnType("decimal(10,2)").HasColumnName("price_paid");
            entity.Property(p => p.Status).HasColumnType("smallint").HasColumnName("status");
            entity.Property(p => p.IuguEvent).HasColumnType("varchar(50)").HasColumnName("iugu_event");
            entity.Property(p => p.IuguFaturaId).HasColumnType("varchar(50)").HasColumnName("iugu_fatura_id");
            entity.Property(p => p.IuguAccountId).HasColumnType("varchar(50)").HasColumnName("iugu_account_id");
            entity.Property(p => p.IuguStatus).HasColumnType("varchar(50)").HasColumnName("iugu_status");
            entity.Property(p => p.IuguOrderId).HasColumnType("varchar(50)").HasColumnName("iugu_order_id");
            entity.Property(p => p.IuguExternalReference).HasColumnType("varchar(50)").HasColumnName("iugu_external_reference");
            entity.Property(p => p.IuguPaymentMethod).HasColumnType("varchar(50)").HasColumnName("iugu_payment_method");
            entity.Property(p => p.IuguPaidAt).HasColumnType("varchar(50)").HasColumnName("iugu_paid_at");
            entity.Property(p => p.IuguPayerCpfCnpj).HasColumnType("varchar(50)").HasColumnName("iugu_payer_cpf_cnpj");
            entity.Property(p => p.IuguPixEndToEndId).HasColumnType("varchar(1000)").HasColumnName("iugu_pix_end_to_end_id");
            entity.Property(p => p.IuguPaidCents).HasColumnType("varchar(50)").HasColumnName("iugu_paid_cents");
            entity.Property(p => p.IuguJsonResult).HasColumnType("text").HasColumnName("iugu_json_result");

            entity.HasOne(p => p.Order).WithMany(uv => uv.Payments).HasForeignKey(p => p.OrderId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<UserBookLog>(entity =>
        {
            entity.ToTable("user_book_logs");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");

            entity.Property(u => u.UserId).HasColumnType("bigint").HasColumnName("user_id");
            entity.Property(u => u.BookId).HasColumnType("bigint").HasColumnName("Book_id");
            entity.Property(u => u.Log).IsRequired().HasColumnType("varchar(100)").HasColumnName("log");

            entity.HasOne(u => u.User).WithMany(u => u.UserBookLogs).HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(u => u.Book).WithMany(u => u.UserBookLogs).HasForeignKey(u => u.BookId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Param>(entity =>
        {
            entity.ToTable("params");
            entity.Property(v => v.Id).HasColumnName("id");
            entity.HasKey(v => v.Id);
            entity.Property(v => v.IsActive).IsRequired().HasColumnType("boolean").HasColumnName("is_active");
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");

            entity.Property(v => v.Key).HasColumnType("varchar(50)").HasColumnName("key");
            entity.Property(v => v.Value).HasColumnType("text").HasColumnName("value");
        });

        b.Entity<UserLog>(entity =>
        {
            entity.ToTable("user_logs");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");

            entity.Property(u => u.UserId).HasColumnType("bigint").HasColumnName("user_id");
            entity.Property(u => u.Log).IsRequired().HasColumnType("varchar(100)").HasColumnName("log");

            entity.HasOne(u => u.User).WithMany(u => u.UserLogs).HasForeignKey(u => u.UserId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<Plan>(entity =>
        {
            entity.ToTable("plans");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(v => v.Title).IsRequired().HasColumnType("varchar(100)").HasColumnName("title");
            entity.Property(v => v.Price).IsRequired().HasColumnType("decimal(10,2)").HasColumnName("price");
            entity.Property(v => v.Currency).IsRequired().HasColumnType("varchar(10)").HasColumnName("currency");
            entity.Property(u => u.InitialValidityPeriod).HasColumnType("timestamp").HasColumnName("initial_validity_period");
            entity.Property(u => u.FinalValidityPeriod).HasColumnType("timestamp").HasColumnName("final_validity_period");
            entity.Property(u => u.MaxLimitSendDataIA).HasColumnType("smallint").HasColumnName("max_limit_send_data_IA");
            entity.Property(u => u.CaractersLimitFactor).HasColumnType("smallint").HasColumnName("caracters_limit_factor");
        });

        b.Entity<Chapter>(entity =>
        {
            entity.ToTable("chapters");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(v => v.Title).IsRequired().HasColumnType("varchar(100)").HasColumnName("title");
            entity.Property(v => v.ChapterNumber).HasColumnType("integer").HasColumnName("chapter_number");
        });

        b.Entity<Question>(entity =>
        {
            entity.ToTable("questions");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(v => v.Title).IsRequired().HasColumnType("varchar(500)").HasColumnName("title");
            entity.Property(u => u.MaxLimitCharacters).HasColumnType("smallint").HasColumnName("max_limit_characters");
            entity.Property(u => u.MinLimitCharacters).HasColumnType("smallint").HasColumnName("min_limit_characters");
            entity.Property(u => u.Subject).HasColumnType("varchar(100)").HasColumnName("subject");
        });

        b.Entity<Theme>(entity =>
        {
            entity.ToTable("themes");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(v => v.Title).IsRequired().HasColumnType("varchar(100)").HasColumnName("title");
        });

        b.Entity<PlanChapter>(entity =>
        {
            entity.ToTable("plan_chapter");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(u => u.PlanId).HasColumnType("bigint").HasColumnName("plan_id");
            entity.Property(u => u.ChapterId).HasColumnType("bigint").HasColumnName("chapter_id");

            entity.HasOne(u => u.Plan).WithMany(u => u.PlanChapters).HasForeignKey(u => u.PlanId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });

        b.Entity<PlanChapterQuestion>(entity =>
        {
            entity.ToTable("plan_chapter_question");
            entity.Property(u => u.Id).HasColumnName("id");
            entity.HasKey(u => u.Id);
            entity.Property(u => u.CreatedAt).IsRequired().HasColumnType("timestamp").HasColumnName("created_at");
            entity.Property(u => u.UpdatedAt).HasColumnType("timestamp").HasColumnName("updated_at");
            entity.Property(u => u.DeletedAt).HasColumnType("timestamp").HasColumnName("deleted_at");
            entity.Property(u => u.UpdatedBy).HasColumnType("varchar(50)").HasColumnName("updated_by");

            entity.Property(u => u.PlanChapterId).HasColumnType("bigint").HasColumnName("plan_chapter_id");
            entity.Property(u => u.QuestionId).HasColumnType("bigint").HasColumnName("question_id");

            //entity.HasOne(u => u.PlanChapter).WithMany(u => u.PlanChapterQuestions).HasForeignKey(u => u.QuestionId).IsRequired().OnDelete(DeleteBehavior.Restrict);
        });
    }
}