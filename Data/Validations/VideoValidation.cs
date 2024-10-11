namespace IAutor.Api.Data.Validations;

public sealed class VideoCreateValidator : Validation<Video>
{
    public VideoCreateValidator()
    {
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(80);
        RuleFor(p => p.Description).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(p => p.ReleaseDate).NotEmpty().NotNull();
        RuleFor(p => p.Price).NotEmpty().NotNull().GreaterThan(0.99M).WithMessage("Preço deve ser no mínimo R$ 1,00.");
        RuleFor(p => p.CloudinaryPublicId).NotEmpty().NotNull();
        RuleFor(p => p.OwnerVideos).NotNull().Must(m => m != null && m.Any(a => a.OwnerId > 0));
    }
}

public sealed class VideoUpdateValidator : Validation<Video>
{
    public VideoUpdateValidator()
    {
        RuleFor(p => p.Id).NotNull().NotEmpty();
        RuleFor(p => p.Title).NotEmpty().NotNull().MaximumLength(80);
        RuleFor(p => p.Description).NotEmpty().NotNull().MaximumLength(100);
        RuleFor(p => p.ReleaseDate).NotEmpty().NotNull();
        RuleFor(p => p.Price).NotEmpty().NotNull().GreaterThan(0.99M).WithMessage("Preço deve ser no mínimo R$ 1,00.");
        RuleFor(p => p.CloudinaryPublicId).NotEmpty().NotNull();
    }
}

public sealed class VideoPatchValidator : Validation<Video>
{
    public VideoPatchValidator()
    {
        RuleFor(p => p.Id).NotNull().NotEmpty();
    }
}

public sealed class VideoTrailerValidator : Validation<VideoTrailer>
{
    public VideoTrailerValidator()
    {
        RuleFor(p => p.VideoId).NotEmpty().NotNull();
        RuleFor(p => p.CloudinaryPublicId).NotEmpty().NotNull();
    }
}

public static class VideoValidationExtension
{
    public static Task<ValidationResult> ValidateCreateAsync(this Video v) => new VideoCreateValidator().ValidateCustomAsync(v);
    public static Task<ValidationResult> ValidateUpdateAsync(this Video v) => new VideoUpdateValidator().ValidateCustomAsync(v);
    public static Task<ValidationResult> ValidatePatchAsync(this Video v) => new VideoPatchValidator().ValidateCustomAsync(v);
    public static Task<ValidationResult> ValidateAsync(this VideoTrailer v) => new VideoTrailerValidator().ValidateCustomAsync(v);
}