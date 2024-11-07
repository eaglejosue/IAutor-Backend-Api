using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IAutor.Api.Migrations
{
    /// <inheritdoc />
    public partial class Recriate4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "books",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    description = table.Column<string>(type: "varchar(100)", nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    cloudinary_public_id = table.Column<string>(type: "varchar(1000)", nullable: false),
                    thumb_img_url = table.Column<string>(type: "varchar(1000)", nullable: true),
                    sale_expiration_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    download_expiration_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    promotion_price = table.Column<decimal>(type: "numeric(10,2)", nullable: true),
                    promotion_expiration_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_books", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "chapters",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    chapter_number = table.Column<int>(type: "integer", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_chapters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "params",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    key = table.Column<string>(type: "varchar(50)", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_params", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "plans",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    currency = table.Column<string>(type: "varchar(10)", nullable: false),
                    max_limit_send_data_IA = table.Column<short>(type: "smallint", nullable: false),
                    initial_validity_period = table.Column<DateTime>(type: "timestamp", nullable: false),
                    final_validity_period = table.Column<DateTime>(type: "timestamp", nullable: true),
                    caracters_limit_factor = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plans", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(500)", nullable: false),
                    max_limit_characters = table.Column<short>(type: "smallint", nullable: false),
                    min_limit_characters = table.Column<short>(type: "smallint", nullable: false),
                    Subject = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "themes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "varchar(100)", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_themes", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    last_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    email = table.Column<string>(type: "varchar(100)", nullable: false),
                    cpf = table.Column<string>(type: "varchar(50)", nullable: true),
                    sign_in_with = table.Column<string>(type: "varchar(10)", nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: true),
                    birth_date = table.Column<DateTime>(type: "date", nullable: true),
                    ProfileImgUrl = table.Column<string>(type: "text", nullable: true),
                    password_hash = table.Column<byte[]>(type: "bytea", nullable: true),
                    activation_code = table.Column<string>(type: "varchar(50)", nullable: true),
                    activation_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    reset_password = table.Column<bool>(type: "boolean", nullable: true),
                    reset_password_code = table.Column<string>(type: "varchar(50)", nullable: true),
                    reset_password_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "plan_chapter",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    plan_id = table.Column<long>(type: "bigint", nullable: false),
                    chapter_id = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_chapter", x => x.id);
                    table.ForeignKey(
                        name: "FK_plan_chapter_chapters_chapter_id",
                        column: x => x.chapter_id,
                        principalTable: "chapters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plan_chapter_plans_plan_id",
                        column: x => x.plan_id,
                        principalTable: "plans",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "emails",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    Book_id = table.Column<long>(type: "bigint", nullable: true),
                    email_type = table.Column<short>(type: "smallint", nullable: true),
                    schedule_date = table.Column<DateTime>(type: "timestamp", nullable: true),
                    date_sent = table.Column<DateTime>(type: "timestamp", nullable: true),
                    send_attempts = table.Column<short>(type: "smallint", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_emails", x => x.id);
                    table.ForeignKey(
                        name: "FK_emails_books_Book_id",
                        column: x => x.Book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_emails_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    Book_id = table.Column<long>(type: "bigint", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_books_Book_id",
                        column: x => x.Book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_orders_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "owners",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    last_name = table.Column<string>(type: "varchar(50)", nullable: true),
                    social_user_name = table.Column<string>(type: "varchar(50)", nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: true),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    profile_img_url = table.Column<string>(type: "varchar(1000)", nullable: true),
                    instagram = table.Column<string>(type: "varchar(100)", nullable: true),
                    tiktok = table.Column<string>(type: "varchar(100)", nullable: true),
                    person_type = table.Column<string>(type: "varchar(50)", nullable: true),
                    cpf = table.Column<string>(type: "varchar(50)", nullable: true),
                    cnpj = table.Column<string>(type: "varchar(50)", nullable: true),
                    cnpj_rep_name = table.Column<string>(type: "varchar(100)", nullable: true),
                    cnpj_resp_cpf = table.Column<string>(type: "varchar(50)", nullable: true),
                    address = table.Column<string>(type: "varchar(100)", nullable: true),
                    cep = table.Column<string>(type: "varchar(50)", nullable: true),
                    city = table.Column<string>(type: "varchar(50)", nullable: true),
                    district = table.Column<string>(type: "varchar(50)", nullable: true),
                    state = table.Column<string>(type: "varchar(50)", nullable: true),
                    telephone = table.Column<string>(type: "varchar(50)", nullable: true),
                    bank = table.Column<string>(type: "varchar(50)", nullable: true),
                    bank_ag = table.Column<string>(type: "varchar(50)", nullable: true),
                    bank_account_number = table.Column<string>(type: "varchar(50)", nullable: true),
                    bank_account_type = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_account_verified = table.Column<bool>(type: "boolean", nullable: true),
                    iugu_account_id = table.Column<string>(type: "varchar(50)", nullable: true),
                    user_token = table.Column<string>(type: "varchar(1000)", nullable: true),
                    live_api_token = table.Column<string>(type: "varchar(1000)", nullable: true),
                    test_api_token = table.Column<string>(type: "varchar(1000)", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owners", x => x.id);
                    table.ForeignKey(
                        name: "FK_owners_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_book_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    Book_id = table.Column<long>(type: "bigint", nullable: false),
                    log = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_book_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_book_logs_books_Book_id",
                        column: x => x.Book_id,
                        principalTable: "books",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_book_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "user_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    log = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_logs_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "plan_chapter_question",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    plan_chapter_id = table.Column<long>(type: "bigint", nullable: false),
                    QuestionId = table.Column<long>(type: "bigint", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    updated_by = table.Column<string>(type: "varchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plan_chapter_question", x => x.id);
                    table.ForeignKey(
                        name: "FK_plan_chapter_question_plan_chapter_plan_chapter_id",
                        column: x => x.plan_chapter_id,
                        principalTable: "plan_chapter",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_plan_chapter_question_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    order_id = table.Column<long>(type: "bigint", nullable: false),
                    price_paid = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    status = table.Column<short>(type: "smallint", nullable: true),
                    iugu_event = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_fatura_id = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_order_id = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_account_id = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_status = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_external_reference = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_payment_method = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_paid_at = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_payer_cpf_cnpj = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_pix_end_to_end_id = table.Column<string>(type: "varchar(1000)", nullable: true),
                    iugu_paid_cents = table.Column<string>(type: "varchar(50)", nullable: true),
                    iugu_json_result = table.Column<string>(type: "text", nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    deleted_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "FK_payments_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "incomes",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true),
                    owner_id = table.Column<long>(type: "bigint", nullable: false),
                    date_reference = table.Column<DateTime>(type: "date", nullable: false),
                    sum_value = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    sales_amount = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_incomes", x => x.id);
                    table.ForeignKey(
                        name: "FK_incomes_owners_owner_id",
                        column: x => x.owner_id,
                        principalTable: "owners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_emails_Book_id",
                table: "emails",
                column: "Book_id");

            migrationBuilder.CreateIndex(
                name: "IX_emails_user_id",
                table: "emails",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_incomes_owner_id",
                table: "incomes",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_Book_id",
                table: "orders",
                column: "Book_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_user_id",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_owners_user_id",
                table: "owners",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_payments_order_id",
                table: "payments",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "IX_plan_chapter_chapter_id",
                table: "plan_chapter",
                column: "chapter_id");

            migrationBuilder.CreateIndex(
                name: "IX_plan_chapter_plan_id",
                table: "plan_chapter",
                column: "plan_id");

            migrationBuilder.CreateIndex(
                name: "IX_plan_chapter_question_QuestionId",
                table: "plan_chapter_question",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_plan_chapter_question_plan_chapter_id",
                table: "plan_chapter_question",
                column: "plan_chapter_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_book_logs_Book_id",
                table: "user_book_logs",
                column: "Book_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_book_logs_user_id",
                table: "user_book_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_logs_user_id",
                table: "user_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "emails");

            migrationBuilder.DropTable(
                name: "incomes");

            migrationBuilder.DropTable(
                name: "params");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "plan_chapter_question");

            migrationBuilder.DropTable(
                name: "themes");

            migrationBuilder.DropTable(
                name: "user_book_logs");

            migrationBuilder.DropTable(
                name: "user_logs");

            migrationBuilder.DropTable(
                name: "owners");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "plan_chapter");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "books");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "chapters");

            migrationBuilder.DropTable(
                name: "plans");
        }
    }
}
