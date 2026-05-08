using edificio_digital.Entity.Data;
using edificio_digital.Entity.Model.Seguridad;
using edificio_digital.Models.Domain.Auth;
using Microsoft.EntityFrameworkCore;

namespace edificio_digital.Entity.Auth;

public class PostgreSqlRefreshTokenRepository(AppDbContext db) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken token)
    {
        db.RefreshTokens.Add(new RefreshTokenEntity
        {
            Id = token.Id,
            UsuarioId = token.UserId,
            TokenHash = token.TokenHash,
            ExpiresAt = token.ExpiresAt,
            CreatedAt = token.CreatedAt,
            RevokedAt = token.RevokedAt,
            ReplacedByTokenId = token.ReplacedByTokenId
        });
        await db.SaveChangesAsync();
    }

    public async Task<RefreshToken?> FindByHashAsync(string tokenHash)
    {
        var entity = await db.RefreshTokens
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.TokenHash == tokenHash);

        return entity is null ? null : Map(entity);
    }

    public async Task UpdateAsync(RefreshToken token)
    {
        var entity = await db.RefreshTokens.FirstOrDefaultAsync(x => x.Id == token.Id);
        if (entity is null) return;

        entity.RevokedAt = token.RevokedAt;
        entity.ReplacedByTokenId = token.ReplacedByTokenId;
        await db.SaveChangesAsync();
    }

    public async Task RevokeAllForUserAsync(Guid userId)
    {
        var now = DateTime.UtcNow;
        await db.RefreshTokens
            .Where(x => x.UsuarioId == userId && x.RevokedAt == null)
            .ExecuteUpdateAsync(s => s.SetProperty(x => x.RevokedAt, now));
    }

    private static RefreshToken Map(RefreshTokenEntity e) => new()
    {
        Id = e.Id,
        UserId = e.UsuarioId,
        TokenHash = e.TokenHash,
        ExpiresAt = e.ExpiresAt,
        CreatedAt = e.CreatedAt,
        RevokedAt = e.RevokedAt,
        ReplacedByTokenId = e.ReplacedByTokenId
    };
}
