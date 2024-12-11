using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IAutor.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialInserts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
              @"INSERT INTO plans(id, title, price, currency, max_qtd_call_ia_sugestions, initial_validity_period, final_validity_period, caracters_limit_factor, description, is_active, created_at) VALUES
                (1, 'Degustação', 0.00, 'R$', 3, '2024-10-25T00:00:00', '3000-01-01T00:00:00', 5, 'Plano de degustação gratuito', true, '2024-12-11T00:00:00'),
                (2, 'Livro do momento', 199.00, 'R$', 5, '2024-10-25T00:00:00', '3000-01-01T00:00:00', 7, 'Plano intermediário', true, '2024-12-11T00:00:00'),
                (3, 'Livro da vida', 399.00, 'R$', 5, '2024-10-25T00:00:00', '3000-01-01T00:00:00', 10, 'Plano Premium', true, '2024-12-11T00:00:00');
              ");

            migrationBuilder.Sql(
              @"INSERT INTO plan_items('PlanId', description, is_active, created_at) VALUES
                (1, '10 até 50 páginas', true, '2024-12-11T00:00:00'),
                (1, '1 até 3 fotos autorais', true, '2024-12-11T00:00:00'),
                (2, '51 até 100 páginas', true, '2024-12-11T00:00:00'),
                (2, '1 até 5 fotos autorais', true, '2024-12-11T00:00:00'),
                (2, '1 Livro Impresso', true, '2024-12-11T00:00:00'),
                (3, '101 até 200 páginas', true, '2024-12-11T00:00:00'),
                (3, '1 até 7 fotos autorais', true, '2024-12-11T00:00:00'),
                (3, '1 Livro Impresso', true, '2024-12-11T00:00:00');
              ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
